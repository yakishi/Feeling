using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayEquipmentList : MonoBehaviour {

    [SerializeField]
    EquipmentWindow equipmentWindow;

    [SerializeField]
    GameObject prefab;

	// Use this for initialization
	void Start () {
		
	}

    public void Select(int equipType)
    {
        foreach (var e in equipmentWindow.gameManager.EquipmentManager.GetCsvDataPlayerState) {
            if (e.Type == equipType) {

            }
        }
    }
}
