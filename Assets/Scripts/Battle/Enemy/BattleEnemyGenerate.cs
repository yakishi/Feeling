using UnityEngine;

/*===============================================================*/
/// <summary>BattleSceneObjectにScriptを関連づけます</summary>
/// <remarks>敵の生成および生成した敵のステータス情報を管理します</remarks>
public class BattleEnemyGenerate : EnemyManager {
	// オブジェクト参照
	[SerializeField, TooltipAttribute( "エネミープレハブ" )]
	private GameObject enemyPrefab;
	[SerializeField, TooltipAttribute( "ゲームキャンバス" )]
	private GameObject canvasGame;
	// 敵生成数上限
	private const int MAX_ENEMY = 5;
	// 生成された敵オブジェクトを配列で管理するための変数準備
	static private GameObject[ ] generateEnemy = new GameObject[ MAX_ENEMY ];
	// 敵の生成位置
	private float firstEnemySpawnX = -2.5f;
	private float firstEnemySpawnY = 2.0f;
	// 乱数を格納するための変数の準備
	static private float rndCnt;
	/// <summary>0～CSVから読み込まれた ID をカウントした範囲の数値がランダムで入る</summary>
	private float rnd;
	// 敵 Sprite を格納するための変数の準備
	static Sprite[ ] myEnemyImg;

	// セッターおよびゲッター定義部
	/// <summary>生成されたgameobjectをgetします,Enemy[index].nameにid属性が付けられています</summary>
	static public GameObject[ ] Enemy { get { return generateEnemy; } }
	/// <summary>生成された敵の数をgetします</summary>
	static public float EnemyNumber { get { return rndCnt; } }

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	void Start( ) {
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	new void Initialize( ) {
		// 敵画像の読み込み
		myEnemyImg = Resources.LoadAll<Sprite>( "Images/BattleScene/Enemy/" );
		// 敵の数をランダムに生成
		for ( rndCnt = 0; rndCnt < Random.Range( 5.0f, MAX_ENEMY ); rndCnt++ ) {
			// 0 ～ CSV から読み込まれた ID をカウントした範囲の数値がランダムで入る
			rnd = Random.Range( 0, CSVLoader.csvId );
			// バトルシーン開始時に敵を生成
			CreateEnemy( ( int )rndCnt );
			// Sprite 画像をコンポーネントにセットする
			ApplyEnemySprite( ( int )rndCnt, ( int )rnd );
			// 敵選択 Arrow の非表示
			ApplyEnemyArrowVisible( ( int )rndCnt, false );

		}

		// プレハブディレクトリ確認用
		//Debug.Log( "<color='red'>generateEnemy : " + generateEnemy[ 0 ] + "</color>" );
		//Debug.Log( "<color='red'>CSVLoader.csvId : " + CSVLoader.csvId + "</color>" );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>この関数はプレハブ化されてエネミーオブジェクト限定で使用します</summary>
	/// <param name="index">for文のループカウント変数</param>
	void CreateEnemy( int index ) {
		GameObject enemy = ( GameObject )Instantiate( enemyPrefab );
		//enemy.transform.SetParent ( canvasGame.transform, false );
		enemy.transform.localPosition = new Vector3 (
			firstEnemySpawnX,
			firstEnemySpawnY,
			0.0f

		);
		// 生成された敵オブジェクトを配列に入れます
		generateEnemy[ index ] = enemy.gameObject;
		// csv を参照し, ランダムに敵を選び gameobject の name 属性に付けます
		generateEnemy[ index ].gameObject.name = GetEnemyStatusData( rnd + "_ID" );
		// enemy に サブ識別子をつけて同じ敵が重複したときに対応出来るようにする
		generateEnemy[ index ].transform.GetChild( 2 ).gameObject.name = index.ToString( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summmary>生成したEnemyにおけるSprite画像を更新します</summmary>
	/// <remarks>
	/// Sprite画像はリソースフォルダにあるImages/Enemy/にある画像が配列で読み込まれています
	/// 解像度 : 311 × 286 以下の敵画像まで対応版
	/// </remarks>
	/// <param name="index">どのEnemyのSpriteに適用するか</param>
	/// <param name="index2">どのSprite画像を適用するか</param>
	static public void ApplyEnemySprite( int index, int index2 ) {
		float allow;
		generateEnemy[ index ].GetComponent<SpriteRenderer>( ).sprite = myEnemyImg[ index2 ];
		// Enemy 自身のスプライトレンダラーコンポーネントの取得
		SpriteRenderer renderer2 = generateEnemy[ index ].GetComponent<SpriteRenderer>( );
		// EnemyAllow を sprite の頭上に配置する
		allow = ( renderer2.bounds.size.y - ( renderer2.bounds.size.y / 2.0f ) ) + 0.3f;
		//Debug.Log( "<color='red'>" + index + " : " + renderer2.bounds.size.y + "</color>" );
		// sprite の縦幅が 1.8f を超えたとき arrow の位置および敵 sprite の調整をする
		if( renderer2.bounds.size.y > 1.8f ) {
			allow = ( renderer2.bounds.size.y - ( renderer2.bounds.size.y / 2.0f ) ) + 0.8f;
			generateEnemy[ index ].transform.localScale = new Vector3( 0.5f, 0.5f, 0.0f );
			generateEnemy[ index ].transform.GetChild( 0 ).transform.localScale = new Vector3( 0.1f, 0.1f, 0.0f );

		}
		generateEnemy[ index ].transform.GetChild( 0 ).transform.localPosition = new Vector3( 0.0f, allow, 0.0f );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>生成したEnemyにおける敵選択Arrowの表示するか否かを決定する</summary>
	/// <param name="index">何番目の敵のArrowに適用するか</param>
	/// <param name="isVisible">true:表示,false:非表示</param>
	static public void ApplyEnemyArrowVisible( int index,bool isVisible ) {
		if( generateEnemy[ index ] != null ) generateEnemy[ index ].transform.GetChild( 0 ).gameObject.SetActive( isVisible );


	}
	/*===============================================================*/


}
/*===============================================================*/