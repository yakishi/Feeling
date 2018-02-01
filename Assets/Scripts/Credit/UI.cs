using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>CreditUIクラス</summary>
/// <remarks>
/// アスペクト比,16:9の環境で正しく表示されるのを確認しています。
/// </remarks>
public class CreditUI {

	/// <summary>タイトルへシーン遷移する秒数</summary>
	const int transitionTitle = 10 * 1000;

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

	struct UI_LinkFlg {
		public bool jumpOnce;

	} UI_LinkFlg myUL;

	/// <summary>コンストラクター</summary>
	public CreditUI( ) {
		// display size
		myWindowSize.width = (float)Screen.width;
		myWindowSize.height = (float)Screen.height;
		// sprite size
		mySpriteSize.width = 1280.0f;
		mySpriteSize.height = 960.0f;

		myUL.jumpOnce = true;

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
		content.padding = new RectOffset( 0, 0, Mathf.RoundToInt( mySpriteSize.height ), -190 );
		RectTransform rc2 = content.transform.GetChild( 0 ).GetComponent<RectTransform>( );
		rc2.localScale = new Vector3( 0.84f, 0.7f, 1.0f );


	}

	/// <summary>クレジットスクロールの上下スクロールをコントロールします</summary>
	/// <param name="plusORminus">' -0.001f 'または' 0.001f 'でUP・DOWN</param>
	public void ScrollUpDown( float plusORminus ) {
		Scrollbar scroll = myUG.Credit.transform.GetChild( 2 ).GetComponent<Scrollbar>( );
		scroll.value += plusORminus;
		// title scene jump
		if( scroll.value <= 0.0f && myUL.jumpOnce ) {
			myUL.jumpOnce = false;
			// n 秒後に jump
			Observable.Timer( TimeSpan.FromMilliseconds( transitionTitle ) )
				.Subscribe( _ => 
					SceneController.sceneTransition(
						Enum.GetName( typeof( SceneName.SceneNames ), 0 ), 2.0f, SceneController.FadeType.Fade

					)
				);

		}


	}


}