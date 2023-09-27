using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameDefine;

public class AudioCtrl : MonoBehaviour
{
    public static AudioCtrl instance;
    AudioSource audioSourceMusic;
    AudioSource audioSourceSound;

    //不能够删除
    static GameObject gob;

    public static void Init()
    {
        gob = new GameObject("audioCtrl");
        gob.AddComponent<AudioCtrl>();

        //场景切换时不被删除
        DontDestroyOnLoad(gob);
    }

    private void Awake()
    {
        this.gameObject.AddComponent<AudioListener>();
        AudioCtrl.instance = this;
    }

    private void Start()
    {
        //audioSource = GameObject.FindObjectOfType<AudioSource>();
        audioSourceMusic = gob.AddComponent<AudioSource>();
        audioSourceMusic.clip = GetAudio("bgm");
        audioSourceMusic.Play();

        audioSourceSound = audioSourceMusic.gameObject.AddComponent<AudioSource>();

        EventDispatcher.instance.Regist(GameEventType.playButtonUiSound, this.playButtonUiSound);
        EventDispatcher.instance.Regist(GameEventType.playHitBodySound, this.playHitBodySound);
        EventDispatcher.instance.Regist<Role>(GameEventType.playIdleVoice, this.PlayIdleVoice);
        EventDispatcher.instance.Regist<Role>(GameEventType.playAttackVoice, this.PlayAttackVoice);

        LoadCfg();
    }

    private void PlayIdleVoice(Role player)
    {
        audioSourceSound.PlayOneShot(GetVoice(player.unitName + "Idle"));
    }

    private void PlayAttackVoice(Role player)
    {
        audioSourceSound.PlayOneShot(GetVoice(player.unitName + "Attack"));
    }

    private void playHitBodySound()
    {
        audioSourceSound.PlayOneShot(GetAudio("hitbody"));
    }

    private void playButtonUiSound()
    {
        audioSourceSound.PlayOneShot(GetAudio("button"));
    }

    private AudioClip GetVoice(string path)
    {
        return ResourcesExt.Load<AudioClip>("Voice/" + path);
    }

    private AudioClip GetAudio(string path)
    {
        return ResourcesExt.Load<AudioClip>("Audio/" + path);
    }

    public void SetSoundValue(float arg0)
    {
        audioSourceSound.volume = arg0;
    }

    public void SetMusicValue(float arg0)
    {
        audioSourceMusic.volume = arg0;
    }

    public float GetSoundValue()
    {
        return audioSourceSound.volume;
    }

    public float GetMusicValue()
    {
        return audioSourceMusic.volume;
    }

    public void LoadCfg()
    {
        this.audioSourceMusic.volume = PlayerPrefs.GetFloat("MusicVolume");
        this.audioSourceSound.volume = PlayerPrefs.GetFloat("SoundVolume");
    }

    public void SaveCfg()
    {
        PlayerPrefs.SetFloat("MusicVolume", this.audioSourceMusic.volume);
        PlayerPrefs.SetFloat("SoundVolume", this.audioSourceMusic.volume);
    }
}
