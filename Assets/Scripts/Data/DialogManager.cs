using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogManager
{
    private static DialogManager _instance;

    public static LANGUAGE Current;

    public Dictionary<int, DialogSegment> segmentsData;
    public Dictionary<string, DialogHeadData> headsData;
    public Dictionary<int, DialogOptionData> optionTextData;
    
    public DialogSegment curSegment;
    public int curDialogIndex;

    public static DialogManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DialogManager();
        }
        return _instance;
    }

    //public static byte[] ToByteArray()
    //{
    //    return SerializeUtil.Serialize<DialogManager>(DialogManager.GetInstance());
    //}

    //public static void FromByteArray(byte[] array)
    //{
    //    DialogManager._instance = array.Deserialize<DialogManager>();
    //}

    public void InitDialogData()
    {
        if (segmentsData == null)
        {
            var jsonData = ResourcesExt.Load<TextAsset>("GameData/temp").text;
            DialogSegment[] tempData = JsonConvert.DeserializeObject<DialogSegment[]>(jsonData);
            segmentsData = new Dictionary<int, DialogSegment>();
            foreach (DialogSegment data in tempData)
            {
                segmentsData.Add(data.id, data);
            }
        }
        if (headsData == null)
        {
            var jsonData = ResourcesExt.Load<TextAsset>("GameData/dialogHeadData").text;
            DialogHeadData[] tempData = JsonConvert.DeserializeObject<DialogHeadData[]>(jsonData);
            headsData = new Dictionary<string, DialogHeadData>();
            foreach (DialogHeadData data in tempData)
            {
                headsData.Add(data.name, data);
            }
        }
    }
}
