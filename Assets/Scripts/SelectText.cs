﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    static public void StaticSelect(GameObject button)
    {
        Text text = button.GetComponent<Text>();
        text.color = Color.red;
    }

    public void Select(GameObject button)
    {
        Text text = button.GetComponent<Text>();
        text.color = Color.red;
    }

    static public void StaticDeSelect(GameObject button)
    {
        Text text = button.GetComponent<Text>();
        if (text != null)
            text.color = Color.white;
    }

    public void DeSelect(GameObject button)
    {
        Text text = button.GetComponent<Text>();
        if (text != null)
            text.color = Color.white;
    }
}
