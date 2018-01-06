using UnityEngine;

public class ExampleTestSkill : MonoBehaviour {

	SingltonSkillManager mySkill;
	GV myGV;

	// Use this for initialization
	void Start( ) {
		mySkill = SingltonSkillManager.Instance;
		myGV = GV.Instance;

		int cnt, cnt2 = 1;
		// CSV データの取得例
		foreach ( SingltonSkillManager.SkillInfo items in mySkill.CDSkill ) {
			Debug.Log( "CSVDATA スキル ID : " + items.ID
				+ ", スキル名 : " + items.NAME
				+ ", 魔法倍率 : " + items.MAGICDIAMETER
				+ ", スキル威力 : " + items.SKILLPOWER
				+ ", 習得値 : " + items.LEARNINGFEELINGVALUE );
				cnt = items.ID;
				if( cnt > 8 ) {
					Debug.Log( "<color='red'>↑プレイヤー" + cnt2 + "↑</color>" );
					cnt = 0;
					cnt2++;

				}

		}

		bool isSave = true; // true → false にすることで save された値を確認できます
		if ( isSave ) {

			// 保存されているキーを取得します
			foreach( string key in SaveData.getKeys( ) ) {
				// スキルのセーブデータキーが保存されていたら
				if( key == GV.SaveDataKey.SKILL_PARAM ) {
					// add された要素を削除します
					mySkill.SDSkill[ 0 ].Name.RemoveAt( 0 );
					mySkill.SDSkill[ 1 ].Name.RemoveAt( 0 );
					mySkill.SDSkill[ 3 ].Name.RemoveAt( 0 );
					mySkill.SDSkill[ 3 ].Name.RemoveAt( 0 );

				}

			}

			mySkill.SDSkill[ 0 ].Name.Add( "プレイヤー 1 のスキル追加" );
			mySkill.SDSkill[ 1 ].Name.Add( "プレイヤー 2 のスキル追加" );
			mySkill.SDSkill[ 3 ].Name.Add( "プレイヤー 4 のスキル追加" );
			mySkill.SDSkill[ 3 ].Name.Add( "しました。" );

			myGV.GameDataSave( myGV.slot );
			myGV.DebugKeyPrint( );

		}

		myGV.GameDataLoad( myGV.slot );

		// SAVEDATA データの取得例
		foreach( SingltonSkillManager.SkillParam items1 in mySkill.SDSkill ) {
			foreach( string items2 in items1.Name ) {
				Debug.Log( "<color='red'>セーブデータ : " + items2 + "</color>" );

			}

		}


	}


}