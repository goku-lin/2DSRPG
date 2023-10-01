using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 世界的基础逻辑
/// </summary>
public class GameWorld : MonoBehaviour
{
    private GameObject characterUI;

    private void Start()
    {
        var d = GameObject.Find("LevelButton").GetComponent<Button>();
        d.onClick.AddListener(LevelButton);

        d = GameObject.Find("CharacterBtn").GetComponent<Button>();
        d.onClick.AddListener(CharacterBtn);

        characterUI = transform.Find("CharacterUI").gameObject;

        d = characterUI.transform.Find("Cancel").GetComponent<Button>();
        d.onClick.AddListener(() => { characterUI.SetActive(false); characterUI.GetComponent<CharacterUI>().CloseAll(); });

        d = GameObject.Find("SaveBtn").GetComponent<Button>();
        d.onClick.AddListener(() => { Save(); });

        d = GameObject.Find("ReturnBtn").GetComponent<Button>();
        d.onClick.AddListener(() => { ReturnButton(); });
    }

    private void CharacterBtn()
    {
        characterUI.SetActive(true);
    }

    private void LevelButton()
    {
        SceneManagerExt.instance.LoadSceneShowProgress(GameDefine.SceneType.GameLevel);
        GameManager.Instance.SetLevel("BG001");
    }

    private void ReturnButton()
    {
        SceneManagerExt.instance.LoadSceneShowProgress(GameDefine.SceneType.GameTilte);
    }

    private void Save()
    {
        int num = 0;//(int)param[0];
        RecordInfo recordInfo2 = new RecordInfo();
        recordInfo2.isLevelRecord = false;
        recordInfo2.recordTime = DateTime.Now;
        recordInfo2.gameTotalTime = PlayerData.TotalGameTime;
        recordInfo2.chapterId = PlayerData.CurChapter;
        recordInfo2.locId = PlayerData.CurLoc;
        //recordInfo2.buttonId = MenuHelper.GetButtonIDByLocID(recordInfo2.locId);
        recordInfo2.levelFilename = string.Empty;
        recordInfo2.battleRound = 0;
        recordInfo2.difficulty = PlayerData.CurDifficulty;
        //recordInfo2.thumb = PlayerSetting.DefaultThumb;
        //recordInfo2.isCleanup = PlayerData.IsCleanup;
        //recordInfo2.newgamePlus = PlayerData.NewGamePlus;
        //PlayerSetting.DefaultThumb = null;
        //if (num < 0)
        //{
        //    num = PlayerSetting.RecordsCount();
        //    if (num >= 12)
        //    {
        //        return;
        //    }
        //    recordInfo2.id = PlayerSetting.GenaralId();
        //    PlayerSetting.Records.Add(recordInfo2);
        //}
        //else
        //{
        //    if (PlayerSetting.Records.Count > num && PlayerSetting.Records[num] != null)
        //    {
        //        recordInfo2.id = PlayerSetting.Records[num].id;
        //    }
        //    else
        //    {
        //        recordInfo2.id = PlayerSetting.GenaralId();
        //    }
        //    PlayerSetting.Records[num] = recordInfo2;
        //}
        PlayerData.SaveRecord(num, recordInfo2, false, 0);
        //uisaveAnimationManager.Close();
        //Notifier.Notify(10183, new object[0]);
    }
}
