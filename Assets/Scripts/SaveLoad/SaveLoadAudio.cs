using UnityEngine;

public class SaveLoadAudio {

	/// <summary>AudioComponent</summary>
	struct AudioComponent {
		public AudioClip[ ] SE; // 効果音 ( 配列 )
		public AudioSource audio; // AudioSource クラスを使う

	}
	AudioComponent myAudioComponent;

	/*===============================================================*/
	/// <summary>UnityEngineライフサイクルによる初期化</summary>
	public SaveLoadAudio( ) {
		// 初期化関数を呼び出す
		Initialize( );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>初期化</summary>
	void Initialize( ) {
		// Resource 配下からの asset 読込
		myAudioComponent.SE = Resources.LoadAll<AudioClip>( "Sounds/SE/セーブ・ロード/" );
		for ( int i = 0; i < myAudioComponent.SE.Length; i++ )
			Debug.Log( "AudioSE_Index : " + i + " = FileName ( " + myAudioComponent.SE[ i ].name + " )" );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>効果音の再生(Indexで見つけるVersion)</summary>
	/// <param name="playSE_Index">再生させたい効果音</param>
	/// <param name="audio">アタッチされたAudioSourceコンポーネント</param>
	public void PlaySE( int playSE_Index, AudioSource audio ) {
		if ( playSE_Index > -1 && playSE_Index < myAudioComponent.SE.Length ) {
			audio.PlayOneShot( myAudioComponent.SE[ playSE_Index ] );

		} else
			Debug.LogError( "範囲外です！確認して下さい！" );


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>効果音の再生(ファイル名で見つけるVersion)</summary>
	/// <param name="SeFileName">再生させたい効果音のファイル名</param>
	/// <param name="audio">アタッチされたAudioSourceコンポーネント</param>
	public void PlaySE( string SeFileName, AudioSource audio ) {
		for ( int i = 0; i < myAudioComponent.SE.Length; i++ ) {
			if ( myAudioComponent.SE[ i ].name == SeFileName ) {
				audio.PlayOneShot( myAudioComponent.SE[ i ] );
				break;

			} //else if( myAudioComponent.SE[ i ].name != SeFileName ) Debug.LogError( "該当音声ファイルが見つかりませんでした！確認して下さい！, SeFileName : " + SeFileName );
			  //Debug.Log( "SeFileName : " + myAudioComponent.SE[ i ].name );
			  //Debug.Log( "SeFileName ( 入力値 ) : " + SeFileName );

		}


	}
	/*===============================================================*/


}