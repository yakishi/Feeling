using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class SingltonFlagManager {

	private static SingltonFlagManager mInstance = new SingltonFlagManager( );
	private FlagManage myFM;
	GV myGV;

	/// <summary>セーブデータから読み込まれたイベントパラメーターを取得またはセット</summary>
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
		myFM.Key = new List<string>( );
		myFM.Value = new List<bool>( );

		// SaveData 読込
		LoadFlag( myGV.slot );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>NewGame時のセーブのインスタンス生成処理</summary>
	/// <remarks>GV.cs:newGame( )から呼ばれる</remarks>
	public void NewGame( ) {
		// Save 情報フラグのインスタンス生成
		myFM.Key = new List<string>( );
		myFM.Value = new List<bool>( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>GV.FlagManageからのデータ読込</summary>
	/// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
	public void LoadFlag( int loadSlot ) {

		myGV.GameDataLoad( loadSlot ); // セーブデータからの保存したデータを読み込みます

		if ( myGV.GData != null ) {
			// 読み込んだ値を入れていきます
			myFM.Key = myGV.GData.Flag.Key;
			myFM.Value = myGV.GData.Flag.Value;

		}

		KeyCheck( myFM.Key );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>保存されているキー重複検査</summary>
	/// <param name="keyList">キーが保存されたリスト(string型)</param>
	private void KeyCheck( List<string> keyList ) {
		// 元の配列の要素数
		int baseCount = keyList.Count;
		// 元の配列から重複要素を無くした配列の要素数
		int distinctCount = ( from x in keyList select x ).Distinct( ).Count( );

		if ( baseCount != distinctCount ) {
			Debug.LogWarning( "イベントフラグキーに重複したキーが存在しています。" );
			//myFM.Key = ( from x in keyList select x ).Distinct( ).ToList( );

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>イベント管理クラス</summary>
	[System.Serializable]
	public class FlagManage {
		/// <summary>イベントフラグキー</summary>
		public List<string> Key;
		/// <summary>イベントフラグ</summary>
		public List<bool> Value;


	}
	/*===============================================================*/


}