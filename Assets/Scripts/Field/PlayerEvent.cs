using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    MovablePlayer movablePlayer;

    /// <summary>
    /// フィールドイベントのタグ
    /// </summary>
    [SerializeField]
    LayerMask fieldEvent;

    bool isPlayEvent = false;
    public bool IsPlayEvent { get { return isPlayEvent; } }

    public bool CanPlayEvent { get { return !isPlayEvent; } }

    // Use this for initialization
    void Start()
    {
        movablePlayer = GetComponent<MovablePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanPlayEvent) {
            return;
        }

        var hitObject = Physics2D.Raycast(transform.position, movablePlayer.direction, 1.0f, fieldEvent);
        if (hitObject.transform != null) {
            var hitEvent = hitObject.collider.GetComponent<FieldEvent>();
            if (hitEvent != null) {
                if (hitEvent.IsAuto) {
                    hitEvent.EventAction(this);
                    isPlayEvent = true;
                }
                else if (Input.GetButtonDown("Submit")) {
                    // 自動イベントじゃない時,決定ボタンを押したらイベント
                    hitEvent.EventAction(this);
                    isPlayEvent = true;
                }
            }
        }
    }

    /// <summary>
    /// イベント終了時に呼び出す
    /// </summary>
    public void endEvent()
    {
        isPlayEvent = false;
    }
}
