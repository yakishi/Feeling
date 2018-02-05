using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class SaveLoad : MonoBehaviour
{

    GV myGV = GV.Instance; // Save/Load クラスのインスタンスを取得
	SaveLoadAudio mySLA;

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
		mySLA = new SaveLoadAudio( );
        var buttons = GetComponentsInChildren<Button>();
        Observable.NextFrame()
            .Subscribe(_ => {
                EventSystem.current.SetSelectedGameObject(buttons[myGV.slot - 1].gameObject);
            });


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
					mySLA.PlaySE( "Save", slot.transform.parent.parent.parent.GetComponent<AudioSource>( ) );
					Debug.Log("<color='red'>openSaveUI Function Called., saveSlot : " + slotIndex + "</color>");
					// 2 秒後に出す ( 再生し終わった後 )
					Observable.Timer( TimeSpan.FromMilliseconds( 2000 ) )
						.Subscribe( x => Destroy( /*this*/gameObject ) );
				} )
                .AddTo(this);

            slot.OnSelectAsObservable()
                .Subscribe(_ => {
                    slot.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                })
                .AddTo(this);

            slot.OnDeselectAsObservable()
                .Subscribe(_ => {
                    slot.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
                    myGV.SlotChangeParamUpdate(slotIndex);
                    new ExampleTestSaveLoad().LoadTest(); // 読込確認用
					mySLA.PlaySE( "Load", slot.transform.parent.parent.parent.GetComponent<AudioSource>( ) );
                    Debug.Log("<color='red'>openLoadUI Function Called., loadSlot : " + slotIndex + "</color>");
					// 3 秒後に出す ( 再生し終わった後 )
					Observable.Timer( TimeSpan.FromMilliseconds( 3000 ) )
						.Subscribe( x => Destroy( /*this*/gameObject ) );
                })
                .AddTo(this);

            slot.OnSelectAsObservable()
                .Subscribe(_ => {
                    slot.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                })
                .AddTo(this);

            slot.OnDeselectAsObservable()
                .Subscribe(_ => {
                    slot.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                })
                .AddTo(this);
        }
    }

    void setText(Button slot, bool isUsed)
    {
        var text = slot.GetComponentInChildren<Text>();
        Text saveText = slot.transform.parent.GetChild(int.Parse(slot.name) - 1).GetChild(1).GetComponent<Text>();
        if (type == Type.Save) saveText.text = "セーブ" + int.Parse(slot.name).ToString();
        if (type == Type.Load) saveText.text = "ロード" + int.Parse(slot.name).ToString();

        if (isUsed) {
            TimeSpan t = new TimeSpan(0, 0, myGV.GData.fixedTime[int.Parse(slot.name)]);
            text.text = "使われている\nプレイ時間 : " + t;
            return;
        }
        text.text = "データがありません";
    }

    [SerializeField]
    static GameObject savePrefab;
    static GameObject SavePrefab
    {
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
    static GameObject LoadPrefab
    {
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
        else if (type == Type.Save) {
            Category = Type.Save;
            return Instantiate(SavePrefab, parent.transform, false);
        }
        else return null;
    }
}
