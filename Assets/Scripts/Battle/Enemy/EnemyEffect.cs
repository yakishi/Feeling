using UnityEngine;

/*===============================================================*/
/// <summary>敵のエフェクト処理を管理します</summary>
public class EnemyEffect {

	// 適生成クラスで生成されたオブジェクトを保存する変数の準備
	static GameObject[ ] EnemyObj = BattleEnemyGenerate.Enemy;

	/*===============================================================*/
	/// <summary>敵に適応させたいエフェクトをセットし,再生します</summary>
	/// <remarks>
	/// triggerNameで適用するエフェクトが決定されます
	/// positionX,positionYは,scaleX,scaleYを1.0f以上以下にした時弄ります
	/// </remarks>
	/// <param name="enemies">生成された敵のどれに適用するか</param>
	/// <param name="triggerName">EnemyEffectControllerに定義されています</param>
	/// <param name="scaleX">Spriteの大きさ横幅を指定します</param>
	/// <param name="scaleY">Spriteの大きさ縦幅を指定します</param>
	/// <param name="positionX">SpriteのX軸方向の位置を指定します</param>
	/// <param name="positionY">SpriteのY軸方向の位置を指定します</param>
	static public void SetEffectPlay( int enemies, string triggerName, float scaleX, float scaleY, float positionX, float positionY ) {
		Animator animator = EnemyObj[ enemies ].transform.GetChild( 1 ).GetComponent<Animator>( );
		Transform transform = EnemyObj[ enemies ].transform.GetChild( 1 );
		#pragma warning disable CS0618 // 型またはメンバーが古い形式です
		animator.ForceStateNormalizedTime( 0.0f ); // 初めから再生されるようにします
		#pragma warning restore CS0618 // 型またはメンバーが古い形式です
		animator.SetTrigger( triggerName );
		transform.localScale = new Vector3( scaleX, scaleY, 0.0f );
		transform.localPosition = new Vector3( positionX, positionY, 0.0f );
		//Debug.Log( "<color='red'>SwordEffect : " + EnemyObj[ 0 ].transform.GetChild( 1 ) + "</color>" );


	}
	/*===============================================================*/


}
/*===============================================================*/