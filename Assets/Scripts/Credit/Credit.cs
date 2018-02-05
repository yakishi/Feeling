using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour {

    GameObject audioManager;

	// Use this for initialization
	void Start () {
        audioManager = GameObject.Find("AudioManager");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SceneController.sceneTransition("Title",2.0f, SceneController.FadeType.Fade);

            if (audioManager != null)
                DontDestroyOnLoad(audioManager);
        }
	}
}
