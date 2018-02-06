using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour {

    [SerializeField]
    Animator anim;

    [SerializeField]
    MovablePlayer movablePlayer;

	// Use this for initialization
	void Start () {
    }

	// Update is called once per frame
	void Update () {
        if (movablePlayer.direction.x > 0) {
            anim.SetBool("Right", true);
            anim.SetBool("Left", false);
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
        }
        if (movablePlayer.direction.x < 0) {
            anim.SetBool("Left", true);
            anim.SetBool("Right", false);
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
        }
        if (movablePlayer.direction.y > 0) {
            anim.SetBool("Up", true);
            anim.SetBool("Right", false);
            anim.SetBool("Left", false);
            anim.SetBool("Down", false);
        }
        if (movablePlayer.direction.y < 0) {
            anim.SetBool("Down", true);
            anim.SetBool("Right", false);
            anim.SetBool("Left", false);
            anim.SetBool("Up", false);
        }
    }
}
