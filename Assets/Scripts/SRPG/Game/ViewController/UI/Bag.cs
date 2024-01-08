using Game.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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

    protected PopManager _popSubInfo;

    private void CloseSubInfo()
    {
        if (this._popSubInfo != null)
        {
            this._popSubInfo.CloseUI(null);
            this._popSubInfo = null;
        }
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
                Item tempItem = items[i];
                item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    InitItemButton(item.gameObject, tempIndex, tempItem);
                });
            }
            else
            {
                item.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                item.GetComponent<Button>().onClick.AddListener(() => { Debug.Log("无法装备"); });
            }

        }
    }

    private void InitItemButton(GameObject b, long tempValue, Item item)
    {
        if (this._popSubInfo != null)
        {
            this.CloseSubInfo();
            //return;
        }
        //UIButtonTools.onButtonClick();
        this.ItemApply(b, tempValue, true, item);
    }

    private void ItemApply(GameObject go, long tempValue, bool isBag, Item item)
    {
        this._popSubInfo = MenuHelper.PopItemInfo(go.transform.position, item, character.getRole(), tempValue, equipID, this.transform.GetChild(0));
        _popSubInfo.UpdateAction += ChangeEquipAction;
        _popSubInfo.UpdateAction += InitBag;
    }

    //TODO:这里只解决了切换武器，没解决显示问题，应该实时解决
    private void ChangeEquipAction()
    {
        character.InitBecauseEquip();
    }

    public void CloseBag()
    {
        for (int i = 0; i < this.itemButtons.transform.childCount; i++)
        {
            Destroy(itemButtons.transform.GetChild(i).gameObject);
        }
    }

    private void OnDisable()
    {
        CloseBag();
    }

}
