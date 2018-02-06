using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ButtonText : MonoBehaviour
{
    [SerializeField]
    float selectScale = 1.2f;

    [SerializeField]
    Text[] buttonTexts;
    Button button;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();

        // Textが設定されていない場合取得
        if (buttonTexts == null) {
            buttonTexts = button.GetComponentsInChildren<Text>();
        }

        button.OnSelectAsObservable()
            .Subscribe(_ => {
                foreach (var buttonText in buttonTexts) {
                    buttonText.transform.localScale = Vector3.one * selectScale;
                }
            });
        button.OnDeselectAsObservable()
            .Subscribe(_ => {
                foreach (var buttonText in buttonTexts) {
                    buttonText.transform.localScale = Vector3.one;
                }
            });
    }
}
