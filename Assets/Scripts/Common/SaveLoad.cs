using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SaveLoad : MonoBehaviour
{
    public enum Type
    {
        Save,
        Load
    }

    /// <summary>
    /// セーブロードの種類
    /// </summary>
    [SerializeField]
    Type type;

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
        var usedSave = GV.SData.usedSave;
        for (int i = 0; i < usedSave.Length; ++i) {
            var slot = saveSlots[i];
            var isUsed = usedSave[i];

            int slotIndex = i + 1;

            setText(slot, isUsed);
            slot.OnClickAsObservable()
                .Take(1)
                .Subscribe(_ => {
                    GV.save(slotIndex);
                    Destroy(gameObject);
                })
                .AddTo(this);
        }
    }
    void openLoadUI()
    {

        var usedSave = GV.SData.usedSave;
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
                    GV.load(slotIndex);
                    Destroy(this);
                })
                .AddTo(this);
        }
    }

    void setText(Button slot, bool isUsed)
    {
        var text = slot.GetComponentInChildren<Text>();

        if (isUsed) {
            text.text = "使われている";
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
            return Instantiate(LoadPrefab, parent.transform, false);
        }
        return Instantiate(SavePrefab, parent.transform, false);
    }
}
