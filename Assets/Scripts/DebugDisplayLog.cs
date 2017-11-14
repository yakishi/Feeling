using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*===============================================================*/
/// <summary>
/// @brief デバッグ表示クラス カメラオブジェクトにスクリプトを関連づけます
/// </summary>
/// /*===============================================================*/
public class DebugDisplayLog : MonoBehaviour {

	//static string str;

	private void Start( ) {
		this.oneSecondTime = 0f;

	}

	private void Update( ) {
		// Quit パッケージ後の EXE で有効になる
		if( Input.GetKey( KeyCode.Escape ) ) Application.Quit( );

		// Editor の Quit 処理
		if ( Input.GetKey( KeyCode.Escape ) ) {
			UnityEditor.EditorApplication.isPlaying = false; /* パッケージ化時には, コメントアウト化して下さい */
			//ClearConsole( );

		}

		// 特定キーが押されたときにログを消す
		if ( Input.GetKey( KeyCode.C ) ) {
			DebugDisplayLog.displayLog.Clear( );

		}


			// FPS
			if ( this.oneSecondTime >= 1f ) {
			this.fps = this.fpsCounter;
			this.fixedFps = this.fixedFpsCounter;

			// reset
			this.fpsCounter = 0;
			this.fixedFpsCounter = 0;
			this.oneSecondTime = 0f;

		} else {

			this.fpsCounter++;
			this.oneSecondTime += Time.deltaTime;

		}

		// structure debug string
		this.debugString = "CSVデータキー : \n" /* str */;
		int count = DebugDisplayLog.displayLog.Count;

		for ( int i = 0; i < DebugDisplayLog.displayLog.Count; i++ ) {
			this.debugString += "<color=red>" + DebugDisplayLog.displayLog [ i ] + "</color>";
			this.debugString += "<color=green>, </color>";

		}

		//DebugDisplayLog.displayLog.Clear( );

	}

	private void FixedUpdate( ) {
		this.fixedFpsCounter++;

	}

	private void OnGUI( ) {
		GUI.Label(
			new Rect( 0f, 0f, Screen.width, Screen.height ),
			"FPS: " + this.fps + "  FixedUpdate: " + this.fixedFps + "\n" /* + this.debugString */ );

	}

	static public void SetDebugString( string s ) {
		//str = s;


	}

	//[MenuItem( "Tools/Clear Console %#c" )]
	static void ClearConsole( ) {
		var logEntries = System.Type.GetType( "UnityEditorInternal.LogEntries,UnityEditor.dll" );
		var clearMethod = logEntries.GetMethod( "Clear", System.Reflection.BindingFlags.Static 
												| System.Reflection.BindingFlags.Public );
		clearMethod.Invoke( null, null );


	}

	// FPS
	private int fps;
	private int fpsCounter;

	// Fixed FPS
	private int fixedFps;
	private int fixedFpsCounter;

	// Debug Log
	private string debugString;

	// Timer
	private float oneSecondTime;

	static public List<string> displayLog = new List<string>( );


}