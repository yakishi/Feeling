using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UniRx;


public class EquipmentsEditor : EditorWindow
{
    /// <summary>
    /// 装備リストのアセット
    /// </summary>
    EquipmentListAsset equipmentListAsset;
    /// <summary>
    /// 装備リスト
    /// </summary>
    List<EquipmentData> equipments;
    /// <summary>
    /// リスト表示用のスクロール座標
    /// </summary>
    Vector2 listScrollPosition;

    int detailWidth = 500;

    MultipleSelectionContent equipmentsContent;

    /// <summary>
    /// 装備リストで現在選択されているindex
    /// </summary>
    static IntReactiveProperty selected = new IntReactiveProperty(0);
    public static int Selected {
        private set
        {
            selected.Value = value;
        }

        get
        {
            return selected.Value;
        }
    }

    void OnDestroy()
    {
        GetWindow<EquipmentDetailEditor>().Close();
    }

    /// <summary>
    /// 装備設定画面を開く
    /// </summary>
    [MenuItem("RPG_Tools/EquipmentEditor")]
    static void openEquipmentEditor()
    {
        GetWindow<EquipmentsEditor>();
    }


    void OnEnable()
    {
        equipmentListAsset = AssetDatabase.LoadAssetAtPath<EquipmentListAsset>(EquipmentListAsset.Path);
        if (equipmentListAsset == null) { EquipmentListAsset.CreateEquipmentData(); }
        if (equipments == null) { equipments = equipmentListAsset.equipments; }

        var equipmentsIdName = equipments
            .Select((equipment, id) => id + ":" + equipment.name)
            .ToArray();
        equipmentsContent = new MultipleSelectionContent(equipmentsIdName);

        selected
            .Subscribe(index => {
                EquipmentDetailEditor.openEquipmentEditor(equipmentListAsset, index);
            });
    }
    void OnGUI()
    {
        var detailRect = position;
        detailRect.position = new Vector2(detailRect.xMax, detailRect.yMin);
        detailRect.size = new Vector2(detailWidth, detailRect.height);

        EquipmentDetailEditor.SetRect(detailRect);
        GUILayout.BeginHorizontal();
        updateLeftPanel();
        GUILayout.EndHorizontal();

    }

    /// <summary>
    /// 左側のリスト更新用
    /// </summary>
    void updateLeftPanel()
    {
        var equipmentsIdName = equipments
            .Select((equipment, id) => id + ":" + equipment.name)
            .ToArray();
        equipmentsContent.setCaptions(equipmentsIdName);

        GUILayout.BeginVertical(/*GUILayout.Width(borderlinePosX)*/);
        listScrollPosition = GUILayout.BeginScrollView(listScrollPosition);
        Selected = MultipleSelectionContent.MultipleSelectionField(equipmentsContent, Selected);
        EditorGUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add")) {
            addEquipmentData();
        }
        if (GUILayout.Button("Delete")) {
            deleteEquipmentData();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    /// <summary>
    /// 最後尾に空の装備データを追加
    /// </summary>
    void addEquipmentData()
    {
        var newEquipment = new EquipmentData();
        equipments.Add(newEquipment);
    }

    void deleteEquipmentData()
    {
        var indices = equipmentsContent.SelectedIndices;

        foreach (var index in indices) {
            equipments.RemoveAt(index);
        }

        Selected = -1;
    }
}
