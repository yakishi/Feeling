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
			//myEquip.SaveDataPlayerEquipmentParam[ 0 ].ID = 9999;
			myEquip.SaveDataPlayerEquipmentParam[ 3 ].Accessory1 = "スライムピアス";
			myEquip.SaveDataPlayerEquipmentParam[ 5 ].Shoes = "重い靴";

			myGV.GameDataSave( myGV.slot );
			myGV.DebugKeyPrint( );

		}

		myGV.GameDataLoad( myGV.slot );

		// SAVEDATA データの取得例
		foreach( SingltonEquipmentManager.PlayerEquipmentParam items in myEquip.SaveDataPlayerEquipmentParam ) {
			Debug.Log( "<color='red'>SAVEDATA 装備 ID : " + items.ID + ", 武器 : " 
				+ items.Arms + ", 頭 : " + items.Head + ", 足 : " + items.Shoes + "</color>" );

		}

	
	}
	


}