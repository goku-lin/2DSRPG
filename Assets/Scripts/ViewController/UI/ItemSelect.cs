using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
    public bool isNew = false;

    public void Init(Item item)
    {
        isNew = true;
        Transform info = transform.Find("Info");
        info.Find("Power").GetComponent<Text>().text = "威力：" + item.info.Power;
        info.Find("Weight").GetComponent<Text>().text = "重量：" + item.info.Weight;
        info.Find("RangeI").GetComponent<Text>().text = "内射程：" + item.info.RangeI;
        info.Find("RangeO").GetComponent<Text>().text = "外射程：" + item.info.RangeO;
        info.Find("WeaponLevel").GetComponent<Text>().text = "武器等级：" + item.info.WeaponLevel;
        info.Find("Kind").GetComponent<Text>().text = "武器类型：" + item.info.Kind;
        info.Find("Detail").GetComponent<Text>().text = item.info.Help;
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
            Invoke(nameof(Begin), 0.2f);
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
