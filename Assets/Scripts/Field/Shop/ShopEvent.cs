using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ShopEvent : FieldEvent
{
    [SerializeField]
    Canvas targetCanvas;
    [SerializeField]
    ShopBase shopPrefab;

    public override void EventAction(PlayerEvent player)
    {
        base.EventAction(player);
        var shop = Instantiate(shopPrefab, targetCanvas.transform);
        shop.OnDestroyAsObservable()
            .Subscribe(_ => {
                player.endEvent();
            });
    }
}
