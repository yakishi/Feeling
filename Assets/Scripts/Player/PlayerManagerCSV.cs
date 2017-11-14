using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>CSVからのPlayerステータス情報を管理します</summary>
public class PlayerManagerCSV : MonoBehaviour {

	// ベースクラスに, メンバーとして, 各プレイヤーの変数を持つ
	// これらは, 継承先で使える物とする
	/// <summary>
	/// プレイヤー 1 のステータス情報にアクセスします
	/// </summary>
	public PlayerParameters Player1 = new PlayerParameters( );
	/// <summary>
	/// プレイヤー 2 のステータス情報にアクセスします
	/// </summary>
	public PlayerParameters Player2 = new PlayerParameters( );
	/// <summary>
	/// プレイヤー 3 のステータス情報にアクセスします
	/// </summary>
	public PlayerParameters Player3 = new PlayerParameters( );
	/// <summary>
	/// プレイヤー 4 のステータス情報にアクセスします
	/// </summary>
	public PlayerParameters Player4 = new PlayerParameters( );
	/// <summary>
	/// プレイヤー 5 のステータス情報にアクセスします
	/// </summary>
	public PlayerParameters Player5 = new PlayerParameters( );
	/// <summary>
	/// プレイヤー 6 のステータス情報にアクセスします
	/// </summary>
	public PlayerParameters Player6 = new PlayerParameters( );
	
	// CSVLoader loader.GetCSV_Key_Record クラスで使う変数を準備
	// これらの変数には, キー情報とキーに対するデータが入ります
	private string[ ] CSV_CharacterStatusKey = new string[ 1024 ];
	private string[ ] CSV_CharacterStatusKeyData = new string[ 1024 ];

	// プレイヤーステータス情報を格納する変数の準備
	static private List<PlayerParameters> players = new List<PlayerParameters>( );

	// セッターおよびゲッター定義部
	/// <summary>Listでプレイヤーのステータス情報をgetします</summary>
	static public List<PlayerParameters> GetPlayers { get { return players; } }

