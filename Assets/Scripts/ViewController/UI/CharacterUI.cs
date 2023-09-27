using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// 世界人物UI的逻辑
/// </summary>
public class CharacterUI : MonoBehaviour
{
    private GameObject characterFace;
    Transform exchange;
    private GameObject skillUI;
    private GameObject itemButton;
    private Role nowRole;
    int equipID = -1;
    private Dictionary<string, int> currentSkillDic = new Dictionary<string, int>();
    private Dictionary<string, int> allSkillDic = new Dictionary<string, int>();
    Transform itemSelect;
    Transform skillSelect;

    private void Start()
    {
        characterFace = ResourcesExt.Load<GameObject>("Prefabs/CharacterFace");
        skillUI = ResourcesExt.Load<GameObject>("Prefabs/skillUI");
        itemButton = ResourcesExt.Load<GameObject>("Prefabs/ItemButton");
        exchange = this.transform.Find("Exchange");
        itemSelect = this.transform.Find("ItemSelect");
        skillSelect = this.transform.Find("SkillSelect");
        InitCharacter();
    }


    private void InitCharacter()
    {
        Transform face = this.transform.Find("Face");

        foreach (var item in PlayerData.Army.Values)
        {
            GameObject f = Instantiate(characterFace, face);
            f.GetComponent<Button>().onClick.AddListener(() => { CharacterButton(item); });
            f.transform.GetComponent<Image>().sprite = ResourcesExt.Load<Sprite>("Face/" + item.unitName);
        }
        CharacterButton(PlayerData.Army[1]);
    }

    //点击后更新所有角色信息UI
    private void CharacterButton(Role r)
    {
        nowRole = r;

        CharacterPicture picture = this.transform.Find("Picture").GetComponent<CharacterPicture>();
        picture.Init(nowRole);

        UpdateCharacterInfo();
    }

