using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class SaveLoad : MonoBehaviour
{

	GV myGV = GV.Instance; // Save/Load クラスのインスタンスを取得

    public enum Type
    {
        Save,
        Load
    }

    /// <summary>
    /// セーブロードの種類
    /// </summary>
    [SerializeField]
    static Type type;
	private static Type Category { set { type = value; } }

    /// <summary>
    /// セーブデータをロード
    /// </summary>
    [SerializeField]
    Button[] saveSlots;

    void Start()
    {
        if (type == Type.Load) {
            openLoadUI();
			return;
        }
        openSaveUI();
	}

    void openSaveUI()
    {
        var usedSave = myGV.SData.usedSave;
        for (int i = 0; i < usedSave.Length; ++i) {
            var slot = saveSlots[i];
            var isUsed = usedSave[i];

            int slotIndex = i + 1;

            setText(slot, isUsed);
            slot.OnClickAsObservable()
                .Take(1)
                .Subscribe(_ => {
					myGV.slot = slotIndex; // GV 側へ通知
					myGV.GameDataSave(slotIndex);
					Debug.Log( "<color='red'>openSaveUI Function Called., saveSlot : " + slotIndex + "</color>" );
					Destroy(gameObject);
                })
                .AddTo(this);
        }
    }
    void openLoadUI()
    {

        var usedSave = myGV.SData.usedSave;
        for (int i = 0; i < usedSave.Length; ++i) {
            var slot = saveSlots[i];
            var isUsed = usedSave[i];

            if (!isUsed) {
                slot.enabled = false;
            }

            int slotIndex = i + 1;

            setText(slot, isUsed);
            slot.OnClickAsObservable()
                .Take(1)
                .Subscribe(_ => {
					myGV.slot = slotIndex; // GV 側へ通知
					myGV.GameDataLoad(slotIndex);
					myGV.SlotChangeParamUpdate( slotIndex );
					new ExampleTestSaveLoad( ).LoadTest( ); // 読込確認用
					Debug.Log( "<color='red'>openLoadUI Function Called., loadSlot : " + slotIndex + "</color>" );
					Destroy(/*this*/gameObject);
                })
                .AddTo(this);
        }
    }

    void setText(Button slot, bool isUsed)
    {
        var text = slot.GetComponentInChildren<Text>();

        if (isUsed) {
			TimeSpan t = new TimeSpan( 0, 0, myGV.GData.fixedTime[ int.Parse( slot.name ) ] );
			text.text = "使われている\nプレイ時間 : " + t;
            return;
        }
        text.text = "このスロットは使われていません";
    }

    [SerializeField]
    static GameObject savePrefab;
    static GameObject SavePrefab {
        get
        {
            if (savePrefab == null) {
                savePrefab = Resources.Load<GameObject>("Prefabs/Common/Save");
            }
            return savePrefab;
        }
    }

    [SerializeField]
    static GameObject loadPrefab;
    static GameObject LoadPrefab {
        get
        {
            if (loadPrefab == null) {
                loadPrefab = Resources.Load<GameObject>("Prefabs/Common/Load");
            }
            return loadPrefab;
        }
    }
    public static GameObject CreateUI(Type type, GameObject parent)
    {
        if (type == Type.Load) {
			Category = Type.Load;
            return Instantiate(LoadPrefab, parent.transform, false);
        }
        else if( type == Type.Save ) {
			Category = Type.Save;
			return Instantiate(SavePrefab, parent.transform, false);
		}
		else return null;
	}
}
