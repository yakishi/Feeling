using UnityEngine;

public class PrologueCanvasMono : MonoBehaviour {
    [SerializeField]
	float variableSpeed = 3.0f;
    const float fixedMoveSpeed = 0.1f;

	PrologueUI myUI;

	private void Start( ) {
		//PrologueAudio.PlayBGM( 0 );
		myUI = new PrologueUI( );
		myUI.PrologueUICreate( );


	}

	private void Update( ) {
		myUI.ScrollUpDown( ( Time.deltaTime * -fixedMoveSpeed ) / variableSpeed /* スクロール速度 */ );


	}


}