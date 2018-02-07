using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public class SavePoint : FieldEvent {

	GV gv;
	GameObject saveLoad;

	protected override void Start( ) {
        base.Start();
		gv = GV.Instance;
	}

	private void Update( ) {
		if(Input.GetKeyDown(KeyCode.Backspace) && saveLoad != null){
			Destroy(saveLoad);
		}
	}

	public override void EventAction( PlayerEvent player ) {
		base.EventAction( player );
		GameObject canvas = GameObject.Find( "Canvas" );

		saveLoad = SaveLoad.CreateUI( SaveLoad.Type.Save, canvas );

		saveLoad.OnDestroyAsObservable( )
		.Subscribe( d => {
			player.endEvent( );
		} )
		.AddTo( this );

		EventSystem.current.SetSelectedGameObject( canvas.transform.GetChild( 0 ).gameObject );
	}
}
