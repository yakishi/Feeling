using UnityEngine;

/*===============================================================*/
/// <summary>キャラクターのエフェクト処理を管理します</summary>
public class PlayerEffect {

	// HUD 作成クラスで生成されたゲームオブジェクトを保存する変数の準備
	private GameObject[ ] myPlayerObj;

	/*===============================================================*/
	/// <summary>コンストラクター</summary>
	public PlayerEffect( ) {
		Initialize( );
		Debug.Log( "Player Effect Class on the Constructor Function Call." );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	void Initialize( ) {
		myPlayerObj = HUD_BattleScene.CharaHudGameObj;
		//Debug.Log( myPlayerObj[ 0 ].transform.GetChild( 0 ).GetChild( 0 ) );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>味方(敵側からの攻撃を想定)に適応させたいエフェクトをセットし,再生します</summary>
	/// <remarks>
	/// triggerNameで適用するエフェクトが決定されます
	/// positionX,positionYは,scaleX,scaleYを1.0f以上以下にした時弄ります
	/// </remarks>
	/// <param name="players">生成された味方のどれに適用するか</param>
	/// <param name="triggerName">PlayerEffectControllerに定義されています</param>
	/// <param name="scaleX">Spriteの大きさ横幅を指定します</param>
	/// <param name="scaleY">Spriteの大きさ縦幅を指定します</param>
	/// <param name="positionX">SpriteのX軸方向の位置を指定します</param>
	/// <param name="positionY">SpriteのY軸方向の位置を指定します</param>
	public void SetEffectPlay( int players, string triggerName, float scaleX, float scaleY, float positionX, float positionY ) {
		Animator animator = myPlayerObj[ players ].transform.GetChild( 0 ).GetChild( 0 ).GetComponent<Animator>( );
		Transform transform = myPlayerObj[ players ].transform.GetChild( 0 ).GetChild( 0 );
		#pragma warning disable CS0618 // 型またはメンバーが古い形式です
		animator.ForceStateNormalizedTime( 0.0f ); // 初めから再生されるようにします
		#pragma warning restore CS0618 // 型またはメンバーが古い形式です
		animator.SetTrigger( triggerName );
		transform.localScale = new Vector3( scaleX, scaleY, 0.0f );
		transform.localPosition = new Vector3( positionX, positionY, 0.0f );
		Debug.Log( "<color='red'>プレイヤーエフェクト関数が呼ばれました。</color>" );


	}
	/*===============================================================*/


}
/*===============================================================*/