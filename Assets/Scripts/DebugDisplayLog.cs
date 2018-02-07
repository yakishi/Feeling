using UnityEngine;
using System.Collections.Generic;

/*===============================================================*/
/// <summary>デバッグ表示クラス:カメラオブジェクトにスクリプトを関連づけます</summary>
public class DebugDisplayLog : MonoBehaviour {

	private void Start( ) {
		this.oneSecondTime = 0f;

	}

	private void Update( ) {
		// Quit パッケージ後の EXE で有効になる
		#if !UNITY_EDITOR
			if( Input.GetKeyDown( KeyCode.Escape ) ) {
			  Application.Quit( );

			}
		#endif
		// Editor の Quit 処理
		#if UNITY_EDITOR
			if ( Input.GetKey( KeyCode.Escape ) ) {
				UnityEditor.EditorApplication.isPlaying = false;

			}
			// 特定キーが押されたときにカーソルを有効にする
			if ( Input.GetKey( KeyCode.C ) ) {
				//DebugDisplayLog.displayLog.Clear( );
				CustomStandaloneInput.MouseEnable( 3 );

			}
		#endif 

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
		this.debugString = "Debug : \n" /* str */;
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
			"FPS: " + this.fps + "  FixedUpdate: " + this.fixedFps + "\n" + this.debugString );

	}

	static public void SetDebugString( string s ) {
		//str = s;


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
/*===============================================================*/