using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShopNode : MonoBehaviour
{
    Button button;

    [SerializeField]
    Text itemName;

    [SerializeField]
    Text price;

    public string Name { get { return itemName.text; } }
    public int PageNum { get { return pageNum; } }
    public int Price { get { return int.Parse(price.text); } }
    public Button ActionButton { get { return button;  } }

    string id;
    public string Id {
        get { return id; }
    }
    int pageNum;

    public void StoreNode(string inId, string inName, int inPrice, int inPageNum)
    {
        id = inId;
        itemName.text = inName;
        price.text = inPrice.ToString();
        pageNum = inPageNum;
        button = GetComponent<Button>();
    }
}
