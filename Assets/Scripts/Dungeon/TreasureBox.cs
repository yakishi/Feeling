using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TreasureBox : FieldEvent {

    [SerializeField]
    Sprite openImg;

    [SerializeField]
    GameObject textBox;
    Text text;

    [SerializeField]
    string itemId;

    bool isOpen;
    [SerializeField]
    bool isEndDungeon;

    protected override void Start()
    {
        base.Start();
        text = textBox.GetComponentInChildren<Text>();
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public override void EventAction(PlayerEvent player)
    {
        base.EventAction(player);

        if(isOpen) {
            player.endEvent();
            return;
        }

        string itemName = "" ;
        
        foreach(var i in SingltonItemManager.Instance.CDItem) {
            if(i.id == itemId) {

                itemName = i.name;
                bool newItem = true;
                List<string> itemList = new List<string>(GV.Instance.GData.Items.itemList.Keys);

                foreach (var haveItem in itemList) {
                    if (haveItem == itemId) {

                        if (GV.Instance.GData.Items.itemList[haveItem] + 1 > i.max) {
                            textBox.SetActive(true);
                            text.text = itemName + " を手に入れようとしたが このアイテムは以上持てない/n戻しておこう";

                            player.endEvent();
                            return;
                        }

                        GV.Instance.GData.Items.itemList[haveItem] += 1;
                        newItem = false;
                        break;
                    }
                }

                if (newItem) {
                    GV.Instance.GData.Items.itemList.Add(i.id, 1);
                }
            }
        }

        textBox.SetActive(true);
        text.text = itemName + " を 手に入れた";
        isOpen = true;

        Observable.Timer(System.TimeSpan.FromMilliseconds(800.0))
            .Take(1)
            .Subscribe(_ => {
                textBox.SetActive(false);
                gameObject.GetComponent<SpriteRenderer>().sprite = openImg;
                if (isEndDungeon) {
                    SceneController.sceneTransition(SceneName.SceneNames.azito, 2.0f, SceneController.FadeType.Fade);
                }
                player.endEvent();
                return;
            });
    }
}
