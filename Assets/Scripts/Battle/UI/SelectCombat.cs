using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCombat : MonoBehaviour {

    Vector3 defScale = new Vector3(1.0f, 1.0f, 1.0f);

	// Use this for initialization
	void Start () {
		
	}
	
	public void Select(GameObject Img)
    {
        Img.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void DeSelect(GameObject Img)
    {
        Img.transform.localScale = defScale;
    }
}
