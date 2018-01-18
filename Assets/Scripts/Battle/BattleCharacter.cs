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
            id = value;
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

    protected int matk;
    public int MAtk
    {
        get
        {
            return matk;
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

    protected SingltonPlayerManager.PlayerParameters param;
    public SingltonPlayerManager.PlayerParameters Param
    {
        get
        {
            return param;
        }
    }


    public class BuffManager
    {
        private SingltonSkillManager.SkillInfo skill;
        private int remainBuffTurn;

        public BuffManager(SingltonSkillManager.SkillInfo info)
        {
            skill = info;
            remainBuffTurn = info.DT;
        }

        public SingltonSkillManager.SkillInfo GetSkill
        {
            get
            {
                return skill;
            }
        }

        public int RemainBuffTurn
        {
            get
            {
                return remainBuffTurn;
            }
        }

        public void ResetBuff()
        {
            remainBuffTurn = skill.DT;
        }

        public void AdvanceBuffTurn()
        {
            remainBuffTurn--;
        }
    }

    //後で消す
    SingltonSkillManager skillManager;
    protected List<SingltonSkillManager.SkillInfo> skillList;

    protected BattleController battleController;
    protected BattleUI battleUI;

    protected bool isEndAction = false;
    protected Subject<BattleCharacter> onEndActionSubject = new Subject<BattleCharacter>();

    public List<BuffManager> buffList;

    /// <summary>
    /// id からデータを読み込み
    /// </summary>
    /// <param name="id"></param>
    public virtual void loadData(string id,GameManager gameManager,bool dummy = false)
    {
        if (dummy) {
            hp = 100;
            currentHp = hp;
            atk = 10;
            matk = 10;
            def = 10;
            agl = Random.Range(1,6);
            buffList = new List<BuffManager>();
            return;
        }
        List<SingltonPlayerManager.PlayerParameters> playersParam = gameManager.PlayerList;
        foreach(var i in playersParam) {
            if(i.ID == id) {
                param = i;
            }
        }
        if(param == null) {
            Debug.Log("Not Found ID :" + id);
        }
        hp = param.BES.HP;
        currentHp = param.BES.HP;
        atk = param.BES.Atk;
        matk = param.BES.Matk;
        def = param.BES.Def;
        agl = param.BES.Agl;
        buffList = new List<BuffManager>();
        skillManager = gameManager.SkillManager;
        //ToDo PlayerParamにスキルリストが追加され次第変更
        skillList = skillManager.CDSkill;
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

    public void endAction()
    {
        foreach (var n in buffList) {
            n.AdvanceBuffTurn();
        }
        foreach (var n in buffList.ToArray()) {
            if (n.RemainBuffTurn <= 0) {
                buffList.Remove(n);


            }
        }
        if (buffList.Count <= 0) Debug.Log("none");
        foreach (var n in buffList) {
            var s = n.GetSkill;
            Debug.Log(s.skill + " : " + n.RemainBuffTurn);
        }
    }

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
        endAction();
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

    public void setBattlePram(SingltonPlayerManager.PlayerParameters playerParameters)
    {
        param = playerParameters;
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
