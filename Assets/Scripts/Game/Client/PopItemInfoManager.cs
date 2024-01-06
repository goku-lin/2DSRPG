using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client
{
    public class PopItemInfoManager : PopManager
    {
        protected PopItemInfoMenu _menu;
        protected const string UI_PREFAB = "PopItemInfoMenu";
        protected GameObject _window;
        private bool _needRefresh;
        private bool _MenuLoaded;
        protected Vector3 _pos = Vector3.zero;
        protected Item _item;
        private Role nowRole;
        private long num;
        private int equipID;
        private Transform bag;
        protected string _desc;

        public override void DestroyUI(object arg)
        {
            if (_menu != null)
            {
                //_menu.Close();
                Destroy(_menu.gameObject);
            }
            Destroy(gameObject);
        }

        public override void OpenUI(object arg)
        {
            base.OpenUI(arg);
            if (arg != null)
            {
                List<object> list = (List<object>)arg;
                if (list[0] != null)
                {
                    this._pos = (Vector3)list[0];
                }
                if (list[1] != null)
                {
                    this._item = (Item)list[1];
                }
                if (list[2] != null)
                {
                    nowRole = (Role)list[2];
                }
                if (list[3] != null)
                {
                    num = (long)list[3];
                }
                if (list[4] != null)
                {
                    equipID = (int)list[4];
                }
                if (list[5] != null)
                {
                    bag = (Transform)list[5];
                }
            }
            string[] uiName = new string[]
            {
                "PopItemInfoMenu"
            };
            OnInstantiateOver[] onInstOvers = new OnInstantiateOver[]
            {
                new OnInstantiateOver(this.OnInstantiated)
            };
            UIManager.GetInstance().OpenUI(uiName, new OnOpenComplete(this.OnOpenCompleted), onInstOvers, this.anchor, true);
        }

        private void OnOpenCompleted(GameObject go)
        {
        }

        private void OnInstantiated(GameObject window)
        {
            _window = window;
            if (_window != null)
            {
                _menu = _window.GetComponent<PopItemInfoMenu>();
                _menu.equipBtn.onClick.AddListener(ChangeEquip);
                //_menu.useBtn.onClick.AddListener();
                _menu.sendBtn.onClick.AddListener(SendItem);
                _menu.receiveBtn.onClick.AddListener(ReceiveItem);
                _MenuLoaded = true;
                _needRefresh = true;
                InitData();
            }
        }

        private void InitData()
        {
            if (_menu != null)
            {
                _window.transform.position = _pos;
                ItemInfo itemData = _item.info;
                if (itemData.Kind != (int)ItemKind.Tool)
                {
                    _menu.obj_prop.SafeActive(true);
                    _menu.attrText[0].text = itemData.WeaponAttr.ToString();
                    //_menu.label_attr[1].text = itemData.criticalRate.ToString();
                    _menu.attrText[2].text = itemData.RangeI + "-" + itemData.RangeO;
                    //_menu.label_attr[3].text = itemData.hitRate.ToString();
                    //_menu.label_attr[4].text = itemData.dodgeRate.ToString();
                    _menu.attrText[3].text = itemData.WeaponLevel.ToString();
                    _menu.attrText[4].text = itemData.Kind.ToString();
                }
                else
                {
                    _menu.obj_prop.SafeActive(false);
                }
                _menu.weightText.text = string.Empty + itemData.Weight;
                _menu.helpInfoText.text = string.Empty + itemData.Help;
            }
        }

        private void ChangeEquip()
        {
            int selectID = (int)num;
            Item toEquip = nowRole.items[selectID];

            bag.GetChild(equipID).GetComponent<Image>().color = Color.white;
            bag.GetChild(selectID).GetComponent<Image>().color = new Color(0.8f, 1, 0.7f);
            equipID = selectID;
            nowRole.equip = toEquip;
            Destroy(_menu.gameObject);
            UpdateAction();
        }

        private void SendItem()
        {
            int selectID = (int)num;
            Item nowItem = nowRole.items[selectID];
            //数据变化
            if (nowRole.equip != null && nowItem.uid == nowRole.equip.uid)
            {
                nowRole.equip = null;
            }
            PlayerData.Warehouse.Add(nowItem.uid, nowItem);
            nowRole.items[selectID] = null;
            Destroy(_menu.gameObject);
            UpdateAction();
        }

        private void ReceiveItem()
        {
            long uid = num;
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
            Destroy(_menu.gameObject);
            UpdateAction();
        }
    }
}