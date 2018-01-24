using System;
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
        ItemSaveList.itemList = new Dictionary<string, int>();

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
        ItemSaveList.itemList = new Dictionary<string, int>();


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>GV.ItemParamからのデータ読込</summary>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void LoadItem( int loadSlot ) {

		myGV.GameDataLoad( loadSlot ); // セーブデータからの保存したデータを読み込みます

		if ( myGV.GData != null ) {
            // 読み込んだ値を入れていきます
            ItemSaveList.itemList = myGV.GData.Items.itemList;

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>CSVからのデータ読込とListにセットする</summary>
	private void LoadCsvItem( ) {
		// CSV ファイルからのキーとデータの読込
		CSVLoader myLoader = new CSVLoader( );
		myCsvData.data = myLoader.GetCSV_Key_Record(　"CSV/CSV_PlayerItem_IdOrder", myCsvData.key );

		ItemArray = new ItemList[ CSVLoader.csvId ];

		// 配列にデータを格納していく
		for ( int i = 0; i < ItemArray.Length; i++ ) {
			ItemArray[ i ] = new ItemList( );
			ItemArray[ i ].id = myLoader.GetCSVData( myCsvData.key, myCsvData.data, "I" + i + "_ID" );
			ItemArray[ i ].name = myLoader.GetCSVData( myCsvData.key, myCsvData.data, "I" + i + "_NAME" );
            ItemArray[ i ].max = int.Parse( myLoader.GetCSVData(myCsvData.key, myCsvData.data, "I" + i + "_MAX"));
			ItemArray[ i ].area = (Area)Enum.ToObject(typeof(Area), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, "I" + i + "_AREA")));
            ItemArray[ i ].value = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, "I" + i + "_VALUE" ) );
            ItemArray[ i ].category = (SingltonSkillManager.Category)Enum.ToObject(typeof(SingltonSkillManager.Category), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, "I" + i + "_CATEGORY")));
            ItemArray[ i ].target = (SingltonSkillManager.Target)Enum.ToObject(typeof(SingltonSkillManager.Target), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, "I" + i + "_TARGET")));
            ItemArray[ i ].scope = (SingltonSkillManager.Scope)Enum.ToObject(typeof(SingltonSkillManager.Scope), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, "I" + i + "_SCOPE")));
            ItemArray[ i ].param = (BattleParam)Enum.ToObject(typeof(BattleParam), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, "I" + i + "_INFLUENCE")));
            ItemArray[ i ].buffTime = int.Parse( myLoader.GetCSVData( myCsvData.key, myCsvData.data, "I" + i + "_DT" ) );
            ItemArray[ i ].feel = (SingltonSkillManager.Feel)Enum.ToObject(typeof(SingltonSkillManager.Feel), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, "I" + i + "_FC")));
            ItemArray[ i ].feelValue = int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, "I" + i + "_FVC"));
            ItemArray[ i ].Detail = myLoader.GetCSVData( myCsvData.key, myCsvData.data, "I" + i + "_DETAIL" );

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
		public string id;
		/// <summary>アイテム名</summary>
		public string name;
        /// <summary>最大所持数 </summary>
        public int max;
        /// <summary>使用エリア </summary>
        public Area area;
		/// <summary>回復、ダメージ量</summary>
		public int value;
		/// <summary>アイテムの種類</summary>
		public SingltonSkillManager.Category category;
        /// <summary>アイテムの対象</summary>
        public SingltonSkillManager.Target target;
		/// <summary>アイテムの範囲</summary>
		public SingltonSkillManager.Scope scope;
		/// <summary>影響を与えるステータス</summary>
		public BattleParam param;
        /// <summary>バフタイム</summary>
        public int buffTime;
        /// <summary>変わる感情</summary>
		public SingltonSkillManager.Feel feel;
        /// <summary>変わる感情値 </summary>
        public int feelValue;
        /// <summary>アイテムの詳細</summary>
        public string Detail;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>ItemParameters(Save情報)</summary>
	/// <remarks>パラメーターは,暫定的です</remarks>
	[System.Serializable]
	public class ItemParam {
        /// <summary>現在所持しているアイテム一覧</summary>
        
        public Dictionary<string, int> itemList;
	}
	/*===============================================================*/

    /// <summary>
    /// 使用可能エリア
    /// </summary>
    public enum Area
    {
        Field,
        Battle,
        All
    }

}
/*===============================================================*/