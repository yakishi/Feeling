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

    /// <summary>
    /// 選択している項目の色変更
    /// </summary>
    /// <param name="choiceElement"> 選択項目の要素番号 </param>
    public static void ChangeChoice(GameObject[] Items ,int choiceElement)
    {

        //全て白に変更
        for (int i = 0; i < Items.Length; i++) {
            Items[i].GetComponent<Outline>().effectColor = Color.white;
        }

        //選択されているものだけを赤くする
        Items[choiceElement].GetComponent<Outline>().effectColor = Color.red;
    
    }
}
