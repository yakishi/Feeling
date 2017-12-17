using UnityEngine;
using UnityEngine.UI;

public class ExampleTestSaveLoadMono : MonoBehaviour {

	public GameObject btn;
	public GameObject btnCntReset;

	ExampleTestSaveLoad my;

	// Use this for initialization
	void Start( ) {
		//Initialize( );
		btn.GetComponent<Button>( ).onClick.AddListener( ( ) => OnClick( btn ) );
		btnCntReset.GetComponent<Button>( ).onClick.AddListener( ( ) => OnClick2( btnCntReset ) );


		my = new ExampleTestSaveLoad( );


	}

	public void OnClick( GameObject btn ) {

		my.SaveTest( );


	}

	public void OnClick2( GameObject btn ) {

		my.CountReset( );


	}


}