using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public class ShopBase : MonoBehaviour
{
    public class ItemInfo
    {
        public string id;
        public string name;
        public int price;
        public int maxQuantity;
    }

    [SerializeField]
    ShopNode nodePrefab;

    [SerializeField]
    float ScrollTIme = 0.25f;

    [SerializeField]
    ScrollRect shopWindow;

    [SerializeField]
    GameObject pagePrefab;

    [SerializeField]
    int maxNode = 6;

    [SerializeField]
    CanvasGroup buttons;

    [SerializeField]
    protected Button buyButton;

    [SerializeField]
    protected Button sellButton;

    [SerializeField]
    protected Button exitButton;

    [SerializeField]
    QuantityUI quantity;

    [SerializeField]
    Text pageCount;
    [SerializeField]
    Text myGoldsText;
    [SerializeField]
    Text nothingItem;

    Button selectedButton;
    GameObject prevNode;
    GameObject selectedNode;

    int maxPage;
    int currentPage;
    bool isChangePage;
    bool isOpenWindow;
    bool showQuantityUI;
    bool isBuy;
    float deltaY;
    int currentSelectedIndex = 0;
    protected List<ShopNode> nodes = new List<ShopNode>();

    protected virtual void Start()
    {
        buyButton.OnClickAsObservable()
            .Subscribe(_ => {
                selectedButton = buyButton;
                currentSelectedIndex = 0;
                createBuyItems();
                isBuy = true;
            });
        sellButton.OnClickAsObservable()
            .Subscribe(_ => {
                selectedButton = sellButton;
                currentSelectedIndex = 0;
                createSellItems();
                isBuy = false;
            });

        exitButton.OnClickAsObservable()
            .Subscribe(_ => {
                exit();
            });

        quantity.ActionButton
            .OnClickAsObservable()
            .Subscribe(_ => {
                quantity.gameObject.SetActive(false);

                var group = shopWindow.GetComponent<CanvasGroup>();
                group.interactable = true;
                decisionAction(quantity.getInfo(), isBuy);
                var createItemList = createItems();

                if (createItemList.Count == 0) {
                    return;
                }

                currentSelectedIndex = Mathf.Min(currentSelectedIndex, createItemList.Count - 1);
                Observable.NextFrame()
                    .Subscribe(_2 => {
                        EventSystem.current.SetSelectedGameObject(nodes[currentSelectedIndex].gameObject);
                    });
            });

        Observable.NextFrame()
            .Subscribe(_ => {
                EventSystem.current.SetSelectedGameObject(buyButton.gameObject);
            });

        quantity.gameObject.SetActive(false);
        shopWindow.gameObject.SetActive(false);
    }

    protected List<ShopNode> updateWindow(List<ItemInfo> shopItems)
    {
        shopWindow.gameObject.SetActive(true);
        buttons.interactable = false;
        isOpenWindow = true;
        nodes.Clear();
        nothingItem.gameObject.SetActive(false);

        foreach (Transform child in shopWindow.content) {
            Destroy(child.gameObject);
        }

        if (shopItems.Count == 0) {
            nothingItem.gameObject.SetActive(true);

            maxPage = 1;
            return nodes;
        }

        maxPage = shopItems.Count / maxNode + (shopItems.Count % maxNode == 0 ? 0 : 1);
        Debug.Log(maxPage);
        int itemIndex = 0;

        for (int i = 0; i < maxPage; ++i) {
            var pageObj = Instantiate(pagePrefab, shopWindow.content.transform);
            for (int j = 0; j < maxNode && itemIndex < shopItems.Count; ++j) {
                var node = Instantiate(nodePrefab, pageObj.transform);
                var item = shopItems[itemIndex];
                node.StoreNode(item.id, item.name, item.price, i, item.maxQuantity);
                node.ActionButton
                    .OnClickAsObservable()
                    .Subscribe(_ => {
                        var group = shopWindow.GetComponent<CanvasGroup>();
                        group.interactable = false;
                        currentSelectedIndex = shopItems.IndexOf(item);
                        quantity.gameObject.SetActive(true);
                        quantity.storeInfo(node);
                        showQuantityUI = true;

                        Observable.NextFrame()
                            .Subscribe(_2 => {
                                EventSystem.current.SetSelectedGameObject(quantity.ActionButton.gameObject);
                            });
                    });
                nodes.Add(node);
                ++itemIndex;
            }
        }

        Observable.NextFrame()
            .Subscribe(_ => {
                EventSystem.current.SetSelectedGameObject(nodes[currentSelectedIndex].gameObject);
            });

        return nodes;
    }

    List<ShopNode> createItems()
    {
        if (isBuy) {
            return createBuyItems();
        }

        return createSellItems();
    }

    protected virtual List<ShopNode> createBuyItems()
    {
        return new List<ShopNode>();
    }
    protected virtual List<ShopNode> createSellItems()
    {
        return new List<ShopNode>();
    }

    protected virtual void Update()
    {
        myGoldsText.text = SingltonItemManager.Instance.SDItem.possessionGolds.ToString() + " G";

        bool isReturn = false;
        if (Input.GetButtonDown("Cancel")) {
            isReturn = cancelAction();
        }

        if (isReturn || !isOpenWindow) {
            return;
        }
        if (nothingItem.gameObject.activeSelf) {
            return;
        }

        prevNode = selectedNode;
        selectedNode = EventSystem.current.currentSelectedGameObject;
        if (selectedNode == null) {
            return;
        }
        if (selectedNode != prevNode) {
            if (selectedNode.GetComponent<ShopNode>() == null) {
                return;
            }
            currentPage = selectedNode.GetComponent<ShopNode>().PageNum;
            pageCount.text = (currentPage + 1) + "/" + maxPage;
        }
        var targetNormalizedPosition = (float)currentPage / (maxPage - 1);
        var currentNormalizePosition = shopWindow.horizontalNormalizedPosition;

        deltaY = Time.deltaTime / ScrollTIme * (currentNormalizePosition > targetNormalizedPosition ? -1.0f : 1.0f);
        if (Mathf.Abs(currentNormalizePosition - targetNormalizedPosition) <= deltaY) {
            currentNormalizePosition = targetNormalizedPosition;
            deltaY = 0.0f;
        }

        if (deltaY == 0.0f) {
            return;
        }

        currentNormalizePosition += deltaY;
        shopWindow.horizontalNormalizedPosition = currentNormalizePosition;
    }

    bool cancelAction()
    {
        if (!Input.GetButtonDown("Cancel")) {

        }
        if (!isOpenWindow) {
            exit();
            return true;
        }

        if (showQuantityUI) {
            quantity.gameObject.SetActive(false);

            var group = shopWindow.GetComponent<CanvasGroup>();
            group.interactable = true;

            Observable.NextFrame()
                .Subscribe(_2 => {
                    EventSystem.current.SetSelectedGameObject(nodes[currentSelectedIndex].gameObject);
                });
            showQuantityUI = false;
            return true;
        }

        shopWindow.horizontalNormalizedPosition = 0.0f;
        shopWindow.gameObject.SetActive(false);
        buttons.interactable = true;
        isOpenWindow = false;

        Observable.NextFrame()
            .Subscribe(_ => {
                EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
            });
        return false;
    }

    protected virtual void decisionAction(QuantityUI.Info info, bool buy)
    {
    }

    protected virtual void exit()
    {
        Destroy(this);
    }
}
