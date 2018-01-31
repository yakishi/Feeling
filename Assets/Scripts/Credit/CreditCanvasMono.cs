using UnityEngine;

public class CreditCanvasMono : MonoBehaviour {
	CreditUI myUI;

	private void Start( ) {
		myUI = new CreditUI( );
		myUI.CreditUiCreate( );


	}

	private void Update( ) {
		myUI.ScrollUpDown( -0.001f /* スクロール速度 */ );


	}


}