    private void UpdateCharacterInfo()
    {
        Transform attribute = transform.Find("Attribute");

        Image unitPicture = this.transform.Find("Picture").GetComponent<Image>();
        unitPicture.sprite = ResourcesExt.Load<Sprite>("Picture/" + nowRole.unitName);
        if (unitPicture.sprite == null)
        {
            unitPicture.sprite = ResourcesExt.Load<Sprite>("Picture/temp");
        }
        unitPicture.SetNativeSize();
        Item expItem = FindExpItem();

        UpdateBaseInfo(attribute);
        Transform info = this.transform.Find("Info");
        info.Find("Name").GetComponent<Text>().text = nowRole.unitName;
        info.Find("Lv").GetComponent<Text>().text = "Lv：" + nowRole.lv;
        //TODO:暂时先这样，以实现功能为主
        info.Find("LvUp").GetComponent<Button>().onClick.AddListener(() => { UseExpItem(expItem); UpdateBaseInfo(attribute); });



        Transform skill = this.transform.Find("Skill");
        Transform currentSkill = skill.Find("CurrentSkill");
        //已装备技能显示
        currentSkillDic = new Dictionary<string, int>();
        for (int i = 0; i < PlayerData.skillNumber; i++)
        {
            if (i >= currentSkill.childCount)
                Instantiate(skillUI, currentSkill);
            if (i >= nowRole.equipedSkills.Count)
            {
                //空位隐藏掉
                currentSkill.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            currentSkill.GetChild(i).gameObject.SetActive(true);
            GameObject s = currentSkill.GetChild(i).gameObject;
            s.GetComponent<Image>().sprite = ResourcesExt.Load<Sprite>("Textures/skill/" + nowRole.equipedSkills[i].IconLabel);
        }

        skill.Find("SkillInfo").GetComponent<Button>().onClick.AddListener(SkillButton);


        Transform item = this.transform.Find("Item");
        Transform equip = item.Find("Equip");

        Transform info3 = this.transform.Find("Item/Info3");

        if (nowRole.equip != null)
            equip.GetChild(0).GetComponent<Text>().text = nowRole.equip.info.Name;
        else
            equip.GetChild(0).GetComponent<Text>().text = "无";
        item.Find("ItemInfo").GetComponent<Button>().onClick.AddListener(ItemButton);
    }

    private void UpdateBaseInfo(Transform info)
    {
        //info.Find("Lv").GetComponent<Text>().text = "Lv：" + nowRole.lv;
        //info.Find("Exp").GetComponent<Text>().text = "Exp：" + nowRole.exp;
        info.Find("HP").GetComponent<Text>().text = "血量：" + nowRole.maxHp;
        info.Find("Str").GetComponent<Text>().text = "力量：" + nowRole.str;
        info.Find("Magic").GetComponent<Text>().text = "魔力：" + nowRole.magic;
        info.Find("Def").GetComponent<Text>().text = "防御：" + nowRole.def;
        info.Find("Mdef").GetComponent<Text>().text = "魔防：" + nowRole.mdef;
        info.Find("Tech").GetComponent<Text>().text = "技巧：" + nowRole.tech;
        info.Find("Quick").GetComponent<Text>().text = "速度：" + nowRole.quick;

        info.Find("Attack").GetComponent<Text>().text = "攻击：" + nowRole.calcAtt();
    }

    private Item FindExpItem()
    {
        foreach (var i in PlayerData.Warehouse.Values)
        {

            if (i.info.Kind == (int)ItemKind.Exp)
                return i;
        }

        return null;
    }

    private void UseExpItem(Item expItem)
    {
        //Item expItem = PlayerData.Warehouse[selectID];
        //TODO:获得经验改到这里
        //for (int i = 0; i < toEquip.skills.Count; i++)
        //{
        //    Skill skill = toEquip.skills[i];
        //    character.UseItem(skill);
        //}
        if (expItem == null) return;

        if (expItem.info.Endurance > 0)
            nowRole.gainExp(50);
        expItem.info.Endurance--;
        //if (expItem.info.Endurance <= 0)
        //{
        //    nowRole.items[selectID] = null;
        //}
    }

    private void SkillButton()
    {
        Transform skillInfo = this.transform.Find("Info2/Skill");
        skillInfo.gameObject.SetActive(true);
        Transform currentSkill = skillInfo.Find("CurrentSkill");
        skillInfo.Find("Cancel").GetComponent<Button>().onClick.AddListener(() =>
        {
            skillInfo.gameObject.SetActive(false); 
        });
        //已装备技能显示
        currentSkillDic = new Dictionary<string, int>();
        for (int i = 0; i < PlayerData.skillNumber; i++)
        {
            if (i >= currentSkill.childCount)
                Instantiate(skillUI, currentSkill);
            if (i >= nowRole.equipedSkills.Count)
            {
                //空位隐藏掉
                currentSkill.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            currentSkill.GetChild(i).gameObject.SetActive(true);
            GameObject s = currentSkill.GetChild(i).gameObject;
            s.GetComponent<Image>().sprite = ResourcesExt.Load<Sprite>("Textures/skill/" + nowRole.equipedSkills[i].IconLabel);
            //这里不使用临时变量，最后都用i的最大值了
            int value = i;
            //使用字典储存，保证能找到
            currentSkillDic.Add(nowRole.equipedSkills[value].Sid, value);
            //赋值前先去掉所有事件监听
            s.GetComponent<Button>().onClick.RemoveAllListeners();
            s.GetComponent<Button>().onClick.AddListener(() => InitSkillButton(nowRole.equipedSkills[value], s));
        }
        Transform skills = skillInfo.Find("Skills");
        int j = 0;
        //把学习的技能显示
        allSkillDic = new Dictionary<string, int>();
        foreach (var skill in nowRole.learnedSkills.Values)
        {
            //这个是去掉重复的
            //if (nowRole.equipedSkills.Contains(skill)) continue;
            if (j >= skills.childCount) Instantiate(skillUI, skills);
            skills.GetChild(j).gameObject.SetActive(true);
            GameObject s = skills.GetChild(j).gameObject;
            s.GetComponent<Image>().sprite = ResourcesExt.Load<Sprite>("Textures/skill/" + skill.IconLabel);
            if (nowRole.equipedSkills.Contains(skill))
                s.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);

            int value = j;
            allSkillDic.Add(skill.Sid, value);
            s.GetComponent<Button>().onClick.RemoveAllListeners();
            s.GetComponent<Button>().onClick.AddListener(() => InitSkillButton(skill, s));
            j++;
        }
        //空位隐藏掉
        if (skills.childCount >= j - 1)
        {
            for (int i = j; i < skills.childCount; i++)
            {
                skills.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void InitSkillButton(Skill skill, GameObject b)
    {
        skillSelect.gameObject.SetActive(true);
        skillSelect.transform.position = b.transform.position;// + new Vector3(100, 0, 0);

        Transform button = skillSelect.Find("Button");
        button.Find("EquipBtn").GetComponent<Button>().onClick.AddListener(() => { ChangeSkill(skill); });
        skillSelect.GetComponent<SkillSelect>().Init(skill);
    }

    private void ChangeSkill(Skill skill)
    {
        Transform info2 = this.transform.Find("Skill/Info2");
        Transform currentSkill = info2.Find("CurrentSkill");
        Transform skills = info2.Find("Skills");
        if (nowRole.equipedSkills.Contains(skill))
        {
            nowRole.equipedSkills.Remove(skill);
            currentSkill.GetChild(currentSkillDic[skill.Sid]).gameObject.SetActive(false);
            currentSkillDic.Remove(skill.Sid);
            skills.GetChild(allSkillDic[skill.Sid]).GetComponent<Image>().color = Color.white;
        }
        else if (nowRole.equipedSkills.Count < PlayerData.skillNumber)
        {
            nowRole.equipedSkills.Add(skill);
            int j = -1;
            for (int i = 0; i < currentSkill.childCount; i++)
            {
                if (!currentSkill.GetChild(i).gameObject.activeSelf)
                {
                    j = i;
                    break;
                }
            }
            currentSkillDic.Add(skill.Sid, j);
            currentSkill.GetChild(j).gameObject.SetActive(true);
            GameObject s = currentSkill.GetChild(j).gameObject;
            s.GetComponent<Image>().sprite = ResourcesExt.Load<Sprite>("Textures/skill/" + skill.IconLabel);
            skills.GetChild(allSkillDic[skill.Sid]).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void DetailButton()
    {
        Transform info4 = exchange.transform.Find("Info4");
        Person personInfo = nowRole.getBasicInfo();
        info4.Find("Name").GetComponent<Text>().text = "姓名：" + personInfo.Name;
        info4.Find("Detail").GetComponent<Text>().text = "详情：" + personInfo.Help;
        info4.Find("Age").GetComponent<Text>().text = "年龄：" + personInfo.Age;
        info4.Find("Birthday").GetComponent<Text>().text = "生日：" + personInfo.BirthMonth + "." + personInfo.BirthDay;
    }

    #region 装备物品UI
    private void ItemButton()
    {
        Transform ItemInfo = this.transform.Find("Info2/Item");
        ItemInfo.gameObject.SetActive(true);
        Transform bag = ItemInfo.Find("Bag");
        ItemInfo.Find("Cancel").GetComponent<Button>().onClick.AddListener(() =>
        {
            ItemInfo.gameObject.SetActive(false); 
        });

        //背包显示
        for (int i = 0; i < 6; i++)
        {
            if (i >= bag.childCount)
                Instantiate(itemButton, bag);
            if (nowRole.items[i] == null)
            {
                //空位隐藏掉
                bag.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            bag.GetChild(i).gameObject.SetActive(true);

            GameObject s = bag.GetChild(i).gameObject;

            s.GetComponentInChildren<Text>().text = nowRole.items[i].info.Name;
            s.GetComponent<Image>().color = Color.white;

            if (nowRole.equip != null && nowRole.items[i].uid == nowRole.equip.uid)
            {
                equipID = i;
                s.GetComponent<Image>().color = new Color(0.8f, 1, 0.7f);
            }

            int tempValue = i;
            s.GetComponent<Button>().onClick.RemoveAllListeners();
            s.GetComponent<Button>().onClick.AddListener(() => InitItemButton(s, tempValue, true));
        }
        Transform warehouse = ItemInfo.Find("Warehouse");
        int j = 0;
        //仓库显示
        foreach (var i in PlayerData.Warehouse.Values)
        {
            if (j >= warehouse.childCount) Instantiate(itemButton, warehouse);
            warehouse.GetChild(j).gameObject.SetActive(true);

            GameObject s = warehouse.GetChild(j).gameObject;
            s.GetComponentInChildren<Text>().text = PlayerData.Warehouse[i.uid].info.Name;

            int tempValue =j;
            s.GetComponent<Button>().onClick.RemoveAllListeners();
            s.GetComponent<Button>().onClick.AddListener(() => InitItemButton(s, i.uid, false));
            j++;
        }
        //空位隐藏掉
        if (warehouse.childCount >= j - 1)
        {
            for (int i = j; i < warehouse.childCount; i++)
            {
                warehouse.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void InitItemButton(GameObject b, long tempValue, bool isBag)
    {
        itemSelect.gameObject.SetActive(true);
        itemSelect.transform.position = b.transform.position;// + new Vector3(100, 0, 0);
        //TODO:这里判断是否装备还是使用
        Transform button = itemSelect.Find("Button");
        if (isBag)
        {
            Item toEquip = nowRole.items[(int)tempValue];
            button.Find("EquipBtn").GetComponent<Button>().onClick.AddListener(() => { ChangeEquip((int)tempValue); });
            button.Find("UseBtn").GetComponent<Button>().onClick.AddListener(() => {  });

            itemSelect.GetComponent<ItemSelect>().Init(nowRole.items[tempValue]);
            button.Find("SendBtn").gameObject.SetActive(true);
            button.Find("SendBtn").GetComponent<Button>().onClick.AddListener(() => { SendItem((int)tempValue); });
            button.Find("ReceiveBtn").gameObject.SetActive(false);
        }
        else
        {
            //TODO:这个和bag的这个看看能不能优化一下
            itemSelect.GetComponent<ItemSelect>().Init(PlayerData.Warehouse[tempValue]);
            button.Find("ReceiveBtn").gameObject.SetActive(true);
            button.Find("ReceiveBtn").GetComponent<Button>().onClick.AddListener(() => { ReceiveItem(tempValue); });
            button.Find("SendBtn").gameObject.SetActive(false);
        }
    }

    private void ChangeEquip(int selectID)
    {
        Transform item = this.transform.Find("Info2/Item");
        Transform bag = item.Find("Bag");
        Item toEquip = nowRole.items[selectID];

        bag.GetChild(equipID).GetComponent<Image>().color = Color.white;
        bag.GetChild(selectID).GetComponent<Image>().color = new Color(0.8f, 1, 0.7f);
        equipID = selectID;
        nowRole.equip = toEquip;
        UpdateCharacterInfo();
    }

    private void SendItem(int selectID)
    {
        Item nowItem = nowRole.items[selectID];
        Debug.Log(nowItem == null);
        //数据变化
        if (nowRole.equip != null && nowItem.uid == nowRole.equip.uid)
        {
            nowRole.equip = null;
            UpdateCharacterInfo();
        }
        PlayerData.Warehouse.Add(nowItem.uid, nowItem);
        nowRole.items[selectID] = null;
        //表现变化
        ItemButton();
    }

    private void ReceiveItem(long uid)
    {
        Item nowItem = PlayerData.Warehouse[uid];
        Debug.Log(nowItem.uid);
        //数据变化
        PlayerData.Warehouse.Remove(nowItem.uid);
        for (int i = 0; i < nowRole.items.Length; i++)
        {
            if (nowRole.items[i] == null)
            {
                nowRole.items[i] = nowItem;
                Debug.Log(nowRole.items[i].info.Name);
                break;
            }
        }
        //表现变化
        ItemButton();
    }
    #endregion
}
