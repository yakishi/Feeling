using System;
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

    [SerializeField]
    Color disableColor = Color.gray;

    bool canSelect = true;
    bool canClick = true;
    public bool CanSelect { set { canSelect = value; } get { return canSelect; } }
    public bool CanClick { set { canClick = value; } get { return canClick; } }

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();

        if (!CanSelect || !CanClick) {
            updateButtonTexts(text => text.color = disableColor);
        }

        button.interactable = CanSelect;

        // Textが設定されていない場合取得
        if (buttonTexts.Length == 0) {
            buttonTexts = button.GetComponentsInChildren<Text>();
        }

        button.OnSelectAsObservable()
            .Subscribe(_ => {
                    updateButtonTexts(text => text.transform.localScale = Vector3.one * selectScale);
            });
        button.OnDeselectAsObservable()
            .Subscribe(_ => {
                updateButtonTexts(text => text.transform.localScale = Vector3.one);
            });

        button.OnSelectAsObservable()
            .Subscribe(_ => {
                button.transition = Selectable.Transition.None;
            });
        button.OnDeselectAsObservable()
            .Subscribe(_ => {
                button.transition = Selectable.Transition.ColorTint;
            });
    }

    void updateButtonTexts(Action<Text> action)
    {
        foreach (var buttonText in buttonTexts) {
            action(buttonText);
        }
    }
}
