using UnityEngine;

/// <summary>シーン管理クラス</summary>
public sealed class SingltonSceneManage {

	private static SingltonSceneManage mInstance = new SingltonSceneManage( );
	private SpawnScene mySS;
	GV myGV;

	/// <summary>セーブデータから読み込まれた場所情報パラメーターを取得またはセット・ゲット</summary>
	public SpawnScene SDSpawn { get { return mySS; } set { mySS = value; } }
	/// <summary>Singlton</summary>
	public static SingltonSceneManage Instance { get { return mInstance; } }

	private SingltonSceneManage( ) {
		Debug.Log( "Create SingltonSceneManage Instance." );

		mySS = new SpawnScene( );

		SpawnCreate( ); // プレイヤーの出現場所情報などの作成を行います


	}

	/*===============================================================*/
	/// <summary>プレイヤー出現場所情報をなどを作ります</summary>
	private void SpawnCreate( ) {
		myGV = GV.Instance; // Save/Load クラスのインスタンスを取得

		// SaveData 読込
		LoadSpawn( myGV.slot );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>NewGame時のセーブのインスタンス生成処理</summary>
	/// <remarks>GV.cs:newGame( )から呼ばれる</remarks>
	public void NewGame( ) {
		// Save 情報フラグのインスタンス生成
		mySS = new SpawnScene( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>GV.SpawnSceneからのデータ読込</summary>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void LoadSpawn( int loadSlot ) {

		myGV.GameDataLoad( loadSlot ); // セーブデータからの保存したデータを読み込みます

		if( myGV.GData != null ) {
			// 読み込んだ値を入れていきます
			mySS.currentSpawnPlayer = myGV.GData.spawnPlayer.currentSpawnPlayer;


		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>シーン移動用</summary>
	/// <param name="saveDataSceneInfo">セーブデータから読み込んだシーンネーム</param>
	public void SceneTranslation( string saveDataSceneInfo ) {
		if( saveDataSceneInfo != "" && saveDataSceneInfo != "spawn place not  been saved." ) {
			SceneController.sceneTransition( saveDataSceneInfo, 2.0f, SceneController.FadeType.Fade );

		}
		if( saveDataSceneInfo == "spawn place not  been saved." ) Debug.LogWarning( "spawn place not  been saved." );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>プレイヤー出現管理クラス</summary>
	[System.Serializable]
	public class SpawnScene {
		/// <summary>現在のプレイヤー出現場所(SceneName指定)</summary>
		public string currentSpawnPlayer;


	}
	/*===============================================================*/


}