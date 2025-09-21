using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectController_Room))]
[CanEditMultipleObjects]
public class ObjectController_RoomEditor : Editor
{
    private bool showGameObjects = true;
    private bool showItems = true;

    // Transform 欄位
    SerializedProperty _playerWakeUpPos;

    // GameObject 欄位
    SerializedProperty player;
    SerializedProperty _tv;
    SerializedProperty wallClock;
    SerializedProperty _filialPietyCurtain;
    SerializedProperty _riceAndSoup;
    SerializedProperty _rosewoodChair;

    // ItemController 欄位
    SerializedProperty grandmaRoomDoor;
    SerializedProperty lotusPaper;
    SerializedProperty foldedLotusPaper;
    SerializedProperty offeringPlace;
    SerializedProperty grandmaRoomCloset;
    SerializedProperty clothesInCloset;
    SerializedProperty graffitiInCloset;
    SerializedProperty mom;
    SerializedProperty frontDoor;
    SerializedProperty piano;
    SerializedProperty grandmaDeadBody;

    private void OnEnable()
    {
        // Transform 欄位
        _playerWakeUpPos = serializedObject.FindProperty("_playerWakeUpPos");

        // GameObject 欄位
        player = serializedObject.FindProperty("_player");
        wallClock = serializedObject.FindProperty("_wallClock");
        _tv = serializedObject.FindProperty("_tv");
        _filialPietyCurtain = serializedObject.FindProperty("_filialPietyCurtain");
        _riceAndSoup = serializedObject.FindProperty("_riceAndSoup");
        _rosewoodChair = serializedObject.FindProperty("_rosewoodChair");

        // ItemController 欄位
        grandmaRoomDoor = serializedObject.FindProperty("_grandmaRoomDoor");
        lotusPaper = serializedObject.FindProperty("_lotusPaper");
        foldedLotusPaper = serializedObject.FindProperty("_foldedLotusPaper");
        offeringPlace = serializedObject.FindProperty("_offeringPlace");
        grandmaRoomCloset = serializedObject.FindProperty("_grandmaRoomCloset");
        clothesInCloset = serializedObject.FindProperty("_clothesInCloset");
        graffitiInCloset = serializedObject.FindProperty("_graffitiInCloset");
        mom = serializedObject.FindProperty("_mom");
        frontDoor = serializedObject.FindProperty("_frontDoor");
        piano = serializedObject.FindProperty("_piano");
        grandmaDeadBody = serializedObject.FindProperty("_grandmaDeadBody");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 場景座標分組 ====", EditorStyles.boldLabel);
        showGameObjects = EditorGUILayout.Foldout(showGameObjects, "Transform 欄位");
        if (showGameObjects)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_playerWakeUpPos, new GUIContent("玩家面對床起床位置"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 場景物件分組 ====", EditorStyles.boldLabel);

        showGameObjects = EditorGUILayout.Foldout(showGameObjects, "GameObject 欄位");
        if (showGameObjects)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(player, new GUIContent("玩家"));
            EditorGUILayout.PropertyField(_tv, new GUIContent("電視"));
            EditorGUILayout.PropertyField(wallClock, new GUIContent("壁鐘"));
            EditorGUILayout.PropertyField(_filialPietyCurtain, new GUIContent("孝濂"));
            EditorGUILayout.PropertyField(_riceAndSoup, new GUIContent("三菜一湯"));
            EditorGUILayout.PropertyField(_rosewoodChair, new GUIContent("玫瑰花木椅"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 互動物件分組 ====", EditorStyles.boldLabel);

        showItems = EditorGUILayout.Foldout(showItems, "ItemController 欄位");
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
            EditorGUILayout.PropertyField(mom, new GUIContent("媽媽"));
            EditorGUILayout.PropertyField(frontDoor, new GUIContent("前門"));
            EditorGUILayout.PropertyField(piano, new GUIContent("鋼琴"));
            EditorGUILayout.PropertyField(grandmaDeadBody, new GUIContent("奶奶遺體"));
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
