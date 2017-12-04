using UnityEngine;

public class ExampleTestPlayer : MonoBehaviour {

	SingltonPlayerManager example1;
	SingltonPlayerManager example2;

	// Use this for initialization
	void Start( ) {
		Initialize( );

		
	}

	private void Initialize( ) {

		// クラスが参照されたタイミングで, セーブデータから値が読み込ます
		// インスタンス作成前に GV クラスに定義された関数を用いて保存を行います
		// セーブデータへの変動値保存 保存を実行した後にアンコメントアウトするとセーブされたのがロードされていると実感できます
		for( int i = 0; i < GV.GData.Players.Count; i++ ) GV.PlayersSetHPSave( i, i + 10 * ( i + 10 ) );
		for( int i = 0; i < GV.GData.Players.Count; i++ ) GV.PlayersSetMPSave( i, i + 20 * ( i + 10 ) );

		example1 = SingltonPlayerManager.Instance;
		example2 = SingltonPlayerManager.Instance;

		if( example1 == example2 ) Debug.Log( "example1 == example2" );
		
		foreach( SingltonPlayerManager.PlayerParameters items in example1.GetSaveDataPlayerState ) {
			Debug.Log( "-----------------------\nforeach ( セーブデータ ) 出力\nID : " + items.ID + "\nHP : " +
				items.HP + "\nMP : " + items.MP + "\nタイムスタンプ : " 
					+ SaveData.getString( GV.SaveDataKey.KEY_TIME, "-1" ) + "\n-----------------------" );

		}

		foreach( SingltonPlayerManager.PlayerParameters items in example1.GetCsvDataPlayerState ) {
			Debug.Log( "-----------------------\nforeach ( CSV データ ) 出力\nレベル : " + items.Lv + "\nHP : " +
				items.HP + "\nMP : " + items.MP + "\n仮 : " 
					+ items.Skill + "\n-----------------------" );

		}

		// セーブデータに保存されている KEY を出力
		GV.DebugKeyPrint( );


	}


}