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
    public static void ChangeChoice(GameObject[] items ,int choiceElement)
    {

        //全て白に変更
        for (int i = 0; i < items.Length; i++) {
            items[i].GetComponent<Outline>().effectColor = Color.white;
        }

        //選択されているものだけを赤くする
        items[choiceElement].GetComponent<Outline>().effectColor = Color.red;
    
    }

    /// <summary>
    /// 色変更二次元配列用
    /// </summary>
    /// <param name="items">色を変えるオブジェクトの配列</param>
    /// <param name="choiceElementX">z次元の要素</param>
    /// <param name="choiceElementY">2次元の要素</param>
    public static void ChangeChoice(GameObject[,] items, int choiceElementX, int choiceElementY)
    {
        //全て白に変更
        for (int i = 0; i < items.GetLength(0); i++) {
            for (int j = 0; j < items.GetLength(1); j++) {
                items[i,j].GetComponent<Outline>().effectColor = Color.white;
            }
        }

        //選択されているものだけを赤くする
        items[choiceElementX,choiceElementY].GetComponent<Outline>().effectColor = Color.red;
    }

    public static void ChoiceClear(GameObject[] items)
    {
        foreach(GameObject i in items) {
            i.GetComponent<Outline>().effectColor = Color.white;
        }
    }

    public static void ChoiceClear(GameObject[,] items)
    {
        foreach (GameObject i in items) {
            i.GetComponent<Outline>().effectColor = Color.white;
        }
    }
}
