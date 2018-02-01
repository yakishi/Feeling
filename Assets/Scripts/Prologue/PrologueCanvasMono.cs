using UnityEngine;

public class PrologueCanvasMono : MonoBehaviour {
	PrologueUI myUI;

	private void Start( ) {
		PrologueAudio.PlayBGM( 0 );
		myUI = new PrologueUI( );
		myUI.PrologueUICreate( );


	}

	private void Update( ) {
		myUI.ScrollUpDown( -0.002f /* スクロール速度 */ );


	}


}