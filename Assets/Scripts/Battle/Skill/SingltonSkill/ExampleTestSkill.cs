using UnityEngine;

public class ExampleTestSkill : MonoBehaviour {

	SingltonSkillManager mySkill;
	GV myGV;

	// Use this for initialization
	void Start( ) {
		mySkill = SingltonSkillManager.Instance;
		myGV = GV.Instance;

		// CSV データの取得例
		foreach( SingltonSkillManager.SkillInfo item1 in mySkill.CDSkill ) {
			Debug.Log( "要素数 ( 閃き　) : " + item1.flair.Count );
			foreach( SingltonSkillManager.Hirameki item2 in item1.flair ) {
				Debug.Log( "スキル ID ( 派生先 ) : " + item2.skill_ID + "\n確率 ( 派生先 ) : " + item2.probability );

			}
			Debug.Log( "<color='red'>Enum 型 種類 : " + item1.myCategory
				+ "\nEnum 型 何に影響を与えるか : "+ item1.influence
				+ "\nスキル名 : " + item1.skill
				+ "\nEnum 型 感情種類 : " + item1.FVC.Key
				+ "\n感情補正値 : " + item1.FVC.Value
				+ "</color>" );

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