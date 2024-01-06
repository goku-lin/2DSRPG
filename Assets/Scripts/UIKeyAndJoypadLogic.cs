using System;
using System.Collections.Generic;
using Game;
using Game.Client;
using InControl;
using lib.notify;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyAndJoypadLogic : ICallback
{
    //public SwitchableButtonGroup getCurrentGroup()
    //{
    //    return this.currentGroup;
    //}

    //public void SetButtonList(List<UIEventListenerCustom> buttonList)
    //{
    //    this.buttonList = buttonList;
    //    for (int i = 0; i < buttonList.Count; i++)
    //    {
    //        if (buttonList[i].onHover == null)
    //        {
    //            buttonList[i].onHover = new UIEventListenerCustom.BoolDelegate(this.OnItemHover);
    //        }
    //    }
    //}

    //public void SetButtonGroup(List<SwitchableButtonGroup> buttonGroup)
    //{
    //    this.buttonGroup = buttonGroup;
    //}

    //public void SetRoot(Transform root)
    //{
    //    this.transform = root;
    //    this.depthDependentPanel = root.GetComponent<UIPanel>();
    //}

    //public void SetFingerDepthDependentPanel(UIPanel panel)
    //{
    //    this.depthDependentPanel = panel;
    //}

    public void SetEnable()
    {
        Notifier.Register(10216, this);
    }

    public void SetDisable()
    {
        Notifier.Remove(10216, this);
    }

    public void Clear()
    {
        //this.currentGroup = null;
        if (this._finger != null)
        {
            UnityEngine.Object.Destroy(this._finger);
            this._finger = null;
        }
    }

    public void InitButtonsMaps(bool needFocus = true)
    {
        //this.buttonMaps.Clear();
        //int i;
        //for (i = 0; i < this.buttonList.Count; i++)
        //{
        //    this.buttonList[i].sleepOnEnable();
        //    SwitchableButtonGroup value = this.buttonGroup.Find((SwitchableButtonGroup g) => g.root == this.buttonList[i]);
        //    this.buttonMaps.Add(this.buttonList[i], value);
        //}
        //this._isInited = true;
        //InputDevice activeDevice = InputManager.ActiveDevice;
        //if (activeDevice != null && activeDevice.Name != "None" && GameKey.IsJoyPadHintMode && needFocus && this.buttonGroup.Count > 0)
        //{
        //    this.countGroup = this.buttonGroup.Find((SwitchableButtonGroup b) => b.root.gameObject.activeSelf && b.root.ButtonEnable);
        //    if (this.countGroup != null)
        //    {
        //        this.DealFocusItem(false, true);
        //    }
        //}
    }

    public void GoToDefaultGroup(bool isMouse = false, bool needHoverFocus = true)
    {
        //this.countGroup = this.buttonGroup.Find((SwitchableButtonGroup b) => b.root.gameObject.activeSelf && b.root.ButtonEnable);
        //if (this.countGroup != null && this.countGroup.root != null && this.countGroup.root.gameObject.activeInHierarchy && this.countGroup.root.ButtonEnable)
        //{
        //    this.DealFocusItem(isMouse, needHoverFocus);
        //}
    }

    //public void GoToGroup(UIEventListenerCustom buttonEvent, bool isMouse = false, bool needHoverFocus = true)
    //{
    //    if (buttonEvent == null)
    //    {
    //        return;
    //    }
    //    this.buttonMaps.TryGetValue(buttonEvent, out this.countGroup);
    //    if (this.countGroup != null && this.countGroup.root != null && this.countGroup.root.gameObject.activeInHierarchy && this.countGroup.root.ButtonEnable)
    //    {
    //        buttonEvent.sleepOnEnable();
    //        this.DealFocusItem(isMouse, needHoverFocus);
    //    }
    //}

    public void ShowFinger(bool isShow)
    {
        if (this._finger != null)
        {
            this._finger.GetComponentInChildren<Image>().enabled = isShow;
        }
    }

    public void CheckFingerDepth()
    {
        if (this._finger != null)
        {
            //this._finger.GetComponent<UIPanel>().depth = this.transform.GetComponent<UIPanel>().depth + 5;
        }
    }

    public bool DealKeyEvent(int keyCode, int keyState)
    {
        if (!this._isInited)
        {
            this.InitButtonsMaps(true);
        }
        //if (this.buttonGroup.Count == 0)
        //{
        //    return false;
        //}
        //if (this.currentGroup == null && this.buttonGroup.Count > 0)
        //{
        //    this.currentGroup = this.buttonGroup[0];
        //}
        if (keyCode == 3 && keyState == 3 && GameKey.IsKeyRepeated(3, 5))
        {
            if (this.GetNextButtonGroup(3))
            {
                return true;
            }
        }
        else if (keyCode == 4 && keyState == 3 && GameKey.IsKeyRepeated(4, 5))
        {
            if (this.GetNextButtonGroup(4))
            {
                return true;
            }
        }
        else if (keyCode == 5 && keyState == 3 && GameKey.IsKeyRepeated(5, 5))
        {
            if (this.GetNextButtonGroup(5))
            {
                return true;
            }
        }
        else if (keyCode == 6 && keyState == 3 && GameKey.IsKeyRepeated(6, 5))
        {
            if (this.GetNextButtonGroup(6))
            {
                return true;
            }
        }
        else if (keyCode == 18 && keyState == 3 && GameKey.IsKeyRepeated(18, 5))
        {
            if (this.GetNextButtonGroup(18))
            {
                return true;
            }
        }
        else if (keyCode == 19 && keyState == 3 && GameKey.IsKeyRepeated(19, 5))
        {
            if (this.GetNextButtonGroup(19))
            {
                return true;
            }
        }
        else if (keyCode == 20 && keyState == 3 && GameKey.IsKeyRepeated(20, 5))
        {
            if (this.GetNextButtonGroup(20))
            {
                return true;
            }
        }
        else if (keyCode == 21 && keyState == 3 && GameKey.IsKeyRepeated(21, 5))
        {
            if (this.GetNextButtonGroup(21))
            {
                return true;
            }
        }
        else if ((keyCode == 3 && keyState == 1) || (keyCode == 18 && keyState == 1))
        {
            if (this.GetNextButtonGroup(3))
            {
                return true;
            }
        }
        else if ((keyCode == 4 && keyState == 1) || (keyCode == 19 && keyState == 1))
        {
            if (this.GetNextButtonGroup(keyCode))
            {
                return true;
            }
        }
        else if ((keyCode == 5 && keyState == 1) || (keyCode == 20 && keyState == 1))
        {
            if (this.GetNextButtonGroup(keyCode))
            {
                return true;
            }
        }
        else if ((keyCode == 6 && keyState == 1) || (keyCode == 21 && keyState == 1))
        {
            if (this.GetNextButtonGroup(keyCode))
            {
                return true;
            }
        }
        //else if (keyCode == 1 && keyState == 2)
        //{
        //    if (!this.currentGroup.root.gameObject.activeInHierarchy || !this.currentGroup.root.ButtonEnable)
        //    {
        //        return false;
        //    }
        //    this.currentGroup.root.OnClick();
        //    return true;
        //}
        return false;
    }

    protected bool GetNextButtonGroup(int keyCode)
    {
        //if (GetNextButton(currentGroup, keyCode) == null)
        //{
        //    return false;
        //}
        //buttonMaps.TryGetValue(GetNextButton(currentGroup, keyCode), out countGroup);

        //while (countGroup != currentGroup && (!countGroup.root.gameObject.activeInHierarchy || !countGroup.root.ButtonEnable))
        //{
        //    SwitchableButtonGroup nextSpareButton = GetNextSpareButton(currentGroup, keyCode);
        //    if (nextSpareButton != null)
        //    {
        //        countGroup = nextSpareButton;
        //        break;
        //    }

        //    UIEventListenerCustom nextButton = GetNextButton(countGroup, keyCode);
        //    if (nextButton == null)
        //    {
        //        break;
        //    }

        //    buttonMaps.TryGetValue(nextButton, out countGroup);
        //    if (countGroup == null)
        //    {
        //        break;
        //    }
        //}

        //if (countGroup != null && countGroup.root != null && countGroup.root.gameObject.activeInHierarchy && countGroup.root.ButtonEnable)
        //{
        //    DealFocusItem();
        //    return true;
        //}

        return false;
    }

    protected void DealFocusItem(bool isMouse = false, bool needHoverFocus = true)
    {
        GameLogic.IsMouseMoved = true;
        //if (needHoverFocus)
        //{
        //    for (int i = 0; i < this.buttonList.Count; i++)
        //    {
        //        if (this.countGroup.root != this.buttonList[i])
        //        {
        //            this.buttonList[i].OnHover(false);
        //            this.buttonList[i].OnFocusEvent(false);
        //        }
        //    }
        //}
        //this.currentGroup = this.countGroup;
        //if (needHoverFocus)
        //{
        //    this.currentGroup.root.OnHover(true);
        //    this.currentGroup.root.OnFocusEvent(true);
        //}
        //else
        //{
        //    this.dealFingerOnHover();
        //}
        GameLogic.IsMouseMoved = false;
    }

    public void OnItemHover(GameObject gameobject, bool isHover)
    {
        if (isHover)
        {
            //SwitchableButtonGroup switchableButtonGroup = this.buttonGroup.Find((SwitchableButtonGroup b) => b.root.gameObject == gameobject);
            //if (this.currentGroup != null && this.currentGroup != switchableButtonGroup)
            //{
            //    this.currentGroup.root.OnHover(false);
            //}
            //this.currentGroup = switchableButtonGroup;
            this.dealFingerOnHover();
        }
    }

    public void dealFingerOnHover()
    {
        //if (this.currentGroup.finger == null)
        //{
        //    this.currentGroup.finger = this.currentGroup.root.transform.Find("finger");
        //}
        this.updateFingerState();
    }

    //protected UIEventListenerCustom GetNextButton(SwitchableButtonGroup group, int keyCode)
    //{
    //    switch (keyCode)
    //    {
    //        case 3:
    //            return group.up;
    //        case 4:
    //            return group.down;
    //        case 5:
    //            return group.left;
    //        case 6:
    //            return group.right;
    //        case 18:
    //            return group.up;
    //        case 19:
    //            return group.down;
    //        case 20:
    //            return group.left;
    //        case 21:
    //            return group.right;
    //        default:
    //            return group.root;
    //    }
    //}

    //protected SwitchableButtonGroup GetNextSpareButton(SwitchableButtonGroup group, int keyCode)
    //{
    //    List<UIEventListenerCustom> list = null;
    //    switch (keyCode)
    //    {
    //        case 3:
    //            break;
    //        case 4:
    //            goto IL_44;
    //        case 5:
    //            goto IL_50;
    //        case 6:
    //            goto IL_5C;
    //        default:
    //            switch (keyCode)
    //            {
    //                case 18:
    //                    break;
    //                case 19:
    //                    goto IL_44;
    //                case 20:
    //                    goto IL_50;
    //                case 21:
    //                    goto IL_5C;
    //                default:
    //                    goto IL_6D;
    //            }
    //            break;
    //    }
    //    list = group.spareUpList;
    //    goto IL_6D;
    //IL_44:
    //    list = group.spareDownList;
    //    goto IL_6D;
    //IL_50:
    //    list = group.spareLeftList;
    //    goto IL_6D;
    //IL_5C:
    //    list = group.spareRightList;
    //IL_6D:
    //    if (list == null)
    //    {
    //        return null;
    //    }
    //    SwitchableButtonGroup switchableButtonGroup = null;
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        if (list[i] != null)
    //        {
    //            this.buttonMaps.TryGetValue(list[i], out switchableButtonGroup);
    //            if (switchableButtonGroup != null && switchableButtonGroup.root.gameObject.activeInHierarchy && switchableButtonGroup.root.ButtonEnable)
    //            {
    //                return switchableButtonGroup;
    //            }
    //        }
    //    }
    //    return null;
    //}

    private void updateFingerState()
    {
        //if (this.currentGroup == null)
        //{
        //    return;
        //}
        if (GameKey.IsJoyPadHintMode)
        {
            //if (this.currentGroup.finger != null)
            //{
            //    if (this._finger == null)
            //    {
            //        this._finger = GameObjectPoolManager.GetInstance().Pop("UI/img_finger", "UI/img_finger", false);
            //    }
            //    this._finger.SafeActive(true);
            //    this._finger.GetComponent<UIKeepLocation>().target = this.currentGroup.finger;
            //    this._finger.transform.parent = this.transform;
            //    this._finger.GetComponent<UIPanel>().depth = this.depthDependentPanel.depth + 5;
            //    this._finger.transform.localPosition = Vector3.zero;
            //    this._finger.transform.localScale = Vector3.one;
            //    this._finger.GetComponentInChildren<UITexture>().MarkAsChanged();
            //}
            //else if (this._finger != null)
            //{
            //    this._finger.SafeActive(false);
            //}
        }
        else if (this._finger != null)
        {
            this._finger.SafeActive(false);
        }
    }

    public void onMessage(int notificationType, params object[] param)
    {
        if (notificationType == 10216)
        {
            this.updateFingerState();
        }
    }

    public void onRemoveNotify(int notificationType)
    {
    }

    //private List<SwitchableButtonGroup> buttonGroup = new List<SwitchableButtonGroup>();

    //private List<UIEventListenerCustom> buttonList = new List<UIEventListenerCustom>();

    //private Dictionary<UIEventListenerCustom, SwitchableButtonGroup> buttonMaps = new Dictionary<UIEventListenerCustom, SwitchableButtonGroup>();

    //protected SwitchableButtonGroup countGroup;

    //protected SwitchableButtonGroup currentGroup;

    protected bool _isInited;

    protected int _lastKeyCode = -1;

    protected GameObject _finger;
    protected Transform transform;

    protected GameObject depthDependentPanel;

    private const int KeyRepeatTime = 5;
}
