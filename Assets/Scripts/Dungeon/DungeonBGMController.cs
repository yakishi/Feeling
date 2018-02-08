using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBGMController : MonoBehaviour {

    [SerializeField]
    AudioSource BGMSource;

    [SerializeField]
    MapManager map;


	// Use this for initialization
	void Start () {
        BGMSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (map.PlayBattle) {
            BGMSource.Stop();
            return;
        }
        else {

            BGMSource.Play();
        }
	}
}
