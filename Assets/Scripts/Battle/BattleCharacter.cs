using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public abstract class BattleCharacter : MonoBehaviour
{
    protected SingltonPlayerManager.PlayerParameters param;
    public SingltonPlayerManager.PlayerParameters Param
    {
        get
        {
            return param;
        }
    }

    protected SingltonPlayerManager.Status status;
    public SingltonPlayerManager.Status Status
    {
        get
        {
            return status;
        }
    }
    public string ID
    {
        get
        {
            return param.ID;
        }
        //モンスターがデータができるまでの仮
        set
        {
            param.ID = value;
        }

    }

    public int Hp {
        get
        {
            return status.HP;
        }
    }

    protected int currentHp;
    public int CurrentHp {
        get
        {
            return currentHp;
        }
    }

    public int Atk {
        get
        {
            return status.Atk;
        }
    }

    public int Def
    {
        get
        {
            return status.Def;
        }
    }

    public int MAtk
    {
        get
        {
            return status.Matk;
        }
    }

    public int Agl {
        get
        {
            return status.Agl;
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
    public virtual void loadData(string id, GameManager gameManager, bool dummy = false)
    {
        if (dummy) {
            status = new SingltonPlayerManager.Status(100, 100, 10, 10, 10, 10, 10, Random.Range(1, 20), SingltonSkillManager.Feel.Do, 20);
            currentHp = status.HP;
            buffList = new List<BuffManager>();
            return;
        }
        List<SingltonPlayerManager.PlayerParameters> playersParam = gameManager.PlayerList;
        foreach (var i in playersParam) {
            if (i.ID == id) {
                param = i;
            }
        }
        if (param == null) {
            Debug.Log("Not Found ID :" + id);
        }
        status = param.STATUS;
        currentHp = param.STATUS.HP;
        buffList = new List<BuffManager>();
        skillManager = gameManager.SkillManager;
        //ToDo PlayerParamにスキルリストが追加され次第変更
        skillList = CreateSkillList(param.SkillList);
    }

    List<SingltonSkillManager.SkillInfo> CreateSkillList(List<string> idList)
    {
        List<SingltonSkillManager.SkillInfo> list = new List<SingltonSkillManager.SkillInfo>();
        foreach(var id in idList) {
            foreach(var skill in skillManager.CDSkill) {
                if(id == skill.ID) {
                    list.Add(skill);
                }
            }
        }

        return list;
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
                    status.HP += effects[targetParam];
                    break;
                case BattleParam.Def:
                    status.Def += effects[targetParam];
                    break;
                case BattleParam.Agl:
                    status.Agl += effects[targetParam];
                    break;
            }
        }
    }

    public IObservable<BattleCharacter> onEndActionAsObservable()
    {
        return onEndActionSubject;
    }

}
