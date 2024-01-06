using Game.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuHelper
{
    public static PopManager PopItemInfo(Vector3 worldPos, Item item, Role role, long num, int equipID, Transform bag)
    {
        List<object> list = new List<object>();
        list.Add(worldPos);
        list.Add(item);
        list.Add(role);
        list.Add(num);
        list.Add(equipID);
        list.Add(bag);
        return Singleton<MainUIManager>.Instance.CreatePopUpWindowsByClassName("Game.Client.PopItemInfoManager", list, false, false, false, false);
    }

    public static PopManager PopSkillInfo(Vector3 worldPos, Skill skill, Role role, Dictionary<string, GameObject> learnSkillDic)
    {
        List<object> list = new List<object>();
        list.Add(worldPos);
        list.Add(skill);
        list.Add(role);
        list.Add(learnSkillDic);
        return Singleton<MainUIManager>.Instance.CreatePopUpWindowsByClassName("Game.Client.PopSkillInfoManager", list, false, false, false, false);
    }

    public static PopManager PopDialog(int dialogGroupId)
    {
        List<object> list = new List<object>();
        list.Add(dialogGroupId);
        //Debug.Log("Game.Client.PopDialogManager" == typeof(PopDialogManager).FullName);
        //Debug.Log(typeof(PopDialogManager).FullName);
        return Singleton<MainUIManager>.Instance.CreatePopUpWindowsByClassName("Game.Client.PopDialogManager", list, true, true, false, false);
    }

    public static void CloseDialog()
    {
        Singleton<MainUIManager>.Instance.ClosePopUpWindowsByName("Game.Client.PopDialogManager", null, true);
    }

    public static void CloseMessageMenu()
    {
        Singleton<MainUIManager>.Instance.ClosePopUpWindowsByName("Game.Client.PopMessageManager", null, true);
    }
}
