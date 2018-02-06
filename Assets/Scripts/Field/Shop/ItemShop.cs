using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class ItemShop : ShopBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override List<ShopNode> createBuyItems()
    {
        var possessionGolds = GV.Instance.GData.possessionGolds;
        var items = SingltonItemManager.Instance
        .CDItem
        // 現在進行用のフラグが不明なため1のみ販売
        .Where(item => item.shopFlag >= 1 && item.shopFlag <= 1)
        .Select(item => {
            var maxQuantity = Mathf.Min(possessionGolds / item.buy, item.max);
            return new ItemInfo
            {
                id = item.id,
                name = item.name,
                price = item.buy,
                maxQuantity = maxQuantity
            };
        })
        .ToList();

        var itemNodes = updateWindow(items);

        return itemNodes;
    }

    protected override List<ShopNode> createSellItems()
    {
        var itemManager = SingltonItemManager.Instance;
        var items = itemManager
            .CDItem
            .Where(item => {
                if (itemManager.SDItem.itemList.ContainsKey(item.id)) {
                    return itemManager.SDItem.itemList[item.id] != 0;
                }
                return false;
            })
            .Select(item => {
                var maxQuantity = itemManager.SDItem.itemList[item.id];
                return new ItemInfo
                {
                    id = item.id,
                    name = item.name,
                    price = item.sell,
                    maxQuantity = maxQuantity
                };
            })
            .ToList();
        var itemNodes = updateWindow(items);

        return itemNodes;
    }

    protected override void decisionAction(QuantityUI.Info info, bool buy)
    {
        var itemManager = SingltonItemManager.Instance;
        var items = itemManager.SDItem.itemList;

        if (buy) {
            if (items.ContainsKey(info.id)) {
                items[info.id] += info.quantity;
            }
            else {
                items[info.id] = info.quantity;
            }

            GV.Instance.GData.possessionGolds -= info.quantity * info.prize;
            return;
        }

        items[info.id] -= info.quantity;
        GV.Instance.GData.possessionGolds += info.quantity * info.prize;
    }

    protected override void Update()
    {
        base.Update();
    }
}
