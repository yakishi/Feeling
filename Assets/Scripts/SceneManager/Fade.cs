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

    Subject<Unit> onEndFadeSubject = new Subject<Unit>();
    Subject<Unit> onEndFadeInSubject = new Subject<Unit>();

    public float fadeTime = 1.0f;

    public float alpha { set; get; }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void startFade()
    {
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

        onEndFadeInSubject.OnNext(Unit.Default);

        alpha = 1.0f;
        while (!fadeIn())
        {
            yield return null;
        }

        onEndFadeSubject.OnNext(Unit.Default);
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

    public IObservable<Unit> onEndFadeAsObservable()
    {
        return onEndFadeSubject;
    }

    public IObservable<Unit> onEndFadeInAsObservable()
    {
        return onEndFadeInSubject;
    }
}
