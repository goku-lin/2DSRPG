using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager
{
    private static DataManager _instance;
    public Dictionary<int, Person> personData;
    public Dictionary<string, Job> jobData;
    public Dictionary<string, Skill> skillData;
    public Dictionary<string, ItemInfo> itemData;
    public Dictionary<string, Dialog> dialogData;
    public Dictionary<int, Story> storyData;

    public static DataManager GetInstance()
    {
        if (DataManager._instance == null)
        {
            DataManager._instance = new DataManager();
        }
        return DataManager._instance;
    }

    public void InitPersonData()
    {
        if (personData == null)
        {
            var jsonData = ResourcesExt.Load<TextAsset>("GameData/person").text;

            Person[] tempData = JsonConvert.DeserializeObject<Person[]>(jsonData);

            personData = new Dictionary<int, Person>();
            foreach (Person data in tempData)
            {
                personData.Add(data.Pid, data);
            }
        }
    }

    public void InitJobData()
    {
        if (jobData == null)
        {
            var jsonData = ResourcesExt.Load<TextAsset>("GameData/job").text;

            Job[] tempData = JsonConvert.DeserializeObject<Job[]>(jsonData);

            jobData = new Dictionary<string, Job>();
            foreach (Job data in tempData)
            {
                jobData.Add(data.Jid, data);
            }
        }
    }

    public void InitSkillData()
    {
        if (skillData == null)
        {
            var jsonData = ResourcesExt.Load<TextAsset>("GameData/skill").text;

            Skill[] tempData = JsonConvert.DeserializeObject<Skill[]>(jsonData);

            skillData = new Dictionary<string, Skill>();
            foreach (Skill data in tempData)
            {
                skillData.Add(data.Sid, data);
            }
        }
    }

    public void InitItemData()
    {
        if (itemData == null)
        {
            var jsonData = ResourcesExt.Load<TextAsset>("GameData/item").text;

            ItemInfo[] tempData = JsonConvert.DeserializeObject<ItemInfo[]>(jsonData);

            itemData = new Dictionary<string, ItemInfo>();
            foreach (ItemInfo data in tempData)
            {
                itemData.Add(data.Iid, data);
            }
        }
    }

    public void InitDialogData()
    {
        if (dialogData == null)
        {
            var jsonData = ResourcesExt.Load<TextAsset>("GameData/dialog").text;

            Dialog[] tempData = JsonConvert.DeserializeObject<Dialog[]>(jsonData);

            dialogData = new Dictionary<string, Dialog>();
            foreach (Dialog data in tempData)
            {
                dialogData.Add(data.did, data);
            }
        }
    }

    public void GetStoryData(string storyName)
    {
        var jsonData = ResourcesExt.Load<TextAsset>("GameData/" + storyName).text;

        Story[] tempData = JsonConvert.DeserializeObject<Story[]>(jsonData);

        storyData = new Dictionary<int, Story>();
        foreach (Story data in tempData)
        {
            storyData.Add(data.did, data);
        }
    }

}
