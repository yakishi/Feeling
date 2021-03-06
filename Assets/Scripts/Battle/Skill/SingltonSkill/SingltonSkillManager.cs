﻿using System;
using System.Collections.Generic;
using UnityEngine;

/*===============================================================*/
/// <summary>スレッドセーフ:SkillManager</summary>
/// <remarks> クラスが参照されたタイミングでインスタンスが生成されます</remarks>
public sealed class SingltonSkillManager
{

    private static SingltonSkillManager mInstance = new SingltonSkillManager();
    private SkillInfo[] SkillArray;
    private List<SkillParam> SkillSaveList;
    private List<SkillInfo> SkillCsvList;

    GV myGV;

    /// <summary>セーブデータから読み込まれたプレイヤースキルパラメーターを取得またはセット</summary>
    public List<SkillParam> SDSkill { get { return SkillSaveList; } set { SkillSaveList = value; } }
    /// <summary>CSVから読み込まれたプレイヤースキルパラメーターを取得します</summary>
    public List<SkillInfo> CDSkill { get { return SkillCsvList; } }
    /// <summary>Singlton</summary>
    public static SingltonSkillManager Instance { get { return mInstance; } }

    /// <summary>CSVのデータとキーを格納する構造体</summary>
    struct CSVDATA
    {
        public string[] key;
        public string[] data;

    }
    CSVDATA myCsvData;

