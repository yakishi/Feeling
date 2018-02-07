using UnityEngine;

public class ExampleTestItem : MonoBehaviour {

	SingltonItemManager myItem;
	GV myGV;

	// Use this for initialization
	void Start( ) {
		myItem = SingltonItemManager.Instance;
		myGV = GV.Instance;

		// CSV データの取得例
		foreach ( SingltonItemManager.ItemList items in myItem.CDItem ) {
			Debug.Log( "CSVDATA アイテム ID : " + items.id
				+ "\nアイテム種別 : " + items.name
				+ "\n感情値変動種別 : " + items.max
				+ "\n所持できるアイテム最大個数 : " + items.area
				+ "\nヒットポイント回復値 : " + items.value
				+ "\nマジックポイント回復値 : " + items.category
				+ "\n変動する感情値変動数値 : " + items.target
				+ "\nアイテム名 : " + items.scope
				+ "\nアイテムの詳細 : " + items.Detail );

		}

		bool isSave = true; // true → false にすることで save された値を確認できます
		if ( isSave ) {

			// 保存されているキーを取得します
			foreach ( string key in SaveData.getKeys( ) ) {
				// アイテムのセーブデータキーが保存されていたら
				if ( key == GV.SaveDataKey.ITEM_PARAM ) {
					// add された要素を削除します
                    myItem.SDItem.itemList.Clear();

				}

			}

			// アイテム一覧 CSV を参照
			foreach( SingltonItemManager.ItemList item in myItem.CDItem ) {
				if( item.id == "I0" ) /* CSV の ID 0 番目を参照してセーブデータに入れる */ {
                    // 特定のアイテムの所持した時に, ID を用いて CSV から情報を持ってくる時に使えます ( 参考程度 )
                    myItem.SDItem.itemList.Add(item.id, item.max);

				}
				if( item.id == "I1" ) {
                    myItem.SDItem.itemList.Add(item.id, item.max);
                }

			}

			myItem.SDItem.possessionGolds = 1500;

			myGV.GameDataSave( myGV.slot );
			myGV.DebugKeyPrint( );

		} else SaveData.remove( myGV.slot );

		myGV.GameDataLoad( myGV.slot );

		// SAVEDATA データの取得例
		foreach ( string key in myItem.SDItem.itemList.Keys )
			Debug.Log( "<color='red'>セーブデータ\n現在所持しているアイテム : " + key
			+ "\n現在所持しているアイテムに対するアイテム所持数 : " + myItem.SDItem.itemList[ key ]
			+ "\n現在所持している所持金 : " + myItem.SDItem.possessionGolds
			+ "</color>" );

		}


}
