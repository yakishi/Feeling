using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEvent : MonoBehaviour
{
    /// <summary>
    /// 触れた時に自動的にイベントが始まるか
    /// </summary>
    [SerializeField]
    protected bool isAuto;
    public bool IsAuto { get { return isAuto; } }

    [SerializeField]
    bool hideSprite;

    /// <summary>
    /// イベントを起こしているプレイヤー
    /// </summary>
    PlayerEvent target;

    void Start()
    {
        if (hideSprite) {
            GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }
    /// <summary>
    /// イベント処理
    /// </summary>
    /// <param name="player">プレイヤー</param>
    public virtual void EventAction(PlayerEvent player)
    {
        target = player;
    }

    /// <summary>
    /// イベント終了時に呼び出す
    /// </summary>
    void endEvent()
    {
        if (target != null) {
            target.endEvent();
        }
    }
}
