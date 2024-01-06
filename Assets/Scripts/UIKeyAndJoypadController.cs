using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class UIKeyAndJoypadController : MonoBehaviour
{
    private void Awake()
    {
        if (this._logic == null)
        {
            this._logic = new UIKeyAndJoypadLogic();
            //this._logic.SetRoot(base.transform);
            //this._logic.SetButtonList(this.buttonList);
            //this._logic.SetButtonGroup(this.buttonGroup);
        }
    }

    private void OnEnable()
    {
        this._logic.SetEnable();
    }

    private void OnDisable()
    {
        this._logic.SetDisable();
    }

    public void Clear()
    {
        this._logic.Clear();
    }

    public void InitButtonsMaps(bool needFocus = true)
    {
        this._logic.InitButtonsMaps(needFocus);
    }

    public void GoToDefaultGroup(bool isMouse = false, bool needHoverFocus = true)
    {
        this._logic.GoToDefaultGroup(isMouse, needHoverFocus);
    }

    //public void GoToGroup(UIEventListenerCustom buttonEvent, bool isMouse = false, bool needHoverFocus = true)
    //{
    //    this._logic.GoToGroup(buttonEvent, isMouse, needHoverFocus);
    //}

    public void ShowFinger(bool isShow)
    {
        this._logic.ShowFinger(isShow);
    }

    public void CheckFingerDepth()
    {
        this._logic.CheckFingerDepth();
    }

    //public void SetFingerDepthDependentPanel(UIPanel panel)
    //{
    //    this._logic.SetFingerDepthDependentPanel(panel);
    //    this._logic.SetEnable();
    //}

    public bool DealKeyEvent(int keyCode, int keyState)
    {
        return this._logic.DealKeyEvent(keyCode, keyState);
    }

    public void OnItemHover(GameObject gameobject, bool isHover)
    {
        this._logic.OnItemHover(gameobject, isHover);
    }

    //public SwitchableButtonGroup getCurrentGroup()
    //{
    //    return this._logic.getCurrentGroup();
    //}

    //[SerializeField]
    //public List<SwitchableButtonGroup> buttonGroup;

    //[SerializeField]
    //public List<UIEventListenerCustom> buttonList;

    protected const string LOGICNAME = "UIKeyAndJoypadLogic";

    protected Dictionary<string, MethodInfo> _methodList = new Dictionary<string, MethodInfo>();

    protected UIKeyAndJoypadLogic _logic;

    protected Type _logicType;
}
