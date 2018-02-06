using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public class ShopBase : FieldEvent
{
    public enum ShopTyep
    {
        EquipmentShop,
        ItemShop
    }

    public class ShopInfo
    {
        int id;
        string name;
        int price;
    }

    [SerializeField]
    protected ShopNode nodePrefab;

    [SerializeField]
    float ScrollTIme = 0.5f;

    [SerializeField]
    protected ScrollRect pages;

    [SerializeField]
    protected GameObject pagePrefab;

    [SerializeField]
    protected int maxNode = 6;

    [SerializeField]
    protected Button buyButton;

    [SerializeField]
    protected Button sellButton;

    [SerializeField]
    protected Button exitButton;

    protected int maxPage;

    static public ShopBase currentShop;
    protected GameObject prevNode;
    protected GameObject selectedNode;

    protected int currentPage;
    protected bool isChangePage;
    protected float deltaY;
    protected List<ShopNode> nodes = new List<ShopNode>();

    public void show()
    {
        currentShop.buyButton
            .OnClickAsObservable()
            .Subscribe(_ => {
                currentShop.showWindow(true);
            })
            .AddTo(currentShop);

        currentShop.sellButton
            .OnClickAsObservable()
            .Subscribe(_ => {
                currentShop.showWindow(false);
            })
            .AddTo(currentShop);

        currentShop.exitButton
            .OnClickAsObservable()
            .Subscribe(_ => {
                Destroy(currentShop);
            })
            .AddTo(currentShop);

        Observable.NextFrame()
            .Subscribe(_ => {
                EventSystem.current.SetSelectedGameObject(currentShop.buyButton.gameObject);
            });
    }

    protected virtual void showWindow()
    {
        // TODO : 現在のイベント進行度からアイテムを取得
        var itemList = SingltonItemManager.Instance.CDItem;
        //.Where(item => {
        //    return true;
        //});

        maxPage = itemList.Count / maxNode + itemList.Count % maxNode == 0 ? 0 : 1;
        Debug.Log(maxPage);
        int itemIndex = 0;
        for (int i = 0; i < maxPage; ++i) {
            Debug.Log(i);
            var pageObj = Instantiate(pagePrefab, scrollRect.content.transform);
            for (int j = 0; j < maxNode && itemIndex <itemList.Count; ++j) {
                var node = Instantiate(nodePrefab, pageObj.transform);
                var item = itemList[itemIndex];
                var price = buy ? item.buy : item.sell;
                node.StoreNode(item.id, item.name, price, j);
                nodes.Add(node);
                ++itemIndex;
            }
        }

        Observable.NextFrame()
            .Subscribe(_ => {
                EventSystem.current.SetSelectedGameObject(nodes.First().gameObject);
            });
    }

    protected virtual void Update()
    {
        prevNode = selectedNode;
        selectedNode = EventSystem.current.currentSelectedGameObject;
        if (selectedNode != prevNode) {
            currentPage = selectedNode.GetComponent<ShopNode>().PageNum;
        }
        var targetNormalizedPosition = (float)currentPage / maxPage;
        var currentNormalizePosition = scrollRect.horizontalNormalizedPosition;

        deltaY = Time.deltaTime / ScrollTIme * (currentNormalizePosition > targetNormalizedPosition ? -1.0f : 1.0f);
        if (Mathf.Abs(currentNormalizePosition - targetNormalizedPosition) <= deltaY) {
            currentNormalizePosition = targetNormalizedPosition;
            deltaY = 0.0f;
        }

        if (deltaY == 0.0f) {
            return;
        }

        currentNormalizePosition += deltaY;
        scrollRect.horizontalNormalizedPosition = currentNormalizePosition;
    }
}
