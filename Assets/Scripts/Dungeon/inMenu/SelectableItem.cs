using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void Select(GameObject textObj)
    {
        textObj.GetComponent<Text>().color = Color.red;
    }

    public void DeSelect(GameObject textObj)
    {
        textObj.GetComponent<Text>().color = Color.white;
    }
}
