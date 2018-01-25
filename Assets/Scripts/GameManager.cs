using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*===============================================================*/
/// <summary>ゲーム進行管理クラス カメラオブジェクトにスクリプトを関連づけます</summary>
public class GameManager : MonoBehaviour {

	/// <summary>ゲームの状態定義</summary>
	public enum GameState { 
		/// <summary>ダンジョンシーン</summary>
		DUNGEON,
		/// <summary>バトルシーン</summary>
		BATTLE,	

	}

	// ゲーム状態
	static private GameState state;

	// LoadScene に対するフラグ変数
	private bool loadLevelFlg;

	// セッターおよびゲッター定義部
	/// <summary>現在のシーン状態をgetまたはsetします</summary>
	/// <remarks>setには,GameManager.GameStateで定義されたシーンをいれます</remarks>
	static public GameState SetGetGameState { get { return state; } set { state = value; } }

    GV myGV;
    SingltonPlayerManager mySPM;
    SingltonEquipmentManager mySEM;
    public SingltonEquipmentManager EquipmentManager
    {
        get
        {
            return mySEM;
        }
    }
    SingltonSkillManager mySSM;
    public SingltonSkillManager SkillManager
    {
        get
        {
            return mySSM;
        }
    }
    SingltonItemManager mySIM;
    public SingltonItemManager ItemManager
    {
        get
        {
            return mySIM;
        }
    }

    List<SingltonPlayerManager.PlayerParameters> players;
    public List<SingltonPlayerManager.PlayerParameters> PlayerList
    {
        get
        {
            return players;
        }
    }
	SingltonFlagManager mySFM;

    public SingltonItemManager.ItemParam possessionItem;

    /*===============================================================*/
    /// <summary>brief UnityEngine ライフサイクルによる初期化 </summary>
    void Awake( ) {
		// 初期化関数を呼び出す
		Initialize( );

        possessionItem = mySIM.SDItem;
	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>brief 初期化</summary>
	void Initialize( ) {

		// セーブデータを扱うシングルトンクラスインスタンス生成処理
		myGV = GV.Instance;
		mySPM = SingltonPlayerManager.Instance;
		mySEM = SingltonEquipmentManager.Instance;
		mySSM = SingltonSkillManager.Instance;
		mySIM = SingltonItemManager.Instance;
		mySFM = SingltonFlagManager.Instance;

		Application.targetFrameRate = 60; // 60 FPS に設定
		Debug.Log( "現在のシーン : " + state.ToString( ) );

        players = new List<SingltonPlayerManager.PlayerParameters>();
        int cnt = 0;
        foreach(var i in mySPM.GetCsvDataPlayerState) {
            if(cnt % 11 == 0) {
                players.Add(i);
            }

            cnt++;
        }
	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary> 毎フレーム呼ばれます</summary>
	void Update( ) {
		// GameState によるシーン遷移
		switch ( state ) {
			case GameState.DUNGEON : {
				// BattleScene に遷移出来るようにフラグを変更
				loadLevelFlg = true;
				break;

			}

			case GameState.BATTLE : {
				if( loadLevelFlg ) {
					// LoadLevel
					SceneManager.LoadScene( "BattleScene" );
					// load scene が1回だけ呼ばれるようにする
					loadLevelFlg = false;

				}
				break;

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary> 現在の押されている key code を取得します キーを確認したいときに使います </summary>
	static public void DownKeyCheck( ) {
		if ( Input.anyKeyDown ) {
			foreach ( KeyCode code in Enum.GetValues( typeof( KeyCode ) ) ) {
				if ( Input.GetKeyDown( code ) ) {
					Debug.Log( code );
					break;

				}

			}

		}


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>スプライトレンダラーコンポーネントのカラー属性アルファ値を減衰する関数</summary>
	/// <param name="obj">スプライトレンダラーコンポーネントを指定</param>
	/// <returns>現在のアルファ値を返します</returns>
	static public Color FadeOut( SpriteRenderer obj ) {
		// アルファ値は, 初期値として 0.0f より大きくなっている必要があります
		// http://rikoubou.hatenablog.com/entry/2016/01/30/222448
		SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>( );
		Color color = renderer.color;
		color.r = 1.0f; // RGBのR(赤)値
		color.g = 1.0f; // RGBのG(緑)値
		color.b = 1.0f; // RGBのB(青)値
		if ( 0.0f <= color.a ) color.a -= 0.01f;   // RGBのアルファ値(透明度の値)
		renderer.color = color; // 変更した色情報に変更
		// カラー値を返す
		return color;


	}
	/*===============================================================*/

	/*===============================================================*/
	/// <summary>スプライトレンダラーコンポーネントのカラー属性アルファ値を増大する関数</summary>
	/// <param name="obj">SpriteRenderer スプライトレンダラーコンポーネントを指定</param>
	/// <returns>現在のアルファ値を返します</returns>
	static public Color FadeIn( SpriteRenderer obj ) {
		// アルファ値は, 初期値として 1.0f 以下になっている必要があります
		// http://rikoubou.hatenablog.com/entry/2016/01/30/222448
		SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>( );
		Color color = renderer.color;
		color.r = 1.0f;	// RGBのR(赤)値
		color.g = 1.0f;	// RGBのG(緑)値
		color.b = 1.0f;	// RGBのB(青)値
		if( color.a <= 1.0f ) color.a += 0.01f;	// RGBのアルファ値(透明度の値)
		renderer.color = color; // 変更した色情報に変更
		// カラー値を返す
		return color;


	}
	/*===============================================================*/


}