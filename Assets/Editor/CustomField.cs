using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UniRx;

public class EquipmentDetailEditor : EditorWindow
{
    static EquipmentDetailEditor editor;

    static bool statusFoldout = true;
    static bool shopFlodout = true;
    static EquipmentListAsset equipmentList;
    static int currentSelect = -1;

    static public void openEquipmentEditor(EquipmentListAsset inEquipmentList, int index)
    {
        equipmentList = inEquipmentList;
        if (editor == null) {
            editor = CreateInstance<EquipmentDetailEditor>();
            editor.ShowPopup();
        }
        if (currentSelect != index) {
            editor.Close();
            editor = CreateInstance<EquipmentDetailEditor>();
            editor.ShowPopup();
        }
        currentSelect = index;
    }

    static public void SetRect(Rect newRect)
    {
        editor.position = newRect;
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Number", "NAME");

        EquipmentData temp = equipmentList.equipments[currentSelect];
        temp.name = EditorGUILayout.DelayedTextField(currentSelect.ToString(), equipmentList.equipments[currentSelect].name);

        EditorGUILayout.Space();
        Status newStatus = equipmentList.equipments[currentSelect].status;


        statusFoldout = EditorGUILayout.Foldout(statusFoldout, "Status");
        EditorGUI.indentLevel++;

        if (statusFoldout) {
            for (int i = 0; i < Status.ParamCount; ++i) {
                if (i % 2 == 0) {
                    EditorGUILayout.BeginHorizontal();
                }
                newStatus[i] = EditorGUILayout.IntField(Status.ParamStrs[i], newStatus[i]);
                if (i % 2 == 1) {
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        temp.status = newStatus;

        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        shopFlodout = EditorGUILayout.Foldout(shopFlodout, "Shop");

        EditorGUI.indentLevel++;
        if (shopFlodout) {

            EditorGUILayout.BeginHorizontal();

            temp.buy = EditorGUILayout.IntField("Buy", temp.buy);
            temp.sell = EditorGUILayout.IntField("Buy", temp.sell);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.indentLevel--;

        equipmentList.equipments[currentSelect] = temp;
    }
}

public class MultipleSelectionContent
{
    /// <summary>
    /// 表示する文字列の配列
    /// </summary>
    string[] captions;

    /// <summary>
    /// 要素が選択されているか
    /// </summary>
    ReactiveCollection<bool> IsSelectedNums;

    /// <summary>
    /// 現在選択されているIndex
    /// </summary>
    int lastSelected = 0;

    /// <summary>
    /// 一個前に選択されてたIndex
    /// </summary>
    int prevSelected = 0;

    /// <summary>
    /// 何個選択されているか
    /// </summary>
    int selectedCount;

    /// <summary>
    /// クリックされたか
    /// </summary>
    BoolReactiveProperty isClicked = new BoolReactiveProperty(false);

    /// <summary>
    /// 要素が選択されたときに流れるストリーム
    /// </summary>
    /// <returns></returns>
    public IObservable<int> OnClickAsObservable()
    {
        return isClicked
            .Where(value => value)
            .Select(_ => lastSelected);
    }

    /// <summary>
    /// 選択されているIndexを取得
    /// </summary>
    public int[] SelectedIndices {
        get
        {
            return IsSelectedNums
                .Select((v, i) => new { value = v, index = i })
                .Where(value => value.value)
                .Select(value => value.index)
                .OrderByDescending(index => index)
                .ToArray();
        }
    }

    /// <summary>
    /// コンストラクター
    /// </summary>
    /// <param name="newCaptions">表示する文字列</param>
    public MultipleSelectionContent(string[] newCaptions)
    {
        selectedCount = 1;
        setCaptions(newCaptions);
    }

    /// <summary>
    /// 表示する文字列を設定
    /// </summary>
    /// <param name="newCaptions">設定する文字列</param>
    public void setCaptions(string[] newCaptions)
    {
        captions = newCaptions;
        if (IsSelectedNums == null || IsSelectedNums.Count != captions.Length) {
            lastSelected = captions.Length - 1;

            IsSelectedNums = new ReactiveCollection<bool>();
            for (int i = 0; i < captions.Length; ++i) {
                IsSelectedNums.Add(false);
            }
            IsSelectedNums[0] = true;
            IsSelectedNums
                .ObserveReplace()
                .Where(replaceEvent => replaceEvent.NewValue != replaceEvent.OldValue && !isClicked.Value)
                .Subscribe(replaceEvent => {
                    prevSelected = lastSelected;
                    lastSelected = replaceEvent.Index;
                    isClicked.Value = true;
                    Observable.Timer(System.TimeSpan.FromMilliseconds(10)).Subscribe(_ => isClicked.Value = false);
                    if (Event.current.control) {
                        if (selectedCount == 1 && !IsSelectedNums[lastSelected]) {
                            IsSelectedNums[lastSelected] = true;
                            return;
                        }
                        if (IsSelectedNums[lastSelected]) {
                            selectedCount++;
                        }
                        else {
                            selectedCount--;
                        }

                        if (selectedCount == 1) {
                            lastSelected = IsSelectedNums.ToList().FindIndex(value => value);
                        }
                    }
                    else if (Event.current.shift) {
                        int startIndex = prevSelected;
                        int endIndex = prevSelected;

                        if (prevSelected < lastSelected) {
                            endIndex = lastSelected;
                        }
                        else {
                            startIndex = lastSelected;
                        }

                        for (int i = startIndex; i <= endIndex; ++i) {
                            if (!IsSelectedNums[i]) {
                                IsSelectedNums[i] = true;
                                selectedCount++;
                            }
                        }
                    }
                    else {
                        selectedCount = 1;
                    }
                });
        }
    }

    /// <summary>
    /// 複数選択可能なリストを表示
    /// </summary>
    /// <param name="content">表示するコンテンツ</param>
    /// <param name="selected">現在選択されているIndex</param>
    /// <returns></returns>
    public static int MultipleSelectionField(MultipleSelectionContent content, int selected)
    {
        if (selected == -1) {
            content.lastSelected = 0;
        }
        if (Event.current.shift || Event.current.control) {
            MultipleSelect(content);
        }
        else if (content.selectedCount == 1) {
            SingleSelect(content);
        }
        else {
            MultipleSelect(content);
        }

        return content.lastSelected;
    }

    /// <summary>
    /// シングル選択時に呼ばれる関数
    /// </summary>
    /// <param name="content">表示するコンテンツ</param>
    public static void SingleSelect(MultipleSelectionContent content)
    {
        for (int i = 0; i < content.captions.Length; ++i) {
            content.IsSelectedNums[i] = GUILayout.Toggle(i == content.lastSelected, content.captions[i], "PreferencesKeysElement");
        }
    }
    /// <summary>
    /// 複数選択時に呼ばれる関数
    /// </summary>
    /// <param name="content">表示するコンテンツ</param>
    public static void MultipleSelect(MultipleSelectionContent content)
    {
        for (int i = 0; i < content.captions.Length; ++i) {
            content.IsSelectedNums[i] = GUILayout.Toggle(content.IsSelectedNums[i], content.captions[i], "PreferencesKeysElement");
        }
    }
}
