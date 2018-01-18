using UnityEngine;
using UnityEngine.UI;

public class ExampleTestPlayer : MonoBehaviour {

	SingltonPlayerManager example1;
	GV myGV;
	public GameObject btn;
	int saveTest;

	// Use this for initialization
	void Start( ) {
		Initialize( );
		

	}

	private void Initialize( ) {
		
		example1 = SingltonPlayerManager.Instance;
		myGV  = GV.Instance;

		// クラスが参照されたタイミングで, セーブデータから値が読み込ます
		// インスタンス作成前に GV クラスに定義された関数を用いて保存を行います
		// セーブデータへの変動値保存 保存を実行した後にアンコメントアウトするとセーブされたのがロードされていると実感できます
		bool test = false;
		if ( test ) {
			// コンストラクタで呼び出して書き換える場合, セーブデータの削除が必要
			SaveData.remove( myGV.slot );

			for( int i = 0; i < example1.SaveDataPlayerState.Count; i++ ) {
				// CSV データ取得例したものをセーブデータに格納する例
				for( int j = 0; j < example1.GetCsvDataPlayerState.Count; j++ ) {
					// 例 : P1 ～ P6 ( Player1 から Player6 ) かつ ID が 7 のデータ
					if( example1.GetCsvDataPlayerState[ j ].Name == "P" + ( i + 1 ) &&
						example1.GetCsvDataPlayerState[ j ].ID == "7" ) {
						// 上の条件の時の, 感情値 ( CSV ) データを 現在の感情値 ( セーブデータ ) にいれる
						example1.SaveDataPlayerState[ i ].CFV = example1.GetCsvDataPlayerState[ j ].BES.FeelingValue;

					}

				}
				example1.SaveDataPlayerState[ i ].AES.HP = 100;
				example1.SaveDataPlayerState[ i ].AES.MP = 200;

			}
			myGV.GameDataSave( myGV.slot ); // セーブを行います

		}

		int cntTest = 0;
		foreach ( SingltonPlayerManager.PlayerParameters items in example1.SaveDataPlayerState ) {
			Debug.Log( "-----------------------\nforeach ( セーブデータ ) 出力\n現在の感情値 : " + items.CFV + "\nHP : " +
				items.AES.HP + "\nMP : " + items.AES.MP + "\n-----------------------" );
			cntTest++;

		}

		foreach ( SingltonPlayerManager.PlayerParameters items in example1.GetCsvDataPlayerState ) {
			Debug.Log( "-----------------------\nforeach ( CSV データ ) 出力\nプレイヤー種別 : " + items.Name + "\nHP : " +
				items.BES.HP + "\nMP : " + items.BES.MP + "\nID : "
					+ items.ID + "\n-----------------------" );

		}

		// セーブデータに保存されている KEY を出力
		myGV.DebugKeyPrint( );

		btn.GetComponent<Button>( ).onClick.AddListener( ( ) => OnClick( btn ) );

		saveTest = 0;


	}

	public void OnClick( GameObject btn ) {

		saveTest += 10;

		for ( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			SingltonPlayerManager.Instance.SaveDataPlayerState[ i ].AES.HP = i + 100 * ( i + saveTest );
			SingltonPlayerManager.Instance.SaveDataPlayerState[ i ].AES.MP = i + 200 * ( i + saveTest );

		}
		myGV.GameDataSave( myGV.slot ); // セーブを行います


	}


}