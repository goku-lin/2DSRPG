using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect : MonoBehaviour
{
    public bool isNew = false;

    public void Init(Skill skill)
    {
        isNew = true;
        Transform info = transform.Find("Info");
        info.Find("Kind").GetComponent<Text>().text = "技能类型：" + skill.SkillType;
        info.Find("Detail").GetComponent<Text>().text = skill.Help;
    }

    private void OnEnable()
    {
        isNew = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.Find("Button").GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isNew = false;
            Invoke("Begin", 0.2f);
        }
    }

    private void Begin()
    {
        if (!isNew)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        isNew = false;
        for (int i = 0; i < transform.Find("Button").childCount; i++)
        {
            transform.Find("Button").GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
