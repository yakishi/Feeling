using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class ItemShop : ShopBase
{
    protected override ShopBase showWindow(bool buy)
    {
        base.showWindow(buy);
        
        buyButton
            .OnClickAsObservable()
            .Subscribe(_ => {
                showWindow(true);
            })
            .AddTo(currentShop);

        // TODO : 現在のイベント進行度からアイテムを取得
        var itemList = SingltonItemManager.Instance.CDItem;
        //.Where(item => {
        //    return true;
        //});

        maxPage = itemList.Count / maxNode + (itemList.Count % maxNode == 0 ? 0 : 1);
        int itemIndex = 0;
        for (int i = 0; i < maxPage; ++i) {
            var pageObj = Instantiate(pagePrefab, scrollRect.content.transform);
            for (int j = 0; j < maxNode && itemIndex < itemList.Count; ++j) {
                var node = Instantiate(nodePrefab, pageObj.transform);
                var item = itemList[itemIndex];
                var price = true ? item.buy : item.sell;
                node.StoreNode(item.id, item.name, price, true, i);
                nodes.Add(node);
                ++itemIndex;
            }
        }

        Observable.NextFrame()
            .Subscribe(_ => {
                EventSystem.current.SetSelectedGameObject(nodes.First().gameObject);
            });

        return this;
    }

    protected override void Update()
    {
        base.Update();
    }
}
