using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(LevelEditor))]
public class MapEditor : Editor
{
    LevelEditor levelEditor;
    private bool isUnit = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //GUILayout.Label("设置配置文件的生成路径");
        //this.mapPath = GUILayout.TextField(mapPath);
        if (Application.isPlaying)
        {
            levelEditor = target as LevelEditor;
            if (GUILayout.Button("生成地图框架", GUILayout.Height(40)))
            {
                levelEditor.Init();
            }
            if (!this.isUnit && GUILayout.Button("切换人物配置", GUILayout.Height(40)))
            {
                isUnit = true;
                levelEditor.isUnit = true;
            }
            if (this.isUnit && GUILayout.Button("切换地图配置", GUILayout.Height(40)))
            {
                isUnit = false;
                levelEditor.isUnit = false;
            }
            if (GUILayout.Button("保存数据"))
            {
                //保存关卡
                SaveLevel();
            }
            if (GUILayout.Button("读取数据"))
            {
                //读取关卡列表
                levelEditor.Load();
            }
            if (GUILayout.Button("保存角色配置"))
            {
                //保存关卡
                SaveUnit();
            }
            if (GUILayout.Button("读取角色配置"))
            {
                //读取关卡列表
                levelEditor.LoadUnit();
            }
        }
    }

    void SaveLevel()
    {
        GLevel level = levelEditor.Level;
        //获取当前加载的关卡

        //路径
        //string fileName = m_files[m_selectIndex].FullName;
        string fileName = Application.streamingAssetsPath + "/Level/" + level.levelName + ".xml";
        Debug.Log(fileName);
        //保存关卡
        Utilitys.SaveLevel(fileName, level);

        //弹框提示
        EditorUtility.DisplayDialog("保存关卡数据", "保存成功", "确定");
    }

    void SaveUnit()
    {
        GLevel level = levelEditor.Level;
        //获取当前加载的关卡

        //路径
        //string fileName = m_files[m_selectIndex].FullName;
        string fileName = Application.streamingAssetsPath + "/Character/" + level.levelName + ".xml";
        Debug.Log(fileName);
        //保存关卡
        Utilitys.SaveUnit(fileName, level);

        //弹框提示
        EditorUtility.DisplayDialog("保存角色数据", "保存成功", "确定");
    }

}
