using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionOrder : MonoBehaviour
{
    public bool isNew = false;
    public GameObject skillBtn;
    private Role beforeRole;
    Transform order;
    Transform skillOrder;
    Transform detail;

    public void Init(Role role)
    {
        isNew = true;
        order = transform.Find("Order");
        skillOrder = transform.Find("SkillOrder");
        detail = transform.Find("Detail");
        //当更换当前角色时，刷新技能列表
        if (beforeRole == role) return;
        for (int i = 0; i < skillOrder.childCount; i++)
        {
            Destroy(skillOrder.GetChild(i).gameObject);
        }
        //遍历角色的携带技能，添加进来，同时加上描述
        int j = 0;
        foreach (Skill skill in role.equipedSkills)
        {
            GameObject s = Instantiate(skillBtn, skillOrder);
            s.GetComponentInChildren<Text>().text = skill.Info.Name;
            //闭包
            int tempValue = j;
            s.GetComponent<LongClickButton>().OnClickUp.AddListener(() => { Skill_slotClick(tempValue); });
            s.GetComponent<LongClickButton>().OnLongPress.AddListener(() => { ShowSkillInfo(skill); });
            s.GetComponent<LongClickButton>().OnLongClickUp.AddListener(() => { CancelSkillInfo(); });
            j++;
        }
        //记录当前角色
        beforeRole = role;
        order.Find("BagBtn").GetComponent<Button>().onClick.AddListener(() => { BattleUIManager.Instance.OpenBag(); });
        //判断角色是否是主角，如果是，则可以使用仓库
    }

    private void OnEnable()
    {
        //isNew = true;
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.Find("Button").GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        //}
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gameObject.SetActive(false);
        }
    }

    //按钮使用技能
    private void Skill_slotClick(int id)
    {
        BattleManager.Instance.UseSkill(id);
    }

    private void ShowSkillInfo(Skill skill)
    {
        detail.GetComponent<Text>().text = skill.Info.Help;
    }

    private void CancelSkillInfo()
    {
        detail.GetComponent<Text>().text = "";
    }

    private void OnDisable()
    {
        //isNew = false;
        //for (int i = 0; i < transform.Find("Button").childCount; i++)
        //{
        //    transform.Find("Button").GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        //}
    }
}
