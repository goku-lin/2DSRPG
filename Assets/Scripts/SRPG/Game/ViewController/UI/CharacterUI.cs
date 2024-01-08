using Game.Client;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 世界人物UI的逻辑
/// </summary>
public class CharacterUI : MonoBehaviour
{
    private GameObject characterFacePrefab;
    private GameObject skillUIPrefab;
    private GameObject itemButtonPrefab;
    private Role nowRole;
    int equipID = -1;
    private ItemKind[] canEquip = { ItemKind.Sword, ItemKind.Lance, ItemKind.Axe, ItemKind.Bow, ItemKind.Dagger, ItemKind.Magic, ItemKind.Fist, ItemKind.Special };
    protected PopManager _popSubInfo;
    private Dictionary<string, GameObject> learnedSkillDic = new Dictionary<string, GameObject>();

    private Transform faceParent;
    private Transform attributeParent;
    private Transform mainInfoParent;
    private Transform skillParent;
    private Transform itemParent;

    private void LoadPrefabs()
    {
        characterFacePrefab = Resources.Load<GameObject>("Prefabs/CharacterFace");
        skillUIPrefab = Resources.Load<GameObject>("Prefabs/skillUI");
        itemButtonPrefab = Resources.Load<GameObject>("Prefabs/ItemButton");
    }

    private void InitUIElements()
    {
        faceParent = this.transform.Find("Face");
        attributeParent = transform.Find("Attribute");
        mainInfoParent = this.transform.Find("MainInfo");
        skillParent = this.transform.Find("Skill");
        itemParent = this.transform.Find("Item");
    }

    private void Start()
    {
        LoadPrefabs();
        InitUIElements();
        InitCharacterFaces();
    }

