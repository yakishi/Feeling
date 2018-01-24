using System;
using UnityEngine;

public class ExampleTestSaveLoad {

	GV myGV;
	// 一括で作成する理由は GV.cs:231 を参照してください。この場合は test のためここで一括で作成しています。
	SingltonPlayerManager example1;
	SingltonEquipmentManager example2;
	SingltonSkillManager example3;
	SingltonItemManager example4;
	SingltonFlagManager example5;
	//
	static int saveTest;

	private GameObject copyCanvasTitle;
	public GameObject canvasTitle { set { copyCanvasTitle = value; } }

	public ExampleTestSaveLoad( ) {
		myGV = GV.Instance;
		// 一括で作成する理由は GV.cs:231 を参照してください。インスタンスを一括で作成, この場合は test のためここで一括で作成しています。
		example1 = SingltonPlayerManager.Instance;
		example2 = SingltonEquipmentManager.Instance;
		example3 = SingltonSkillManager.Instance;
		example4 = SingltonItemManager.Instance;
		example5 = SingltonFlagManager.Instance;
		//
		saveTest = example1.SaveDataPlayerState[ 0 ].Lv;
		Debug.Log( saveTest );

		// セーブデータに保存されている KEY を出力
		myGV.DebugKeyPrint( );


	}

	public void LoadTest( ) {

		//myGV.GameDataLoad( myGV.slot ); // 値取得の前に必ず game data load を行います

		Debug.Log( "<color='red'>SaveDataPlayerState[ 0 ].LV : " + example1.SaveDataPlayerState[ 0 ].Lv + "\n"
			/* player */
			+ "SaveDataPlayerState[ 1 ].HP : " + example1.SaveDataPlayerState[ 1 ].EquipmentStatus.HP + "\n"
			+ "SaveDataPlayerState[ 2 ].HP : " + example1.SaveDataPlayerState[ 2 ].EquipmentStatus.HP + "\n"
			/* equip */
			+ "SaveDataPlayerEquipmentParam[ 0 ].ID : " + example2.SaveDataPlayerEquipmentParam[ 0 ].ID + "\n"
			+ "SaveDataPlayerEquipmentParam[ 1 ].Accessory2 : " + example2.SaveDataPlayerEquipmentParam[ 1 ].Accessory2 + "\n"
			+ "GV.slot : " + myGV.slot + "</color>" );

		foreach( SingltonPlayerManager.PlayerParameters items1 in example1.SaveDataPlayerState ) {
			foreach( string items2 in items1.SkillList ) {
				Debug.Log( "<color='red'>セーブデータ ( スキルリスト ) : " + items2 + "</color>" );

			}

		}
		
		foreach( SingltonSkillManager.SkillParam items1 in example3.SDSkill ) {
			foreach( string items2 in items1.Name) {
				Debug.Log( "<color='red'>セーブデータ ( スキル ) : " + items2 + "</color>" );

			}

		}

		foreach ( string key in example4.SDItem.itemList.Keys ) {
			Debug.Log( "<color='red'>セーブデータ ( アイテム )\n"
				+ "( Key : Value ) = ( " + key + " : " + example4.SDItem.itemList[ key ] + " )"
				+ "</color>" );

		}

		foreach ( string key in example5.SDFlg.EventFlag.Keys ) {
			Debug.Log( "<color='red'>セーブデータ ( イベント ) " +
				"( Key : Value ) : ( " + key + " : " + example5.SDFlg.EventFlag[ key ] + " )</color>" );

		}

		TimeSpan t = new TimeSpan( 0, 0, myGV.GData.timeSecond );
		Debug.Log( "slot " + myGV.slot + " load time : " + t ); 

		// セーブデータに保存されている KEY を出力
		myGV.DebugKeyPrint( );


	}

	public void SaveTest( ) {

		// 注意! ゲーム開始時 ロードを行ってから, セーブを行うようにします
		// ゲーム開始時のロードスロットが 1 のため, 始めたいゲームデータスロットを選んでスロットを更新するようにして下さい
		// ゲーム開始時に 1 セーブスロットから開始したい場合には, ロードを行わないでも問題ありません
		// 初期セーブスロットの定義は, GV:19 です

		// セーブする値の仮セット
		saveTest++;
		Debug.Log( saveTest );
		for( int i = 0; i < myGV.GData.Players.Count; i++ ) {
			// player
			example1.SaveDataPlayerState[ i ].EquipmentStatus.HP = i + 100 * ( i + saveTest );
			example1.SaveDataPlayerState[ i ].EquipmentStatus.MP = i + 200 * ( i + saveTest );
			example1.SaveDataPlayerState[ i ].SkillList.Add( "プレイヤー " + i + " にスキル" + ( i + saveTest + 10 ) + "名を追加しました。" );

			// equip
			example2.SaveDataPlayerEquipmentParam[ i ].ID = i + ( saveTest + 1000 );
			example2.SaveDataPlayerEquipmentParam[ i ].Accessory2 = "save test : " + saveTest + i;

			// skill
			example3.SDSkill[ i ].Name.Add( "プレイヤー " + i + " にスキル" + i + saveTest + "名を追加しました。" );

            // item
            example4.SDItem.itemList.Add("アイテム" + i + "を追加しました。", i);

			// event
			example5.SDFlg.EventFlag.Add( "Event:Talk" + ( i + saveTest ), false );
			//try {
			//	example5.SDFlg.EventFlag.Add( "Event:Talk" + ( i + saveTest ), false ); 

			//} catch ( ArgumentException e ) /* 状況によって使い分けてください 例外をキャッチしない場合, セーブ段階で止まりセーブされません */ {
			//	Debug.LogError( e.Message );

			//}

		}
		example1.SaveDataPlayerState[ 0 ].Lv = saveTest;

		//myGV.GameDataSave( myGV.slot ); // セーブを行います セーブ UI を開いた先で, セーブが行われるので不必要
		// セーブ UI を開く 開いた先でセーブが行われる
		SaveLoad.CreateUI( SaveLoad.Type.Save, copyCanvasTitle /* TitleCanvas の ゲームオブジェクトが必要 */ );

		//TimeSpan t = new TimeSpan( 0, 0, myGV.GData.timeSecond[ myGV.slot ] );
		//Debug.Log( "slot " + myGV.slot + " save time : " + t );


	}

	public void CountReset( ) {

		saveTest = 0;
		Debug.Log( "<color='red'>カウントをリセットしました。</color>" );


	}


}