using UnityEngine;
using UnityEngine.UI;

public class ExampleTestSaveLoadMono : MonoBehaviour {

	public GameObject btn;

	ExampleTestSaveLoad my;

	// Use this for initialization
	void Start( ) {
		//Initialize( );
		btn.GetComponent<Button>( ).onClick.AddListener( ( ) => OnClick( btn ) );


		my = new ExampleTestSaveLoad( );


	}

	public void OnClick( GameObject btn ) {

		my.SaveTest( );


	}


}