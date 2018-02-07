using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class Title : MonoBehaviour
{
	
	GV myGV = GV.Instance; // Save/Load クラスのインスタンスを取得

    /// <summary>
    /// ゲームを新しく始めるボタン
    /// </summary>
    [SerializeField]
    Button newGameButton;

    /// <summary>
    /// ロードボタン
    /// </summary>
    [SerializeField]
    Button loadGameButton;

    /// <summary>
    /// クレジット
    /// </summary>
    [SerializeField]
    Button creditButton;

    /// <summary>
    /// 終了ボタン
    /// </summary>
    [SerializeField]
    Button exitButton;

    [SerializeField]
    GameObject grid;

    [SerializeField]
    GameObject audioManager;

    [SerializeField]
    GameObject select;

	private GameObject saveUiObj;
	private bool sceneLoadOnce;
	private bool newGameFlg; // newGame Button が押されたか否か
	static private bool saveFlg; // セーブスロットが押されたか否か
	/// <summary>TitleNewGameSlotButtonを押したら戻れないようにする</summary>
	static public bool SavedFlg { set { saveFlg = value; } }

	private GameObject saveLoad;

    GameObject[] audioClips;
    // Use this for initialization
    void Start()
    {
		saveUiObj = newGameButton.gameObject; // 初期 null 回避
		sceneLoadOnce = true; // update 中に 1 回だけ呼ばれるようにするフラグ
		newGameFlg = false;
		saveFlg = false;

		newGameButton.OnClickAsObservable()
            .Subscribe(_ => {
				// セーブスロット UI 表示
				// newGame 時にこの処理がないと newGame 時に初期スロット 1 でセーブされる
				saveUiObj = SaveLoad.CreateUI( SaveLoad.Type.Save, gameObject );
				newGameFlg = true;
				//myGV.newGame(audioManager);

				BattleUI.NotActiveButton(select);
                
            })
            .AddTo(this);
        loadGameButton.OnClickAsObservable()
            .Subscribe(_ => {
                saveLoad = SaveLoad.CreateUI(SaveLoad.Type.Load, gameObject);

                saveLoad.OnDestroyAsObservable()
                .Subscribe(u => {
                    BattleUI.ActiveButton(select);
                });
				BattleUI.NotActiveButton(select);
            })
            .AddTo(this);
        creditButton.OnClickAsObservable()
            .Subscribe(_ => {
				//クレジットシーンに遷移
				SceneController.sceneTransition( Enum.GetName( typeof( SceneName.SceneNames), 5 ), 2.0f, SceneController.FadeType.Fade );
                DontDestroyOnLoad(audioManager);
			} )
            .AddTo(this);
        exitButton.OnClickAsObservable()
            .Subscribe(_ => {
                // Quit パッケージ後の EXE で有効になる
				#if !UNITY_EDITOR
					Application.Quit( );
				#endif
				// Editor の Quit 処理
				#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
				#endif
            })
            .AddTo(this);

        BattleUI.ActiveButton(grid, newGameButton.gameObject);

        audioClips = GameObject.FindGameObjectsWithTag("AudioManager");
    }

	void Update( ) {
		// newGame 時にこの処理がないと newGame 時に初期スロット 1 でセーブされる
		// セーブスロット UI 表示 → 任意セーブスロット選択 → newGame → 任意セーブスロット削除 → SystemData 更新 → 遷移
		if( saveUiObj == null && sceneLoadOnce && newGameFlg ) {
			sceneLoadOnce = false;
			myGV.newGame( audioManager );
			//SingltonItemManager.Instance.SDItem.possessionGolds = 2048;

		}
		// Title load 時 bgm 引き継ぎ
		if ( saveLoad == null ) DontDestroyOnLoad( audioManager );

		if( Input.GetKeyDown(KeyCode.Backspace) && saveLoad != null) {
            Destroy(saveLoad);
        } else if( Input.GetKeyDown( KeyCode.Backspace ) && newGameFlg && !saveFlg ) {
			newGameFlg = false;
			Destroy( saveUiObj );
			BattleUI.ActiveButton( grid, newGameButton.gameObject );
		}

        if(audioClips != null) {
            if (audioClips.Length <= 1) return;

            DestroyObject(audioClips[1]);
        }
	}
}
