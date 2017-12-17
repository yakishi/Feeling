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
public sealed class GV {
	#region Classes

	////////////////////////////////////////////
	int saveLoadSlot = 1; // 初期セーブスロット
	/// <summary>Title.csのsaveおよびloadでのみ値セットするものとする</summary>
	public int slot { get { return saveLoadSlot; } set { saveLoadSlot = value; } }
	///////////////////////////////////////////

	static public DateTime[ ] oldPlayTime;

	private const int PLAYERS = 6;

	private static GV mInstance = new GV( );
	/// <summary>Singlton</summary>
	public static GV Instance { get { return mInstance; } }

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	private GV( ) {
		Debug.Log( "Create GV Instance." );

		// インスタンスの作成
		// 毎回 new すると前のセーブデータが消えるので一回だけ new する
		gameData = new GameData( );

		gameData.Players = new List<PlayerParam>( );
		gameData.playTime = new string[ 4 ];
		oldPlayTime = new DateTime[ 4 ];

		gameData.Equipments = new List<EquipmentParam>( );

		for( int i = 0; i < PLAYERS /* !変更しない! */; i++ ) {
			gameData.Players.Add( new PlayerParam( ) );
			gameData.Equipments.Add( new EquipmentParam( ) );

		}

		// データのロードを行う ( 前回のデータ保持 )
		GameDataLoad( slot );


	}
	/*===============================================================*/

	/// <summary>
	/// Save するゲームデータ
	/// </summary>
	[Serializable]
	public class GameData {
		#region Other
		public string[ ] playTime;
		#endregion

		#region Player
		public List<PlayerParam> Players;
		//public int PartyCount;
		#endregion

		#region Items
		// 所持している装備のIDの配列
		//public List<int> Equipments;
		public List<EquipmentParam> Equipments;
		#endregion
	}

	// ゲームデータを読み込むときは先にセーブデータのロードまたはnewGame で関数を初期化して使用
	private GameData gameData;
	public GameData GData { get { return gameData; } }

	[Serializable]
	public class SystemData {
		public bool[ ] usedSave;
		public SystemData( ) {
			usedSave = new bool[ SaveData.SaveSlotCount ];
		}
	}

	private SystemData systemData;
	public SystemData SData {
		get {
			if ( systemData == null ) {
				gameAwake( );
			}
			return systemData;
		}
	}

	#endregion

	#region Properties
	#endregion

	/// <summary>
	/// ゲーム起動時に呼び出し
	/// </summary>
	public void gameAwake( ) {
		// SystemDataの読み込み
		SaveData.setSlot( 0 );
		SaveData.load( );
		systemData = SaveData.getClass<SystemData>( "SystemData", null );
		if ( systemData == null ) {
			systemData = new SystemData( );
		}
	}

	/// <summary>
	/// ゲームを新しく始めるときに呼び出される関数
	/// </summary>
	public void newGame( ) {

		Debug.Log( "<color='red'>newGame Function Called.</color>" );

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
		
		UpdatePlayTime( );


	}

