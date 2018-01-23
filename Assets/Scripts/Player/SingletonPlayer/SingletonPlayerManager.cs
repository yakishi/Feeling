using System;
using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>スレッドセーフ:PlayerManager</summary>
/// <remarks> クラスが参照されたタイミングでインスタンスが生成されます</remarks>
public sealed class SingltonPlayerManager {

	private static SingltonPlayerManager mInstance = new SingltonPlayerManager( );
	private PlayerParameters[ , ] PlayerArray;
	private List<PlayerParameters> PlayerSaveList;
	private List<PlayerParameters> PlayerCsvList;

	GV myGV;

	/// <summary>セーブデータから読み込まれたプレイヤーパラメーターを取得またはセット</summary>
	public List<PlayerParameters> SaveDataPlayerState { get { return PlayerSaveList; } set { PlayerSaveList = value; } }
	/// <summary>CSVから読み込まれたプレイヤーパラメーターを取得します</summary>
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

		// 人数分の player を生成する
		for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			PlayerSaveList.Add( new PlayerParameters( ) );

		}

		LoadPlayer( myGV.slot ); // セーブデータの読込

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
	/// <summary>NewGame時のセーブのインスタンス生成処理</summary>
	/// <remarks>GV.cs:newGame( )から呼ばれる</remarks>
	public void NewGame( ) {
		// 人数分の player を生成する
		for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			PlayerSaveList.Add( new PlayerParameters( ) );
			PlayerSaveList[ i ].EquipmentStatus = new Status( );
			PlayerSaveList[ i ].SkillList = new List<string>( );

		}


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
				PlayerSaveList[ cnt ].Lv = item.Lv;
				PlayerSaveList[ cnt ].CFV = item.CFV;
				PlayerSaveList[ cnt ].SkillList = new List<string>( );
				PlayerSaveList[ cnt ].SkillList = item.SkillList;
				PlayerSaveList[ cnt ].EquipmentStatus = new Status(item.HP,item.MP,item.Atk,item.Def,item.Matk,item.Mgr,item.Luc,item.Agl,item.Feeling,item.FeelingValue );
				
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
				PlayerArray[ i, j ].ID = myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_ID" );
				if( PlayerArray[ i, j ].ID == "P" + ( i + 1 ) + "_" + j ) {
					PlayerArray[ i, j ].Name = PlayerName( "P" + ( i + 1 ) + "_0" );
					PlayerArray[ i, j ].RF = ( SingltonSkillManager.Feel )Enum.ToObject( typeof( SingltonSkillManager.Feel ), PlayerFeeling( PlayerArray[ i, j ].Name ) );

				}
				PlayerArray[ i, j ].STATUS = new Status( );
				PlayerArray[ i, j ].STATUS.HP = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_HP" ) );
				PlayerArray[ i, j ].STATUS.MP = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_MP" ) );
				PlayerArray[ i, j ].STATUS.Atk = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_ATK" ) );
				PlayerArray[ i, j ].STATUS.Def = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_DEF" ) );
				PlayerArray[ i, j ].STATUS.Matk = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_MATK" ) );
				PlayerArray[ i, j ].STATUS.Mgr = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_MGR" ) );
				PlayerArray[ i, j ].STATUS.Luc = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_LUC" ) );
				PlayerArray[ i, j ].STATUS.Agl = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_AGL" ) );
				PlayerArray[ i, j ].STATUS.Feeling = ( SingltonSkillManager.Feel )Enum.ToObject( typeof( SingltonSkillManager.Feel ), int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_FEELING" ) ) );
				PlayerArray[ i, j ].STATUS.FeelingValue = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_FEELINGVALUE" ) );

				PlayerArray[ i, j ].SkillList = new List<string>( );
				List<string> div = new List<string>( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, "P" + ( i + 1 ) + "_" + j + "_InitSkillList(ID/ID・・・)" ).Split( '/' ) );
				foreach( string items in div ) PlayerArray[ i, j ].SkillList.Add( items );

				PlayerCsvList.Add( PlayerArray[ i, j ] ); // 入れ込む

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤー名定義</summary>
	/// <param name="id">CSVのIDを指定します</param>
	/// <returns>IDに対応したプレイヤーネームを返します</returns>
	private string PlayerName( string id ) {
		switch( id ) {
			default : {
				Debug.LogError( "Player_ID定義外です。" );
				return "Not Found";

			}
			case "P1_0" : { return "カイウス・スノードロップ"; }
			case "P2_0" : { return "アイリ・スノードロップ"; }
			case "P3_0" : { return "ディオス・スノードロップ"; }
			case "P4_0" : { return "カリス・スノードロップ"; }
			case "P5_0" : { return "フィリア・スノードロップ"; }
			case "P6_0" : { return "セレーマ・スノードロップ"; }

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>名前を元に感情を取得する</summary>
	/// <param name="playerName">プレイヤーの名前</param>
	/// <returns>プレイヤーの名前に対応した感情を整数型で返します</returns>
	private int PlayerFeeling( string playerName ) {
		if( playerName == PlayerName( "P1_0" ) ) return (int)SingltonSkillManager.Feel.Do;
		else if( playerName == PlayerName( "P2_0" ) ) return (int)SingltonSkillManager.Feel.Zou;
		else if( playerName == PlayerName( "P3_0" ) ) return (int)SingltonSkillManager.Feel.Raku;
		else if( playerName == PlayerName( "P4_0" ) ) return (int)SingltonSkillManager.Feel.Ki;
		else if( playerName == PlayerName( "P5_0" ) ) return (int)SingltonSkillManager.Feel.Love;
		else if( playerName == PlayerName( "P6_0" ) ) return (int)SingltonSkillManager.Feel.Ai;
		else {
			Debug.LogError( "プレイヤーネーム定義外です。" );
			return -1;

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>PlayerParameters</summary>
	/// <remarks>
	/// CSVの項目およびSaveの項目間は,完全に分かれています
	/// 従って,CSVの情報からSaveの情報にアクセスすることは不可能です逆も然りです
	/// </remarks>
	[System.Serializable]
	public class PlayerParameters {
		/// <summary>ID(CSV)</summary>
		public string ID;
		/// <summary>キャラクターの名前(CSV)</summary>
		public string Name;
		/// <summary>レベル(Save)</summary>
		public int Lv;
		/// <summar>標準ステータス</summary>
		public Status STATUS;
        /// <summary>装備後ステータス</summary>
        public Status EquipmentStatus;
		/// <summary>上昇しやすい感情:RisingFeel(CSV)</summary>
		public SingltonSkillManager.Feel RF;
		/// <summary>現在の感情値:CurrentFeelingValue(Save)</summary>
		public int CFV;
		/// <summary>
		/// 現在覚えているスキルIDリスト(Save)
		/// レベル毎に初期で覚えているスキルIDリスト(CSV)
		/// </summary>
		public List<string> SkillList;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>ステータス</summary>
	[System.Serializable]
	public class Status {
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
		/// <summary>運</summary>
		public int Luc;
		/// <summary>回避率</summary>
		public int Agl;
		/// <summary>感情</summary>
		public SingltonSkillManager.Feel Feeling;
		/// <summary>感情値(仮)一定以上の感情値で技を覚えるため値を保持する</summary>
		public int FeelingValue;

        public Status()
        {
        }

        public Status(int hp,int mp,int atk,int def,int mAtk,int mgr,int luc,int agl,SingltonSkillManager.Feel feel,int feelValue)
        {
            HP = hp;
            MP = mp;
            Atk = atk;
            Def = def;
            Matk = mAtk;
            Mgr = mgr;
            Luc = luc;
            Agl = agl;
            Feeling = feel;
            FeelingValue = feelValue;

        }
	}
	/*===============================================================*/


}
/*===============================================================*/