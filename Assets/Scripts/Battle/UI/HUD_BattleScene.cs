using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/*===============================================================*/
/// <summary>BattleSceneオブジェクトにスクリプトを関連づけます</summary>
/// <remarks>BattleSceneのステータス(UI{HUD})情報を操作するクラス,HPやMPなど・・・</remarks>
public class HUD_BattleScene : PlayerManagerCSV {

	[SerializeField, TooltipAttribute( "キャラクターステータスプレハブ" )]
	private GameObject characterStatusHUDPrefabObj;
	[SerializeField, TooltipAttribute( "Messageプレハブ" )]
	private GameObject messagePrefab;
	[SerializeField, TooltipAttribute( "StandingPicturesプレハブ" )]
	private GameObject spPrefab;
	[SerializeField, TooltipAttribute( "ゲームキャンバス" )]
	private GameObject canvasUi;
	
	// キャラクターステータスHUDを操作するための変数の準備
	static public GameObject[ ] myHudObj;
	static Sprite[ ] myHudImg;

	// メッセージプレハブを操作するための変数の準備
	static public GameObject myMessageObj;

	// 立ち絵プレハブを操作するための変数の準備
	static public GameObject mySpObj;
	static Sprite[ ] mySpImg;

	// 更新用変数の準備
	private float[ ] updateValHp; // HP 更新用変数
	private float[ ] updateValMp; // MP 更新用変数

	// セッターおよびゲッター定義部
	/// <summary>生成されたキャラクターHUDのゲームオブジェクトgetします</summary>
	/// <remarks>初期化された後に値が入るので初期化関数などで呼ばないでください</remarks>
	static public GameObject[ ] CharaHudGameObj { get { return myHudObj; } }

