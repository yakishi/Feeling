using UnityEngine;

public class PrologueCanvasMono : MonoBehaviour {
    [SerializeField]
    float moveSpeed = 0.1f;

	PrologueUI myUI;

	private void Start( ) {
		PrologueAudio.PlayBGM( 0 );
		myUI = new PrologueUI( );
		myUI.PrologueUICreate( );


	}

	private void Update( ) {
		myUI.ScrollUpDown( -moveSpeed * Time.deltaTime /* スクロール速度 */ );


	}


}