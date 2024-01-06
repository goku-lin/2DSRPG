using Game.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTitle : MonoBehaviour
{
    private void Start()
    {
        DataManager.GetInstance().InitPersonData();
        DataManager.GetInstance().InitJobData();
        DataManager.GetInstance().InitSkillData();
        DataManager.GetInstance().InitItemData();
        DataManager.GetInstance().InitDialogData();

        DialogManager.GetInstance().InitDialogData();

        UIManager.GetInstance().Initialize();

        if (AudioCtrl.instance == null)
            AudioCtrl.Init();

        var d = GameObject.Find("NewGame").GetComponent<Button>();

        d.onClick.AddListener(OnNewGameButtonClick);

        d = GameObject.Find("ContinueGame").GetComponent<Button>();

        d.onClick.AddListener(OnContinueGameButtonClick);
    }

    void OnNewGameButtonClick()
    {
        Destroy(this.gameObject);
        PlayerData.Init();
        Item nowItem = PlayerData.CreateItem("IID_经验药");
        PlayerData.Warehouse.Add(nowItem.uid, nowItem);
        SceneManagerExt.instance.LoadSceneShowProgress(GameDefine.SceneType.GameWorld);
    }

    void OnContinueGameButtonClick()
    {
        Destroy(this.gameObject);
        PlayerData.LoadRecord(0, "R0");
        SceneManagerExt.instance.LoadSceneShowProgress(GameDefine.SceneType.GameWorld);
    }

    [ContextMenu("TestDispatchEvent")]
    void TestDispatchEvent()
    {
        EventDispatcher.instance.DispatchEvent<Vector3>(GameEventType.showHitEffect, Vector3.zero);
    }
}
