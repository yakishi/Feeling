using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class Title : MonoBehaviour
{
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
        newGameButton.OnClickAsObservable()
            .Subscribe(_ => {
                GV.newGame();
                // TODO: 仮でワールドマップに遷移
                SceneManager.LoadScene(SceneName.WorldMap);
            })
            .AddTo(this);
        loadGameButton.OnClickAsObservable()
            .Subscribe(_ => {
                // TODO: 仮でワールドマップに遷移
                SceneManager.LoadScene(SceneName.WorldMap);
            })
            .AddTo(this);
        exitButton.OnClickAsObservable()
            .Subscribe(_ => {
                Application.Quit();
            })
            .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
