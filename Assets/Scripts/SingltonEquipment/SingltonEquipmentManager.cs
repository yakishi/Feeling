using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>スレッドセーフ:EquipmentManager</summary>
/// <remarks>クラスが参照されたタイミングでインスタンスが生成されます</remarks>
public sealed class SingltonEquipmentManager {

	private static SingltonEquipmentManager mInstance = new SingltonEquipmentManager( );
	private EquipmentArticleList[ ] EquipmentArray;
	private List<PlayerEquipmentParam> EquipmentSaveList;
	private List<EquipmentArticleList> EquipmentCsvList;

	GV myGV;

	/// <summary>セーブデータから読み込まれた装備パラメーターを取得またはセット</summary>
	public List<PlayerEquipmentParam> SaveDataPlayerEquipmentParam { get { return EquipmentSaveList; } set { EquipmentSaveList = value; } }
	/// <summary>CSVから読み込まれた装備パラメーターを取得します</summary>
	public List<EquipmentArticleList> GetCsvDataPlayerState { get { return EquipmentCsvList; } }
	/// <summary>Singlton</summary>
	public static SingltonEquipmentManager Instance { get { return mInstance; } }

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	private SingltonEquipmentManager( ) {
		Debug.Log( "Create SingltonEquipmentManager Instance." );

		EquipmentSaveList = new List<PlayerEquipmentParam>( );
		EquipmentCsvList = new List<EquipmentArticleList>( );

		EquipmentCreate( ); // プレイヤーパラメーターの作成を行います


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>装備のパラメーター情報をなどを作ります</summary>
	private void EquipmentCreate( ) {

		myGV = GV.Instance; // Save/Load クラスのインスタンスを取得
		myGV.newGame( ); // 必ず new Game 下後に変動値などの保存を行う

		// 人数分の装備を生成する
		for ( int i = 0; i < myGV.GData.Equipments.Count; i++ ) {
			EquipmentSaveList.Add( new PlayerEquipmentParam( ) );

		}

		LoadEquipment( myGV.slot ); // セーブデータの読込

		LoadCsvEquipment( ); // CSV からのデータ読込


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>GV.EquipmentParamからのデータ読込</summary>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void LoadEquipment( int loadSlot ) {

		myGV.GameDataLoad( loadSlot ); // セーブデータからの保存したデータを読み込みます

		int cnt = 0; // foreach カウント用変数
		
		if( myGV.GData != null ) {
			// 読み込んだ値を入れていきます
			foreach ( GV.EquipmentParam item in myGV.GData.Equipments ) {
				EquipmentSaveList[ cnt ].ID = item.ID;
				EquipmentSaveList[ cnt ].Arms = item.Arms;
				EquipmentSaveList[ cnt ].Head = item.Head;
				EquipmentSaveList[ cnt ].Shoes = item.Shoes;
				EquipmentSaveList[ cnt ].Accessory1 = item.Accessory1;
				EquipmentSaveList[ cnt ].Accessory2 = item.Accessory2;
				cnt++;

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>CSVからのデータ読込とListにセットする</summary>
	private void LoadCsvEquipment( ) {

		// 配列確保
		CSVLoader myLoader = new CSVLoader( );
		string[ ] key = new string[ 254 ];
		string[ ] keyData = new string[ 254 ];
		keyData = myLoader.GetCSV_Key_Record( "CSV/CSV_EquipmentList", key );

		// 装備リスト の数分配列を確保
		EquipmentArray = new EquipmentArticleList[ CSVLoader.csvId ];
		for( int i = 0; i < CSVLoader.csvId; i++ ) EquipmentArray[ i ] = new EquipmentArticleList( );

		// CSVLoader を用いて CSV のデータを配列にぶち込む ( 装備リスト分 )
		for( int equipment = 0; equipment < CSVLoader.csvId; equipment++ ) {
			EquipmentArray[ equipment ].ID = int.Parse( myLoader.GetCSVData( key, keyData, equipment + "_ID" ) );
			EquipmentArray[ equipment ].Name = myLoader.GetCSVData( key, keyData, equipment + "_Name" );
			EquipmentArray[ equipment ].Type = int.Parse( myLoader.GetCSVData( key, keyData, equipment + "_Type" ) );
			EquipmentArray[ equipment ].Type2 = int.Parse( myLoader.GetCSVData( key, keyData, equipment + "_Type2" ) );

		}
		
		// 配列に入れたデータをリストにぶち込む
		for ( int equipment = 0; equipment < CSVLoader.csvId; equipment++ ) EquipmentCsvList.Add( EquipmentArray[ equipment ] );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>装備品リスト(CSV情報)</summary>
	/// <remarks>パラメーターは,暫定的です</remarks>
	public class EquipmentArticleList {
		/// <summary>装備品リスト識別ID(0からの連番で管理)</summary>
		public int ID;
		/// <summary>装備名</summary>
		public string Name;
		/// <summary>0:武器,1:頭,2:体,3:靴,4:アクセサリー</summary>
		public int Type;
		/// <summary>(武器{0:片手剣,1:斧,2:短剣,3:杖,4:槍}),(頭{0:兜,1:帽子}),(体{0:鎧,1:ローブ})</summary>
		public int Type2;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤーが現在装備しているリスト(Save情報)</summary>
	/// <remarks>パラメーターは,暫定的です</remarks>
	[ System.Serializable ]
	public class PlayerEquipmentParam {
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
	/*===============================================================*/


}
/*===============================================================*/