	/*===============================================================*/
	/// <summary>
	/// @brief UnityEngine ライフサイクルによる初期化
	/// </summary>
	void Awake( ) {
		// 初期化関数を呼び出す
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>
	/// @brief 初期化
	/// </summary>
	void Initialize( ) {
		// CSV読み込み機能を使った配列へのデータ読み込み
		CSVLoader loader = new CSVLoader( );
		CSV_CharacterStatusKeyData = loader.GetCSV_Key_Record( "CSV/CSV_CharacterStatus", CSV_CharacterStatusKey );

		// Player1 に対するパラメーターの初期化
		Player1._Player1 = GetPlayerStatusData( "Character01_ID" );
		Player1.HP = int.Parse( GetPlayerStatusData( "Character01_HP" ) );
		Player1.MP = int.Parse( GetPlayerStatusData( "Character01_MP" ) );
		Player1.ATK = int.Parse( GetPlayerStatusData( "Character01_ATK" ) );
		Player1.MATK = int.Parse( GetPlayerStatusData( "Character01_MATK" ) );
		Player1.DEF = int.Parse( GetPlayerStatusData( "Character01_DEF" ) );
		Player1.MDEF = int.Parse( GetPlayerStatusData( "Character01_MDEF" ) );
		Player1.SPD = int.Parse( GetPlayerStatusData( "Character01_SPD" ) );
		Player1.LUCKY = int.Parse( GetPlayerStatusData( "Character01_lucky" ) );
		Player1.INT = int.Parse( GetPlayerStatusData( "Character01_INT" ) );
		Player1.WEAPON01 = GetPlayerStatusData( "Character01_Weapon01" );
		Player1.WEAPON02 = GetPlayerStatusData( "Character01_Weapon02" );
		Player1.FEELING = GetPlayerStatusData( "Character01_Feeling" );
		Player1.SKILL = GetPlayerStatusData( "Character01_Skill" );
		Player1.OverDrive = GetPlayerStatusData( "Character01_OverDrive" );


		// Player2 に対するパラメーターの初期化
		Player2._Player2 = GetPlayerStatusData( "Character02_ID" );
		Player2.HP = int.Parse( GetPlayerStatusData( "Character02_HP" ) );
		Player2.MP = int.Parse( GetPlayerStatusData( "Character02_MP" ) );
		Player2.ATK = int.Parse( GetPlayerStatusData( "Character02_ATK" ) );
		Player2.MATK = int.Parse( GetPlayerStatusData( "Character02_MATK" ) );
		Player2.DEF = int.Parse( GetPlayerStatusData( "Character02_DEF" ) );
		Player2.MDEF = int.Parse( GetPlayerStatusData( "Character02_MDEF" ) );
		Player2.SPD = int.Parse( GetPlayerStatusData( "Character02_SPD" ) );
		Player2.LUCKY = int.Parse( GetPlayerStatusData( "Character02_lucky" ) );
		Player2.INT = int.Parse( GetPlayerStatusData( "Character02_INT" ) );
		Player2.WEAPON01 = GetPlayerStatusData( "Character02_Weapon01" );
		Player2.WEAPON02 = GetPlayerStatusData( "Character02_Weapon02" );
		Player2.FEELING = GetPlayerStatusData( "Character02_Feeling" );
		Player2.SKILL = GetPlayerStatusData( "Character02_Skill" );
		Player2.OverDrive = GetPlayerStatusData( "Character02_OverDrive" );


		// Player3 に対するパラメーターの初期化
		Player3._Player3 = GetPlayerStatusData( "Character03_ID" );
		Player3.HP = int.Parse( GetPlayerStatusData( "Character03_HP" ) );
		Player3.MP = int.Parse( GetPlayerStatusData( "Character03_MP" ) );
		Player3.ATK = int.Parse( GetPlayerStatusData( "Character03_ATK" ) );
		Player3.MATK = int.Parse( GetPlayerStatusData( "Character03_MATK" ) );
		Player3.DEF = int.Parse( GetPlayerStatusData( "Character03_DEF" ) );
		Player3.MDEF = int.Parse( GetPlayerStatusData( "Character03_MDEF" ) );
		Player3.SPD = int.Parse( GetPlayerStatusData( "Character03_SPD" ) );
		Player3.LUCKY = int.Parse( GetPlayerStatusData( "Character03_lucky" ) );
		Player3.INT = int.Parse( GetPlayerStatusData( "Character03_INT" ) );
		Player3.WEAPON01 = GetPlayerStatusData( "Character03_Weapon01" );
		Player3.WEAPON02 = GetPlayerStatusData( "Character03_Weapon02" );
		Player3.FEELING = GetPlayerStatusData( "Character03_Feeling" );
		Player3.SKILL = GetPlayerStatusData( "Character03_Skill" );
		Player3.OverDrive = GetPlayerStatusData( "Character03_OverDrive" );


		// Player4 に対するパラメーターの初期化
		Player4._Player4 = GetPlayerStatusData( "Character04_ID" );
		Player4.HP = int.Parse( GetPlayerStatusData( "Character04_HP" ) );
		Player4.MP = int.Parse( GetPlayerStatusData( "Character04_MP" ) );
		Player4.ATK = int.Parse( GetPlayerStatusData( "Character04_ATK" ) );
		Player4.MATK = int.Parse( GetPlayerStatusData( "Character04_MATK" ) );
		Player4.DEF = int.Parse( GetPlayerStatusData( "Character04_DEF" ) );
		Player4.MDEF = int.Parse( GetPlayerStatusData( "Character04_MDEF" ) );
		Player4.SPD = int.Parse( GetPlayerStatusData( "Character04_SPD" ) );
		Player4.LUCKY = int.Parse( GetPlayerStatusData( "Character04_lucky" ) );
		Player4.INT = int.Parse( GetPlayerStatusData( "Character04_INT" ) );
		Player4.WEAPON01 = GetPlayerStatusData( "Character04_Weapon01" );
		Player4.WEAPON02 = GetPlayerStatusData( "Character04_Weapon02" );
		Player4.FEELING = GetPlayerStatusData( "Character04_Feeling" );
		Player4.SKILL = GetPlayerStatusData( "Character04_Skill" );
		Player4.OverDrive = GetPlayerStatusData( "Character04_OverDrive" );


		// Player5 に対するパラメーターの初期化
		Player5._Player5 = GetPlayerStatusData( "Character05_ID" );
		Player5.HP = int.Parse( GetPlayerStatusData( "Character05_HP" ) );
		Player5.MP = int.Parse( GetPlayerStatusData( "Character05_MP" ) );
		Player5.ATK = int.Parse( GetPlayerStatusData( "Character05_ATK" ) );
		Player5.MATK = int.Parse( GetPlayerStatusData( "Character05_MATK" ) );
		Player5.DEF = int.Parse( GetPlayerStatusData( "Character05_DEF" ) );
		Player5.MDEF = int.Parse( GetPlayerStatusData( "Character05_MDEF" ) );
		Player5.SPD = int.Parse( GetPlayerStatusData( "Character05_SPD" ) );
		Player5.LUCKY = int.Parse( GetPlayerStatusData( "Character05_lucky" ) );
		Player5.INT = int.Parse( GetPlayerStatusData( "Character05_INT" ) );
		Player5.WEAPON01 = GetPlayerStatusData( "Character05_Weapon01" );
		Player5.WEAPON02 = GetPlayerStatusData( "Character05_Weapon02" );
		Player5.FEELING = GetPlayerStatusData( "Character05_Feeling" );
		Player5.SKILL = GetPlayerStatusData( "Character05_Skill" );
		Player5.OverDrive = GetPlayerStatusData( "Character05_OverDrive" );


		// Player6 に対するパラメーターの初期化
		Player6._Player6 = GetPlayerStatusData( "Character06_ID" );
		Player6.HP = int.Parse( GetPlayerStatusData( "Character06_HP" ) );
		Player6.MP = int.Parse( GetPlayerStatusData( "Character06_MP" ) );
		Player6.ATK = int.Parse( GetPlayerStatusData( "Character06_ATK" ) );
		Player6.MATK = int.Parse( GetPlayerStatusData( "Character06_MATK" ) );
		Player6.DEF = int.Parse( GetPlayerStatusData( "Character06_DEF" ) );
		Player6.MDEF = int.Parse( GetPlayerStatusData( "Character06_MDEF" ) );
		Player6.SPD = int.Parse( GetPlayerStatusData( "Character06_SPD" ) );
		Player6.LUCKY = int.Parse( GetPlayerStatusData( "Character06_lucky" ) );
		Player6.INT = int.Parse( GetPlayerStatusData( "Character06_INT" ) );
		Player6.WEAPON01 = GetPlayerStatusData( "Character06_Weapon01" );
		Player6.WEAPON02 = GetPlayerStatusData( "Character06_Weapon02" );
		Player6.FEELING = GetPlayerStatusData( "Character06_Feeling" );
		Player6.SKILL = GetPlayerStatusData( "Character06_Skill" );
		Player6.OverDrive = GetPlayerStatusData( "Character06_OverDrive" );

		// List に格納する
		players.Add( Player1 );
		players.Add( Player2 );
		players.Add( Player3 );
		players.Add( Player4 );
		players.Add( Player5 );
		players.Add( Player6 );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>
	/// @brief プレイヤーの ID 属性
	/// </summary>
	public class PlayerId {

		/// <summary>
		/// プレイヤー ID1
		/// </summary>
		public string _Player1;

		/// <summary>
		/// プレイヤー ID2
		/// </summary>
		public string _Player2;

		/// <summary>
		/// プレイヤー ID3
		/// </summary>
		public string _Player3;

		/// <summary>
		/// プレイヤー ID4
		/// </summary>
		public string _Player4;

		/// <summary>
		/// プレイヤー ID5
		/// </summary>
		public string _Player5;

		/// <summary>
		/// プレイヤー ID6
		/// </summary>
		public string _Player6;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>
	/// @brief プレイヤーの現在のステータス 型は暫定的です
	/// </summary>
	public class PlayerParameters : PlayerId {
	
		/// <summary>
		/// ヒットポイント
		/// </summary>
		public int HP;

		/// <summary>
		/// マジックポイント
		/// </summary>
		public int MP;

		/// <summary>
		/// 攻撃力
		/// </summary>
		public int ATK;

		/// <summary>
		/// 魔法攻撃力
		/// </summary>
		public int MATK;

		/// <summary>
		/// 防御力
		/// </summary>
		public int DEF;

		/// <summary>
		/// 魔法防御力
		/// </summary>
		public int MDEF;

		/// <summary>
		/// 素早さ
		/// </summary>
		public int SPD;

		/// <summary>
		/// 運
		/// </summary>
		public int LUCKY;

		/// <summary>
		/// 知性,知力
		/// </summary>
		public int INT;

		/// <summary>
		/// 手装備01
		/// </summary>
		public string WEAPON01;

		/// <summary>
		/// 手装備02
		/// </summary>
		public string WEAPON02;

		/// <summary>
		/// 感情
		/// </summary>
		public string FEELING;

		/// <summary>
		/// スキル
		/// </summary>
		public string SKILL;

		/// <summary>
		/// オーバードライブ
		/// </summary>
		public string OverDrive;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>
	/// @brief Player のキーデータを元にキーに対するデータを取得します
	/// @param string 例：Character01_IDなどを指定します
	/// @return GetPlayerStatusData 例:Character01_IDに対するデータ
	/// </summary>
	private string GetPlayerStatusData( string key ) {
		// data を格納する変数
		string str = "";
		// key を元に該当データを探し出す
		for( int i = 0; i < CSV_CharacterStatusKey.Length; i++ ) {
			// 引数 key と CSV_CharacterStatusKey の値が同じの場合
			if( CSV_CharacterStatusKey[ i ] == key ) {
				// data を str に格納する
				str = CSV_CharacterStatusKeyData[ i ];

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