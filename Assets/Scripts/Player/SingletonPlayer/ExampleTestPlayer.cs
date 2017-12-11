using UnityEngine;
using UnityEngine.UI;

public class ExampleTestPlayer : MonoBehaviour {

	SingltonPlayerManager example1;
	SingltonPlayerManager example2;
	GV myGV;
	public GameObject btn;
	int saveTest;

	// Use this for initialization
	void Start( ) {
		Initialize( );
		

	}

	private void Initialize( ) {

		example1 = SingltonPlayerManager.Instance;
		example2 = SingltonPlayerManager.Instance;
		myGV  = GV.Instance;

		// クラスが参照されたタイミングで, セーブデータから値が読み込ます
		// インスタンス作成前に GV クラスに定義された関数を用いて保存を行います
		// セーブデータへの変動値保存 保存を実行した後にアンコメントアウトするとセーブされたのがロードされていると実感できます
		bool test = false;
		if ( test ) {
			for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
				SingltonPlayerManager.Instance.SaveDataPlayerState[ i ].HP = i + 100 * ( i + 10 );
				SingltonPlayerManager.Instance.SaveDataPlayerState[ i ].MP = i + 200 * ( i + 10 );

			}
			myGV.GameDataSave( GV.slot ); // セーブを行います

		}

		if ( example1 == example2 ) Debug.Log( "example1 == example2" );

		foreach ( SingltonPlayerManager.PlayerParameters items in example1.SaveDataPlayerState ) {
			Debug.Log( "-----------------------\nforeach ( セーブデータ ) 出力\nID : " + items.ID + "\nHP : " +
				items.HP + "\nMP : " + items.MP + "\nプレイタイム : "
					+ myGV.GData.playTime[ GV.slot ] + "\n-----------------------" );

		}

		foreach ( SingltonPlayerManager.PlayerParameters items in example1.GetCsvDataPlayerState ) {
			Debug.Log( "-----------------------\nforeach ( CSV データ ) 出力\nレベル : " + items.Lv + "\nHP : " +
				items.HP + "\nMP : " + items.MP + "\n仮 : "
					+ items.Skill + "\n-----------------------" );

		}

		// セーブデータに保存されている KEY を出力
		myGV.DebugKeyPrint( );

		btn.GetComponent<Button>( ).onClick.AddListener( ( ) => OnClick( btn ) );

		saveTest = 0;


	}

	public void OnClick( GameObject btn ) {

		saveTest += 10;

		for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			SingltonPlayerManager.Instance.SaveDataPlayerState[ i ].HP = i + 100 * ( i + saveTest );
			SingltonPlayerManager.Instance.SaveDataPlayerState[ i ].MP = i + 200 * ( i + saveTest );

		}
		myGV.GameDataSave( GV.slot ); // セーブを行います


	}


}