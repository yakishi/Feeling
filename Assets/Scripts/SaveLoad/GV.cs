using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ゲーム共通用のクラス
/// ロード時に値が入る予定
/// </summary>
/// <remarks>
/// クラスが参照されたタイミングでインスタンスが生成されます
/// Singlton を採用
/// </remarks>
public sealed class GV
{
	#region Classes

	private static GV mInstance = new GV( );
	/// <summary>Singlton</summary>
	public static GV Instance { get { return mInstance; } }

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	private GV( ) {
		Debug.Log( "Create GV Instance." );

		// データのロードを行う ( 前回のデータ保持 )
		GV.load( );

		// 毎回 new すると前のセーブデータが消えるので一回だけ new する
		gameData = new GameData( );

		// プレイヤー 6 人数分のパラメータの作成
		gameData.Players = new List<PlayerParam>( );
		for( int player = 0; player < 6; player++ ) {
			gameData.Players.Add( new PlayerParam( ) );

		}


	}
	/*===============================================================*/

    /// <summary>
    /// プレイヤーの情報
    /// 変動する情報を保存用
    /// </summary>
    [Serializable]
    public class PlayerParam
    {
        public int ID;
        public int Lv;
        public int HP;
        public int MP;
        public int Atk;
        public int Def;
        public int Int;
        public int Mgr;
        public int Agl;
        public int Luc;
        public int StatusPoint;
        public int[] currentEquipment = new int[6];
    }

    /// <summary>
    /// Save するゲームデータ
    /// </summary>
    [Serializable]
    public class GameData
    {
        #region Other
        public int playTime;
        #endregion

        #region Player
        public List<PlayerParam> Players;
        public int PartyCount;
        #endregion

        #region Items
        // 所持している装備のIDの配列
        public List<int> Equipments;
        #endregion
    }

    // ゲームデータを読み込むときは先にセーブデータのロードまたはnewGame で関数を初期化して使用
    static GameData gameData;
    static public GameData GData 
    {
        get {
            return gameData;
        }
    }

    [Serializable]
    public class SystemData
    {
        public bool[] usedSave;
        public SystemData()
        {
            usedSave = new bool[SaveData.SaveSlotCount];
        }
    }

    static SystemData systemData;
    static public SystemData SData
    {
        get {
            if (systemData == null)
            {
                gameAwake();
            }
            return systemData;
        }
    }

    #endregion

    #region Properties
    #endregion

    /// <summary>
    /// GameData をセーブする
    /// </summary>
    /// <param name="slot">セーブするスロット(0はシステムデータ保存用)</param>
    static public void save(int slot = 1)
    {
        //gameData = new GameData();
        gameData.playTime = 20;

       // gameData.Players = new List<PlayerParam>();
		//var player = new PlayerParam();
		//gameData.Players.Add(player);
		SData.usedSave[slot - 1] = true;

        SaveData.setSlot(0);
        SaveData.setClass("SystemData", SData);
        SaveData.save();

        SaveData.setSlot(slot);
        SaveData.setClass("GameData", GData);
        SaveData.save();
    }

    static public void load(int slot = 1)
    {
        SaveData.setSlot(slot);
        SaveData.load();
        gameData = SaveData.getClass<GameData>("GameData", null);
    }

    /// <summary>
    /// ゲーム起動時に呼び出し
    /// </summary>
    static public void gameAwake()
    {
        // SystemDataの読み込み
        SaveData.setSlot(0);
        SaveData.load();
        systemData = SaveData.getClass<SystemData>("SystemData", null);
        if (systemData == null) {
            systemData = new SystemData();
        }
    }

    /// <summary>
    /// ゲームを新しく始めるときに呼び出される関数
    /// </summary>
    static public void newGame()
    {

       // gameData = new GameData();

		// 装備の仮データ読み込み
		{
            //gameData.Equipments = new List<int>();
            // 仮で全装備を ID + 1 所持している状態にする
            /*var equipments = EquipmentManager.getEquipmentList();
            foreach (var equipment in equipments) {
                for (int i = 0; i <= equipment.ID; ++i) {
                    gameData.Equipments.Add(equipment.ID);
                }
            }*/
        }

        // プレイヤーの仮作成
        {
            //gameData.Players = new List<PlayerParam>();
			//for( int players = 0; players < 6; players++ ) gameData.Players.Add( gameData.Players[ players ] );
            for( int i = 0; i < gameData.Players.Count; i++ ) {
				gameData.Players[ i ]/*newPlayer*/.ID = 0;
				gameData.Players[ i ]/*newPlayer*/.Lv = 0;
				gameData.Players[ i ]/*newPlayer*/.HP = 0;
				gameData.Players[ i ]/*newPlayer*/.MP = 0;
				gameData.Players[ i ]/*newPlayer*/.Atk = 0;
				gameData.Players[ i ]/*newPlayer*/.Def = 0;
				gameData.Players[ i ]/*newPlayer*/.Int = 0;
				gameData.Players[ i ]/*newPlayer*/.Mgr = 0;
				gameData.Players[ i ]/*newPlayer*/.Agl = 0;
				gameData.Players[ i ]/*newPlayer*/.Luc = 0;
				gameData.Players[ i ]/*newPlayer*/.StatusPoint = 0;

				//gameData.Players.Add( gameData.Players[ i ] );
            }

        }


    }

