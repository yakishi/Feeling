using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
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

	GameObject saveUiObj;
	bool sceneLoadOnce;
    
    // Use this for initialization
    void Start()
    {

		saveUiObj = newGameButton.gameObject; // 初期 null 回避

		sceneLoadOnce = true; // update 中に 1 回だけ呼ばれるようにするフラグ

		newGameButton.OnClickAsObservable()
            .Subscribe(_ => {
                // セーブスロット UI 表示
                //saveUiObj = SaveLoad.CreateUI( SaveLoad.Type.Save, gameObject );
                myGV.newGame();
            })
            .AddTo(this);
        loadGameButton.OnClickAsObservable()
            .Subscribe(_ => {
                SaveLoad.CreateUI(SaveLoad.Type.Load, gameObject);
            })
            .AddTo(this);
        creditButton.OnClickAsObservable()
            .Subscribe(_ => {
				//クレジットシーンに遷移
				SceneController.sceneTransition( Enum.GetName( typeof( SceneName.SceneNames), 5 ), 2.0f, SceneController.FadeType.Fade );
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
    }

	void Update( ) {
		// セーブスロット UI 表示 → 任意セーブスロット選択 → newGame → 任意セーブスロット削除 → SystemData 更新 → 遷移
		if ( saveUiObj == null && sceneLoadOnce ) {
			sceneLoadOnce = false;
			myGV.newGame( );

		}
	}
}
