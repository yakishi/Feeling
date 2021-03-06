﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour{

    ItemWindow itemWindow;

    Text text;

	// Use this for initialization
	void OnEnable () {
        itemWindow = GameObject.Find("ItemWindow").GetComponent<ItemWindow>();
        text = GameObject.Find("ItemDetail").GetComponentInChildren<Text>();
	}
	
	public void Select(GameObject button)
    {
        var name = button.GetComponentInChildren<Text>().text;

        foreach (var item in itemWindow.itemButtonList) {
            if (name == item.ItemInfo.name) {
                text.text = item.ItemInfo.Detail;
            }
        }

        SelectText.StaticSelect(button);
    }

    public void DeSelect(GameObject button)
    {
        SelectText.StaticDeSelect(button);
    }
}
