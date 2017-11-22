using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EquipmentListAsset : ScriptableObject
{
    /// <summary>
    /// アセットを制作するパス
    /// </summary>
    public const string Path = "Assets/Resources/GameData/Equipments.asset";

    /// <summary>
    /// 装備データ
    /// </summary>
    public List<EquipmentData> equipments;

    /// <summary>
    /// 装備リストアセットを作成
    /// </summary>
    [MenuItem ("CreateGameData/EquipmentData")]
    public static void CreateEquipmentData()
    {

        var equipmentData = AssetDatabase.LoadAssetAtPath<EquipmentListAsset>(Path);
        if (equipmentData != null) {
            return;
        }
        equipmentData = CreateInstance<EquipmentListAsset>();

        AssetDatabase.CreateAsset(equipmentData, Path);
        AssetDatabase.Refresh();
    }
}
