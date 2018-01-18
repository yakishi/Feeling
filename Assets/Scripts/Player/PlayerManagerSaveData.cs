using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>SaveDataからのPlayerステータス情報を管理します</summary>
public class PlayerManagerSaveData {

	/*===============================================================*/
	/// <summary>player 変動値</summary>
	public class State {
		/// <summary>ID</summary>
		public int ID;
		/// <summary>レベル</summary>
		public int Lv;
		/// <summary>ヒットポイント</summary>
		public int HP;
		/// <summary>マジックポイント</summary>
		public int MP;
		/// <summary>攻撃力</summary>
		public int Atk;
		/// <summary>防御力</summary>
		public int Def;
		/// <summary>魔法攻撃力</summary>
		public int Matk;
		/// <summary>魔法防御</summary>
		public int Mgr;
		/// <summary>素早さ</summary>
		//public int Spd; Agl に移行・・・
		/// <summary>運</summary>
		public int Luc;
		/// <summary>回避率</summary>
		public int Agl;
		/// <summary>知性,知力</summary>
		public int Int;
		/// <summary>感情</summary>
		public string Feeling;
		/// <summary>スキル</summary>
		public string Skill;
		/// <summary>ステータスポイント</summary>
		public int StatusPoint;
		/// <summary>オーバードライブ</summary>
		public string OverDrive;
		/// <summary>手装備01</summary>
		public string WEAPON01;
		/// <summary>手装備02</summary>
		public string WEAPON02;


	}
	/*===============================================================*/

	public class Character {
		#region Load from SaveData
		protected List<State> state = new List<State>( );
		/// <summary>現在のプレイヤーステータスをgetします</summary>
		public List<State> CurrentState { get { return state; } }


	}

	//Equipment equipment;
	//Skill[ ] skills;
	#endregion

	#region Load from GameData
	//Equipment canEquip;
	#endregion

	//virtual void action( ) {
	//
	//}

	public class Player : Character {
		public int level { private set; get; }
		private int currentHP; // 現在の HP
		//private State state;
		private bool isDead; //加入可能か

		//void override action( ) {
		//}

		void statusAllocation( ) {
		}

		void changeEquipment( ) {
		}


	}

	/*===============================================================*/
	/// <summary>SaveLoad機能からプレイヤー情報を管理する</summary>
	public class PlayerManager : Player {

		GV myGV = GV.Instance;

		/*===============================================================*/
		/// <summary>コンストラクタ</summary>
		public PlayerManager( ) {
			Debug.Log( "Player Manager Class on the Constructor Function Call." );
			// 人数分の player を生成する
			for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
				state.Add( new State( ) );

			}


		}
		/*===============================================================*/

		/*===============================================================*/
		/// <summary>GV.PlayerParamからのデータ読込</summary>
		public void LoadPlayer( ) {
			int cnt = 0; // foreach カウント用変数
			foreach ( GV.PlayerParam item in myGV.GData.Players ) {
				// item は GV.newGame( ) でファイルから読み込んでくるようですが・・・
				// 現状, テキトウな値で初期化されています
				// GV.PlayerParam に定義されているメンバ変数を players に入れていく
				state[ cnt ].ID = /*item.ID*/-1;
				state[ cnt ].Lv = item.Lv;
				state[ cnt ].HP = item.HP;
				state[ cnt ].MP = item.MP;
				state[ cnt ].Atk = item.Atk;
				state[ cnt ].Def = item.Def;
				state[ cnt ].Matk = ( cnt + 1 ) * 1; // item に存在しないのでテキトウな値を入れる
				state[ cnt ].Mgr = item.Mgr;
				state[ cnt ].Agl = item.Agl;
				state[ cnt ].Luc = item.Luc;
				state[ cnt ].Feeling = "about"; // item に存在しないのでテキトウな値を入れる
				state[ cnt ].Skill = "about"; // item に存在しないのでテキトウな値を入れる
				state[ cnt ].StatusPoint = /*item.StatusPoint*/-1;
				state[ cnt ].OverDrive = "about"; // item に存在しないのでテキトウな値を入れる
				state[ cnt ].WEAPON01 = "about"; // item に存在しないのでテキトウな値を入れる
				state[ cnt ].WEAPON02 = "about"; // item に存在しないのでテキトウな値を入れる
				cnt++;

			}


		}
		/*===============================================================*/


		public void InitializePlayerData( ) {
		}

		/*===============================================================*/
		/// <summary>戦闘シーンから生存しているか否かgetします</summary>
		/// <remarks>仮の実装です</remarks>
		/// <param name="index">0:Player1,1:Player2,2:Player3,3:Player4で指定します</param>
		/// <returns>Playerの生存状態</returns>
		public bool GetIsDead( int index ) {
			return Combat.GetCombatState[ index ].playerIsDead;

		}
		/*===============================================================*/


	}
	/*===============================================================*/


}
/*===============================================================*/