    /*===============================================================*/
    /// <summary>コンストラクター</summary>
    private SingltonSkillManager()
    {
        Debug.Log("Create SingltonSkillManager Instance.");

        SkillSaveList = new List<SkillParam>();
        SkillCsvList = new List<SkillInfo>();

        SkillCreate(); // スキルパラメーターの作成を行います


    }
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>プレイヤーのスキルパラメーター情報をなどを作ります</summary>
    private void SkillCreate()
    {

        myGV = GV.Instance; // Save/Load クラスのインスタンスを取得

        // 人数分の player を生成する
        for (int i = 0; i < myGV.GData.Players.Count; i++) {
            SkillSaveList.Add(new SkillParam());
            SkillSaveList[i].Name = new List<string>();

        }

        LoadSkill(myGV.slot); // セーブデータの読込

        // 配列確保
        myCsvData = new CSVDATA();
        myCsvData.data = new string[2048];
        myCsvData.key = new string[2048];

        LoadCsvSkill(); // CSV からのデータ読込


    }
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>NewGame時のセーブのインスタンス生成処理</summary>
    /// <remarks>GV.cs:newGame( )から呼ばれる</remarks>
    public void NewGame()
    {
        // 人数分の player を生成する
        for (int i = 0; i < myGV.GData.Players.Count; i++) {
            SkillSaveList.Add(new SkillParam());
            SkillSaveList[i].Name = new List<string>();

        }


    }
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>GV.SkillParamからのデータ読込</summary>
    /// <param name="loadSlot">ロードするセーブデータスロットを指定します</param>
    public void LoadSkill(int loadSlot)
    {

        myGV.GameDataLoad(loadSlot); // セーブデータからの保存したデータを読み込みます

        int cnt = 0; // foreach カウント用変数

        if (myGV.GData != null) {
            // 読み込んだ値を入れていきます
            foreach (GV.SkillParam item in myGV.GData.PlayersSkills) {
                // GV.SkillParam に定義されているメンバ変数を SkillSaveList に入れていく
                SkillSaveList[cnt].Name = item.Name;
                cnt++;

            }

        }


    }
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>CSVからのデータ読込とListにセットする</summary>
    private void LoadCsvSkill()
    {
        // CSV ファイルからのキーとデータの読込
        CSVLoader myLoader = new CSVLoader();
        myCsvData.data = myLoader.GetCSV_Key_Record("CSV/CSV_Skill_IdOrder", myCsvData.key);

        int odd = 0, even = 1; // 奇数, 偶数 hirameki で使用します

        SkillArray = new SkillInfo[CSVLoader.csvId];

        // 配列にデータを格納していく
        for (int i = 0; i < SkillArray.Length; i++) {
            SkillArray[i] = new SkillInfo();
            SkillArray[i].ID = myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_ID");
            SkillArray[i].skill = myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_SKILL");
            SkillArray[i].MP = int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_MP"));
            SkillArray[i].ADV = float.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_ADV"));
            SkillArray[i].MDV = float.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_MDV"));
            SkillArray[i].PER = float.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_PER"));
            SkillArray[i].myCategory = (Category)Enum.ToObject(typeof(Category), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_CATEGORY")));
            SkillArray[i].myTarget = (Target)Enum.ToObject(typeof(Target), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_TARGET")));
            SkillArray[i].myScope = (Scope)Enum.ToObject(typeof(Scope), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_SCOPE")));
            SkillArray[i].influence = (BattleParam)Enum.ToObject(typeof(BattleParam), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_INFLUENCE")));
            SkillArray[i].DT = int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_DT"));

            SkillArray[i].flair = new List<Hirameki>();

            List<string> hiramekiAfterDivision = new List<string>(HiramekiDivision(myLoader, i, "_ID:確率/ID:確率・・・", myCsvData));
            // 閃き ( CSV ) ID:確率/ID:確率 分回し入れてく
            for (int hirameki = 0; hirameki < hiramekiAfterDivision.Count; hirameki++) {
                // 分解したものをコンストラクタで入れる
                SkillArray[i].flair.Add(new Hirameki(int.Parse(hiramekiAfterDivision[odd]), float.Parse(hiramekiAfterDivision[even])));
                if (even <= hiramekiAfterDivision.Count - 2) {
                    even += 2;
                    odd += 2;

                }
                else break;

            }
            even = 1;
            odd = 0;

            SkillArray[i].FVC = new CollectionValue();
            SkillArray[i].FVC.Key = (Feel)Enum.ToObject(typeof(Feel), int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_FC")));
            SkillArray[i].FVC.Value = int.Parse(myLoader.GetCSVData(myCsvData.key, myCsvData.data, i + "_FVC"));

            SkillCsvList.Add(SkillArray[i]); // 入れ込む

        }

        //Debug.Log( "CSV1 ファイル当りのデータ数 : " + CSVLoader.csvRecordAll );


    }
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>ID:確率/ID:確率フォーマット形式を分割します</summary>
    /// <param name="myLoader">CSVLoaderクラスのインスタンス</param>
    /// <param name="index">CSV行インデックス</param>
    /// <param name="str">_CSVに定義されたヘッダー名</param>
    /// <param name="csvData">CSVDATA構造体し格納します</param>
    /// <returns>:で分割されたデータが返ります</returns>
    private List<string> HiramekiDivision(CSVLoader myLoader, int index, string str, CSVDATA csvData)
    {
        List<string> hiramekiDivision1 = new List<string>(myLoader.GetCSVData(csvData.key, csvData.data, index + str).Split('/'));
        List<string> hiramekiDivision2 = new List<string>();

        foreach (string item1 in hiramekiDivision1) {
            var itemData = item1.Split(':');

            for (int i = 0; i < 2; i++) {
                hiramekiDivision2.Add(itemData[i]);
            }
        }
        return hiramekiDivision2;


    }
    /*===============================================================*/

    /*===============================================================*/
    // 列挙型の定義(SkillInfo)
    /// <summary>種類(バフまたはデバフなど・・・)</summary>
    public enum Category
    {
        /// <summary>バフ</summary>
        Buff,
        /// <summary>ダメージ</summary>
        Damage

    };
    /// <summary>ターゲット(スキル効果対象)</summary>
    public enum Target
    {
        /// <summary>敵</summary>
        Enemy,
        /// <summary>味方</summary>
        Supporter,
        /// <summary>自分</summary>
        MySelf

    };
    /// <summary>範囲</summary>
    public enum Scope
    {
        /// <summary>単体</summary>
        Simplex,
        /// <summary>全体</summary>
        OverAll

    };
    /// <summary>感情種類</summary>
    public enum Feel
    {
        /// <summary>喜</summary>
        Ki,
        /// <summary>怒</summary>
        Do,
        /// <summary>哀</summary>
        Ai,
        /// <summary>楽</summary>
        Raku,
        /// <summary>愛</summary>
        Love,
        /// <summary>憎</summary>
        Zou

    };
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>SkillInfo(CSV情報)</summary>
    /// <remarks>パラメーターは,暫定的です</remarks>
    public class SkillInfo
    {
        /// <summary>ID</summary>
        public string ID;
        /// <summary>スキル名</summary>
        public string skill;
        /// <summary>消費MP</summary>
        public int MP;
        /// <summary>攻撃依存値:AttackDependValue</summary>
        public float ADV;
        /// <summary>魔法依存値:MagicDependValue</summary>
        public float MDV;
        /// <summary> バフの倍率</summary>
        public float PER;
        /// <summary>種類(バフまたはデバフなど・・・)</summary>
        public Category myCategory;
        /// <summary>ターゲット(スキル効果対象)</summary>
        public Target myTarget;
        /// <summary>範囲</summary>
        public Scope myScope;
        /// <summary>何に影響を与えるか</summary>
        public BattleParam influence;
        /// <summary>持続ターン:DurationTurn{0:即時終了},{例...1:1ターン持続,2:2ターン持続・・・}</summary>
        public int DT;
        /// <summary>確率で覚える</summary>
        public List<Hirameki> flair;
        /// <summary>感情値補正:FeelingValueCorrect</summary>
        public CollectionValue FVC;

    }
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>閃き管理</summary>
    public class Hirameki
    {

        public Hirameki(int _skill_ID, float _probability)
        {
            this.skill_ID = _skill_ID;
            this.probability = _probability;


        }

        /// <summary>派生先スキルID</summary>
        public int skill_ID;
        /// <summary>派生先確率</summary>
        public float probability;


    }
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>感情値補正管理</summary>
    public class CollectionValue
    {
        /// <summary>感情種類</summary>
        public Feel Key;
        /// <summary>感情値補値</summary>
        public int Value;

        public CollectionValue()
        {

        }

        public CollectionValue(Feel feel,int value)
        {
            Key = feel;
            Value = value;
        }
    }
    /*===============================================================*/

    /*===============================================================*/
    /// <summary>PlayerSkillsParameters(Save情報)</summary>
    /// <remarks>パラメーターは,暫定的です</remarks>
    [System.Serializable]
    public class SkillParam
    {
        /// <summary>現在覚えているスキルの名前</summary>
        public List<string> Name;


    }
    /*===============================================================*/

    static public string FeelName(SingltonSkillManager.Feel feel)
    {
        switch (feel) {
            case SingltonSkillManager.Feel.Ki:
                return "喜";
            case SingltonSkillManager.Feel.Do:
                return "怒";
            case SingltonSkillManager.Feel.Ai:
                return "哀";
            case SingltonSkillManager.Feel.Raku:
                return "楽";
            case SingltonSkillManager.Feel.Love:
                return "愛";
            case SingltonSkillManager.Feel.Zou:
                return "憎";
            default:
                return "Missing";
        }
    }

}
/*===============================================================*/
