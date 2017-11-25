using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>BattleSceneオブジェクトにスクリプトをアタッチします</summary>
/// <remarks>BattleSceneにおける効果音およびBGMの再生を管理します</remarks>
public class BattleAudio : MonoBehaviour {

	[SerializeField, TooltipAttribute( "MainCameraをアタッチ" )]
	#pragma warning disable CS0108 // メンバーは継承されたメンバーを非表示にします。キーワード new がありません
	private GameObject camera;
	#pragma warning restore CS0108 // メンバーは継承されたメンバーを非表示にします。キーワード new がありません

	/// <summary>AudioComponent</summary>
	struct AudioComponent {
		public AudioClip[ ] SE; // 敵効果音 ( 配列 )
		public AudioClip[ ] BGM; // BattleScene 内 BGM ( 配列 )
		public AudioSource audio; // AudioSource クラスを使う

	}  static AudioComponent myAudioComponent;

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	void Awake( ) {
		// 初期化関数を呼び出す
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	void Initialize( ) {
		// Resource 配下からの asset 読込
		myAudioComponent.SE = Resources.LoadAll<AudioClip>( "Sounds/BattleScene/SE/" );
		myAudioComponent.BGM = Resources.LoadAll<AudioClip>( "Sounds/BattleScene/BGM/" );
		for( int i = 0; i < myAudioComponent.SE.Length; i++ ) Debug.Log( "AudioSE_Index : " + i + " = FileName ( " + myAudioComponent.SE[ i ].name + " )" );
		for( int i = 0; i < myAudioComponent.BGM.Length; i++ ) Debug.Log( "<color='red'>AudioBGM_Index : " + i + " = FileName ( " + myAudioComponent.BGM[ i ].name + " )</color>" );
		myAudioComponent.audio = camera.gameObject.GetComponent<AudioSource>( ); // MainCamera の AudioSource の取得


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>効果音の再生(Indexで見つけるVersion)</summary>
	/// <param name="playSE_Index">再生させたい効果音</param>
	/// <param name="enemy">エネミー側にアタッチされたAudioSourceコンポーネント</param>
	static public void PlaySE( int playSE_Index, AudioSource enemy ) {
		if( playSE_Index > -1 && playSE_Index < myAudioComponent.SE.Length ) {
			enemy.PlayOneShot( myAudioComponent.SE[ playSE_Index ] );

		} else Debug.LogError( "範囲外です！確認して下さい！" );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>効果音の再生(ファイル名で見つけるVersion)</summary>
	/// <remarks>使用例:Attack.cs:67</remarks>
	/// <param name="SeFileName">再生させたい効果音のファイル名</param>
	/// <param name="enemy">エネミー側にアタッチされたAudioSourceコンポーネント</param>
	static public void PlaySE( string SeFileName, AudioSource enemy ) {
		for( int i = 0; i < myAudioComponent.SE.Length; i++ ) {
			if( myAudioComponent.SE[ i ].name == SeFileName ) {
				enemy.PlayOneShot( myAudioComponent.SE[ i ] );
				break;

			} //else if( myAudioComponent.SE[ i ].name != SeFileName ) Debug.LogError( "該当音声ファイルが見つかりませんでした！確認して下さい！, SeFileName : " + SeFileName );
			//Debug.Log( "SeFileName : " + myAudioComponent.SE[ i ].name );
			//Debug.Log( "SeFileName ( 入力値 ) : " + SeFileName );

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>BGMの再生</summary>
	/// <remarks>
	/// http://hiyotama.hatenablog.com/entry/2015/05/22/134029
	/// 例えば画面をクリックした時にBGMを鳴らし始めたい時は、以下のスクリプトをMain Cameraに取り付けると実現します
	/// 使用例:BattleScene.cs:49
	/// </remarks>
	/// <param name="playBGM_Index">再生させたいBGM</param>
	static public void PlayBGM( int playBGM_Index ) {
		if( playBGM_Index > -1 && playBGM_Index < myAudioComponent.BGM.Length ) {
			myAudioComponent.audio.clip = myAudioComponent.BGM[ playBGM_Index ];
			myAudioComponent.audio.Play( );

		} else Debug.LogError( "範囲外です！確認して下さい！" );


	}
	/*===============================================================*/


}
/*===============================================================*/