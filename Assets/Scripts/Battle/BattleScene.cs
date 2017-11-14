using UnityEngine;

/*===============================================================*/
/// <summary>戦闘シーン管理クラス</summary>
/// <remarks>BattleSceneオブジェクトにスクリプトを関連づけます</remarks>
public class BattleScene : MonoBehaviour {

	[SerializeField, TooltipAttribute( "戦闘シーン背景" )]
	// "directory : BattleScene/CanvasGame/Back"
	private GameObject imgBack; // 戦闘シーン背景

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


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによって毎フレーム呼ばれます</summary>
	void Update ( ) {
		// 場面転換関数を呼ぶ
		GameManager.FadeIn( imgBack.GetComponent<SpriteRenderer>( ) );


	}
	/*===============================================================*/


}