    private Sprite LoadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }

    private void InitCharacterFaces()
    {
        foreach (var role in PlayerData.Army.Values)
        {
            GameObject face = Instantiate(characterFacePrefab, faceParent);
            face.GetComponent<Button>().onClick.AddListener(() => CharacterButton(role));
            face.GetComponent<Image>().sprite = LoadSprite("Face/" + role.unitName);
        }
        if (PlayerData.Army.Any())
            CharacterButton(PlayerData.Army.First().Value);
    }

    //点击后更新所有角色信息UI
    private void CharacterButton(Role role)
    {
        if (nowRole != role)
        {
            nowRole = role;
            UpdateCharacterInfo();
        }
        CloseAll();
    }

    private void UpdateCharacterInfo()
    {
        UpdateCharacterPicture();
        UpdateBaseInfo();
        UpdateSkills();
        UpdateItems();
    }

    private void UpdateCharacterPicture()
    {
        Image unitPicture = transform.Find("Picture").GetComponent<Image>();
        unitPicture.sprite = LoadSprite("Picture/" + nowRole.unitName) ?? LoadSprite("Picture/temp");
        unitPicture.SetNativeSize();

        Item expItem = FindExpItem();

        mainInfoParent.Find("Name").GetComponent<Text>().text = nowRole.unitName;
        mainInfoParent.Find("Lv").GetComponent<Text>().text = "Lv：" + nowRole.lv;
        //TODO:暂时先这样，以实现功能为主
        mainInfoParent.Find("LvUp").GetComponent<Button>().onClick.AddListener(() =>
        {
            UseExpItem(expItem);
            UpdateBaseInfo();
            mainInfoParent.Find("Lv").GetComponent<Text>().text = "Lv：" + nowRole.lv;
        });
    }

    private void UpdateBaseInfo()
    {
        attributeParent.Find("HP").GetComponent<Text>().text = "血量：" + nowRole.maxHp;
        attributeParent.Find("Str").GetComponent<Text>().text = "力量：" + nowRole.str;
        attributeParent.Find("Magic").GetComponent<Text>().text = "魔力：" + nowRole.magic;
        attributeParent.Find("Def").GetComponent<Text>().text = "防御：" + nowRole.def;
        attributeParent.Find("Mdef").GetComponent<Text>().text = "魔防：" + nowRole.mdef;
        attributeParent.Find("Tech").GetComponent<Text>().text = "技巧：" + nowRole.tech;
        attributeParent.Find("Quick").GetComponent<Text>().text = "速度：" + nowRole.quick;

        attributeParent.Find("Attack").GetComponent<Text>().text = "攻击：" + nowRole.calcAtt();
    }

    private void UpdateSkills()
    {
        // 假设nowRole.equipedSkills是一个包含当前角色已装备技能的列表
        Transform currentSkill = skillParent.Find("CurrentSkill");
        //已装备技能显示,这里设定固定数量，nowRole.equipedSkills.Count是全部
        for (int i = 0; i < PlayerData.skillNumber; i++)
        {
            if (i >= currentSkill.childCount)
                Instantiate(skillUIPrefab, currentSkill);

            if (i >= nowRole.equipedSkills.Count)
            {
                //空位隐藏掉
                currentSkill.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            GameObject skillGO = currentSkill.GetChild(i).gameObject;
            skillGO.SetActive(true);
            skillGO.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/skill/" + nowRole.equipedSkills[i].Info.IconLabel);
            // 这里可以添加按钮点击事件
        }
        skillParent.Find("SkillInfo").GetComponent<Button>().onClick.AddListener(SkillButton);
    }

    private void UpdateItems()
    {
        Transform equip = itemParent.Find("Equip");
        if (nowRole.equip != null)
            equip.GetChild(0).GetComponent<Text>().text = nowRole.equip.info.Name;
        else
            equip.GetChild(0).GetComponent<Text>().text = "无";
        itemParent.Find("ItemInfo").GetComponent<Button>().onClick.AddListener(ItemButton);
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
        if (expItem == null) return;

        if (expItem.count > 0)
        {
            nowRole.gainExp(50);
            expItem.count--;
        }
    }

    private void SkillButton()
    {
        Transform skillInfo = this.transform.Find("Info/Skill");
        skillInfo.gameObject.SetActive(true);
        Transform currentSkill = skillInfo.Find("CurrentSkill");
        skillInfo.Find("Cancel").GetComponent<Button>().onClick.AddListener(() =>
        {
            skillInfo.gameObject.SetActive(false); 
        });
        learnedSkillDic = new Dictionary<string, GameObject>();
        //已装备技能显示
        for (int i = 0; i < PlayerData.skillNumber; i++)
        {
            if (i >= currentSkill.childCount)
                Instantiate(skillUIPrefab, currentSkill);
            GameObject skillGO = currentSkill.GetChild(i).gameObject;
            if (i >= nowRole.equipedSkills.Count)
            {
                //空位隐藏掉
                skillGO.SetActive(false);
                continue;
            }
            skillGO.SetActive(true);
            skillGO.GetComponent<Image>().sprite = ResourcesExt.Load<Sprite>("Textures/skill/" + nowRole.equipedSkills[i].Info.IconLabel);
            //这里不使用临时变量，最后都用i的最大值了
            int value = i;
            //赋值前先去掉所有事件监听
            skillGO.GetComponent<Button>().onClick.RemoveAllListeners();
            skillGO.GetComponent<Button>().onClick.AddListener(() => InitSkillButton(nowRole.equipedSkills[value], skillGO));
        }
        Transform skills = skillInfo.Find("Skills");
        int j = 0;
        //把学习的技能显示
        foreach (var skill in nowRole.learnedSkills.Values)
        {
            //这个是去掉重复的
            if (j >= skills.childCount) Instantiate(skillUIPrefab, skills);
            skills.GetChild(j).gameObject.SetActive(true);
            GameObject s = skills.GetChild(j).gameObject;
            s.GetComponent<Image>().sprite = ResourcesExt.Load<Sprite>("Textures/skill/" + skill.Info.IconLabel);
            learnedSkillDic.Add(skill.Info.Sid, s);
            if (nowRole.equipedSkills.Contains(skill)) s.SetActive(false);
            int value = j;
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
        if (this._popSubInfo != null)
        {
            this.CloseSubInfo();
            //return;
        }
        //UIButtonTools.onButtonClick();
        this.SkillApply(b, skill, learnedSkillDic);
    }

    private void SkillApply(GameObject go, Skill skill, Dictionary<string, GameObject> learnSkillDic)
    {
        this._popSubInfo = MenuHelper.PopSkillInfo(go.transform.position, skill, nowRole, learnSkillDic);
        _popSubInfo.UpdateAction += UpdateCharacterInfo;
        _popSubInfo.UpdateAction += SkillButton;
    }

    private void DetailButton()
    {
        //Transform info4 = exchange.transform.Find("Info4");
        //Person personInfo = nowRole.getBasicInfo();
        //info4.Find("Name").GetComponent<Text>().text = "姓名：" + personInfo.Name;
        //info4.Find("Detail").GetComponent<Text>().text = "详情：" + personInfo.Help;
        //info4.Find("Age").GetComponent<Text>().text = "年龄：" + personInfo.Age;
        //info4.Find("Birthday").GetComponent<Text>().text = "生日：" + personInfo.BirthMonth + "." + personInfo.BirthDay;
    }

    #region 装备物品UI
    private void ItemButton()
    {
        Transform ItemInfo = this.transform.Find("Info/Item");
        ItemInfo.gameObject.SetActive(true);
        Transform bag = ItemInfo.Find("Bag");
        ItemInfo.Find("Cancel").GetComponent<Button>().onClick.AddListener(() =>
        {
            ItemInfo.gameObject.SetActive(false);
            CloseSubInfo();
        });

        //背包显示
        for (int i = 0; i < 6; i++)
        {
            if (i >= bag.childCount)
                Instantiate(itemButtonPrefab, bag);
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
            s.GetComponent<Button>().onClick.AddListener(() => InitItemButton(s, tempValue, true, nowRole.items[tempValue]));
        }
        Transform warehouse = ItemInfo.Find("Warehouse");
        int j = 0;
        //仓库显示
        foreach (var i in PlayerData.Warehouse.Values)
        {
            if (j >= warehouse.childCount) Instantiate(itemButtonPrefab, warehouse);
            warehouse.GetChild(j).gameObject.SetActive(true);

            GameObject s = warehouse.GetChild(j).gameObject;
            s.GetComponentInChildren<Text>().text = PlayerData.Warehouse[i.uid].info.Name;

            Item tempItem = i;
            s.GetComponent<Button>().onClick.RemoveAllListeners();
            s.GetComponent<Button>().onClick.AddListener(() => InitItemButton(s, i.uid, false, tempItem));
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

    private void InitItemButton(GameObject b, long tempValue, bool isBag, Item item)
    {
        if (this._popSubInfo != null)
        {
            this.CloseSubInfo();
            //return;
        }
        //UIButtonTools.onButtonClick();
        this.ItemApply(b, tempValue, isBag, item);
    }

    private void ItemApply(GameObject go, long tempValue, bool isBag, Item item)
    {
        //Item[] items = this.nowRole.items;
        this._popSubInfo = MenuHelper.PopItemInfo(go.transform.position, item, nowRole, tempValue, equipID, this.transform.Find("Info/Item/Bag"));
        _popSubInfo.UpdateAction += UpdateCharacterInfo;
        _popSubInfo.UpdateAction += ItemButton;
    }
    #endregion

    private void CloseSubInfo()
    {
        if (this._popSubInfo != null)
        {
            this._popSubInfo.CloseUI(null);
            this._popSubInfo = null;
        }
    }

    public void CloseAll()
    {
        Transform skillInfo = this.transform.Find("Info/Skill");
        skillInfo.gameObject.SetActive(false);
        Transform ItemInfo = this.transform.Find("Info/Item");
        ItemInfo.gameObject.SetActive(false);
        CloseSubInfo();
    }
}
