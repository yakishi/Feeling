using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSceneEvent : FieldEvent
{
    [SerializeField]
    SceneName.SceneNames sceneName;

    public override void EventAction(PlayerEvent player)
    {
        base.EventAction(player);
        SceneController.sceneTransition(sceneName, 1.0f, SceneController.FadeType.Fade);
    }
}
