using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    private Image picture;
    private Transform infoPanel;
    private Transform skillPanel;

    private void Awake()
    {
        picture = transform.GetChild(0).GetComponent<Image>();
        infoPanel = transform.GetChild(1);
        skillPanel = transform.GetChild(2);
    }

    private void OnEnable()
    {
        Role nowRole = BattleManager.Instance.nowCharacter.getRole();
        infoPanel.Find("Name").GetComponent<Text>().text = "姓名：" + nowRole.unitName;
        infoPanel.Find("Profession").GetComponent<Text>().text = "职业：" + nowRole.job.Name;
        infoPanel.Find("Weapon").GetComponent<Text>().text = "武器：" + nowRole.equip.info.Name;
        infoPanel.Find("Lv").GetComponent<Text>().text = "Lv：" + nowRole.lv;
        infoPanel.Find("Move").GetComponent<Text>().text = "移动：" + nowRole.movePower;
        infoPanel.Find("HP").GetComponent<Text>().text = "HP：" + nowRole.hp + "/" + nowRole.maxHp;
        infoPanel.Find("Attack").GetComponent<Text>().text = "攻击：" + nowRole.calcAtt();
        infoPanel.Find("Defend").GetComponent<Text>().text = "防御：" + nowRole.def;
        infoPanel.Find("MDef").GetComponent<Text>().text = "魔防：" + nowRole.mdef;
        infoPanel.Find("Quick").GetComponent<Text>().text = "速度：" + nowRole.quick;

        picture.sprite = ResourcesExt.Load<Sprite>("Picture/" + nowRole.unitName);
        if (picture.sprite == null)
        {
            picture.enabled = false;
        }
        else
        {
            picture.enabled = true;
        }
        picture.SetNativeSize();


        List<Skill> learnedSkill = nowRole.equipedSkills;
        for (int i = 0; i < 3; i++)
        {
            string id;
            if (i >= learnedSkill.Count)
                id = "";
            else id = learnedSkill[i].Info.Name;
            var showIcon = id != "";

            if (showIcon) skillPanel.GetChild(i).GetComponent<Image>().sprite = BattleManager.Instance.GetSkillTexture(learnedSkill[i].Info.IconLabel);
            skillPanel.GetChild(i).gameObject.SetActive(showIcon);

            if (i >= learnedSkill.Count) continue;
            var skill = learnedSkill[i];

            if (skill.Info.SkillType != 0)
            {
                var cd = skill.cd;
                skillPanel.GetChild(i).GetChild(0).gameObject.SetActive(cd > 0);
                skillPanel.GetChild(i).GetChild(0).GetComponentInChildren<Text>().text = cd.ToString();
            }
            else
                skillPanel.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
}
