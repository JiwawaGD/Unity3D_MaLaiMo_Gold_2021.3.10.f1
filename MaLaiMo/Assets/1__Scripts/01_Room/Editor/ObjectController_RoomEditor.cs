using UnityEditor;
using UnityEngine;

//請幫我把 ObjectController_Room.cs 這個檔案中所有的 ItemController 和 GameObject 欄位
//都加到 ObjectController_RoomEditor.cs 的自訂 Inspector 介面裡
//並且用可收合的群組方式顯示，像你之前幫我做的那樣。

[CustomEditor(typeof(ObjectController_Room))]
[CanEditMultipleObjects]
public class ObjectController_RoomEditor : Editor
{
    private bool showGameObjects = true;
    private bool showItems = true;

    // GameObject 欄位
    SerializedProperty player;
    SerializedProperty wallClock;

    // ItemController 欄位
    SerializedProperty grandmaRoomDoor;
    SerializedProperty lotusPaper;
    SerializedProperty foldedLotusPaper;
    SerializedProperty offeringPlace;
    SerializedProperty grandmaRoomCloset;
    SerializedProperty clothesInCloset;
    SerializedProperty graffitiInCloset;

    private void OnEnable()
    {
        // GameObject 欄位
        player = serializedObject.FindProperty("_player");
        wallClock = serializedObject.FindProperty("_wallClock");

        // ItemController 欄位
        grandmaRoomDoor = serializedObject.FindProperty("_grandmaRoomDoor");
        lotusPaper = serializedObject.FindProperty("_lotusPaper");
        foldedLotusPaper = serializedObject.FindProperty("_foldedLotusPaper");
        offeringPlace = serializedObject.FindProperty("_offeringPlace");
        grandmaRoomCloset = serializedObject.FindProperty("_grandmaRoomCloset");
        clothesInCloset = serializedObject.FindProperty("_clothesInCloset");
        graffitiInCloset = serializedObject.FindProperty("_graffitiInCloset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 場景物件分組 ====", EditorStyles.boldLabel);

        showGameObjects = EditorGUILayout.Foldout(showGameObjects, "Game Object 欄位");
        if (showGameObjects)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(player, new GUIContent("玩家"));
            EditorGUILayout.PropertyField(wallClock, new GUIContent("壁鐘"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 互動物件分組 ====", EditorStyles.boldLabel);

        showItems = EditorGUILayout.Foldout(showItems, "Item Controller 欄位");
        if (showItems)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(grandmaRoomDoor, new GUIContent("奶奶房間門"));
            EditorGUILayout.PropertyField(lotusPaper, new GUIContent("蓮花紙張"));
            EditorGUILayout.PropertyField(foldedLotusPaper, new GUIContent("摺完的蓮花"));
            EditorGUILayout.PropertyField(offeringPlace, new GUIContent("放拜飯的地方"));
            EditorGUILayout.PropertyField(grandmaRoomCloset, new GUIContent("奶奶房間衣櫃"));
            EditorGUILayout.PropertyField(clothesInCloset, new GUIContent("衣櫃裡的衣服"));
            EditorGUILayout.PropertyField(graffitiInCloset, new GUIContent("衣櫃裡的塗鴉畫"));
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}