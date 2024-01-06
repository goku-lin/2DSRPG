using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLanguage
{
    public MultiLanguage()
    {
    }

    public MultiLanguage(string defaultValue)
    {
        this.defaultValue = defaultValue;
        this.multiValues[LANGUAGE.ZHCN] = defaultValue;
    }

    public void setValue(LANGUAGE language, string value)
    {
        this.multiValues[language] = value;
    }

    public override string ToString()
    {
        string text = this.defaultValue;
        try
        {
            text = this.multiValues[MultiLanguage.Current];
        }
        catch
        {
        }
        if (MultiLanguage.WordsMapping != null && MultiLanguage.WordsMapping.ContainsKey(text))
        {
            return MultiLanguage.WordsMapping[text];
        }
        return text;
    }
    public const LANGUAGE DEFAULT_LANGUAGE = LANGUAGE.ZHCN;

    public static LANGUAGE Current;

    //[ProtoMember(1)]
    public string defaultValue;
    //[ProtoMember(2)]
    private Dictionary<LANGUAGE, string> multiValues = new Dictionary<LANGUAGE, string>();

    public static Dictionary<string, string> WordsMapping;
}
