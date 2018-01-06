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
			Debug.Log( "CSVDATA アイテム ID : " + items.ID
				+ "\nアイテム種別 : " + items.TYPE
				+ "\n感情値変動種別 : " + items.TYPE2
				+ "\n所持できるアイテム最大個数 : " + items.MAX
				+ "\nヒットポイント回復値 : " + items.HP
				+ "\nマジックポイント回復値 : " + items.MP
				+ "\n変動する感情値変動数値 : " + items.FEELINGVALUE
				+ "\nアイテム名 : " + items.NAME
				+ "\nアイテムの詳細 : " + items.DETAIL );

		}

		bool isSave = true; // true → false にすることで save された値を確認できます
		if ( isSave ) {

			// 保存されているキーを取得します
			foreach ( string key in SaveData.getKeys( ) ) {
				// アイテムのセーブデータキーが保存されていたら
				if ( key == GV.SaveDataKey.ITEM_PARAM ) {
					// add された要素を削除します
					myItem.SDItem.Name.RemoveAt( 0 );
					myItem.SDItem.Name.RemoveAt( 0 );
					myItem.SDItem.Stock.RemoveAt( 0 );
					myItem.SDItem.Stock.RemoveAt( 0 );

				}

			}

			// アイテム一覧 CSV を参照
			foreach( SingltonItemManager.ItemList items in myItem.CDItem ) {
				if( items.ID == 0 ) /* CSV の ID 0 番目を参照してセーブデータに入れる */ {
					// 特定のアイテムの所持した時に, ID を用いて CSV から情報を持ってくる時に使えます ( 参考程度 )
					myItem.SDItem.Name.Add( items.NAME );
					myItem.SDItem.Stock.Add( 5 /*items.MAX*/ );

				}
				if( items.ID == 1 ) {
					myItem.SDItem.Name.Add( items.NAME );
					myItem.SDItem.Stock.Add( 10 /*items.MAX*/ );

				}

			}

			myGV.GameDataSave( myGV.slot );
			myGV.DebugKeyPrint( );

		}

		myGV.GameDataLoad( myGV.slot );

		// SAVEDATA データの取得例
		for( int i = 0; i < myItem.SDItem.Name.Count; i++ ) {
			Debug.Log( "<color='red'>セーブデータ\n現在所持しているアイテム : " + myItem.SDItem.Name[ i ]
			+ "\n現在所持しているアイテムに対するアイテム所持数 : " + myItem.SDItem.Stock[ i ] + "</color>" );

		}


	}


}