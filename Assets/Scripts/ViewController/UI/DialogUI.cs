using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    private Text talkUnit;
    private Text mainTalk;
    private Image pictureL;
    private Image pictureR;
    public bool canTalk = false;

    private void Awake()
    {
        Transform panel = transform.Find("Panel");
        pictureL = panel.Find("PictureL").GetComponent<Image>();
        pictureR = panel.Find("PictureR").GetComponent<Image>();
        talkUnit = panel.Find("DialogPanel").Find("TalkUnit").GetComponent<Text>();
        mainTalk = panel.Find("DialogPanel").Find("MainTalk").GetComponent<Text>();
    }

    public void UpdateDialog(Story story)
    {
        talkUnit.text = story.unit;
        mainTalk.text = story.content;

        //立绘图片
        pictureL.sprite = ResourcesExt.Load<Sprite>("Picture/" + story.unit);
        if (pictureL.sprite == null)
        {
            pictureL.enabled = false;
        }
        else
        {
            pictureL.enabled = true;
        }
        pictureL.SetNativeSize();
    }

    private void Update()
    {
        //TODO:对话这样弄感觉会不会很浪费,暂时先这样吧
        if (!canTalk)
        {
            if (UIManager.Instance != null)
                UIManager.Instance.gameObject.SetActive(true);
        }
        else
        {
            if (UIManager.Instance != null)
                UIManager.Instance.gameObject.SetActive(false);
        }
    }
}
