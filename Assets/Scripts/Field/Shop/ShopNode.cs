using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShopNode : MonoBehaviour
{
    ButtonText button;

    [SerializeField]
    Text itemName;

    [SerializeField]
    Text price;

    public string Name { get { return itemName.text; } }
    public int PageNum { get { return pageNum; } }
    public int Price { get { return int.Parse(price.text); } }
    public ButtonText ActionButton { get { return button;  } }
    public int MaxQuantity { private set; get; }

    string id;
    public string Id {
        get { return id; }
    }
    int pageNum;

    public void StoreNode(string inId, string inName, int inPrice, int inPageNum, int maxQuantity)
    {
        id = inId;
        itemName.text = inName;
        price.text = inPrice.ToString();
        pageNum = inPageNum;
        MaxQuantity = maxQuantity;
        button = GetComponent<ButtonText>();
        if (maxQuantity == 0) {
            button.CanClick = false;
        }
    }
}
