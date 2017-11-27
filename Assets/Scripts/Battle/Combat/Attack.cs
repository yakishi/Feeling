using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/*===============================================================*/
/// <summary>攻撃コマンドの処理を管理します</summary>
public class Attack /*: Combat !注意 : extends すると二回呼び出されてしまう！*/ {

	// 現在のターン中の敵または味方の行動状態
	static private List<Combat.CombatState> myCombatState;
	static private List<Combat.CombatStateEx> myCombatStateEx;
	private int rnd; // 攻撃対象を乱数で確定する為の変数の準備
	private PlayerManagerSaveData.PlayerManager fluctuationVal; // 変動値を格納する変数を準備

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	public Attack() {
		Debug.Log( "Attack Class on the Constructor Function Call." );
		Initialize( );
		

	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	void Initialize( ) {
		myCombatState = Combat.GetCombatState;
		myCombatStateEx = Combat.GetCombatStateEx;
		rnd = UnityEngine.Random.Range( 0, 4 );
		fluctuationVal = new PlayerManagerSaveData.PlayerManager( );
		fluctuationVal.LoadPlayer( ); // Attack 関数が呼ばれたときに現在のプレイヤーの変動値を持ってくる


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>攻撃コマンド時の敵AI処理</summary>
	/// <param name="generate"></param>
	public void EnemyAttack( BattleEnemyGenerate generate ) {
		Debug.Log( "EnemyAttack Function Call." );

		// 注意! foreach で回すと挙動が可笑しくなるので使わない
		for( int i = 0; i < myCombatState.Count; i++ ) {
			if( myCombatState[ i ].isAction ) {
				Debug.Log( "befor : " + myCombatState[ i ].isAction );
				// 敵側ターンの時は, ボタンは選択できない
				UI_BattleScene.ApplyCmdBtnIsActive( false );
				// 誰が ( 敵側 ) 攻撃しているか分かるようにメッセージを出す
				HUD_BattleScene.ApplyMessageText( generate.GetEnemyStatusData
					( myCombatState[ i ].id.gameObject.name + "_NAME" ) + "の攻撃" );

				int sleep = i; // i を一時的に格納します i は 行動順番目

				// 1.8 秒後に出す ( 攻撃エフェクト終了直前 ) 
				Observable.Timer( TimeSpan.FromMilliseconds( 1800 ) )
					.Subscribe( _ => BattleAudio.PlaySE( myCombatState[ sleep ].id.gameObject.name, myCombatState[ sleep ].id.transform.GetChild( 3 ).GetComponent<AudioSource>( ) ) );

				// 敵側からの攻撃 → Player 側への攻撃エフェクト表示をする
				// エフェクト再生関数を呼ぶ
				// 2 秒後に出す
				Observable.Timer( TimeSpan.FromMilliseconds( 2000 ) )
					.Subscribe( _ => new
						 PlayerEffect( ).SetEffectPlay( rnd/* ランダムに攻撃 */, myCombatState[ sleep ].id.gameObject.name, 36.0f, 36.0f, 0.0f, -70.0f ) );

				// 2.5 秒後に出す ( 攻撃エフェクトが表示し終わった後 )
				Observable.Timer( TimeSpan.FromMilliseconds( 2500 ) )
					.Subscribe( _ => ApplyDamageHp( rnd, generate, sleep ) );

				// 5 秒後に次の行動者に移る
				Observable.Timer( TimeSpan.FromMilliseconds( 5000 ) )
					.Subscribe( _ => Sleep( sleep ) );

				// 現在の行動可能フラグを無効にし行動終了とする
				myCombatState[ i ].isAction = false;

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>Playerに攻撃を与えます</summary>
	/// <remarks>
	/// TODO
	/// Playerの入れ替え処理には対応してません
	/// Player側はObject.Destroy関数を使わないようにします(HUDのため)
	/// </remarks>
	/// <param name="playersIndex">攻撃対象プレイヤー</param>
	/// <param name="generate">BattleEnemyGenerateクラス</param>
	/// <param name="currentAttackName">敵の現在の行動順</param>
	private void ApplyDamageHp( int playersIndex, BattleEnemyGenerate generate, int currentAttackName ) {
		try {
			if( myCombatState[ currentAttackName ].id != null ) {
				PlayerManagerSaveData.State state = fluctuationVal.CurrentState[ playersIndex ];

				// ダメージ量算出式 = 攻撃力の二乗(敵)×Level(敵)÷プレイヤーの防御力×1.0×乱数（0.9～1.1）
				float enemyAtk = float.Parse( generate.GetEnemyStatusData( myCombatState[ currentAttackName ].id.gameObject.name  + "_ATK" ) );
				float enemyLv = float.Parse( generate.GetEnemyStatusData( myCombatState[ currentAttackName ].id.gameObject.name  + "_LV" ) );
				float playerDef = state.Def;
				float rnd = UnityEngine.Random.Range( 0.9f, 1.1f );
				float sum = ( ( enemyAtk * 2.0f * enemyLv ) / ( playerDef * 1.0f * rnd ) );

				// TODO
				// 仮で CSV 側の固定値 - sum ( ダメージ値 ) で算出しています
				// 将来的に, CSV 側は, 固定値ではなく, player manager savedata class の変動値に変えます
				float hpLeftovers = PlayerManagerCSV.GetPlayers[ playersIndex ].HP - sum;

				Debug.Log( "<color='red'>通常攻撃ダメージ量 : " + ( int )sum + ", 乱数 : " + rnd + ", playersIndex : " + playersIndex + "</color>" );

				myCombatState[ playersIndex ].HP -= ( int )hpLeftovers; // 現在のプレイヤー行動ステートを更新する ( HP )
				HUD_BattleScene.ApplyCharaHP( playersIndex, ( int )hpLeftovers ); // HUD ( HP ) を更新する

				if( ( int )hpLeftovers > -100 ) {
					myCombatState[ playersIndex ].HP = ( int )hpLeftovers; // 現在のプレイヤー行動ステートを更新する ( HP )
					HUD_BattleScene.ApplyCharaHP( playersIndex,  ( int )hpLeftovers ); // HUD ( HP ) を更新する

				} myCombatStateEx[ playersIndex ].playerIsDead = true; // 残り HP が 0 の時, 死亡

			}

		} catch( DivideByZeroException ) {
			// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/
			Debug.LogError( "0 による整数除算が行われました! : NaN" );

		} catch( OverflowException ) {
			// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/
			Debug.LogError( "浮動小数点数の算術オーバーフローが行われました!" );

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>次の行動者に移行するまでの時間稼ぎ</summary>
	/// <param name="index">myCombatState[i].isActionが真の時の行動番目</param>
	private void Sleep( int index ) {
		Debug.Log( "Sleep Function Call." );
		Debug.Log( "<color='red'>Sleep( index ) : " + index + "</color>" );
		Combat.CombatState removeBefore = myCombatState[ index ]; // 一巡した後に先頭が変わってしまうので削除前のを入れる
		myCombatState.RemoveAt( index ); // 現在の要素番目を削除
		myCombatState[ index ].isAction = true; // 削除したので次の番目が最初に来る = 次の行動者を有効にする
		myCombatState.Add( /*myCombatState[ index ]*/removeBefore ); // 削除した要素番目を追加する ( 巡回できるようになる ) ( これがないと行動済の敵が倒せなくなる )

		for( int i = 0; i < myCombatState.Count; i++ ) Debug.Log( "Remove After Count[ " + i + " ] : 行動者一覧 ( " + myCombatState[ i ].id.name + " )" );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤー側から敵側への攻撃を管理します</summary>
	/// <param name="choiceName">敵選択矢印で選択された名前</param>
	public void PlayerAttack( string choiceName ) {
		Debug.Log( "PlayerAttack Function Call." );
		Debug.Log( choiceName );

		string subIdentifier;

		for( int i = 0; i < myCombatState.Count; i++ ) {
			// combat state の i 番目の 副識別子 gameobject name と選択された gameobject name
			// 等しい時の敵ステータス情報を管理する
			if( myCombatState[ i ].id != null ) {
				subIdentifier = myCombatState[ i ].id.gameObject.transform.GetChild( 2 ).name;
				Debug.Log( myCombatState[ i ].id );


				/////////////////////////////////////////////////////////////////////////////////////////////////////////
				// TODO 編集中コード 行動プレイヤーによるスキル切替処理テスト
				if ( myCombatState[ i ].id.name == "Character2" ){
					for( int piyo = 0; piyo < SkillLoader.GetPlayersSkills.GetLength( 1 ); piyo++ ) {
						// 仮でプレイヤー 3 のスキルデータをぶち込んでおく
						UI_BattleScene.ScrollBarAddBtn( 0, SkillLoader.GetPlayersSkills[ 2, piyo ].NAME );

					}
				}
				/////////////////////////////////////////////////////////////////////////////////////////////////////////


				if ( subIdentifier == UI_BattleScene.GetEnemyArrowChoice.ToString( ) ) {
					// エフェクト再生関数を呼ぶ
					EnemyEffect.SetEffectPlay( UI_BattleScene.GetEnemyArrowChoice, UnityEngine.Random.Range( 0, 5 ).ToString( ), 1.0f, 1.0f, 0.0f, 0.0f );

					// 2 秒後に出す
					// 敵を倒すのが早いとエフェクトが表示できないので関数を呼ぶ
					Observable.Timer( TimeSpan.FromMilliseconds( 2000 ) )
						.Subscribe( _ => EnemyDestroy( i, subIdentifier ) );

					break;

				}

			}

		}

		for ( int i = 0; i < Combat.GetCombatState.Count; i++ ) {
			if ( Combat.GetCombatState[ i ].isAction ) {
				Combat.GetCombatState[ i ].isAction = false; // 攻撃コマンドが押されたら行動終了とする
				int sleep = i; // i を一時的に格納します

				// 2.1 秒後に次の行動者に移る
				Observable.Timer( TimeSpan.FromMilliseconds( 2100 ) )
					.Subscribe( _ => Sleep( sleep ) );
				
				break; // i がカウントされ続けるので抜ける

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>敵Destroy処理を行います</summary>
	/// <param name="index">Destroyしようとしている敵GameObject</param>
	/// <param name="identifier">敵側のサブ識別子</param>
	private void EnemyDestroy( int index, string identifier ) {
		Debug.Log( "<color='red'>" + "敵Destroy名 : " + myCombatState[ index ].id.name + ", ( index, サブ識別子 ) : ( " + index + ", " + identifier + " )</color>" );
		// 試しにゲームオブジェクトを消してみる
		//myCombatState[ index ].enemyIsDead = true;
		myCombatStateEx[ int.Parse( identifier ) ].enemyIsDead = true;
		UnityEngine.Object.Destroy( myCombatState[ index ].id.gameObject/*, 1.0f*/ );
		myCombatState[ index ].id = null;
		myCombatState.RemoveAt( index ); // 敵を倒したら要素を削除する


	}
	/*===============================================================*/


}
/*===============================================================*/