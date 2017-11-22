using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>CSVLoaderを用いてプレイヤーのスキル情報を読み込んで管理するクラス</summary>
/// <remarks>
/// どこかのクラスで一度だけインスタンスを作成して後はゲッターで値を受け取るようにする
/// 現状,UI_BattleScene.cs:67でインスタンスを作成しています。
/// シーン毎にインスタンスの作成するのもありかもしれない・・・
/// </remarks>
class SkillLoader {

	static SkillInfo[ , ] PlayersSkillsTest;

	// セッターおよびゲッター定義部
	/// <summary>各プレイヤーのスキルデータをgetします</summary>
	static public SkillInfo[ , ] GetPlayersSkills { get { return PlayersSkillsTest; } }

	/// <summary>各プレイヤーへの内部アクセス変数関係</summary>
	struct PlayersSkill {
		public static List<SkillInfo> players1SkillInfo = new List<SkillInfo>( );
		public static List<SkillInfo> players2SkillInfo = new List<SkillInfo>( );
		public static List<SkillInfo> players3SkillInfo = new List<SkillInfo>( );
		public static List<SkillInfo> players4SkillInfo = new List<SkillInfo>( );
		public static List<SkillInfo> players5SkillInfo = new List<SkillInfo>( );
		public static List<SkillInfo> players6SkillInfo = new List<SkillInfo>( );

	}

	/*===============================================================*/
	/// <summary>CSVLoaderで使う</summary>
	class CSVDATA {
		public string[ ] CSV_Key = new string[ 1024 ];
		public string[ ] CSV_KeyData = new string[ 1024 ];


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>SkillLoader構成要素(暫定的)</summary>
	public class SkillInfo {
		/// <summary>ID:0から順に管理</summary>
		public int ID;
		/// <summary>スキル名</summary>
		public string NAME;
		/// <summary>魔法倍率</summary>
		public float MAGICDIAMETER;
		/// <summary>スキル威力</summary>
		public int SKILLPOWER;

