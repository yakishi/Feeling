using System.Collections.Generic;
using UnityEngine;

public sealed class SingltonFlagManager {

	private static SingltonFlagManager mInstance = new SingltonFlagManager( );
	private FlagManage myFM;
	GV myGV;

	/// <summary>セーブデータから読み込まれたイベントパラメーターを取得またはセット・ゲット</summary>
	public FlagManage SDFlg { get { return myFM; } set { myFM = value; } }
	/// <summary>Singlton</summary>
	public static SingltonFlagManager Instance { get { return mInstance; } }

	private SingltonFlagManager( ) {
		Debug.Log( "Create SingltonFlagManager Instance." );

		myFM = new FlagManage( );

		FlagCreate( ); // イベントパラメーターの作成を行います


	}

	/*===============================================================*/
	/// <summary>イベントパラメーター情報をなどを作ります</summary>
	private void FlagCreate( ) {
		myGV = GV.Instance; // Save/Load クラスのインスタンスを取得

		// Save 情報フラグのインスタンス生成
		myFM.EventFlag = new Dictionary<string, bool>( );

		// SaveData 読込
		LoadFlag( myGV.slot );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>NewGame時のセーブのインスタンス生成処理</summary>
	/// <remarks>GV.cs:newGame( )から呼ばれる</remarks>
	public void NewGame( ) {
		// Save 情報フラグのインスタンス生成
		myFM.EventFlag = new Dictionary<string, bool>( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>GV.FlagManageからのデータ読込</summary>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void LoadFlag( int loadSlot ) {

		myGV.GameDataLoad( loadSlot ); // セーブデータからの保存したデータを読み込みます

		if ( myGV.GData != null ) {
			// 読み込んだ値を入れていきます
			myFM.EventFlag = myGV.GData.Flag.EventFlag;
			

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>イベント管理クラス</summary>
	[System.Serializable]
	public class FlagManage {
		/// <summary>イベントキー名, イベントフラグで管理するディクショナリ</summary>
		public Dictionary<string, bool> EventFlag;


	}
	/*===============================================================*/


}