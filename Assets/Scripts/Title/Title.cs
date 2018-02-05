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

    private GameObject saveLoad;

    GameObject[] audioClips;
    // Use this for initialization
    void Start()
    {
		newGameButton.OnClickAsObservable()
            .Subscribe(_ => {
                myGV.newGame();
                SceneController.sceneTransition(SceneName.SceneNames.Prologue, 2.0f, SceneController.FadeType.Fade);

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
        if (Input.GetKeyDown(KeyCode.Backspace) && saveLoad != null) {
            Destroy(saveLoad);
        }

        if(audioClips != null) {
            if (audioClips.Length <= 1) return;

            DestroyObject(audioClips[1]);
        }
	}
}