		public SkillInfo( int _ID, string _NAME, float _MAGICDIAMETER, int _SKILLPOWER ) {
			ID = _ID;
			NAME = _NAME;
			MAGICDIAMETER = _MAGICDIAMETER;
			SKILLPOWER = _SKILLPOWER;


		}


	} 
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>コンストラクタ</summary>
	public SkillLoader( ) {
		Debug.Log( "SkillLoader Class on the Constructor Function Call." );
		//次のフレームで実行する
		Observable.NextFrame( )
			.Subscribe( _ => Initialize( ) );
		//Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	private void Initialize( ) {
		CSVDATA myData1 = new CSVDATA( );
		CSVDATA myData2 = new CSVDATA( );
		CSVDATA myData3 = new CSVDATA( );
		CSVDATA myData4 = new CSVDATA( );
		CSVDATA myData5 = new CSVDATA( );
		CSVDATA myData6 = new CSVDATA( );
		// CSV読み込み機能を使った配列へのデータ読み込み
		CSVLoader loader = new CSVLoader( );
		myData1.CSV_KeyData = loader.GetCSV_Key_Record( "CSV/Skill/CSV_Player1_IdOrder", myData1.CSV_Key );
		myData2.CSV_KeyData = loader.GetCSV_Key_Record( "CSV/Skill/CSV_Player2_IdOrder", myData2.CSV_Key );
		myData3.CSV_KeyData = loader.GetCSV_Key_Record( "CSV/Skill/CSV_Player3_IdOrder", myData3.CSV_Key );
		myData4.CSV_KeyData = loader.GetCSV_Key_Record( "CSV/Skill/CSV_Player4_IdOrder", myData4.CSV_Key );
		myData5.CSV_KeyData = loader.GetCSV_Key_Record( "CSV/Skill/CSV_Player5_IdOrder", myData5.CSV_Key );
		myData6.CSV_KeyData = loader.GetCSV_Key_Record( "CSV/Skill/CSV_Player6_IdOrder", myData6.CSV_Key );

		PlayersSkillsTest = new SkillInfo[ 6, CSVLoader.csvId ];
		
		for( int i = 0; i < CSVLoader.csvId; i++ ) {
			PlayersSkill.players1SkillInfo.Add( new SkillInfo( 0, "", 0.0f, 0 ) );
			PlayersSkill.players2SkillInfo.Add( new SkillInfo( 0, "", 0.0f, 0 ) );
			PlayersSkill.players3SkillInfo.Add( new SkillInfo( 0, "", 0.0f, 0 ) );
			PlayersSkill.players4SkillInfo.Add( new SkillInfo( 0, "", 0.0f, 0 ) );
			PlayersSkill.players5SkillInfo.Add( new SkillInfo( 0, "", 0.0f, 0 ) );
			PlayersSkill.players6SkillInfo.Add( new SkillInfo( 0, "", 0.0f, 0 ) );

		}

		for( int i = 0; i < PlayersSkill.players1SkillInfo.Count; i++ ) {
			PlayersSkill.players1SkillInfo[ i ].ID = int.Parse( GetSkillData( myData1.CSV_Key, myData1.CSV_KeyData, i + "_ID" ) );
			PlayersSkill.players1SkillInfo[ i ].NAME = GetSkillData( myData1.CSV_Key, myData1.CSV_KeyData, i + "_NAME" );
			PlayersSkill.players1SkillInfo[ i ].MAGICDIAMETER = float.Parse( GetSkillData( myData1.CSV_Key, myData1.CSV_KeyData, i + "_MAGICDIAMETER" ) );
			PlayersSkill.players1SkillInfo[ i ].SKILLPOWER = int.Parse( GetSkillData( myData1.CSV_Key, myData1.CSV_KeyData, i + "_SKILLPOWER" ) );
			//
			PlayersSkill.players2SkillInfo[ i ].ID = int.Parse( GetSkillData( myData2.CSV_Key, myData2.CSV_KeyData, i + "_ID" ) );
			PlayersSkill.players2SkillInfo[ i ].NAME = GetSkillData( myData2.CSV_Key, myData2.CSV_KeyData, i + "_NAME" );
			PlayersSkill.players2SkillInfo[ i ].MAGICDIAMETER = float.Parse( GetSkillData( myData2.CSV_Key, myData2.CSV_KeyData, i + "_MAGICDIAMETER" ) );
			PlayersSkill.players2SkillInfo[ i ].SKILLPOWER = int.Parse( GetSkillData( myData2.CSV_Key, myData2.CSV_KeyData, i + "_SKILLPOWER" ) );
			//
			PlayersSkill.players3SkillInfo[ i ].ID = int.Parse( GetSkillData( myData3.CSV_Key, myData3.CSV_KeyData, i + "_ID" ) );
			PlayersSkill.players3SkillInfo[ i ].NAME = GetSkillData( myData3.CSV_Key, myData3.CSV_KeyData, i + "_NAME" );
			PlayersSkill.players3SkillInfo[ i ].MAGICDIAMETER = float.Parse( GetSkillData( myData3.CSV_Key, myData3.CSV_KeyData, i + "_MAGICDIAMETER" ) );
			PlayersSkill.players3SkillInfo[ i ].SKILLPOWER = int.Parse( GetSkillData( myData3.CSV_Key, myData3.CSV_KeyData, i + "_SKILLPOWER" ) );
			//
			PlayersSkill.players4SkillInfo[ i ].ID = int.Parse( GetSkillData( myData4.CSV_Key, myData4.CSV_KeyData, i + "_ID" ) );
			PlayersSkill.players4SkillInfo[ i ].NAME = GetSkillData( myData4.CSV_Key, myData4.CSV_KeyData, i + "_NAME" );
			PlayersSkill.players4SkillInfo[ i ].MAGICDIAMETER = float.Parse( GetSkillData( myData4.CSV_Key, myData4.CSV_KeyData, i + "_MAGICDIAMETER" ) );
			PlayersSkill.players4SkillInfo[ i ].SKILLPOWER = int.Parse( GetSkillData( myData4.CSV_Key, myData4.CSV_KeyData, i + "_SKILLPOWER" ) );
			//
			PlayersSkill.players5SkillInfo[ i ].ID = int.Parse( GetSkillData( myData5.CSV_Key, myData5.CSV_KeyData, i + "_ID" ) );
			PlayersSkill.players5SkillInfo[ i ].NAME = GetSkillData( myData5.CSV_Key, myData5.CSV_KeyData, i + "_NAME" );
			PlayersSkill.players5SkillInfo[ i ].MAGICDIAMETER = float.Parse( GetSkillData( myData5.CSV_Key, myData5.CSV_KeyData, i + "_MAGICDIAMETER" ) );
			PlayersSkill.players5SkillInfo[ i ].SKILLPOWER = int.Parse( GetSkillData( myData5.CSV_Key, myData5.CSV_KeyData, i + "_SKILLPOWER" ) );
			//
			PlayersSkill.players6SkillInfo[ i ].ID = int.Parse( GetSkillData( myData6.CSV_Key, myData6.CSV_KeyData, i + "_ID" ) );
			PlayersSkill.players6SkillInfo[ i ].NAME = GetSkillData( myData6.CSV_Key, myData6.CSV_KeyData, i + "_NAME" );
			PlayersSkill.players6SkillInfo[ i ].MAGICDIAMETER = float.Parse( GetSkillData( myData6.CSV_Key, myData6.CSV_KeyData, i + "_MAGICDIAMETER" ) );
			PlayersSkill.players6SkillInfo[ i ].SKILLPOWER = int.Parse( GetSkillData( myData6.CSV_Key, myData6.CSV_KeyData, i + "_SKILLPOWER" ) );


		}

		// 二次元配列に変換する
		for( int i = 0; i < 6; i++ ) {
			for( int j = 0; j < CSVLoader.csvId; j++ ) {
				if( i == 0 ) PlayersSkillsTest[ i, j ] = PlayersSkill.players1SkillInfo[ j ];
				if( i == 1 ) PlayersSkillsTest[ i, j ] = PlayersSkill.players2SkillInfo[ j ];
				if( i == 2 ) PlayersSkillsTest[ i, j ] = PlayersSkill.players3SkillInfo[ j ];
				if( i == 3 ) PlayersSkillsTest[ i, j ] = PlayersSkill.players4SkillInfo[ j ];
				if( i == 4 ) PlayersSkillsTest[ i, j ] = PlayersSkill.players5SkillInfo[ j ];
				if( i == 5 ) PlayersSkillsTest[ i, j ] = PlayersSkill.players6SkillInfo[ j ];

			}

		}

		// 出力の仕方 ( 例 ) 6 人分のプレイヤーのスキルを吐き出す
		for( int i = 0; i < PlayersSkillsTest.GetLength( 0 ); i++ ) {
			for( int j = 0; j < PlayersSkillsTest.GetLength( 1 ); j++ ) {
				Debug.Log( "Player" + i + "のスキル : " + PlayersSkillsTest[ i, j ].NAME );

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>キーデータを元にキーに対するデータを取得します</summary>
	/// <param name="CSV_Key">配列:読み込んだキーを保存する</param>
	/// <param name="CSV_KeyData">配列:読み込んだデータを保存する</param>
	/// <param name="key">CSVLoader.cs:227行目をコメントアウトすることでキーを確認できます</param>
	/// <returns>例:0_NAMEに対するデータ</returns>
	private string GetSkillData( string[ ] CSV_Key, string[ ] CSV_KeyData, string key ) {
		// data を格納する変数
		string str = "";
		// key を元に該当データを探し出す
		for ( int i = 0; i < CSV_Key.Length; i++ ) {
			// 引数 key と CSV_CharacterStatusKey の値が同じの場合
			if ( CSV_Key[ i ] == key ) {
				// data を str に格納する
				str = CSV_KeyData[ i ];

			}

		}
		// 戻り値が空の時
		if ( str == "" ) Debug.LogError( "引数に対するデータが不正です。\nキーを確認して下さい。" );
		// 格納したデータを返す
		return str;


	}
	/*===============================================================*/


}
/*===============================================================*/
