using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>スレッドセーフ:PlayerManager</summary>
/// <remarks> クラスが参照されたタイミングでインスタンスが生成されます</remarks>
public sealed class SingltonPlayerManager {

	//////////////////////////////////////////////////////////
	//	TODO
	//	エネミー, 装備, プレイヤーなどのHPやMPをクラスのメンバとして持っておく
	//	ID と Key で読み込んでくる共通した機能・・・
	//	シングルトンパターンで作りこみ
	//	https://qiita.com/calmbooks/items/9cf32c6dd36b724b155e
	//	Save は, 最後に呼ぶ slot 指定
	/////////////////////////////////////////////////////////

	private static SingltonPlayerManager mInstance = new SingltonPlayerManager( );
	private PlayerParameters[ , ] PlayerArray;
	private List<PlayerParameters> PlayerSaveList;
	private List<PlayerParameters> PlayerCsvList;

	GV myGV;

	/// <summary>セーブデータから読み込まれたプレイヤーパラメーターを取得またはセット</summary>
	public List<PlayerParameters> SaveDataPlayerState { get { return PlayerSaveList; } set { PlayerSaveList = value; } }
	/// <summary>CSVから読み込まれた敵パラメーターを取得します</summary>
	public List<PlayerParameters> GetCsvDataPlayerState { get { return PlayerCsvList; } }
	/// <summary>Singlton</summary>
	public static SingltonPlayerManager Instance { get { return mInstance; } }

