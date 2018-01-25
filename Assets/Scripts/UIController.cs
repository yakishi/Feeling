using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private GameObject MenuUI;  //メニューUI

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        MenuUI = GameObject.Find("Menu");
        MenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            MenuUI.SetActive(true);
        }
	}

    public static void DisplayText(Text item, int value)
    {
        item.text = value.ToString();
    }
    public static void DisplayText(Text item,string value)
    {
        item.text = value;
    }

    public static void DisplaySlider(Slider item, int value)
    {
        item.value = value;
        item.maxValue = value;
    }

    public static void DisplayImage(Image img,Sprite sprite)
    {
        img.sprite = sprite;
    }
}
