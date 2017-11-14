﻿using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/*===============================================================*/
/// <summary>攻撃コマンドの処理を管理します</summary>
public class Attack /*: Combat !注意 : extends すると二回呼び出されてしまう！*/ {

	// 現在のターン中の敵または味方の行動状態
	static private List<Combat.CombatState> myCombatState;
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
		rnd = UnityEngine.Random.Range( 0, 4 );
		fluctuationVal = new PlayerManagerSaveData.PlayerManager( );
		fluctuationVal.LoadPlayer( ); // Attack 関数が呼ばれたときに現在のプレイヤーの変動値を持ってくる


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>攻撃コマンド時の敵AI処理,ダメージなどはまだあたえられない</summary>
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
				// 敵側からの攻撃 → Player 側への攻撃エフェクト表示をする
				// エフェクト再生関数を呼ぶ
				// 2 秒後に出す
				// TODO
				// 現状, 敵側がプレイヤー側に攻撃するエフェクトがないので
				// 将来的に, 変更します。
				Observable.Timer( TimeSpan.FromMilliseconds( 2000 ) )
					.Subscribe( _ => new
						 PlayerEffect( ).SetEffectPlay( rnd/* ランダムに攻撃 */, "IsTest", 36.0f, 36.0f, 0.0f, -70.0f ) );

				int sleep = i; // i を一時的に格納します

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
			// 敵側からプレイヤー側へのダメージ量算出式ではなく, プレイヤー側から敵側へのダメージ計算式になっているので将来的に変更します TODO
			// ダメージ量算出式 = 攻撃力の二乗×Level÷敵の防御力×威力×（0.9～1.1）
			PlayerManagerSaveData.State state = fluctuationVal.CurrentState[ playersIndex ];
			float sum1 = float.Parse( generate.GetEnemyStatusData( myCombatState[ currentAttackName ].id.gameObject.name + "_DEF" ) );
			float sum = ( ( state.Atk * 2.0f * state.Lv ) / ( sum1 * state.Atk * UnityEngine.Random.Range( 0.9f, 1.1f ) ) );

			// TODO
			// 仮で CSV 側の固定値 - sum ( ダメージ値 ) ( 変動値 ) で算出しています
			// 将来的に, CSV 側の固定値ではなく, player manager savedata class の変動値に変えます
			float hpLeftovers = PlayerManagerCSV.GetPlayers[ playersIndex ].HP - sum;

			Debug.Log( "<color='red'>通常攻撃ダメージ量 : " + ( int )sum + "</color>" );
			if( ( int )hpLeftovers > 0 ) {
				myCombatState[ playersIndex ].HP = ( int )hpLeftovers; // 現在のプレイヤー行動ステートを更新する ( HP )
				HUD_BattleScene.ApplyCharaHP( playersIndex, ( int )hpLeftovers ); // HUD ( HP ) を更新する

			} else myCombatState[ playersIndex ].playerIsDead = true; // 残り HP が 0 の時, 死亡

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
		Debug.Log( "<color='red'>index : " + index + "</color>" );
		myCombatState[ index + 1 ].isAction = true; // 次の順番が行動できるようにする


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

				if ( subIdentifier == UI_BattleScene.GetEnemyArrowChoice.ToString( ) ) {
					Debug.Log( "<color='red'>行動番目 : " + i + ", 実際の敵選択矢印選択番目 : " + subIdentifier + "番目のステータス情報</color>" );
					// エフェクト再生関数を呼ぶ
					EnemyEffect.SetEffectPlay( UI_BattleScene.GetEnemyArrowChoice, "IsSword", 1.0f, 1.0f, 0.0f, 0.0f );
					// 試しにゲームオブジェクトを消してみる
					UnityEngine.Object.Destroy( myCombatState[ i ].id.gameObject/*, 1.0f*/ );
					myCombatState[ i ].enemyIsDead = true;
					break;

				}

			}

		}

		for ( int i = 0; i < Combat.GetCombatState.Count; i++ ) {
			if ( Combat.GetCombatState[ i ].isAction ) {
				Debug.Log( i + "番目が行動可能状態です。" );
				Combat.GetCombatState[ i ].isAction = false; // 攻撃コマンドが押されたら行動終了とする
				Combat.GetCombatState[ i + 1 ].isAction = true; // 攻撃コマンドが押された時に次ぎの人に回す

				break; // i がカウントされ続けるので抜ける

			}

		}


	}
	/*===============================================================*/


}
/*===============================================================*/