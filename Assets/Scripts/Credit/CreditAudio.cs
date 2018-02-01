using UnityEngine;

/*===============================================================*/
/// <summary>CreditSceneオブジェクトにスクリプトをアタッチします</summary>
/// <remarks>CreditSceneにおける効果音およびBGMの再生を管理します</remarks>
public class CreditAudio : MonoBehaviour {

	[SerializeField, TooltipAttribute( "MainCameraをアタッチ" )]
#pragma warning disable CS0108 // メンバーは継承されたメンバーを非表示にします。キーワード new がありません
	private GameObject camera;
#pragma warning restore CS0108 // メンバーは継承されたメンバーを非表示にします。キーワード new がありません

	/// <summary>AudioComponent</summary>
	struct AudioComponent {
		public AudioClip[ ] BGM; // CreditScene 内 BGM ( 配列 )
		public AudioSource audio; // AudioSource クラスを使う

	}
	static AudioComponent myAudioComponent;

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
		myAudioComponent.BGM = Resources.LoadAll<AudioClip>( "Sounds/Title/" );
		//for ( int i = 0; i < myAudioComponent.BGM.Length; i++ )
		//	Debug.Log( "<color='red'>AudioBGM_Index : " + i + " = FileName ( " + myAudioComponent.BGM[ i ].name + " )</color>" );
		myAudioComponent.audio = camera.gameObject.GetComponent<AudioSource>( ); // MainCamera の AudioSource の取得


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>BGMの再生</summary>
	/// <remarks>
	/// http://hiyotama.hatenablog.com/entry/2015/05/22/134029
	/// 例えば画面をクリックした時にBGMを鳴らし始めたい時は、以下のスクリプトをMain Cameraに取り付けると実現します
	/// </remarks>
	/// <param name="playBGM_Index">再生させたいBGM</param>
	static public void PlayBGM( int playBGM_Index ) {
		if ( playBGM_Index > -1 && playBGM_Index < myAudioComponent.BGM.Length ) {
			myAudioComponent.audio.clip = myAudioComponent.BGM[ playBGM_Index ];
			myAudioComponent.audio.Play( );

		} else
			Debug.LogError( "範囲外です！確認して下さい！" );


	}
	/*===============================================================*/


}
/*===============================================================*/
