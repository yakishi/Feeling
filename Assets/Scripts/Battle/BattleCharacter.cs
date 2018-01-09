using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public abstract  class BattleCharacter : MonoBehaviour
{
    protected int id;
    public int ID
    {
        get
        {
            return id;
        }

        set
        {
            id = value;             //死亡管理用の仮組
        }
    }

    protected int hp;
    public int Hp {
        get
        {
            return hp;
        }
    }

    protected int currentHp;
    public int CurrentHp {
        get
        {
            return currentHp;
        }
    }

    protected int atk;
    public int Atk {
        get
        {
            return atk;
        }
    }

    protected int def;
    public int Def
    {
        get
        {
            return def;
        }
    }

    protected int agl;
    public int Agl {
        get
        {
            return agl;
        }
    }

    public bool IsDead {
        get
        {
            return currentHp <= 0;
        }
    }

    public bool CanAction {
        get
        {
            return !isEndAction && !IsDead;
        }
    }

    protected int buffTurn;

    protected BattleController battleController;
    protected BattleUI battleUI;

    protected bool isEndAction = false;
    protected Subject<BattleCharacter> onEndActionSubject = new Subject<BattleCharacter>();

    /// <summary>
    /// id からデータを読み込み
    /// </summary>
    /// <param name="id"></param>
    public virtual void loadData(int id)
    {
        // 仮で作成
        hp = 100;
        currentHp = hp;
        atk = 10;
        def = 3;
        agl = Random.Range(10, 21);
        buffTurn = 0;
    }

    /// <summary>
    /// ターンの最初に呼び出し
    /// </summary>
    public virtual void beginTurn()
    {
        isEndAction = false;
    }

    /// <summary>
    /// 行動処理
    /// </summary>
    /// <remarks>
    /// ここで行動を決める
    /// </remarks>
    public abstract void startAction();

    /// <summary>
    /// アクションを実行
    /// </summary>
    /// <param name="actions">実行するアクション</param>
    protected void playAction(List<BattleAction> actions)
    {
        foreach (var act in actions) {
            foreach(var target in act.targets) {
                target.receiveEffect(act.effects);
            }
        }

        // TODO: アニメーションを再生する場合再生終了後に呼び出す必要あり
        isEndAction = true;
        onEndActionSubject.OnNext(this);
    }

    /// <summary>
    /// バトルコントローラーの設定
    /// </summary>
    /// <param name="setController">バトルコントローラー</param>
    public void setBattleController(BattleController setController)
    {
        battleController = setController;
    }

    public void setBattleUI(BattleUI setUI)
    {
        battleUI = setUI;
    }

    /// <summary>
    /// バトル開始時に呼ばれる
    /// </summary>
    public virtual void battleStart()
    {
    }

    public void receiveEffect(Dictionary<BattleParam, int> effects)
    {
        // 仮でswitch を使用 パラメータ系をDictionary で保存するとらくかもしれない
        // TODO: 一定ターンのバフなどの処理を入れる必要あり
        foreach (var targetParam in effects.Keys) {
            switch (targetParam) {
                case BattleParam.HP:
                    currentHp += effects[targetParam];
                    break;
                case BattleParam.Atk:
                    atk += effects[targetParam];
                    break;
                case BattleParam.Def:
                    def += effects[targetParam];
                    break;
                case BattleParam.Agl:
                    agl += effects[targetParam];
                    break;
            }
        }
    }

    public IObservable<BattleCharacter> onEndActionAsObservable()
    {
        return onEndActionSubject;
    }

}
