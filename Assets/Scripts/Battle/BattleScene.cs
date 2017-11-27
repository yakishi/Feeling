using UnityEngine;

/*===============================================================*/
/// <summary>戦闘シーン管理クラス</summary>
/// <remarks>BattleSceneオブジェクトにスクリプトを関連づけます</remarks>
public class BattleScene : MonoBehaviour {

	[SerializeField, TooltipAttribute( "戦闘シーン背景" )]
	// "directory : BattleScene/CanvasGame/Back"
	private GameObject imgBack; // 戦闘シーン背景

	struct FadeBGM {
		public Color clr;
		public bool once;

	} FadeBGM myFadeBGM;

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	void Awake( ) {
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	void Initialize( ) {
		// 場面転換前準備
		SpriteRenderer renderer = imgBack.GetComponent<SpriteRenderer>( );
		Color color = renderer.color;
		color.a = 0.0f; /* 透明にしておく */
		renderer.color = color; // 変更した色情報に変更

		myFadeBGM.clr.a = 0.0f;
		myFadeBGM.once = true;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによって毎フレーム呼ばれます</summary>
	void Update ( ) {
		// 場面転換関数を呼ぶ
		if( myFadeBGM.clr.a <= 1.0 ) myFadeBGM.clr = GameManager.FadeIn( imgBack.GetComponent<SpriteRenderer>( ) );
		if( myFadeBGM.clr.a >= 1.0f && myFadeBGM.once ) {
			BattleAudio.PlayBGM( 0 );
			myFadeBGM.once = false;

		}


	}
	/*===============================================================*/


}