	/*===============================================================*/
	/// <summary>各,セーブスロットのプレイ時間更新を行います</summary>
	/// <remarks>
	/// newGame()または,saveで呼びます
	/// </remarks>
	private void UpdatePlayTime( ) {
		Debug.Log( "<color='red'>UpdatePlayTime Function Called.</color>" );

		Debug.Log( "<color='red'>セーブスロット : " + slot + "</color>" );
		// slot 別の初期時間保存処理
		if( SaveData.getString( slot + SaveDataKey.KEY_TIME_OLD, "-1" ) == "-1" ) {
			SaveData.setString( slot + SaveDataKey.KEY_TIME_OLD, System.DateTime.Now.ToString( ) );
			SaveData.setSlot( slot );
			SaveData.save( );
			Debug.Log( "<color='red'>oldPlayTime[ " + slot + " ] : " + System.DateTime.Now + ", で保存されました。</color>" );
			oldPlayTime[ slot ] = System.DateTime.Now;

		} else if( SaveData.getString( slot + SaveDataKey.KEY_TIME_OLD, "-1" ) != "-1" ) {
			foreach( string items in SaveData.getKeys( ) ) {
				if( items == slot + SaveDataKey.KEY_TIME_OLD ) {
					oldPlayTime[ slot ] = DateTime.Parse( SaveData.getString( slot + SaveDataKey.KEY_TIME_OLD, "-1" ) );
					Debug.Log( "<color='red'>初期時間が更新されました。</color>" );

				}

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>TitleでのLoad時にパラメーターを読み込みます</summary>
	/// <param name="slotIndex">ロードするスロット番号</param>
	public void SlotChangeParamUpdate( int slotIndex ) {
		Debug.Log( "<color='red'>各種パラメーターが更新されました。</color>" );
		SingltonPlayerManager.Instance.LoadPlayer( slotIndex ); // slot を変えたときに値を更新する
		SingltonEquipmentManager.Instance.LoadEquipment( slotIndex );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>セーブデータに保存されているKEYのデバッグ出力を行います</summary>
	public void DebugKeyPrint( ) {
		foreach ( string items in SaveData.getKeys( ) ) {
			Debug.Log( "------------------\nセーブデータキー :\n" + items + "\n------------------" );

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>セーブデータキー定義</summary>
	private class SaveDataKey {
		public const string PRAYER_PARAM = "PLAYER_PARAM";
		public const string EQUIPMENT_PARAM = "EQUIPMENT_PARAM";
		public const string ITEM_PARAM = "ITEM_PARAM";
		public const string KEY_TIME = "KEY_TIME";
		public const string KEY_TIME_OLD = "KEY_TIME_OLD";


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>セットした変動値をセーブデータから読み込みます</summary>
	/// <remarks>
	/// コンティニューなどに使います
	/// (注意) gamemaneger classなどで最初にsave data を扱う,player singlton class および
	/// equip singlton class のインスタンスを作成しておかないと各,singlton class
	/// のloadで前の値が読み込まれる可能性があるので一括で作成するようにします。
	/// </remarks>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void GameDataLoad( int loadSlot ) {

		SaveData.setSlot( loadSlot );
		SaveData.load( );

		/*===============================================================*/
		// プレイヤーステータス情報読込
		List<SingltonPlayerManager.PlayerParameters> myPlayerState = SaveData.getList<SingltonPlayerManager.PlayerParameters>( GV.SaveDataKey.PRAYER_PARAM );

		for ( int i = 0; i < gameData.Players.Count; i++ ) {
			if ( myPlayerState != null ) {
				gameData.Players[ i ].ID = myPlayerState[ i ].ID;
				gameData.Players[ i ].Lv = myPlayerState[ i ].Lv;
				gameData.Players[ i ].HP = myPlayerState[ i ].HP;
				gameData.Players[ i ].MP = myPlayerState[ i ].MP;
				gameData.Players[ i ].Atk = myPlayerState[ i ].Atk;
				gameData.Players[ i ].Def = myPlayerState[ i ].Def;
				gameData.Players[ i ].Int = myPlayerState[ i ].Int;
				gameData.Players[ i ].Mgr = myPlayerState[ i ].Mgr;
				gameData.Players[ i ].Agl = myPlayerState[ i ].Agl;
				gameData.Players[ i ].Luc = myPlayerState[ i ].Luc;
				gameData.Players[ i ].StatusPoint = myPlayerState[ i ].StatusPoint; // 経験値が溜まってレベルがあがると5ポイントのステータスポイントが獲得できる。この5ポイントは固定

			}

		}
		/*===============================================================*/

		/*===============================================================*/
		// 装備情報読込
		List<SingltonEquipmentManager.PlayerEquipmentParam> myEquip = SaveData.getList<SingltonEquipmentManager.PlayerEquipmentParam>( GV.SaveDataKey.EQUIPMENT_PARAM );

		for( int i = 0; i < gameData.Equipments.Count; i++ ) {
			if ( myEquip != null ) {
				gameData.Equipments[ i ].ID = myEquip[ i ].ID;
				gameData.Equipments[ i ].Arms = myEquip[ i ].Arms;
				gameData.Equipments[ i ].Head = myEquip[ i ].Head;
				gameData.Equipments[ i ].Shoes = myEquip[ i ].Shoes;
				gameData.Equipments[ i ].Accessory1 = myEquip[ i ].Accessory1;
				gameData.Equipments[ i ].Accessory2 = myEquip[ i ].Accessory2;

			}

		}
		/*===============================================================*/

		/*===============================================================*/
		// アイテム情報読込

		/*===============================================================*/

		// セーブスロット + キーでプレイ時間を読み込む
		if ( loadSlot > 0 ) gameData.playTime[ loadSlot ] = SaveData.getString( loadSlot + SaveDataKey.KEY_TIME );

		Debug.Log( loadSlot + ", " + saveLoadSlot );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>セーブを行います</summary>
	/// <param name="saveSlot">セーブするセーブデータスロットを指定します</param>
	public void GameDataSave( int saveSlot ) {

		SData.usedSave[ saveSlot - 1 ] = true;

		SaveData.setSlot( 0 );
		SaveData.setClass( "SystemData", SData );
		SaveData.save( );

		SaveData.setSlot( saveSlot );

		/*===============================================================*/
		// セーブデータへの値セット部 START

		SaveData.setList( SaveDataKey.PRAYER_PARAM, SingltonPlayerManager.Instance.SaveDataPlayerState ); // プレイヤーパラメーター
		SaveData.setList( SaveDataKey.EQUIPMENT_PARAM, SingltonEquipmentManager.Instance.SaveDataPlayerEquipmentParam ); // プレイヤー装備ーパラメーター

		// セーブデータへの値セット部 END
		/*===============================================================*/
		
		UpdatePlayTime( );
		SaveData.setString( saveSlot + SaveDataKey.KEY_TIME, ( oldPlayTime[ saveSlot ] - System.DateTime.Now ).Duration( ).ToString( ) );

		SaveData.save( );

		Debug.Log( saveSlot + ", " + saveLoadSlot );


	}
	/*===============================================================*/

	/*===============================================================*/
	// パラメーター部 START

	/// <summary>
	/// プレイヤーの情報
	/// 変動する情報を保存用
	/// </summary>
	[Serializable]
	public class PlayerParam {
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
		public int[ ] currentEquipment = new int[ 6 ];


	}

	/// <summary>
	/// 装備情報
	/// 変動する情報を保存用
	/// </summary>
	[Serializable]
	public class EquipmentParam {
		/// <summary>プレイヤー識別ID(0からの連番で管理)</summary>
		public int ID;
		/// <summary>武器(装備名)</summary>
		public string Arms;
		/// <summary>頭(装備名)</summary>
		public string Head;
		/// <summary>足(装備名)</summary>
		public string Shoes;
		/// <summary>アクセサリー1(装備名)</summary>
		public string Accessory1;
		/// <summary>アクセサリー2(装備名)</summary>
		public string Accessory2;


	}

	// パラメーター部 END
	/*===============================================================*/


}