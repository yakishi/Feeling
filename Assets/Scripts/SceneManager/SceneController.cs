using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class SceneController
{
    public enum FadeType
    {
        Fade,
        Plane
    }

    static SceneController instance;
    static SceneController Instance {
        get
        {
            if (instance == null) {
                instance = new SceneController();
            }
            return instance;
        }
    }

    /// <summary>
    /// Fade 用のクラス
    /// </summary>
    Fade fade;

    private SceneController()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Common/Fade");

        var fadeInstance = GameObject.Find(prefab.name);
        if (fadeInstance == null) {
            fadeInstance = GameObject.Instantiate(prefab);
        }

        fade = fadeInstance.GetComponent<Fade>();
    }

    public static void sceneTransition(string sceneName, float fadeTime = 1.0f, FadeType type = FadeType.Plane)
    {

        if (type == FadeType.Plane) {
            SceneManager.LoadScene(sceneName);
            return;
        }

        Instance.fade.fadeTime = fadeTime;
        Instance.fade.onEndFadeAsObservable()
            .Take(1)
            .Subscribe(_ => {
                SceneManager.LoadScene(sceneName);
            });

        Instance.fade.startFade();
    }
    public static void sceneTransition(SceneName.SceneNames sceneName, float fadeTime = 1.0f, FadeType type = FadeType.Plane)
    {
        string name = Enum.GetName(typeof(SceneName.SceneNames), sceneName);
        sceneTransition(name, fadeTime, type);
    }

    public static Fade startFade(Action<Fade> fadeOutAction, float fadeTime, bool isWait = false)
    {
        Instance.fade.fadeTime = fadeTime;
        Instance.fade.onEndFadeAsObservable()
            .Take(1)
            .Subscribe(_ => {
                if (fadeOutAction != null) {
                    fadeOutAction(Instance.fade);
                }
            });
        Instance.fade.startFade(isWait);

        return Instance.fade;
    }
}

