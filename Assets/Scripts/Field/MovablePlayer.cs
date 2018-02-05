using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MovablePlayer : MonoBehaviour
{
    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    float speed = 10.0f;

    /// <summary>
    /// プレイヤーの方向
    /// </summary>
    public Vector2 direction { private set; get; }

    /// <summary>
    /// Rigidbody2D
    /// </summary>
    Rigidbody2D rigidbody2d;

    /// <summary>
    /// プレイヤーイベント
    /// </summary>
    PlayerEvent playerEvent;

    /// <summary>
    /// 移動時に流れるストリーム
    /// </summary>
    Subject<float> onMoveSubject = new Subject<float>();

    MapManager mapManager;

    /// <summary>
    /// 移動可能か
    /// </summary>
    public bool CanMove {
        get
        {
            if (mapManager != null) {
                return !playerEvent.IsPlayEvent || !mapManager.PlayBattle;
            }

            return !playerEvent.IsPlayEvent;
        }
    }


    // Use this for initialization
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerEvent = GetComponent<PlayerEvent>();
    }

    public void SetMapManager(MapManager manager)
    {
        mapManager = manager;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanMove) {
            rigidbody2d.velocity = getInputDirection() * speed;
            if (rigidbody2d.velocity != Vector2.zero) {
                onMoveSubject.OnNext(rigidbody2d.velocity.magnitude * Time.deltaTime);
                updateDirection();
            }
        }
    }

    /// <summary>
    /// 入力方向を取得
    /// </summary>
    /// <returns>入力された方向</returns>
    Vector3 getInputDirection()
    {
        var ret = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
            ret += Vector3Int.left;
        if (Input.GetKey(KeyCode.RightArrow))
            ret += Vector3Int.right;
        if (Input.GetKey(KeyCode.UpArrow))
            ret += Vector3Int.up;
        if (Input.GetKey(KeyCode.DownArrow))
            ret += Vector3Int.down;

        return ret.normalized;
    }

    void updateDirection()
    {
        if (rigidbody2d.velocity.x > 0) {
            direction = Vector2.right;
        }
        if (rigidbody2d.velocity.x < 0) {
            direction = Vector2.left;
        }
        if (rigidbody2d.velocity.y > 0) {
            direction = Vector2.up;
        }
        if (rigidbody2d.velocity.y < 0) {
            direction = Vector2.down;
        }
    }

    public bool canEncount()
    {
        return true;
    }

    public IObservable<float> onMoveAsObsevable()
    {
        return onMoveSubject;
    }
}
