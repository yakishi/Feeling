using UnityEngine;
using UnityEngine.UI;

/// <summary>CreditUIクラス</summary>
/// <remarks>
/// アスペクト比,16;9または16:10の環境で正しく表示されるのを確認しています。
/// </remarks>
public class CreditUI {

	struct WindowSize {
		public float width;
		public float height;

	} WindowSize myWindowSize;

	struct SpriteSize {
		public float width;
		public float height;

	} SpriteSize mySpriteSize;

	struct UI_GameObject {
		public GameObject Credit;
		public GameObject Camera;

	} UI_GameObject myUG;

	/// <summary>コンストラクター</summary>
	public CreditUI( ) {
		// display size
		myWindowSize.width = (float)Screen.width;
		myWindowSize.height = (float)Screen.height;
		// sprite size
		mySpriteSize.width = 1280.0f;
		mySpriteSize.height = 960.0f;

		myUG.Credit = GameObject.Find( "CreditUI" );

		CameraSetting( );
		


	}

	/// <summary>カメラSetting</summary>
	private void CameraSetting( ) {
		float aspect = myWindowSize.height / myWindowSize.width;
		float bgAcpect = mySpriteSize.height / mySpriteSize.width;

		// カメラコンポーネントを取得します
		myUG.Camera = GameObject.Find( "Main Camera" );
		Camera camC = myUG.Camera.GetComponent<Camera>( );
		// カメラの orthographicSize を設定
		camC.orthographicSize = ( mySpriteSize.height / 2.0f / 100.0f );

		// 倍率
		float bgScale = mySpriteSize.width / myWindowSize.width;
		// viewport rectの幅
		float camHeight = mySpriteSize.height / ( myWindowSize.height * bgScale );
		// viewportRectを設定
		camC.rect = new Rect( 0.0f, ( 1.0f - camHeight ) / 2.0f, 1.0f, camHeight );


	}

	/// <summary>クレジット画面を作ります</summary>
	public void CreditUiCreate( ) {
		GameObject ui = myUG.Credit;
		RectTransform rc = ui.GetComponent<RectTransform>( );
		VerticalLayoutGroup content = ui.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<VerticalLayoutGroup>( );
		rc.sizeDelta = new Vector2( mySpriteSize.width, myWindowSize.height );
		// content group
		content.padding = new RectOffset( 70, 0, Mathf.RoundToInt( mySpriteSize.height ), -130 );
		RectTransform rc2 = content.GetComponent<RectTransform>( );
		rc2.localScale = new Vector3( 0.9f, 0.98f, 1.0f );


	}

	/// <summary>クレジットスクロールの上下スクロールをコントロールします</summary>
	/// <param name="plusORminus">' -0.001f 'または' 0.001f 'でUP・DOWN</param>
	public void ScrollUpDown( float plusORminus ) {
		Scrollbar scroll = myUG.Credit.transform.GetChild( 2 ).GetComponent<Scrollbar>( );
		scroll.value += plusORminus;


	}


}