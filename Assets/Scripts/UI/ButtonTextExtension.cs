using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public static class ButtonTextExtension
{
    public static IObservable<Unit> OnClickAsObservable(this ButtonText buttonText)
    {
        var button = buttonText.GetComponent<Button>();
        return button.onClick.AsObservable().Where(_ => buttonText.CanClick);
    }
}