	/// <summary>CSVのデータとキーを格納する構造体</summary>
	struct CSVDATA {
		public string[ ] key;
		public string[ ] data;

	} CSVDATA[ ] myCsvData;

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	private SingltonPlayerManager( ) {
		Debug.Log( "Create SingltonPlayerManager Instance." );

		PlayerSaveList = new List<PlayerParameters>( );
		PlayerCsvList = new List<PlayerParameters>( );

		PlayerCreate( ); // プレイヤーパラメーターの作成を行います


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤーのパラメーター情報をなどを作ります</summary>
	private void PlayerCreate( ) {

		myGV = GV.Instance; // Save/Load クラスのインスタンスを取得
		myGV.newGame( ); // 必ず new Game 下後に変動値などの保存を行う

		// 人数分の player を生成する
		for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			PlayerSaveList.Add( new PlayerParameters( ) );

		}

		LoadPlayer( GV.slot ); // セーブデータの読込

		// 配列確保およびプレイヤー分用意
		myCsvData = new CSVDATA[ myGV.GData.Players.Count ];
		for ( int players = 0; players < myGV.GData.Players.Count; players++ ) {
			myCsvData[ players ] = new CSVDATA( );
			myCsvData[ players ].data = new string[ 512 ];
			myCsvData[ players ].key = new string[ 512 ];

		}

		LoadCsvPlayer( ); // CSV からのデータ読込


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>GV.PlayerParamからのデータ読込</summary>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void LoadPlayer( int loadSlot ) {

		myGV.GameDataLoad( loadSlot ); // セーブデータからの保存したデータを読み込みます

		int cnt = 0; // foreach カウント用変数
		
		if( myGV.GData != null ) {
			// 読み込んだ値を入れていきます
			foreach ( GV.PlayerParam item in myGV.GData.Players ) {
				// GV.PlayerParam に定義されているメンバ変数を players に入れていく
				PlayerSaveList[ cnt ].ID = item.ID;
				PlayerSaveList[ cnt ].Lv = item.Lv;
				PlayerSaveList[ cnt ].NextLvExp = -1; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].HP = item.HP;
				PlayerSaveList[ cnt ].MP = item.MP;
				PlayerSaveList[ cnt ].Atk = item.Atk;
				PlayerSaveList[ cnt ].WeaponAtk = -1; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].Def = item.Def;
				PlayerSaveList[ cnt ].EquipmentDef = -1; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].Matk = -1; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].Mgr = item.Mgr;
				PlayerSaveList[ cnt ].Agl = item.Agl;
				PlayerSaveList[ cnt ].Luc = item.Luc;
				PlayerSaveList[ cnt ].Int = item.Int;
				PlayerSaveList[ cnt ].Feeling = "about"; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].FeelingValue = -1; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].Skill = "about"; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].StatusPoint = item.StatusPoint;
				PlayerSaveList[ cnt ].OverDrive = "about"; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].WEAPON01 = "about"; // item に存在しないのでテキトウな値を入れる
				PlayerSaveList[ cnt ].WEAPON02 = "about"; // item に存在しないのでテキトウな値を入れる
				cnt++;

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>CSVからのデータ読込とListにセットする</summary>
	private void LoadCsvPlayer( ) {
		// CSV ファイルからのキーとデータの読込
		CSVLoader myLoader = new CSVLoader( );
		myCsvData[ 0 ].data = myLoader.GetCSV_Key_Record( "CSV/PlayerStatus/CSV_Player1_LvOrder", myCsvData[ 0 ].key );
		myCsvData[ 1 ].data = myLoader.GetCSV_Key_Record( "CSV/PlayerStatus/CSV_Player2_LvOrder", myCsvData[ 1 ].key );
		myCsvData[ 2 ].data = myLoader.GetCSV_Key_Record( "CSV/PlayerStatus/CSV_Player3_LvOrder", myCsvData[ 2 ].key );
		myCsvData[ 3 ].data = myLoader.GetCSV_Key_Record( "CSV/PlayerStatus/CSV_Player4_LvOrder", myCsvData[ 3 ].key );
		myCsvData[ 4 ].data = myLoader.GetCSV_Key_Record( "CSV/PlayerStatus/CSV_Player5_LvOrder", myCsvData[ 4 ].key );
		myCsvData[ 5 ].data = myLoader.GetCSV_Key_Record( "CSV/PlayerStatus/CSV_Player6_LvOrder", myCsvData[ 5 ].key );

		PlayerArray = new PlayerParameters[ myGV.GData.Players.Count, CSVLoader.csvId ];

		// 配列にデータを格納していく
		for( int i = 0; i < PlayerArray.GetLength( 0 ); i++ ) { // Player 人数
			for( int j = 0; j < PlayerArray.GetLength( 1 ); j++ ) { // CSV 1 ヘッダー当りのデータ量
				PlayerArray[ i, j ] = new PlayerParameters( );
				PlayerArray[ i, j ].ID = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_ID" ) );
				PlayerArray[ i, j ].Lv = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_LV" ) );
				PlayerArray[ i, j ].NextLvExp = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_NEXTLVEXP" ) );
				PlayerArray[ i, j ].HP = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_HP" ) );
				PlayerArray[ i, j ].MP = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_MP" ) );
				PlayerArray[ i, j ].Atk = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_ATK" ) );
				PlayerArray[ i, j ].WeaponAtk = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_WEAPONATK" ) );
				PlayerArray[ i, j ].Matk = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_MATK" ) );
				PlayerArray[ i, j ].Def = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_DEF" ) );
				PlayerArray[ i, j ].Mgr = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_MGR" ) );
				PlayerArray[ i, j ].EquipmentDef = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_EQUIPMENTDEF" ) );
				PlayerArray[ i, j ].Agl = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_AGL" ) );
				PlayerArray[ i, j ].Luc = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_LUC" ) );
				PlayerArray[ i, j ].Int = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_INT" ) );
				PlayerArray[ i, j ].WEAPON01 = myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_WEAPON01" );
				PlayerArray[ i, j ].WEAPON02 = myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_WEAPON02" );
				PlayerArray[ i, j ].Feeling = myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_FEELING" );
				PlayerArray[ i, j ].FeelingValue = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_FEELINGVALUE" ) );
				PlayerArray[ i, j ].Skill = myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_SKILL" );
				PlayerArray[ i, j ].StatusPoint = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_STATUSPOINT" ) );
				PlayerArray[ i, j ].OverDrive = myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_OVERDRIVE" );

				PlayerCsvList.Add( PlayerArray[ i, j ] ); // 入れ込む

			}

		}

		//Debug.Log( myLoader.GetCSVData( myCsvData[ 0 ].key, myCsvData[ 0 ].data, "0_SKILL" ) );
		//Debug.Log( "プレイヤーステータス CSV1 ファイル当りのデータ数 : " + CSVLoader.csvRecordAll );

		//foreach( PlayerParameters items in PlayerCsvList ) {
		//	Debug.Log( items.Skill );

		//}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>PlayerParameters</summary>
	/// <remarks>パラメーターは,暫定的です</remarks>
	[ System.Serializable ]
	public class PlayerParameters {
		/// <summary>ID</summary>
		public int ID;
		/// <summary>レベル</summary>
		public int Lv;
		/// <summary>次のレベルまで必要経験値量</summary>
		public int NextLvExp;
		/// <summary>ヒットポイント</summary>
		public int HP;
		/// <summary>マジックポイント</summary>
		public int MP;
		/// <summary>攻撃力</summary>
		public int Atk;
		/// <summary>武器攻撃力(装備している武器の合計)</summary>
		public int WeaponAtk;
		/// <summary>防御力</summary>
		public int Def;
		/// <summary>防具防御力(装備している防具の合計)</summary>
		public int EquipmentDef;
		/// <summary>魔法攻撃力</summary>
		public int Matk;
		/// <summary>魔法防御</summary>
		public int Mgr;
		/// <summary>運</summary>
		public int Luc;
		/// <summary>回避率</summary>
		public int Agl;
		/// <summary>知性,知力</summary>
		public int Int;
		/// <summary>感情</summary>
		public string Feeling;
		/// <summary>感情値(仮)一定以上の感情値で技を覚えるため値を保持する</summary>
		public int FeelingValue;
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


}
/*===============================================================*/
