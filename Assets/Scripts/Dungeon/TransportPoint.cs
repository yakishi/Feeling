using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPoint : FieldEvent {

    [SerializeField]
    protected TransportPoint nextTransPoint;
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void EventAction(PlayerEvent player)
    {
        base.EventAction(player);
        
        SceneController.startFade((fade) => {
            player.transform.position = nextTransPoint.gameObject.transform.position;

            fade.endWait();
            player.endEvent();
        }
        ,1.0f
        ,true );

        
    }
}
