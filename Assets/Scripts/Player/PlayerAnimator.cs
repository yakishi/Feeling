using UnityEngine;

/*===============================================================*/
/// <summary>Playerのアニメーションを管理します,プレイヤーオブジェクトに割り当てます
public class PlayerAnimator : PlayerMover {

	// プレイヤーアニメーション制御用 Animator
	private Animator animatorObj;

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	void Awake( ) {
		// 初期化関数を呼び出す
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	void Initialize( ) {
		// プレイヤーの Animator コンポーネントをセットしておきます
		animatorObj = GetComponent<Animator>( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによって毎フレーム呼ばれます</summary>
	void Update( ) {
		// プレイヤーの左右移動アニメーション制御
		switch ( moveDirection ) {
			// 停止
			case MOVE_DIR.STOP : {
				animatorObj.speed = 0.0f;
				animatorObj.SetBool( "IsWalkLeft", false );
				animatorObj.SetBool( "IsWalkRight", false );
				break;

			}
			// 左アニメーション
			case MOVE_DIR.LEFT : {
				animatorObj.SetBool( "IsWalkLeft", true );
				animatorObj.speed = 1.0f;
				break;

			}
			// 右アニメーション
			case MOVE_DIR.RIGHT : {
				animatorObj.SetBool( "IsWalkRight", true );
				animatorObj.speed = 1.0f;
				break;

			}

		}

		//Debug.Log( moveDirection );

		
	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>生成された立ち絵プレハブのアニメーション操作</summary>
	/// <param name="slideAnimType">0:SlideOut,1:SlideInで指定します</param>
	static public void StandPictureSlide( int slideAnimType ) {
		// バトルシーンで生成されたオブジェクトを格納します
		GameObject slide = HUD_BattleScene.StandPicture;
		// アニメーターコンポーネントの取得
		Animator anim = slide.GetComponent<Animator>( );
		// Trigger による animation 制御
		if( slideAnimType == 1 ) { /* スライドインアニメーション */
			//anim.SetBool( "IsSlideOut", false );
			//anim.SetBool( "IsSlideIn", true );
			anim.Play( "slideIn" );

		} else if( slideAnimType == 0 ) { /* スライドアウトアニメーション */
			//anim.SetBool( "IsSlideIn", false );
			//anim.SetBool( "IsSlideOut", true );
			anim.Play( "slideOut" );

		} else Debug.LogError( "引数が不正です！" );
		

	}
	/*===============================================================*/


}