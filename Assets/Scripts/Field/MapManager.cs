using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/*
 * TODO : バトルの読み込み
 *        エンカウントするモンスターのロード
 */

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
                Debug.Log(movableDistance);

                if (nextEncount <= movableDistance) {
                    SceneController.startFade(() => {
                        battleController = Instantiate(battleControllerPrefab).GetComponentInChildren<BattleController>();
                        playBattle = true;
                    }, 1.0f);
                }
            });

        // TODO : BattleController でバトル終了の実装後アンコメント
        battleController.OnDestroyAsObservable()
            .Subscribe(_ => {
                nextEncount = Random.Range(encountMin, encountMax);
                playBattle = false;
            });
    }
}
