using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameDefine;

public class UISetting : MonoBehaviour
{
    Slider musicSlider;
    Slider soundSlider;

    Button gotoHome;
    Button backWorld;
    Button cancel;

    private void Start()
    {
        musicSlider = transform.Find("Item/MusicSlider").GetComponent<Slider>();
        soundSlider = transform.Find("Item/SoundSlider").GetComponent<Slider>();
        gotoHome = transform.Find("Item/GotoHome").GetComponent<Button>();
        backWorld = transform.Find("Item/BackWorld").GetComponent<Button>();

        cancel = transform.Find("Cancel").GetComponent<Button>();
        //先更新UI再订阅回调
        musicSlider.value = AudioCtrl.instance.GetMusicValue();
        soundSlider.value = 1;
        musicSlider.onValueChanged.AddListener(OnMusicToggle);
        soundSlider.onValueChanged.AddListener(OnSoundToggle);

        cancel.onClick.AddListener(CancelSetting);
        gotoHome.onClick.AddListener(OnGotoHomeClICK);
        backWorld.onClick.AddListener(BackWorldClICK);
    }

    private void CancelSetting()
    {
        GameObject.Destroy(gameObject);
        AudioCtrl.instance.SaveCfg();
    }

    void OnGotoHomeClICK()
    {
        EventDispatcher.instance.DispatchEvent(GameEventType.GotoHomeClICK);
    }

    void BackWorldClICK()
    {
        SceneManagerExt.instance.LoadSceneShowProgress(SceneType.GameWorld);
    }

    private void OnSoundToggle(float arg0)
    {
        AudioCtrl.instance.SetSoundValue(soundSlider.value);
    }

    private void OnMusicToggle(float arg0)
    {
        AudioCtrl.instance.SetMusicValue(musicSlider.value);
    }
}
