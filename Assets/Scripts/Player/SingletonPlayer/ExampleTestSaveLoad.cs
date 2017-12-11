using UnityEngine;

public class ExampleTestSaveLoad {

	GV myGV;
	SingltonPlayerManager example1;
	static int saveTest;

	public ExampleTestSaveLoad( ) {
		myGV = GV.Instance;
		example1 = SingltonPlayerManager.Instance;
		saveTest = example1.SaveDataPlayerState[ 0 ].ID;

		// セーブデータに保存されている KEY を出力
		myGV.DebugKeyPrint( );


	}

	public void LoadTest( ) {
		//foreach( SingltonPlayerManager.PlayerParameters items in example1.SaveDataPlayerState ) {
		//	Debug.Log( "-----------------------\nforeach ( セーブデータ ) 出力\nID : " + items.ID + "\nHP : " +
		//		items.HP + "\nMP : " + items.MP + "\nプレイタイム : " 
		//			+ myGV.GData.playTime[ GV.slot ] + "\n-----------------------" );

		//}

		//myGV.GameDataLoad( GV.slot );
		//SingltonPlayerManager.Instance.LoadPlayer( GV.slot );

		Debug.Log( "<color='red'>SaveDataPlayerState[ 0 ].ID : " + example1.SaveDataPlayerState[ 0 ].ID + "\n"
			+ "myGV.GData.playTime[ GV.slot ] : " + myGV.GData.playTime[ GV.slot ] + "\n"
			+ "GV.slot : " + GV.slot + "</color>" );

		// セーブデータに保存されている KEY を出力
		myGV.DebugKeyPrint( );


	}

	public void SaveTest( ) {

		saveTest++;

		for( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			example1.SaveDataPlayerState[ i ].HP = i + 100 * ( i + saveTest );
			example1.SaveDataPlayerState[ i ].MP = i + 200 * ( i + saveTest );

		}
		example1.SaveDataPlayerState[ 0 ].ID = saveTest;

		myGV.GameDataSave( GV.slot ); // セーブを行います

		//foreach( SingltonPlayerManager.PlayerParameters items in example1.SaveDataPlayerState ) {
		//	Debug.Log( "-----------------------\nforeach ( セーブデータ ) 出力\nID : " + items.ID + "\nHP : " +
		//		items.HP + "\nMP : " + items.MP + "\nプレイタイム : " 
		//			+ myGV.GData.playTime[ GV.slot ] + "\n-----------------------" );

		//}

		//// セーブデータに保存されている KEY を出力
		//myGV.DebugKeyPrint( );


	}


}