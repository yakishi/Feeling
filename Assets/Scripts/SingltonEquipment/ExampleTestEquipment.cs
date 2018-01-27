using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleTestEquipment : MonoBehaviour {

	SingltonEquipmentManager myEquip;
	GV myGV;

	// Use this for initialization
	void Start( ) {
		myEquip = SingltonEquipmentManager.Instance;
		myGV = GV.Instance;

		// CSV データの取得例
		foreach( SingltonEquipmentManager.EquipmentArticleList items in myEquip.GetCsvDataPlayerState ) {
			Debug.Log( "CSVDATA 装備 ID : " + items.ID + ", 装備名 : " 
				+ items.Name + ", Type : " + items.Type + ". Type2 : " + items.Type2 );

		}

		bool isSave = true; // true → false にすることで save された値を確認できます
		if( isSave ) {
			for( int i = 0; i < myGV.GData.Players.Count; i++ ) myEquip.SaveDataPlayerEquipmentParam[ i ].ID = ( i + 1 ).ToString( );
			myEquip.SaveDataPlayerEquipmentParam[ 3 ].Accessory1 = "スライムピアス";
			myEquip.SaveDataPlayerEquipmentParam[ 5 ].Shoes = "重い靴";
			myEquip.SaveDataPlayerEquipmentParam[ 1 ].Armor = "伊狩鎧";

			myGV.GameDataSave( myGV.slot );
			myGV.DebugKeyPrint( );

		} else SaveData.remove( myGV.slot );

		myGV.GameDataLoad( myGV.slot );

		// SAVEDATA データの取得例
		foreach( SingltonEquipmentManager.PlayerEquipmentParam items in myEquip.SaveDataPlayerEquipmentParam ) {
			Debug.Log( "<color='red'>SAVEDATA 装備 ID : " + items.ID + "\n武器 : " 
				+ items.Arms + "\n頭 : " + items.Head + "\n足 : " + items.Shoes
				+ "\n鎧 : " + items.Armor + "</color>" );

		}

	
	}
	


}