	/// <summary>生成された立ち絵のゲームオブジェクトgetします</summary>
	/// <remarks>初期化された後に値が入るので初期化関数などで呼ばないでください</remarks>
	static public GameObject StandPicture { get { return mySpObj; } }

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	void Start( ) {
		// 初期化関数を呼び出す
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	void Initialize( ) {
		// 更新用変数の初期化
		updateValHp = new float[ ] { Player1.HP, Player2.HP, Player3.HP, Player4.HP };
		updateValMp = new float[ ] { Player1.MP, Player2.MP, Player3.MP, Player4.MP };
		// キャラクター画像の読み込み
		myHudImg = Resources.LoadAll<Sprite>( "Images/BattleScene/Character/HUD/" );
		// キャラクターステータス HUD の作成
		myHudObj = CreateCharacterStatusHUD( -150.0f, -250.0f, 3 );
		for( int i = 0; i < myHudObj.Length; i++ ) {
			// Sprite 画像をコンポーネントにセットする
			ApplyCharaSprite( i, i );
			// HP の初期化
			ApplyCharaHP( i, updateValHp[ i ] );
			// MP の初期化
			ApplyCharaMP( i, updateValMp[ i ] );

		}
		// MessageUI の作成
		myMessageObj = CreateMessage( -100.0f, 253.0f, false );
		// 立ち絵の作成
		mySpObj = CreateStandPicture( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによって毎フレーム呼ばれます</summary>
	void Update( ) {

	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>キャラクターステータスHUDの作成を行います</summary>
	/// <param name="initializePositionX">キャラクターステータスHUDの生成位置(X軸)</param>
	/// <param name="initializePositionY">キャラクターステータスHUDの生成位置(Y軸)</param>
	/// <param name="index">キャラクターステータスHUDの複製数,通常3を指定</param>
	/// <returns>生成元および生成したゲームオブジェクトを返します</returns>
	private GameObject[ ] CreateCharacterStatusHUD( float initializePositionX, float initializePositionY, int index ) {
		GameObject prefabObj = ( GameObject )Instantiate( characterStatusHUDPrefabObj );
		GameObject[ ] prefabAllObj = new GameObject[ index + 1 ];
		int prefabAllObjCnt = 1; // 0 番目にはオリジナルが入るので 1 からスタート
		prefabObj.transform.GetChild( 0 ).gameObject.name = "Character0"; // コピー元のオリジナルの name を変更
		prefabAllObj[ 0 ] = prefabObj.transform.GetChild( 0 ).gameObject; // コピー元のオリジナルを格納
		prefabObj.transform.SetParent( canvasUi.transform, false );
		prefabObj.transform.localPosition = new Vector3 ( initializePositionX, initializePositionY, 0.0f );
		HorizontalLayoutGroup hlg = prefabObj.GetComponent<HorizontalLayoutGroup>( );
		for( int i = 0; i < index; i++ ) {
			if( i < prefabAllObj.Length ) { // PlayerSprite 残り分を入れる
				GameObject prefabHud = ( GameObject )Instantiate( prefabObj.transform.GetChild( 0 ).gameObject );
				prefabHud.name = "Character" + prefabAllObjCnt; // コピー先のオブジェクト名を変更
				prefabHud.transform.SetParent( hlg.transform, false );
				prefabAllObj[ prefabAllObjCnt ] = prefabHud; // コピー先のオブジェクトを格納
				prefabAllObjCnt++; // 入れ物変更

			}

		}
		return prefabAllObj;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>Messageプレハブの作成を行います</summary>
	/// <param name="initPosX">MessageUIの生成位置(X軸)</param>
	/// <param name="initPosY">MessageUIの生成位置(Y軸)</param>
	/// <param name="isVisible">MessageUIを表示するか否か,true:表示,false:非表示</param>
	/// <param name="message">生成時の初期メッセージを指定します</param>
	private GameObject CreateMessage( float initPosX, float initPosY, bool isVisible, string message = "init message" ) {
		GameObject prefabObj = ( GameObject )Instantiate( messagePrefab );
		prefabObj.transform.SetParent( canvasUi.transform, false );
		prefabObj.transform.localPosition = new Vector3( initPosX, initPosY, 0.0f );
		prefabObj.gameObject.SetActive( isVisible );
		prefabObj.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<Text>( ).text = message;
		return prefabObj;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>立ち絵プレハブの作成を行います</summary>
	private GameObject CreateStandPicture( ) {
		GameObject prefabObj = ( GameObject )Instantiate( spPrefab );
		prefabObj.transform.SetParent( canvasUi.transform, false );
		return prefabObj;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>キャラクターステータスHUDにおけるSprite画像を更新します</summary>
	/// <remarks>Sprite画像はリソースフォルダにあるImages/Character/にある画像が配列で読み込まれています</remarks>
	/// <param name="index">各キャラクターのSprite画像のどのキャラクターのSprite画像に適用するか</param>
	/// <param name="index2">なんのSprite画像に適用するか</param> 
	static public void ApplyCharaSprite( int index, int index2 ) {
		// Sprite 画像をコンポーネントにセットする
		myHudObj[ index ].transform.GetChild( 0 ).GetComponent<Image>( ).sprite = myHudImg[ index2 ];


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>キャラクターステータスHUDにおけるHPを更新します</summary>
	/// <param name="index">各キャラクターのHPのどのキャラクターのHPに適用するか</param>
	/// <param name="hp">適用させたいHPの値</param>
	static public void ApplyCharaHP( int index, float hp ) {
		// HP バー Slider コンポーネントの Value 更新
		myHudObj[ index ].transform.GetChild( 1 ).GetComponent<Slider>( ).value = hp;
		// HP Text コンポーネント更新
		myHudObj[ index ].transform.GetChild( 1 ).GetChild( 2 ).GetComponent<Text>( ).text = "HP" + hp.ToString( ) + "/100";


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>キャラクターステータスHUDにおけるMPを更新します</summary>
	/// <param name="index">各キャラクターのMPのどのキャラクターのMPに適用するか</param>
	/// <param name="mp">適用させたいMPの値</param>
	static public void ApplyCharaMP( int index, float mp ) {
		// HP バー Slider コンポーネントの Value 更新
		myHudObj[ index ].transform.GetChild( 2 ).GetComponent<Slider>( ).value = mp;
		// HP Text コンポーネント更新
		myHudObj[ index ].transform.GetChild( 2 ).GetChild( 2 ).GetComponent<Text>( ).text = "HP" + mp.ToString( ) + "/100";


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>生成されたMessageプレハブのテキストを更新します</summary>
	/// <param name="message">適用したい文字列を入れます</param>
	/// <param name="isVisible">MessageUIを表示するか否か,true:表示,false:非表示</param>
	static public void ApplyMessageText( string message, bool isVisible = true ) {
		myMessageObj.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<Text>( ).text = message/* + "の攻撃"*/;
		myMessageObj.gameObject.SetActive( isVisible );
		if( isVisible ) {
			// 1500ミリ秒後に自分自身を呼ぶ
			Observable.Timer( TimeSpan.FromMilliseconds( 1500 ) )
				.Subscribe( _ => ApplyMessageText( "", false /* 非表示状態にする */ ) );

		}

	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>生成されたMessageプレハブのテキストを更新します</summary>
	/// <param name="message">適用したい文字列を入れます</param>
	/// <param name="isVisible">MessageUIを表示するか否か,true:表示,false:非表示</param>
	/// <param name="ms">メッセージを表示したい時間(ミリ秒単位)</param>
	static public void ApplyMessageText( string message, bool isVisible, int ms ) {
		myMessageObj.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<Text>( ).text = message/* + "の攻撃"*/;
		myMessageObj.gameObject.SetActive( isVisible );
		if( isVisible ) {
			// n ミリ秒後に自分自身を呼ぶ
			Observable.Timer( TimeSpan.FromMilliseconds( ms ) )
				.Subscribe( _ => ApplyMessageText( "", false /* 非表示状態にする */ ) );

		}

	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>生成された立ち絵プレハブの画像を更新します</summary>
	/// <param name="posX">RootGameobject自身の位置(X軸)</param>
	/// <param name="posY">RootGameObject自身の位置(Y軸)</param>
	/// <param name="scaleX">sprite自身のスケール(X軸方向に拡大)</param>
	/// <param name="scaleY">sprite自身のスケール(Y軸方向に拡大)</param>
	/// <param name="index">読み込ませる画像,通常0～3を指定</param>
	static public void ApplyStandPictureSprite( float posX, float posY, float scaleX, float scaleY, int index ) {
		// 立ち絵画像を読み込みます
		mySpImg = Resources.LoadAll<Sprite>( "Images/BattleScene/Character/StandPicture/" );
		// sprite 画像の改造度が分からないため以下で対応させる ( 位置およびスケール )
		mySpObj.transform.transform.localPosition = new Vector3( posX, posY, 0.0f);
		mySpObj.transform.GetChild( 0 ).transform.localScale = new Vector3( scaleX, scaleY, 0.0f );
		// 読み込まれた画像の配列 ( index 番目の sprite )
		if( index != -1 ) { /* -1 の時, 画像を読み込まないようにする */
			mySpObj.transform.GetChild( 0 ).GetComponent<SpriteRenderer>( ).sprite = mySpImg[ index ];

		}


	}
	/*===============================================================*/


}
/*===============================================================*/