using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>スレッドセーフ:EnemyManager</summary>
/// <remarks> クラスが参照されたタイミングでインスタンスが生成されます</remarks>
public sealed class SingltonEnemyManager {

	private static SingltonEnemyManager mInstance = new SingltonEnemyManager( );
	private EnemyParameters[ ] EnemyArray;
	private List<EnemyParameters> EnemyList;

	/// <summary>CSVから読み込まれた敵パラメーターを取得します</summary>
	public List<EnemyParameters> GetEnemyState { get { return EnemyList; } }
	/// <summary>Singlton</summary>
	public static SingltonEnemyManager Instance { get { return mInstance; } }

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	private SingltonEnemyManager( ) {
		Debug.Log( "Create SingltonEnemyManager Instance." );

		EnemyList = new List<EnemyParameters>( );

		EnemyCreate( ); // 敵パラメーターの作成を行います

	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>敵のパラメーター情報をなどを作ります</summary>
	private void EnemyCreate( ) {

		CSVLoader myLoader = new CSVLoader( );
		string[ ] key = new string[ 254 ];
		string[ ] keyData = new string[ 254 ];
		keyData = myLoader.GetCSV_Key_Record( "CSV/CSV_EnemyStatus", key );
		//for( int i = 0; i < keyData.Length; i++ ) {
		//	if( keyData[ i ] != null ) Debug.Log( i + "番目 : " + key[ i ] + " : " + keyData[ i ] );

		//}
		//Debug.Log( CSVLoader.csvId ); // Enemy 数
		//Debug.Log( CSVLoader.csvHeader ); // Header 数
		//Debug.Log( CSVLoader.csvRecordAll ); // CSV のデータ数

		// Enemy の数分配列を確保
		EnemyArray = new EnemyParameters[ CSVLoader.csvId ];
		for( int i = 0; i < CSVLoader.csvId; i++ ) EnemyArray[ i ] = new EnemyParameters( );

		// CSVLoader を用いて CSV のデータを配列にぶち込む ( エネミー数分 )
		for( int enemies = 0; enemies < CSVLoader.csvId; enemies++ ) {
			EnemyArray[ enemies ].ID = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_ID" ) );
			EnemyArray[ enemies ].NAME = myLoader.GetCSVData( key, keyData, enemies + "_NAME" );
			EnemyArray[ enemies ].LV = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_LV" ) );
			EnemyArray[ enemies ].HP = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_HP" ) );
			EnemyArray[ enemies ].MP = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_MP" ) );
			EnemyArray[ enemies ].ATK = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_ATK" ) );
			EnemyArray[ enemies ].MATK = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_MATK" ) );
			EnemyArray[ enemies ].DEF = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_DEF" ) );
			EnemyArray[ enemies ].MDEF = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_MDEF" ) );
			EnemyArray[ enemies ].SPD = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_SPD" ) );
			EnemyArray[ enemies ].LUCKY = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_LUCKY" ) );
			EnemyArray[ enemies ].FEELING = myLoader.GetCSVData( key, keyData, enemies + "_FEELING" );
			EnemyArray[ enemies ].DROPEXP = int.Parse( myLoader.GetCSVData( key, keyData, enemies + "_DROPEXP" ) );

		}
		
		// 配列に入れたデータをリストにぶち込む
		for ( int enemies = 0; enemies < CSVLoader.csvId; enemies++ ) EnemyList.Add( EnemyArray[ enemies ] );

		//foreach( EnemyParameters items in EnemyArray ) Debug.Log( items.NAME );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>EnemyParameters</summary>
	[ SerializeField ]
	public class EnemyParameters {
		/// <summary>0からの連番:敵を識別します</summary>
		public int ID;
		/// <summary>敵の名前</summary>
		public string NAME;
		/// <summary>敵のレベル</summary>
		public int LV;
		/// <summary>体力</summary>
		public int HP;
		/// <summary>魔力</summary>
		public int MP;
		/// <summary>攻撃力</summary>
		public int ATK;
		/// <summary>魔法攻撃力</summary>
		public int MATK;
		/// <summary>防御力</summary>
		public int DEF;
		/// <summary>魔法防御力</summary>
		public int MDEF;
		/// <summary>素早さ</summary>
		public int SPD;
		/// <summary>幸運</summary>
		public int LUCKY;
		/// <summary>感情</summary>
		public string FEELING;
		/// <summary>敵が落とす経験値量</summary>
		public int DROPEXP;


	}
	/*===============================================================*/


}
/*===============================================================*/