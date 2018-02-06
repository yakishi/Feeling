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
    private string tempId;
    public string ID
    {
        get
        {
            if(param != null) return param.ID;
            return tempId;
        }
        //モンスターがデータができるまでの仮
        set
        {
            if(param != null) param.ID = value;
            tempId = value;
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

    public int Mp
    {
        get
        {
            return status.MP;
        }
    }

    protected int currentMp;
    public int CurrentMp
    {
        get
        {
            return currentMp;
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

    public int MDef
    {
        get
        {
            return status.Mgr;
        }
    }

    public int Agl {
        get
        {
            return status.Agl;
        }
    }

    public int Luc
    {
        get
        {
            return status.Luc;
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
        private string id;
        private BattleParam influence;
        private int value;
        private int buffTurn;
        private int remainBuffTurn;

        public BuffManager(SingltonSkillManager.SkillInfo info)
        {
            id = info.ID;
            influence = info.influence;
            value = 5;
            buffTurn = info.DT;
            remainBuffTurn = info.DT;
        }

        public BuffManager(SingltonItemManager.ItemList info)
        {
            id = info.id;
            buffTurn = info.buffTime;
            remainBuffTurn = info.buffTime;
        }

        public BattleParam getInfluence()
        {
            return influence;
        }

        public int Value()
        {
            return value;
        }

        public string GetSkillID
        {
            get
            {
                return id; ;
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
            remainBuffTurn = buffTurn;
        }

        public void AdvanceBuffTurn()
        {
            remainBuffTurn--;
        }
    }

    SingltonSkillManager skillManager;
    protected List<SingltonSkillManager.SkillInfo> skillList;

    protected BattleController battleController;
    protected BattleUI battleUI;

    protected bool isEndAction = false;
    protected Subject<BattleCharacter> onEndActionSubject = new Subject<BattleCharacter>();

    public List<BuffManager> buffList;

    [SerializeField]
    public GlowImage img;

    public bool frontMember;

    public SingltonSkillManager.CollectionValue feelInfo;

    /// <summary>
    /// id からデータを読み込み
    /// </summary>
    /// <param name="id"></param>
    public virtual void loadData(string id, GameManager gameManager)
    {
        List<SingltonPlayerManager.PlayerParameters> playersParam = gameManager.PlayerList;
        int cnt = 0;
        foreach (var i in playersParam) {
            if (i.ID == id) {
                param = i;
            }
        }

       
        if (param == null) {
            List<SingltonEnemyManager.EnemyParameters> enemyParam = gameManager.EnemyManager.GetEnemyState;
            param = new SingltonPlayerManager.PlayerParameters();
            foreach(var m in enemyParam) {
                if(m.ID == id) {
                    param.ID = m.ID;
                    param.Lv = m.LV;
                    param.Name = m.NAME;
                    param.STATUS = new SingltonPlayerManager.Status(m.HP, m.MP, m.ATK, m.DEF, m.MATK, m.MDEF, m.LUCKY, m.SPD);
                    feelInfo = new SingltonSkillManager.CollectionValue(m.FEELING,  -3);
                    param.TotalExp = m.DROPEXP;
                    frontMember = true;
                }
            }
        }
        status = param.STATUS;
        currentHp = param.STATUS.HP;
        currentMp = param.STATUS.MP;
        buffList = new List<BuffManager>();
        skillManager = gameManager.SkillManager;

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

    public virtual void endAction()
    {
        foreach (var n in buffList) {
            n.AdvanceBuffTurn();
        }
        foreach (var n in buffList.ToArray()) {
            if (n.RemainBuffTurn <= 0) {

                switch (n.getInfluence()) {
                    case BattleParam.HP:
                        currentHp -= n.Value();
                        break;
                    case BattleParam.MP:
                        currentMp -= n.Value();
                        break;
                    case BattleParam.Atk:
                        status.HP -= n.Value(); ;
                        break;
                    case BattleParam.Def:
                        status.Def -= n.Value();
                        break;
                    case BattleParam.Agl:
                        status.Agl -= n.Value();
                        break;
                }

                buffList.Remove(n);
            }
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

    public void InfluenceFeel(SingltonSkillManager.CollectionValue collection)
    {
        DownReFeelValue(ReFeel(collection.Key), - collection.Value);

        foreach(var feel in param.currentFeel.Keys) {
            if(feel == collection.Key) {
                int feelValue = collection.Value;

                if(param.RF == feel) {
                    feelValue = Mathf.FloorToInt((float) (feelValue * 1.2));
                }

                param.currentFeel[feel] += feelValue;

                param.currentFeel[feel] = Mathf.Min(param.currentFeel[feel], 25);

                param.currentFeel[feel] = Mathf.Max(param.currentFeel[feel], -25);

                return;
            }
        }
    }

    SingltonSkillManager.Feel ReFeel(SingltonSkillManager.Feel feel)
    {
        switch (feel) {
            case SingltonSkillManager.Feel.Ki:
                return SingltonSkillManager.Feel.Ai;

            case SingltonSkillManager.Feel.Do:
                return SingltonSkillManager.Feel.Raku;

            case SingltonSkillManager.Feel.Ai:
                return SingltonSkillManager.Feel.Ki;

            case SingltonSkillManager.Feel.Raku:
                return SingltonSkillManager.Feel.Do;

            case SingltonSkillManager.Feel.Love:
                return SingltonSkillManager.Feel.Zou;

            case SingltonSkillManager.Feel.Zou:
                return SingltonSkillManager.Feel.Love;

            default:
                return SingltonSkillManager.Feel.Ki;
        }


    }

    void DownReFeelValue(SingltonSkillManager.Feel feel,int value)
    {
        foreach (var f in param.currentFeel.Keys) {
            if (f == feel) {

                param.currentFeel[f] += value;

                param.currentFeel[f] = Mathf.Min(param.currentFeel[f], 25);

                param.currentFeel[f] = Mathf.Max(param.currentFeel[f], -25);

                return;
            }
        }
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

                    if(currentHp >= Hp) {
                        currentHp = Hp;
                    }
                    break;
                case BattleParam.MP:
                    currentMp += effects[targetParam];

                    if (currentMp >= Mp) {
                        currentMp = Mp;
                    }
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
