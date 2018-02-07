using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDoor : TransportPoint
{

    [SerializeField]
    SwitchEvent[] eventSwitches;

    bool isAllConditions = false;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        int cnt = 0;

        for (int i = 0; i < eventSwitches.Length; i++) {
            if (eventSwitches[i].IsSwitch) {
                cnt++;
            }
        }

        if (cnt >= eventSwitches.Length) {
            isAllConditions = true;
            isAuto = true;
        }
    }

    public override void EventAction(PlayerEvent player)
    {
        if (!isAllConditions) {
            player.endEvent();
            return;
        }

        base.EventAction(player);
    }
}
