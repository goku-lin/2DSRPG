using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelEditor))]
public class MapEditor : Editor
{
    LevelEditor levelEditor;
    private bool isUnit = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            levelEditor = target as LevelEditor;

            DrawMapFrameworkButton();
            DrawToggleConfigurationButton();
            DrawSaveLoadButtons();
        }
    }

    private void DrawMapFrameworkButton()
    {
        if (GUILayout.Button("生成地图框架", GUILayout.Height(40)))
        {
            levelEditor.Init();
        }
    }

    private void DrawToggleConfigurationButton()
    {
        string buttonText = isUnit ? "切换地图配置" : "切换人物配置";
        if (GUILayout.Button(buttonText, GUILayout.Height(40)))
        {
            isUnit = !isUnit;
            levelEditor.isUnit = isUnit;
        }
    }

    private void DrawSaveLoadButtons()
    {
        if (GUILayout.Button("保存数据"))
        {
            SaveLevel();
        }

        if (GUILayout.Button("读取数据"))
        {
            levelEditor.Load();
        }

        if (GUILayout.Button("保存角色配置"))
        {
            SaveUnit();
        }

        if (GUILayout.Button("读取角色配置"))
        {
            levelEditor.LoadUnit();
        }
    }

    void SaveLevel()
    {
        SaveData("/Level/", "保存关卡数据", false);
    }

    void SaveUnit()
    {
        SaveData("/Character/", "保存角色数据", true);
    }

    void SaveData(string directory, string dialogTitle, bool isUnit)
    {
        GLevel level = levelEditor.Level;
        string fileName = Application.streamingAssetsPath + directory + level.levelName + ".xml";
        Debug.Log(fileName);
        if (isUnit)
            Utilitys.SaveUnit(fileName, level);
        else
            Utilitys.SaveLevel(fileName, level);
        EditorUtility.DisplayDialog(dialogTitle, "保存成功", "确定");
    }
}
