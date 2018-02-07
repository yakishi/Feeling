using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow :MonoBehaviour {

    private Vector3 defScale;

    [SerializeField]
    public UI_Menu menu;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Backspace)) {
            BattleUI.NotActiveButton(menu.statusGrid);
            BattleUI.ActiveButton(menu.menuWindow);

            menu.statusWindow.SetActive(false);
            menu.beforeTarget.RemoveAt(menu.beforeTarget.Count - 1);
        }
    }

}
