using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ProtoContract]
public class DialogOptionData
{
    //[ProtoMember(1)]
    public int id;

    //[ProtoMember(2)]
    public MultiLanguage text;

    //[ProtoMember(3)]
    public string icon;
}