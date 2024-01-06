using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : SingletonMono<StoryManager>
{
    public int did = 0;
    public DialogUI dialogUI;
    public bool canTalk = false;


    protected override void Awake()
    {
        base.Awake();
        dialogUI = transform.Find("DialogUI").GetComponent<DialogUI>();
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 初始化剧情数据
    /// </summary>
    /// <param name="storyName"></param>
    public void InitStory(string storyName)
    {
        canTalk = true;
        dialogUI.canTalk = true;
        dialogUI.gameObject.SetActive(true);
        DataManager.GetInstance().GetStoryData(storyName);
        did = 0;
    }

    /// <summary>
    /// 更新对话
    /// </summary>
    public void UpdateDialog()
    {
        if (did == -1)
        {
            dialogUI.gameObject.SetActive(false);
            canTalk = false;
            dialogUI.canTalk = false;
            if (BattleUIManager.Instance != null)
                BattleUIManager.Instance.gameObject.SetActive(true);
            return;
        }
        Story story = DataManager.GetInstance().storyData[did];
        dialogUI.UpdateDialog(story);
        Debug.Log(story.content);
        did = story.jump;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && canTalk)
        {
            UpdateDialog();
        }
    }
}
