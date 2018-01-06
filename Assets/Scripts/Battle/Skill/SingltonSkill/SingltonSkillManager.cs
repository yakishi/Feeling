using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>スレッドセーフ:SkillManager</summary>
/// <remarks> クラスが参照されたタイミングでインスタンスが生成されます</remarks>
public sealed class SingltonSkillManager {

	private static SingltonSkillManager mInstance = new SingltonSkillManager( );
	private SkillInfo[ , ] SkillArray;
	private List<SkillParam> SkillSaveList;
	private List<SkillInfo> SkillCsvList;

	GV myGV;

	/// <summary>セーブデータから読み込まれたプレイヤースキルパラメーターを取得またはセット</summary>
	public List<SkillParam> SDSkill { get { return SkillSaveList; } set { SkillSaveList = value; } }
	/// <summary>CSVから読み込まれたプレイヤースキルパラメーターを取得します</summary>
	public List<SkillInfo> CDSkill { get { return SkillCsvList; } }
	/// <summary>Singlton</summary>
	public static SingltonSkillManager Instance { get { return mInstance; } }

	/// <summary>CSVのデータとキーを格納する構造体</summary>
	struct CSVDATA {
		public string[ ] key;
		public string[ ] data;

	}
	CSVDATA[ ] myCsvData;

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	private SingltonSkillManager( ) {
		Debug.Log( "Create SingltonSkillManager Instance." );

		SkillSaveList = new List<SkillParam>( );
		SkillCsvList = new List<SkillInfo>( );

		SkillCreate( ); // スキルパラメーターの作成を行います


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤーのスキルパラメーター情報をなどを作ります</summary>
	private void SkillCreate( ) {

		myGV = GV.Instance; // Save/Load クラスのインスタンスを取得

		// 人数分の player を生成する
		for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			SkillSaveList.Add( new SkillParam( ) );
			SkillSaveList[ i ].Name = new List<string>( );

		}

		LoadSkill( myGV.slot ); // セーブデータの読込

		// 配列確保およびプレイヤー分用意
		myCsvData = new CSVDATA[ myGV.GData.PlayersSkills.Count ];
		for ( int players = 0; players < myGV.GData.PlayersSkills.Count; players++ ) {
			myCsvData[ players ] = new CSVDATA( );
			myCsvData[ players ].data = new string[ 256 ];
			myCsvData[ players ].key = new string[ 256 ];

		}

		LoadCsvSkill( ); // CSV からのデータ読込


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>NewGame時のセーブのインスタンス生成処理</summary>
	/// <remarks>GV.cs:newGame( )から呼ばれる</remarks>
	public void NewGame( ) {
		// 人数分の player を生成する
		for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			SkillSaveList.Add( new SkillParam( ) );
			SkillSaveList[ i ].Name = new List<string>( );

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>GV.SkillParamからのデータ読込</summary>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void LoadSkill( int loadSlot ) {

		myGV.GameDataLoad( loadSlot ); // セーブデータからの保存したデータを読み込みます

		int cnt = 0; // foreach カウント用変数

		if ( myGV.GData != null ) {
			// 読み込んだ値を入れていきます
			foreach ( GV.SkillParam item in myGV.GData.PlayersSkills ) {
				// GV.SkillParam に定義されているメンバ変数を SkillSaveList に入れていく
				SkillSaveList[ cnt ].Name = item.Name;
				cnt++;

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>CSVからのデータ読込とListにセットする</summary>
	private void LoadCsvSkill( ) {
		// CSV ファイルからのキーとデータの読込
		CSVLoader myLoader = new CSVLoader( );
		myCsvData[ 0 ].data = myLoader.GetCSV_Key_Record( "CSV/Skill/CSV_Player1_IdOrder", myCsvData[ 0 ].key );
		myCsvData[ 1 ].data = myLoader.GetCSV_Key_Record( "CSV/Skill/CSV_Player2_IdOrder", myCsvData[ 1 ].key );
		myCsvData[ 2 ].data = myLoader.GetCSV_Key_Record( "CSV/Skill/CSV_Player3_IdOrder", myCsvData[ 2 ].key );
		myCsvData[ 3 ].data = myLoader.GetCSV_Key_Record( "CSV/Skill/CSV_Player4_IdOrder", myCsvData[ 3 ].key );
		myCsvData[ 4 ].data = myLoader.GetCSV_Key_Record( "CSV/Skill/CSV_Player5_IdOrder", myCsvData[ 4 ].key );
		myCsvData[ 5 ].data = myLoader.GetCSV_Key_Record( "CSV/Skill/CSV_Player6_IdOrder", myCsvData[ 5 ].key );

		SkillArray = new SkillInfo[ myGV.GData.Players.Count, CSVLoader.csvId ];

		// 配列にデータを格納していく
		for ( int i = 0; i < SkillArray.GetLength( 0 ); i++ ) { // Player 人数
			for ( int j = 0; j < SkillArray.GetLength( 1 ); j++ ) { // CSV 1 ヘッダー当りのデータ量
				SkillArray[ i, j ] = new SkillInfo( );
				SkillArray[ i, j ].ID = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_ID" ) );
				SkillArray[ i, j ].NAME = myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_NAME" );
				SkillArray[ i, j ].MAGICDIAMETER = float.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_MAGICDIAMETER" ) );
				SkillArray[ i, j ].SKILLPOWER = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_SKILLPOWER" ) );
				SkillArray[ i, j ].LEARNINGFEELINGVALUE = int.Parse( myLoader.GetCSVData( myCsvData[ i ].key, myCsvData[ i ].data, j + "_LEARNINGFEELINGVALUE" ) );

				SkillCsvList.Add( SkillArray[ i, j ] ); // 入れ込む

			}

		}

		//Debug.Log( myLoader.GetCSVData( myCsvData[ 0 ].key, myCsvData[ 0 ].data, "0_SKILL" ) );
		//Debug.Log( "CSV1 ファイル当りのデータ数 : " + CSVLoader.csvRecordAll );

		//foreach( SkillParam items in SkillCsvList ) {
		//	Debug.Log( items.Skill );

		//}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>SkillInfo(CSV情報)</summary>
	/// <remarks>パラメーターは,暫定的です</remarks>
	public class SkillInfo {
		/// <summary>ID:0から順に管理</summary>
		public int ID;
		/// <summary>スキル名</summary>
		public string NAME;
		/// <summary>魔法倍率</summary>
		public float MAGICDIAMETER;
		/// <summary>スキル威力</summary>
		public int SKILLPOWER;
		/// <summary>戦闘中に一定以上の感情値に到達すると技を覚える（例：攻撃→怒＋5→閃めいて火炎切り習得</summary>
		public int LEARNINGFEELINGVALUE;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>PlayerSkillsParameters(Save情報)</summary>
	/// <remarks>パラメーターは,暫定的です</remarks>
	[System.Serializable]
	public class SkillParam {
		/// <summary>現在覚えているスキルの名前</summary>
		public List<string> Name;


	}
	/*===============================================================*/


}
/*===============================================================*/