using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    public GameObject itemButton;
    public Character character;
    private Transform itemSelect;
    private Transform itemButtons;
    private int equipID;

    private void Start()
    {
        itemButton = ResourcesExt.Load<GameObject>("Prefabs/ItemButton");
        itemSelect = transform.Find("ItemSelect");
        itemButtons = transform.Find("ItemButtons");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        InitOpenBag();
    }

    public void InitOpenBag()
    {
        itemButton = ResourcesExt.Load<GameObject>("Prefabs/ItemButton");
        character = BattleManager.Instance.nowCharacter;
        if (character == null) return;
        if (itemButtons == null) itemButtons = transform.Find("ItemButtons");
        InitBag();
    }

    private void InitBag()
    {
        Item[] items = character.getRole().items;
        for (int i = 0; i < items.Length; i++)
        {
            if (i >= itemButtons.childCount)
                Instantiate(itemButton, itemButtons);
            Transform item = itemButtons.GetChild(i);
            if (items[i] == null)
            {
                //空位隐藏掉
                item.gameObject.SetActive(false);
                continue;
            }

            item.GetComponentInChildren<Text>().text = items[i].info.Name;
            if (character.getRole().CanEquip(character.getRole().job, items[i]))
            {
                var tempIndex = i;
                if (items[i].uid == character.getRole().equip.uid)
                {
                    equipID = tempIndex;
                    itemButtons.transform.GetChild(equipID).GetComponent<Image>().color = new Color(0.8f, 1, 0.7f);
                }
                item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    InitItemButton(item.gameObject, tempIndex);
                });
            }
            else
            {
                item.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                item.GetComponent<Button>().onClick.AddListener(() => { Debug.Log("无法装备"); });
            }

        }
    }

    private void InitItemButton(GameObject b, long tempValue)
    {
        itemSelect.gameObject.SetActive(true);
        itemSelect.GetComponent<ItemSelect>().Init(character.getRole().items[tempValue]);
        itemSelect.transform.position = b.transform.position;// + new Vector3(100, 0, 0);
        Item toEquip = character.getRole().items[tempValue];
        //这里判断是否装备还是使用
        if (toEquip.info.Kind != (int)ItemKind.Tool)
            itemSelect.Find("Button/EquipBtn").GetComponent<Button>().onClick.AddListener(() => { ChangeEquip((int)tempValue); });
        else itemSelect.Find("Button/UseBtn").GetComponent<Button>().onClick.AddListener(() => { UseItem((int)tempValue); });
    }

    private void ChangeEquip(int selectID)
    {
        Item toEquip = character.getRole().items[selectID];

        character.getRole().equip = character.getRole().items[selectID];
        character.InitBecauseEquip();

        itemButtons.transform.GetChild(equipID).GetComponent<Image>().color = Color.white;
        itemButtons.transform.GetChild(selectID ).GetComponent<Image>().color = new Color(0.8f, 1, 0.7f);
        equipID = selectID;
        character.getRole().equip = toEquip;
    }

    private void UseItem(int selectID)
    {
        Item toEquip = character.getRole().items[selectID];
        Debug.Log(toEquip.skills.Count);
        for (int i = 0; i < toEquip.skills.Count; i++)
        {
            Skill skill = toEquip.skills[i];
            character.UseItem(skill);
        }
        toEquip.info.Endurance--;
        if (toEquip.info.Endurance <= 0)
        {
            character.getRole().items[selectID] = null;
        }
        InitBag();
    }

    private void OnDisable()
    {
        for (int i = 0; i < this.itemButtons.transform.childCount; i++)
        {
            Destroy(itemButtons.transform.GetChild(i).gameObject);
        }
    }

}
