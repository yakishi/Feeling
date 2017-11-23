using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomStandaloneInput : StandaloneInputModule
{
    /// <summary>
    /// マウスを有効にするか
    /// </summary>
    [SerializeField]
    bool enabledMouse;
    public bool EnabledMouse {
        set { enabledMouse = value; }
        get { return enabledMouse; }
    }
    public override void Process()
    {

        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents) {
            if (!usedEvent)
                usedEvent |= SendMoveEventToSelectedObject();

            if (!usedEvent)
                SendSubmitEventToSelectedObject();
        }

        if (!enabledMouse) {
            return;
        }

        if (input.mousePresent) {
            ProcessMouseEvent();
        }
    }
}
