using System;
using System.Collections.Generic;
using UniRx;
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

		gameData.Equipments = new List<EquipmentParam>( );

		gameData.PlayersSkills = new List<SkillParam>( );

		gameData.Items = new ItemParam( );
		gameData.Items.Name = new List<string>( );
		gameData.Items.Stock = new List<int>( );

		for( int i = 0; i < PLAYERS /* !変更しない! */; i++ ) {
			gameData.Players.Add( new PlayerParam( ) );
			gameData.Equipments.Add( new EquipmentParam( ) );
			gameData.PlayersSkills.Add( new SkillParam( ) );
			gameData.PlayersSkills[ i ].Name = new List<string>( );

		}

		// データのロードを行う ( 前回のデータ保持 )
		GameDataLoad( slot );

		// 一秒ごとに, タイムカウントアップします
		Observable
			.Interval( TimeSpan.FromSeconds( 1 ) )
			.Subscribe( x => {
				TimeCountUp( );
			}
		);



	}
	/*===============================================================*/

	/// <summary>
	/// Save するゲームデータ
	/// </summary>
	[Serializable]
	public class GameData {
		#region Other
		/// <summary>プレイ時間[秒]</summary>
		/// <remarks>
		/// TimeSpan example = new TimeSpan( 0, 0, timeSecond );
		/// 上の様に変換しプレイ時間を取得します
		/// </remarks>
		public int timeSecond;
		#endregion

		#region Player
		public List<PlayerParam> Players;
		public List<SkillParam> PlayersSkills;
		//public int PartyCount;
		#endregion

		#region Items
		// 所持している装備のIDの配列
		//public List<int> Equipments;
		public List<EquipmentParam> Equipments;
		/// <summary>現在所持しているアイテム一覧</summary>
		public ItemParam Items;
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
	/// <remarks>Title.csのnewGameButton.OnClickAsObservable()から呼ばれる</remarks>
	public void newGame( ) {

		Debug.Log( "<color='red'>newGame Function Called.</color>" );

		// save data file delete, save prefab で任意のセーブスロットを選択すると
		// 選択されたファイルが一旦, 削除される, その後, 初期化されたデータで保存される
		SaveData.remove( slot );

		// save data を扱う各, singlton class の初期化処理
		// player
		SingltonPlayerManager.Instance.SaveDataPlayerState.Clear( );
		SingltonPlayerManager.Instance.NewGame( );
		// equip
		SingltonEquipmentManager.Instance.SaveDataPlayerEquipmentParam.Clear( );
		SingltonEquipmentManager.Instance.NewGame( );
		// item
		SingltonItemManager.Instance.SDItem.Name.Clear( );
		SingltonItemManager.Instance.NewGame( );
		// skill
		SingltonSkillManager.Instance.SDSkill.Clear( );
		SingltonSkillManager.Instance.NewGame( );
		// time
		gameData.timeSecond = 0;

		GameDataSave( slot );


	}

	/*===============================================================*/
	/// <summary>TitleでのLoad時にパラメーターを読み込みます</summary>
	/// <param name="slotIndex">ロードするスロット番号</param>
	public void SlotChangeParamUpdate( int slotIndex ) {
		Debug.Log( "<color='red'>各種パラメーターが更新されました。</color>" );
		SingltonPlayerManager.Instance.LoadPlayer( slotIndex ); // slot を変えたときに値を更新する
		SingltonEquipmentManager.Instance.LoadEquipment( slotIndex );
		SingltonSkillManager.Instance.LoadSkill( slotIndex );
		SingltonItemManager.Instance.LoadItem( slotIndex );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>時間をカウントアップします</summary>
	private void TimeCountUp( ) {
		//myTimeSecond = SaveData.getInt( slot + SaveDataKey.KEY_CURRENT_TIME, -1 );
		gameData.timeSecond += 1; // 一秒づつカウント
		//TimeSpan newTime = new TimeSpan( 0, 0, gameData.timeSecond[ slot ] );
		//Debug.Log( "<color='red'>newTime : " + newTime + ", myTimeSecond : " + gameData.timeSecond[ slot ] + " </color>" );
		if( gameData.timeSecond == -1 ) Debug.LogError( "TimeCountUp Function on the Error." );


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
	public class SaveDataKey {
		public const string PRAYER_PARAM = "PLAYER_PARAM";
		public const string EQUIPMENT_PARAM = "EQUIPMENT_PARAM";
		public const string ITEM_PARAM = "ITEM_PARAM";
		public const string SKILL_PARAM = "SKILL_PARAM";
		public const string KEY_CURRENT_TIME = "KEY_CURRENT_TIME";


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>セットした変動値をセーブデータから読み込みます</summary>
	/// <remarks>
	/// コンティニューなどに使います
	/// (注意) gamemaneger class などで最初に save data を扱う,player singlton class や
	/// equip singlton class などのインスタンスを作成しておかないと各,singlton class
	/// のloadで前の値が読み込まれる可能性があるので gamemanager class に一括で作成するようにします。
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
		SingltonItemManager.ItemParam myItem = SaveData.getClass<SingltonItemManager.ItemParam>( GV.SaveDataKey.ITEM_PARAM );

		if( myItem != null ) {
			gameData.Items.Name = myItem.Name;
			gameData.Items.Stock = myItem.Stock;

		}

		/*===============================================================*/

		/*===============================================================*/
		// スキル情報読込
		List<SingltonSkillManager.SkillParam> mySkill = SaveData.getList<SingltonSkillManager.SkillParam>( GV.SaveDataKey.SKILL_PARAM );

		for( int i = 0; i < gameData.PlayersSkills.Count; i++ ) {
			if ( mySkill != null ) {
				gameData.PlayersSkills[ i ].Name = mySkill[ i ].Name;

			}

		}
		/*===============================================================*/

		// ロードスロット + キーでプレイ時間を読み込む
		if ( loadSlot > 0 ) {
			gameData.timeSecond = SaveData.getInt( SaveDataKey.KEY_CURRENT_TIME, 0 );

		}

		//TimeSpan t = new TimeSpan( 0, 0, gameData.timeSecond[ loadSlot ] );
		//Debug.Log( "<color='red'>" + loadSlot + ", " + saveLoadSlot + ", playTime : " + t + "</color>" );


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
		SaveData.setList( SaveDataKey.EQUIPMENT_PARAM, SingltonEquipmentManager.Instance.SaveDataPlayerEquipmentParam ); // プレイヤー装備パラメーター
		SaveData.setList( SaveDataKey.SKILL_PARAM, SingltonSkillManager.Instance.SDSkill ); // プレイヤースキルパラメーター
		SaveData.setClass( SaveDataKey.ITEM_PARAM, SingltonItemManager.Instance.SDItem ); // アイテムパラメーター

		// セーブデータへの値セット部 END
		/*===============================================================*/

		SaveData.setInt( SaveDataKey.KEY_CURRENT_TIME, gameData.timeSecond );

		SaveData.save( );

		//TimeSpan t = new TimeSpan( 0, 0, gameData.timeSecond[ saveSlot ] );
		//Debug.Log( "<color='red'>" + saveSlot + ", " + saveLoadSlot + ", playTime : " + t + "</color>" );


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

	/// <summary>
	/// スキル情報
	/// </summary>
	[Serializable]
	public class SkillParam {
		/// <summary>現在覚えているスキルの名前</summary>
		public List<string> Name;


	}

	/// <summary>
	/// アイテム情報
	/// </summary>
	public class ItemParam {
		/// <summary>現在所持しているアイテム一覧</summary>
		public List<string> Name;
		/// <summary>現在所持しているアイテムに対するアイテム所持数</summary>
		public List<int> Stock;


	}

	// パラメーター部 END
	/*===============================================================*/


}