using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSegment
{
    //有一个全局配置文件，里面有所有segment的以下参数，dialogData就根据文件读
    public int id;
    public string bgFilename;
    public int bgmSoundId;
    public bool keepOpen;
    public bool isCanSkip;
    public List<DialogData> dialogsData = new List<DialogData>();
}
