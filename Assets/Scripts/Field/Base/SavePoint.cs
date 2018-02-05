using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class SavePoint : FieldEvent {

    GV gv;

    private void Start()
    {
        gv = GV.Instance;
    }

    public override void EventAction(PlayerEvent player)
    {
        base.EventAction(player);
        GameObject canvas = GameObject.Find("Canvas");

        SaveLoad.CreateUI(SaveLoad.Type.Save,canvas);

        EventSystem.current.SetSelectedGameObject(canvas.transform.GetChild(0).gameObject);
    }
}
