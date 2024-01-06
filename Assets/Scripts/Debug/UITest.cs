using Game.Client;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    private void Awake()
    {
        GameKey.InitDefalutButtons();
    }

    private void Update()
    {
        //if (this.isInited)
        {
            GameKey.Update();
            //return;
        }
        //if (UIManager.GetInstance().isLoaded && CharacterDataManager.GetInstance().roleDatas.Count > 0 && Singleton<SwordCardConfiguration>.Instance.isReady)
        //{
        //    base.Invoke("InitTest", 2f);
        //    this.isInited = true;
        //}
    }
}
