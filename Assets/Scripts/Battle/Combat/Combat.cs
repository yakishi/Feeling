using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

/*===============================================================*/
/// <summary>戦闘コマンドの基底クラス</summary>
/// <remarks>BattleSceneObjectにScriptを関連づけます</remarks>
public class Combat : MonoBehaviour {
	protected GameObject[ ] generateEnemies = BattleEnemyGenerate.Enemy; // 生成されたゲームオブジェクト

	// MonoBehaviour は GamgeObject に依存しているので一度ゲームオブジェクトにしなければならない
	// MonoBehaviour で new のような呼び出しを行う場合は, 呼び出そうとしているスクリプトがあるオブジェクトにアタッチする必要がある
	private GameObject thisObj;
	
	private HUD_BattleScene hud;
	private BattleEnemyGenerate generate;
	public HUD_BattleScene Hud { get { return hud; } }
	public BattleEnemyGenerate Generate { get { return generate; } }

	// 戦闘開始時のプレイヤー人数
	private const int  startingLineup = 4;
	// 行動順判定の為の変数の準備
	private int[ ] playersSpeed = new int[ startingLineup ];
	// index 更新用変数の準備
	private int IdCnt;
	// 敵および味方の行動順を管理する変数の準備
	private GameObject[ ] actionTurnGameObj; // 初期化関数でゲームオブジェクトが入ります
	// 現在のターン中の敵または味方の行動状態
	static private List<CombatState> ActionState = new List<CombatState>( );
	// 初期ターン終了判定
	private bool InitializeTurn;

