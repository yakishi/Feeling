using UnityEngine;

/*===============================================================*/
/// <summary>
/// @brief Enemyの基底クラス,BattleSceneオブジェクトに関連づけます
/// </summary>
/*===============================================================*/
public class EnemyManager : MonoBehaviour {

	// CSVLoader loader.GetCSV_Key_Record クラスで使う変数を準備
	// これらの変数には, キー情報とキーに対するデータが入ります
	private string[ ] CSV_EnemyStatusKey = new string[ 1024 ];
	private string[ ] CSV_EnemyStatusKeyData = new string[ 1024 ];

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	void Awake( ) {
		// 初期化関数を呼び出す
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	public void Initialize( ) {
		// CSV読み込み機能を使った配列へのデータ読み込み
		CSVLoader loader = new CSVLoader( );
		CSV_EnemyStatusKeyData = loader.GetCSV_Key_Record( "CSV/CSV_EnemyStatus", CSV_EnemyStatusKey );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>Enemy のキーデータを元にキーに対するデータを取得します</summary>
	/// <param name="key">例：Monster01_IDなどを指定します</param>
	/// <returns>例:Monster01_IDに対するデータ</returns>
	public string GetEnemyStatusData( string key ) {
		// data を格納する変数
		string str = "";
		// key を元に該当データを探し出す
		for( int i = 0; i < CSV_EnemyStatusKey.Length; i++ ) {
			// 引数 key と CSV_CharacterStatusKey の値が同じの場合
			if( CSV_EnemyStatusKey[ i ] == key ) {
				// data を str に格納する
				str = CSV_EnemyStatusKeyData[ i ];

			}

		}
		// 戻り値が空の時
		if ( str == "" ) Debug.LogError( "引数に対するデータが不正です。\nキーを確認して下さい。" );
		// 格納したデータを返す
		return str;


	}
	/*===============================================================*/


}