using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ゲーム内で使用するステータス構造体
/// </summary>
/// <summary>
[System.Serializable]
public struct Status
{
    public const int ParamCount = 8;

    public int HP;
    public int MP;
    public int Atk;
    public int Def;
    public int Int;
    public int Mgr;
    public int Agl;
    public int Luc;

    public override string ToString()
    {
        string ret = "";
        var strs = ParamStrs;

        for (int i = 0; i < strs.Length - 1; ++i) {
            ret += ParamStrs[i] + " : " + this[i] + ", ";
        }

        ret += ParamStrs[ParamCount - 1] + " : " + this[ParamCount - 1];
        return ret;
    }

    /// <summary>
    /// ステータスをint配列にして取得
    /// </summary>
    /// <returns>int配列</returns>
    public int[] ToArray()
    {
        int[] ret = new int[ParamCount];
        for (int i = 0; i < ParamCount; ++i) {
            ret[i] = this[i];
        }

        return ret;
    }

    /// <summary>
    /// int 配列からステータスに変更
    /// </summary>
    /// <param name="array">int配列</param>
    public void setFromArray(int [] array)
    {
        for (int i = 0; i < ParamCount; ++i) {
            this[i] = array[i];
        }
    }

    public static string[] ParamStrs {
        get
        {
            return new string[]
            { "HP"
            , "MP"
            , "Atk"
            , "Def"
            , "Int"
            , "Mgr"
            , "Agl"
            , "Luc"
            };
        }
    }
    // ステータスの要素にアクセス
    public int this[int index] {
        set
        {
            switch (index) {
                case 0:
                    HP = value;
                    break;
                case 1:
                    MP = value;
                    break;
                case 2:
                    Atk = value;
                    break;
                case 3:
                    Def = value;
                    break;
                case 4:
                    Int = value;
                    break;
                case 5:
                    Mgr = value;
                    break;
                case 6:
                    Agl = value;
                    break;
                case 7:
                    Luc = value;
                    break;
            }
        }
        get
        {
            switch (index) {
                case 0:
                    return HP;
                case 1:
                    return MP;
                case 2:
                    return Atk;
                case 3:
                    return Def;
                case 4:
                    return Int;
                case 5:
                    return Mgr;
                case 6:
                    return Agl;
                case 7:
                    return Luc;
            }
            return 0;
        }
    }
}
