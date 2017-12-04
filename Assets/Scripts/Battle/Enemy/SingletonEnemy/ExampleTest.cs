using UnityEngine;

public class ExampleTest : MonoBehaviour {

	SingltonEnemyManager example1;
	SingltonEnemyManager example2;

	// Use this for initialization
	void Start( ) {
		Initialize( );

		
	}

	private void Initialize( ) {

		example1 = SingltonEnemyManager.Instance;
		example2 = SingltonEnemyManager.Instance;
		
		foreach( SingltonEnemyManager.EnemyParameters items in example1.GetEnemyState ) {
			Debug.Log( "-----------------------\nforeach 出力\nID : " + items.ID + "\nLV : " +
				items.LV + "\nNAME : " + items.NAME + "\n-----------------------" );

		}


	}


}