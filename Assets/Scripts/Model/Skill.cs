using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Skill
{
    public SkillInfo Info;
    public ActiveSkillAction activeSkillAction;

    public void saveRecord(BinaryWriter writer)
    {
        writer.Write(this.Info.Sid);
    }

    public static Skill readRecord(BinaryReader reader)
    {
        return new Skill
        {
            Info = getSkillInfo(reader.ReadString()),
        };
    }

    public static SkillInfo getSkillInfo(string sid)
    {
        if (!DataManager.GetInstance().skillData.ContainsKey(sid))
        {
            Debug.Log("sid " + sid + " not exist!");
        }
        return DataManager.GetInstance().skillData[sid];
    }
}
