using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public string nowLevel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void SetLevel(string map)
    {
        nowLevel = map;
        StoryManager.Instance.InitStory("m001");
    }
}
