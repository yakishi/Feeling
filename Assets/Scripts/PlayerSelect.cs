using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour {
    BattlePlayer player;
    Vector3 defScale;

    // Use this for initialization
    void Awake () {
        player = this.gameObject.GetComponent<BattlePlayer>();
        defScale = new Vector3(1, 1, 1);
	}

    public void Select()
    {
        defScale = player.img.transform.localScale;
        player.img.transform.localScale = player.img.transform.localScale * 1.2f;
        player.img.glowSize = 10;
    }

    public void DeSelect()
    {
        player.img.transform.localScale = defScale;
        player.img.glowSize = 0;
    }
}