	// セッターおよびゲッター定義部
	/// <summary>現在のターン中の敵または味方の行動状態をgetします</summary>
	static public List<CombatState> GetCombatState { get { return ActionState; } }

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	void Awake( ) {
		// 初期化関数を呼び出す
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	void Start( ) {
		//次のフレームで実行する
		Observable.NextFrame( )
			.Subscribe( _ => InitializeActionTurn( ) );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	private void Initialize( ) {
		// 呼び出そうとしているスクリプトは, Combat スクリプトがアタッチされているオブジェクトと同じオブジェクトにアタッチ
		// されているので現在アタッチされているゲームオブジェクトで初期化する
		thisObj = this.gameObject;
		thisObj.GetComponent<BattleEnemyGenerate>( ).Initialize( );

		generate = thisObj.GetComponent<BattleEnemyGenerate>( );
		hud = thisObj.GetComponent<HUD_BattleScene>( );

		playersSpeed[ 0 ] = hud.Player1.SPD;
		playersSpeed[ 1 ] = hud.Player2.SPD;
		playersSpeed[ 2 ] = hud.Player3.SPD;
		playersSpeed[ 3 ] = hud.Player4.SPD;
		//playersSpeed[ 4 ] = hud.Player5.SPD;
		//playersSpeed[ 5 ] = hud.Player6.SPD;

		InitializeTurn = false; // 戦闘シーン開始じに初期化される


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによって毎フレーム呼ばれます</summary>
	void Update( ) {
		// 戦闘開始時のターンが終了したとき
		if( InitializeTurn ) {
			for( int i = 0; i < ActionState.Count; i++ ) {
				if( ActionState[ i ].isAction ) {
					if( ActionState[ i ].id != null ) {
						ActionTurn( ActionState[ i ].id.gameObject.name );/*new Attack( ).AAttack( generate )*/ /*);*/
						Debug.Log( "<color='red'>" + i + "番目 : " + ActionState[ i ].id.gameObject.name + "</color>" );

					}

				}

			}


		}
		// 戦闘終了処理 ( 敵側を殲滅した時 )
		for( int i = 0; i < ActionState.Count; i++ ) {
			if( ActionState[ i ].enemyIsDead ) {
				if( i == BattleEnemyGenerate.EnemyNumber ) {
					Debug.Log( "<color='red'>敵を殲滅しました！</color>" );
					HUD_BattleScene.ApplyMessageText( "敵を殲滅しました！" );

				}

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>戦闘開始時の行動ゲームオブジェクト決定処理</summary>
	void InitializeActionTurn( ) {
		// 戦闘開始時にターン中に行動するゲームオブジェクトを入れる
		actionTurnGameObj = ActionTurn( );
		// ターン中の行動 State を初期化
		for( int i = 0; i < actionTurnGameObj.GetLength( 0 ); i++ ) {
			// 初期化
			ActionState.Add( new CombatState( actionTurnGameObj[ i ], 1, false, false, false, false ) );

		}
		// 一番目に行動可能とする
		ActionState[ 0 ].isAction = true;
		InitializeTurn = true; // 戦闘開始時ターン終了合図


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>ターン中に行動する敵およびキャラクターの順序を決定します</summary>
	/// <returns>ターン中に行動するゲームオブジェクトの配列</returns>
	private GameObject[ ] ActionTurn( ) {
		// 昇順ソートをするための変数準備
		int AscendingOrderElement = ( int )BattleEnemyGenerate.EnemyNumber + playersSpeed.Length;
		int[ ] AscendingOrderArray = new int[ AscendingOrderElement ]; // 素早さ
		GameObject[ ] AscendingOrderArrayId = new GameObject[ AscendingOrderElement ]; // Enemies gameObj
		int tmp = 0;
		int tmp2 = 0;
		IdCnt = 0;
		// 敵の素早さおよびプレイヤーの素早さを格納します
		for ( int i = 0; i < AscendingOrderElement; i++ ) {
			if( i < BattleEnemyGenerate.EnemyNumber ) {
				AscendingOrderArray[ i ] = int.Parse( generate.GetEnemyStatusData(  generateEnemies[ i ].name + "_SPD" )  );
				AscendingOrderArrayId[ i ] = generateEnemies[ i ].gameObject; // Enemies gameObj

			}
			if ( i <= BattleEnemyGenerate.EnemyNumber ) tmp = i;

		}
		for( int i = tmp; i < AscendingOrderElement; i++ ) {
			if( tmp2 < playersSpeed.Length ) {
				AscendingOrderArray[ i ] = playersSpeed[ tmp2 ];
				AscendingOrderArrayId[ i ] = HUD_BattleScene.CharaHudGameObj[ tmp2 ]; // Players gameObj
				tmp2++;

			}

		}

		List<SortMethod> list = new List<SortMethod>( );

		for( int i = 0; i < AscendingOrderElement; i++ ) {
			list.Add( new SortMethod( AscendingOrderArray[ i ], AscendingOrderArrayId[ i ] ) );

		}

		// speedでソート ( speedフィールドをソート時のキーとして OrderBy で並べ替える )
		IOrderedEnumerable<SortMethod> sorted = list.OrderBy( a => a.speed );
		// gameObj でソートする場合( gameObjフィールドをソート時のキーとして OrderBy で並べ替える )
		//IOrderedEnumerable<SortMethod> sorted = list.OrderBy( a => a.gameObj );

		foreach ( SortMethod a in sorted ) {
			AscendingOrderArrayId[ IdCnt ] = a.gameObj;
			IdCnt++;
			//Debug.Log( a.ID + " : " + a.Name );

		}
		Array.Reverse( AscendingOrderArrayId );

		return AscendingOrderArrayId;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>敵またはプレイヤーの行動処理を管理します</summary>
	private void ActionTurn( string generateGameObj ) {
		// キャラクター側の行動ターンまたは敵側の行動ターンか
		switch( generateGameObj ) {
			default : {
				//Debug.Log( "<color='red'>敵側の行動ターンです。</color>" );
				// posX, posY 敵側立ち絵が右側にスライドアウトするので右側画面外 root object を配置する
				HUD_BattleScene.ApplyStandPictureSprite( 440.0f, -37.0f, 200.0f, 150.0f, -1 ); /* 立ち絵の画像を変更します */
				PlayerAnimator.StandPictureSlide( 0 ); // 立ち絵のスライドアニメーション関数を呼ぶ

				new Attack( ).EnemyAttack( generate /* 呼び出し先で敵の名前を取得できるようにする */ ); // 敵側攻撃関数呼び出し

				break;

			}
			case "Character0" : {
				Debug.Log( "<color='red'>Character1の行動ターンです。</color>" );
				PlayerAnimator.StandPictureSlide( 1 );
				// posX, posY プレイヤー側立ち絵が左側からくるので左側画面外に root object を配置する
				HUD_BattleScene.ApplyStandPictureSprite( -700.0f, -600.0f, 200.0f, 150.0f, 0 );
				if( !UI_BattleScene.GetEnemyArrowFlg ) UI_BattleScene.ApplyCmdBtnIsActive( true ); // 攻撃・防御・・・n 選択可能状態にする
				break;

			}
			case "Character1" : {
				Debug.Log( "<color='red'>Character2の行動ターンです。</color>" );
				PlayerAnimator.StandPictureSlide( 1 );
				HUD_BattleScene.ApplyStandPictureSprite( -700.0f, -600.0f, 200.0f, 150.0f, 1 );
				if( !UI_BattleScene.GetEnemyArrowFlg ) UI_BattleScene.ApplyCmdBtnIsActive( true ); // 攻撃・防御・・・n 選択可能状態にする
				break;

			}
			case "Character2" : {
				Debug.Log( "<color='red'>Character3の行動ターンです。</color>" );
				PlayerAnimator.StandPictureSlide( 1 );
				HUD_BattleScene.ApplyStandPictureSprite( -700.0f, -600.0f, 200.0f, 150.0f, 2 );
				if( !UI_BattleScene.GetEnemyArrowFlg ) UI_BattleScene.ApplyCmdBtnIsActive( true ); // 攻撃・防御・・・n 選択可能状態にする
				break;

			}
			case "Character3" : {
				Debug.Log( "<color='red'>Character4の行動ターンです。</color>" );
				PlayerAnimator.StandPictureSlide( 1 );
				HUD_BattleScene.ApplyStandPictureSprite( -700.0f, -600.0f, 200.0f, 150.0f, 3 );
				if( !UI_BattleScene.GetEnemyArrowFlg ) UI_BattleScene.ApplyCmdBtnIsActive( true ); // 攻撃・防御・・・n 選択可能状態にする
				break;

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>キャラクター入れ替え時に行動順を再計算する時などに使います</summary>
	/// <returns>ターン中に行動するゲームオブジェクト名の配列</returns>
	public GameObject[ ] TurnReset( ) {
		return ActionTurn( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>キャラクターまたは敵の行動順をソートするときに使用するクラス</summary>
	class SortMethod {
		public int speed;
		public GameObject gameObj;

		/*===============================================================*/
		/// <summary>Accountのコンストラクター</summary>
		/// <param name="_speed">素早さ(速さ)が入ります</param>
		/// <param name="_gameObj">生成されたキャラクターと生成された敵ゲームオブジェクトが入ります</param>
		public SortMethod( int _speed, GameObject _gameObj ) {
			this.speed = _speed;
			this.gameObj = _gameObj;


		}
		/*===============================================================*/


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>戦闘中におけるステータス管理</summary>
	public class Params {
		/// <summary>ヒットポイント</summary>
		public int HP;
		/// <summary>マジックポイント</summary>
		public int MP;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>現在のプレイヤーの行動ステートを管理します</summary>
	public class CombatState : Params {
		/// <summary>ID</summary>
		public GameObject id;
		/// <summary>現在のターン数</summary>
		public int currentTurnState;
		/// <summary>行動可能かどうか</summary>
		public bool isAction;
		/// <summary>入れ替え可能かどうか</summary>
		public bool isChange;
		/// <summary>Playerの生存状態</summary>
		public bool playerIsDead;
		/// <summary>敵の生存状態</summary>
		public bool enemyIsDead;

		/*===============================================================*/
		/// <summary>コンストラクター</summary>
		public CombatState( GameObject _id, int _currentTurnState, bool _isAction, bool _isChange, bool _playerIsDead, bool _enemyIsDead ) {
			id = _id;
			currentTurnState = _currentTurnState;
			isAction = _isAction;
			isChange = _isChange;
			playerIsDead = _playerIsDead;
			enemyIsDead = _enemyIsDead;


		} 
		/*===============================================================*/


	}
	/*===============================================================*/


}
/*===============================================================*/