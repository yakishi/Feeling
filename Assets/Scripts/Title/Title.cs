using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class Title : MonoBehaviour
{
	
	//GV myGV = GV.Instance; // Save/Load クラスのインスタンスを取得

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
    /// 終了ボタン
    /// </summary>
    [SerializeField]
    Button exitButton;
    
    // Use this for initialization
    void Start()
    {

		newGameButton.transform.GetChild( 0 ).GetComponent<Text>( ).text = "New Game";
		newGameButton.transform.GetChild( 0 ).GetComponent<Text>( ).fontSize = 32;
		loadGameButton.transform.GetChild( 0 ).GetComponent<Text>( ).text = "Load Game";
		loadGameButton.transform.GetChild( 0 ).GetComponent<Text>( ).fontSize = 32;
		exitButton.transform.GetChild( 0 ).GetComponent<Text>( ).text = "Quit Game";
		exitButton.transform.GetChild( 0 ).GetComponent<Text>( ).fontSize = 32;

		newGameButton.OnClickAsObservable()
            .Subscribe(_ => {
                // TODO: 仮でワールドマップに遷移
                SceneManager.LoadScene(SceneName.WorldMap);
            })
            .AddTo(this);
        loadGameButton.OnClickAsObservable()
            .Subscribe(_ => {
                SaveLoad.CreateUI(SaveLoad.Type.Load, gameObject);
            })
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
    }
}
