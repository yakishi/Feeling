using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>スレッドセーフ:ItemManager</summary>
/// <remarks> クラスが参照されたタイミングでインスタンスが生成されます</remarks>
public sealed class SingltonItemManager {

	private static SingltonItemManager mInstance = new SingltonItemManager( );
	private ItemList[ ] ItemArray;
	private ItemParam ItemSaveList;
	private List<ItemList> ItemCsvList;

	GV myGV;

	/// <summary>セーブデータから読み込まれたアイテムパラメーターを取得またはセット</summary>
	public ItemParam SDItem { get { return ItemSaveList; } set { ItemSaveList = value; } }
	/// <summary>CSVから読み込まれたアイテムパラメーターを取得します</summary>
	public List<ItemList> CDItem { get { return ItemCsvList; } }
	/// <summary>Singlton</summary>
	public static SingltonItemManager Instance { get { return mInstance; } }

	/// <summary>CSVのデータとキーを格納する構造体</summary>
	struct CSVDATA {
		public string[ ] key;
		public string[ ] data;

	}
	CSVDATA myCsvData;

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	private SingltonItemManager( ) {
		Debug.Log( "Create SingltonItemManager Instance." );

		ItemSaveList = new ItemParam( );
		ItemCsvList = new List<ItemList>( );

		ItemCreate( ); // アイテムパラメーターの作成を行います


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>アイテムパラメーター情報をなどを作ります</summary>
	private void ItemCreate( ) {

		myGV = GV.Instance; // Save/Load クラスのインスタンスを取得

		// Save 情報 Item のインスタンス生成
		ItemSaveList.Name = new List<string>( );
		ItemSaveList.Stock = new List<int>( );

		LoadItem( myGV.slot ); // セーブデータの読込

		// 配列確保
		myCsvData = new CSVDATA( );
		myCsvData.data = new string[ 256 ];
		myCsvData.key = new string[ 256 ];

		LoadCsvItem( ); // CSV からのデータ読込


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>NewGame時のセーブのインスタンス生成処理</summary>
	/// <remarks>GV.cs:newGame( )から呼ばれる</remarks>
	public void NewGame( ) {
		// Save 情報 Item のインスタンス生成
		ItemSaveList.Name = new List<string>( );
		ItemSaveList.Stock = new List<int>( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>GV.ItemParamからのデータ読込</summary>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void LoadItem( int loadSlot ) {

		myGV.GameDataLoad( loadSlot ); // セーブデータからの保存したデータを読み込みます

		if ( myGV.GData != null ) {
			// 読み込んだ値を入れていきます
			ItemSaveList.Name = myGV.GData.Items.Name;
			ItemSaveList.Stock = myGV.GData.Items.Stock;

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>CSVからのデータ読込とListにセットする</summary>
	private void LoadCsvItem( ) {
		// CSV ファイルからのキーとデータの読込
		CSVLoader myLoader = new CSVLoader( );
		myCsvData.data = myLoader.GetCSV_Key_Record( "CSV/CSV_PlayerItem_IdOrder", myCsvData.key );

		ItemArray = new ItemList[ CSVLoader.csvId ];

		// 配列にデータを格納していく
		for ( int i = 0; i < ItemArray.Length; i++ ) {
			ItemArray[ i ] = new ItemList( );
			ItemArray[ i ].ID = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_ID" ) );
			ItemArray[ i ].TYPE = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_TYPE" ) );
			ItemArray[ i ].TYPE2 = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_TYPE2" ) );
			ItemArray[ i ].MAX = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_MAX" ) );
			ItemArray[ i ].HP = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_HP" ) );
			ItemArray[ i ].MP = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_MP" ) );
			ItemArray[ i ].FEELINGVALUE = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_FEELINGVALUE" ) );
			ItemArray[ i ].NAME = myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_NAME" );
			ItemArray[ i ].DETAIL = myLoader.GetCSVData( myCsvData.key, myCsvData.data, i + "_DETAIL" );

			ItemCsvList.Add( ItemArray[ i ] ); // 入れ込む

		}

		//Debug.Log( myLoader.GetCSVData( myCsvData.key, myCsvData.data, "0_TYPE2" ) );
		//Debug.Log( "CSV1 ファイル当りのデータ数 : " + CSVLoader.csvRecordAll );

		//foreach( ItemList items in ItemCsvList ) {
		//	Debug.Log( items.DETAIL );

		//}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>ItemList(CSV情報)</summary>
	/// <remarks>パラメーターは,暫定的です</remarks>
	public class ItemList {
		/// <summary>ID:0から順に管理</summary>
		public int ID;
		/// <summary>回復アイテムの種類,{回復ヒットポイントアイテム:1},{回復魔法アイテム:2}</summary>
		public int TYPE;
		/// <summary>変動する感情値,{Do:1},{Love:2},{Ai:3},{Zou:4},{Raku:5}{Ki:6}</summary>
		public int TYPE2;
		/// <summary>所持できる最大アイテム個数</summary>
		public int MAX;
		/// <summary>アイテム使用によるヒットポイント回復値</summary>
		public int HP;
		/// <summary>アイテム使用によるマジックポイント回復値</summary>
		public int MP;
		/// <summary>アイテム使用による感情値変動数値</summary>
		public int FEELINGVALUE;
		/// <summary>アイテム名</summary>
		public string NAME;
		/// <summary>アイテムの詳細</summary>
		public string DETAIL;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>ItemParameters(Save情報)</summary>
	/// <remarks>パラメーターは,暫定的です</remarks>
	[System.Serializable]
	public class ItemParam {
		/// <summary>現在所持しているアイテム一覧</summary>
		public List<string> Name;
		/// <summary>現在所持しているアイテムに対するアイテム所持数</summary>
		public List<int> Stock;


	}
	/*===============================================================*/


}
/*===============================================================*/