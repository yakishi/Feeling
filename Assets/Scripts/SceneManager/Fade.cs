using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class Fade : MonoBehaviour
{
    /// <summary>
    /// fade 用のイメージ
    /// </summary>
    [SerializeField]
    Image FadeImage;

    Subject<Fade> onEndFadeSubject = new Subject<Fade>();
    Subject<Fade> onEndFadeInSubject = new Subject<Fade>();

    public float fadeTime = 1.0f;

    public float alpha { set; get; }
    bool wait;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void startFade(bool isWait = false)
    {
        wait = isWait;
        StartCoroutine(fadeFunc());
    }

    // コルーチン  
    IEnumerator fadeFunc()
    {
        alpha = 0.0f;
        while (!fadeOut())
        {
            yield return null;
        }
        Debug.Log("D");
        onEndFadeSubject.OnNext(this);
        while (wait) {
            yield return null;
        }

        alpha = 1.0f;
        while (!fadeIn())
        {
            yield return null;
        }

        onEndFadeInSubject.OnNext(this);
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <returns>フェードインしているか</returns>
    bool fadeIn()
    {
        var fadeDelta = 2.0f / fadeTime * Time.deltaTime;

        alpha = Mathf.Clamp(alpha - fadeDelta, 0.0f, 1.0f);

        var newColor = FadeImage.color;
        newColor.a = alpha;
        FadeImage.color = newColor;

        return alpha == 0.0f;
    }
    
    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <returns>フェードアウトしているか</returns>
    bool fadeOut()
    {

        var fadeDelta = 2.0f / fadeTime * Time.deltaTime;

        alpha = Mathf.Clamp(alpha + fadeDelta, 0.0f, 1.0f);
        
        var newColor = FadeImage.color;
        newColor.a = alpha;
        FadeImage.color = newColor;

        return alpha == 1.0f;
    }

    public IObservable<Fade> onEndFadeAsObservable()
    {
        return onEndFadeSubject;
    }

    public IObservable<Fade> onEndFadeInAsObservable()
    {
        return onEndFadeInSubject;
    }
    public void endWait()
    {
        wait = false;
    }
}