	/*===============================================================*/
	/// <summary>セーブデータに保存されているKEYのデバッグ出力を行います</summary>
	public static void DebugKeyPrint( ) {
		foreach( string items in SaveData.getKeys( ) ) {
			Debug.Log( "------------------\nセーブデータキー :\n" + items + "\n------------------" );

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤーID:セーブデータキー定義</summary>
	public class PlayerID {
		public const string P1 = "0";
		public const string P2 = "1";
		public const string P3 = "2";
		public const string P4 = "3";
		public const string P5 = "4";
		public const string P6 = "5";


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤーステータス:セーブデータキー定義</summary>
	public class SaveDataKey {
		public const string ID = "_Players";
		public const string LV = "_Player_Lv";
		public const string HP = "_Player_HP";
		public const string MP = "_Player_MP";
		public const string ATK = "_Player_Atk";
		public const string DEF = "_Player_Def";
		public const string INT = "_Player_Int";
		public const string MGR = "_Player_Mgr";
		public const string AGR = "_Player_Agl";
		public const string LUC = "_Player_Luc";
		public const string STATUSPOCONST = "_Player_StatusPoconst";
		public const string CURRENTEQUIPMENT = "_Player_currentEquipment";
		public const string KEY_TIME = "TIME";


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>セットした変動値をセーブデータから読み込みます</summary>
	/// <remarks>コンティニューなどに使います</remarks>
	static public void GameDataLoad( ) {

		//GV.load( );

		//gameData = new GameData( );
		//gameData.Players = new List<PlayerParam>( );

		for ( int i = 0; i < gameData.Players.Count; i++ ) {
			//PlayerParam newPlayer = new PlayerParam( );
			//GV.GData.Players[ i ].ID = ( int )SaveData.getInt( i.ToString( ) + GV.SaveDataKey.ID, 0 );
			//GV.GData.Players[ i ].Lv = ( int )SaveData.getInt( i.ToString( ) + GV.SaveDataKey.LV, 0 );
			gameData.Players[ i ].HP = ( int )SaveData.getInt( i.ToString( ) + GV.SaveDataKey.HP, -1 );
			gameData.Players[ i ].MP = ( int )SaveData.getInt( i.ToString( ) + GV.SaveDataKey.MP, -1 );
			//newPlayer.Atk = ( i + 1 ) * 4/*PlayerManagerCSV.GetPlayers[ i ].ATK*/;
			//newPlayer.Def = ( i + 1 ) * 5/*PlayerManagerCSV.GetPlayers[ i ].DEF*/;
			//newPlayer.Int = ( i + 1 ) * 6/*PlayerManagerCSV.GetPlayers[ i ].INT*/;
			//newPlayer.Mgr = ( i + 1 ) * 7/*PlayerManagerCSV.GetPlayers[ i ].MDEF*/;
			//newPlayer.Agl = ( i + 1 ) * 8/*PlayerManagerCSV.GetPlayers[ i ].SPD*/;
			//newPlayer.Luc = ( i + 1 ) * 9/*PlayerManagerCSV.GetPlayers[ i ].LUCKY*/;
			//newPlayer.StatusPoint = 5; // 経験値が溜まってレベルがあがると5ポイントのステータスポイントが獲得できる。この5ポイントは固定

			//gameData.Players.Add( newPlayer );

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤーのヒットポイント変動値を保存します</summary>
	/// <param name="playerIndex">プレイヤー0～5で指定します</param>
	/// <param name="setHp">保存したいヒットポイント値を指定します</param>
	static public void PlayersSetHPSave( int playerIndex, int setHp ) {
		if( playerIndex < gameData.Players.Count ) {
			// key がある場合, 最初にロードする ( これを行わないと今までの Key が全て無くなる )
			foreach( string items in SaveData.getKeys( ) ) if( items != null ) GV.load( );
			// ID + KEY
			SaveData.setInt( playerIndex + GV.SaveDataKey.HP, setHp );
			SaveData.setString( GV.SaveDataKey.KEY_TIME, System.DateTime.Now.ToBinary( ).ToString( ) );
			GV.save( );

		} else if( playerIndex >= gameData.Players.Count ) Debug.LogError( "プレイヤーは、6人です。0～5で指定して下さい。" );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤーのマジックポイント変動値を保存します</summary>
	/// <param name="playerIndex">プレイヤー0～5で指定します</param>
	/// <param name="setHp">保存したいマジックポイント値を指定します</param>
	static public void PlayersSetMPSave( int playerIndex, int setMp ) {
		if( playerIndex < gameData.Players.Count ) {
			// key がある場合, 最初にロードする ( これを行わないと今までの Key が全て無くなる )
			foreach( string items in SaveData.getKeys( ) ) if( items != null ) GV.load( );
			// ID + KEY
			SaveData.setInt( playerIndex + GV.SaveDataKey.MP, setMp );
			SaveData.setString( GV.SaveDataKey.KEY_TIME, System.DateTime.Now.ToBinary( ).ToString( ) );
			GV.save( );

		} else if( playerIndex >= gameData.Players.Count ) Debug.LogError( "プレイヤーは、6人です。0～5で指定して下さい。" );


	}
	/*===============================================================*/


}