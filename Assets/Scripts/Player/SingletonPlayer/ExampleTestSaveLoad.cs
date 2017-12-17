using UnityEngine;

public class ExampleTestSaveLoad {

	GV myGV;
	// 一括で作成する理由は GV.cs:218 を参照してください。この場合は test のためここで一括で作成しています。
	SingltonPlayerManager example1;
	SingltonEquipmentManager example2;
	//
	static int saveTest;

	public ExampleTestSaveLoad( ) {
		myGV = GV.Instance;
		// 一括で作成する理由は GV.cs:218 を参照してください。インスタンスを一括で作成, この場合は test のためここで一括で作成しています。
		example1 = SingltonPlayerManager.Instance;
		example2 = SingltonEquipmentManager.Instance;
		//
		saveTest = example1.SaveDataPlayerState[ 0 ].ID;
		Debug.Log( saveTest );

		// セーブデータに保存されている KEY を出力
		myGV.DebugKeyPrint( );


	}

	public void LoadTest( ) {

		myGV.GameDataLoad( myGV.slot ); // 値取得の前に必ず game data load を行います

		Debug.Log( "<color='red'>SaveDataPlayerState[ 0 ].ID : " + example1.SaveDataPlayerState[ 0 ].ID + "\n"
			+ "SaveDataPlayerState[ 1 ].HP : " + example1.SaveDataPlayerState[ 1 ].HP + "\n"
			+ "SaveDataPlayerState[ 2 ].HP : " + example1.SaveDataPlayerState[ 2 ].HP + "\n"
			+ "SaveDataPlayerEquipmentParam[ 0 ].ID : " + example2.SaveDataPlayerEquipmentParam[ 0 ].ID + "\n"
			+ "SaveDataPlayerEquipmentParam[ 1 ].Accessory2 : " + example2.SaveDataPlayerEquipmentParam[ 1 ].Accessory2 + "\n"
			+ "myGV.GData.playTime[ GV.slot ] : " + myGV.GData.playTime[ myGV.slot ] + "\n"
			+ "GV.slot : " + myGV.slot + "</color>" );

		// セーブデータに保存されている KEY を出力
		myGV.DebugKeyPrint( );


	}

	public void SaveTest( ) {

		saveTest++;
		Debug.Log( saveTest );
		for( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			// player
			example1.SaveDataPlayerState[ i ].HP = i + 100 * ( i + saveTest );
			example1.SaveDataPlayerState[ i ].MP = i + 200 * ( i + saveTest );

			// equip
			example2.SaveDataPlayerEquipmentParam[ i ].ID = i + ( saveTest + 1000 );
			example2.SaveDataPlayerEquipmentParam[ i ].Accessory2 = "save test : " + saveTest + i;

		}
		example1.SaveDataPlayerState[ 0 ].ID = saveTest;

		myGV.GameDataSave( myGV.slot ); // セーブを行います


	}

	public void CountReset( ) {

		saveTest = 0;
		Debug.Log( "<color='red'>カウントをリセットしました。</color>" );


	}


}