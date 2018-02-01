using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>PrologueUIクラス</summary>
public class PrologueUI {

	/// <summary>タイトルへシーン遷移する秒数</summary>
	const int transitionTitle = 5 * 1000;

	struct WindowSize {
		public float width;
		public float height;

	} WindowSize myWindowSize;

	struct SpriteSize {
		public float width;
		public float height;

	} SpriteSize mySpriteSize;

	struct UI_GameObject {
		public GameObject Prologue;
		public GameObject Camera;

	} UI_GameObject myUG;

	struct UI_LinkFlg {
		public bool jumpOnce;

	} UI_LinkFlg myUL;

	/// <summary>コンストラクター</summary>
	public PrologueUI( ) {
		// display size
		myWindowSize.width = /*(float)Screen.width*/1280.0f;
		myWindowSize.height = /*(float)Screen.height*/960.0f;
		// sprite size
		mySpriteSize.width = 1280.0f;
		mySpriteSize.height = 960.0f;

		myUL.jumpOnce = true;

		myUG.Prologue = GameObject.Find( "PrologueUI" );

		CameraSetting( );


	}

	/// <summary>カメラSetting</summary>
	private void CameraSetting( ) {
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
	public void PrologueUICreate( ) {
		GameObject ui = myUG.Prologue;
		RectTransform rc = ui.GetComponent<RectTransform>( );
		VerticalLayoutGroup content = ui.transform.GetChild( 0 ).GetChild( 0 ).GetComponent<VerticalLayoutGroup>( );
		rc.sizeDelta = new Vector2( mySpriteSize.width, myWindowSize.height );
		// content group
		content.padding = new RectOffset( 0, 0, Mathf.RoundToInt( mySpriteSize.height ), 0 );
		RectTransform rc2 = content.transform.GetChild( 0 ).GetComponent<RectTransform>( );
		rc2.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
		// Scrollbar Vertical Group
		GameObject scrollV_Obj = myUG.Prologue.transform.GetChild( 2 ).gameObject;
		GameObject scrollV_Area = scrollV_Obj.transform.GetChild( 0 ).gameObject;
		scrollV_Area.SetActive( false );


	}

	/// <summary>クレジットスクロールの上下スクロールをコントロールします</summary>
	/// <param name="plusORminus">' -0.001f 'または' 0.001f 'でUP・DOWN</param>
	public void ScrollUpDown( float plusORminus ) {
		Scrollbar scroll = myUG.Prologue.transform.GetChild( 2 ).GetComponent<Scrollbar>( );
		scroll.value += plusORminus;
		// title scene jump
		if( scroll.value <= 0.0f && myUL.jumpOnce ) {
			myUL.jumpOnce = false;
			// n 秒後に jump
			Observable.Timer( TimeSpan.FromMilliseconds( transitionTitle ) )
				.Subscribe( _ =>
					SceneController.sceneTransition(
						/* TODO : 仮でタイトルに移動 */
						Enum.GetName( typeof( SceneName.SceneNames ), 0 ), 2.0f, SceneController.FadeType.Fade

					)
				);

		}


	}


}