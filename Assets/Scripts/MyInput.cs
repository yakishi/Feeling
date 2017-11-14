using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInput{
    
    public static bool isButtonDown()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) 
                || Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.RightArrow);
    }

    public static Vector2 direction(bool onlyVertical = true)
    {
        Vector2 ret = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            ret += Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            ret += Vector2.down;
        }

        if (onlyVertical) {
            return ret;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            ret += Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            ret += Vector2.right;
        }

        return ret;
    }
}
