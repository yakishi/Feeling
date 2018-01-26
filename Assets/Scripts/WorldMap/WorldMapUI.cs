using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class WorldMapUI : MonoBehaviour {

    [SerializeField]
    private Button[] destinationList = new Button[7];
    [SerializeField]
    private Image[] dungeonRedPointArry = new Image[6];
    [SerializeField]
    GameObject listGrid;
    
    [SerializeField]
    private GameObject dungeonDetail;
    [SerializeField]
    private Button[] goOrBack = new Button[2];
    [SerializeField]
    GameObject detailGrid;

    List<GameObject> beforeSelect;

    Vector3 defScale;

    // Use this for initialization
    void Start () {
        beforeSelect = new List<GameObject>();
        defScale = new Vector3(0.5f, 0.5f, 1.0f);

        ButtonsObservableInitialize();

        dungeonDetail.SetActive(false);
        BattleUI.ActiveButton(listGrid);
	}

    void ButtonsObservableInitialize()
    {
        foreach(Button b in destinationList) {
            b.OnClickAsObservable()
                .Subscribe(_ => {
                    dungeonDetail.SetActive(true);
                    BattleUI.NotActiveButton(listGrid);
                    BattleUI.ActiveButton(detailGrid);

                    beforeSelect.Add(b.gameObject);
                })
                .AddTo(this);
        }

        goOrBack[0].OnClickAsObservable()
            .Subscribe(_ => {
                //ダンジョンへのシーン遷移

                beforeSelect.Clear();
            })
            .AddTo(this);

        goOrBack[1].OnClickAsObservable()
            .Subscribe(_ => {
                BattleUI.NotActiveButton(detailGrid);
                BattleUI.ActiveButton(listGrid,beforeSelect[beforeSelect.Count - 1]);
                dungeonDetail.SetActive(false);

                beforeSelect.Clear();
            })
            .AddTo(this);

    }

    // Update is called once per frame
    void Update () {
    }


    public void EnlargementDungeon(int i)
    {
        dungeonRedPointArry[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);
    }

    public void ReductionDungeon(int i)
    {
        dungeonRedPointArry[i].transform.localScale = defScale;
    }
}
