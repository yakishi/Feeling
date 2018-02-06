using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class QuantityUI : MonoBehaviour
{
    public class Info
    {
        public string id;
        public int quantity;
        public int prize;
    }

    [SerializeField]
    Text quantityText;
    [SerializeField]
    Text prizeText;

    Info info = new Info();
    Button button;
    public Button ActionButton {
        get
        {
            if (button == null) {
                button = GetComponent<Button>();
            }
            return button;
        }
    }

    int maxQuantity;

    public void storeInfo(ShopNode shopNode)
    {
        info.id = shopNode.Id;
        info.prize = shopNode.Price;
        maxQuantity = shopNode.MaxQuantity;
    }
    // Use this for initialization
    void Start()
    {
        ActionButton.OnClickAsObservable()
            .Subscribe(_ => {
                gameObject.SetActive(false);
            });
    }

    void OnEnable()
    {
        info.quantity = 1;
        quantityText.text = info.quantity.ToString();
        prizeText.text = (info.quantity * info.prize).ToString();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            --info.quantity;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            ++info.quantity;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            info.quantity += 10;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            info.quantity -= 10;
        }

        info.quantity = Mathf.Clamp(info.quantity, 1, maxQuantity);
        quantityText.text = info.quantity.ToString();
        prizeText.text = (info.quantity * info.prize).ToString();
    }

    public Info getInfo()
    {
        return info;
    }
}
