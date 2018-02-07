using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MapManager : MonoBehaviour
{
    /// <summary>
    /// プレイヤープレファブ
    /// </summary>
    [SerializeField]
    MovablePlayer player;

    [SerializeField]
    GameObject battleControllerPrefab;
    BattleController battleController;

    /// <summary>
    /// エンカウント移動量(最大値)
    /// </summary>
    [SerializeField]
    float encountMax = 15.0f;
    /// <summary>
    /// エンカウント移動量(最低値)
    /// </summary>
    [SerializeField]
    float encountMin = 10.0f;

    float nextEncount;
    float movableDistance;

    bool playBattle = false;
    public bool PlayBattle { get { return playBattle; } }

    void Start()
    {
        nextEncount = Random.Range(encountMin, encountMax);
        player.SetMapManager(this);
        player.onMoveAsObsevable()
            .Subscribe(length => {
                if (!player.canEncount()) {
                    return;
                }
                movableDistance += length;

                if (nextEncount <= movableDistance) {
                    playBattle = true;
                    SceneController.startFade((fade) => {
                        battleController = Instantiate(battleControllerPrefab).GetComponentInChildren<BattleController>();
                        battleController.Initialzie();
                        battleController.OnDestroyAsObservable()
                            .Subscribe(_ => {
                                nextEncount = Random.Range(encountMin, encountMax);
                                movableDistance = 0.0f;

                                playBattle = false;
                            });

                        fade.endWait();
                    }, 1.0f, true);
                }
            });

    }
}
