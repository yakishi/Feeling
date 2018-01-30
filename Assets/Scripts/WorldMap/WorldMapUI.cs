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
    [SerializeField]
    SceneName.SceneNames[] sceneNames;
    SceneName.SceneNames targetScene;

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
                SceneController.sceneTransition(targetScene, 1.0f, SceneController.FadeType.Fade);
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
        if (i < dungeonRedPointArry.Length) {
            dungeonRedPointArry[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);
        }
        targetScene = sceneNames[i];
    }

    public void ReductionDungeon(int i)
    {
        dungeonRedPointArry[i].transform.localScale = defScale;
    }
}
