using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEvent : FieldEvent {

    bool isSwitch = false;
    public bool IsSwitch
    {
        get
        {
            return isSwitch;
        }
    }

    Sprite offSwitchImg;

    [SerializeField]
    Sprite onSwitchImg;

    [SerializeField]
    AudioSource SESource;
    [SerializeField]
    AudioClip SE;

    protected override void Start()
    {
        base.Start();
        offSwitchImg = gameObject.GetComponent<SpriteRenderer>().sprite;
        SESource.clip = SE;
    }

    public override void EventAction(PlayerEvent player)
    {
        base.EventAction(player);

        if (!isSwitch) {
            isSwitch = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = onSwitchImg;

        }
        else {
            isSwitch = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = offSwitchImg;
        }

        SESource.Play();

        player.endEvent();
    }
}
