using DG.Tweening;
using lib.notify;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client
{
    public class PopDialogManager : PopManager, ICallback
    {
        protected PopDialog _menu;
        protected int _dialogGroupId;
        protected GameObject _window;
        private bool _MenuLoaded;
        private string context = string.Empty;
        private GameObject bgObj;
        private int showCount;
        private int delay;
        private int delayCount;
        private List<PlaceHolder> placeholders = new List<PlaceHolder>();

        private int textRoleUpCount;
        private SHAKE_TYPE shakeType;
        private int shakeCount;
        private Vector3 cameraInitPos;
        private Vector3 dialogInitPos;
        private const int SKIP_OUT_TIME = 48;
        private const float SKIP_OUT_SPEED_DEFAULT = 0.020833334f;
        private int skipOutCount;
        private float skipOutSpeed;
        private bool isDialogPressed;
        private int dialogPressCount;
        private int textSpeed = 1;
        private static bool IsAutoMode = false;
        private int autoModeCount;
        private static float DialogCloseTime = 0f;
        private List<int> voicesInDialog = new List<int>();
        private List<int> sfxInDialog = new List<int>();
        private bool _isUsedKeyboard;

        private static int[][] BODY_X = new int[][]
{
            new int[]
            {
                -531,
                1
            },
            new int[]
            {
                531,
                -1
            },
            new int[]
            {
                0,
                -1
            }
};

        private enum SHAKE_TYPE
        {
            NONE,
            DIALOG,
            UI
        }

        public override void DestroyUI(object arg)
        {
            //this._menu.dialog_panel.alpha = 0.1f;
            //UIKeyAndJoypadController component = this._window.GetComponent<UIKeyAndJoypadController>();
            //if (component != null)
            //{
            //    component.Clear();
            //}
            if (this._menu != null)
            {
                //原本代码没有，但是我加上去的
                Destroy(_menu.gameObject);

                //this._menu.Close();
                this._menu = null;
            }
            Notifier.Remove(103, this);
            Notifier.Remove(104, this);

            Destroy(base.gameObject);
        }

        public override void OpenUI(object arg)
        {
            base.OpenUI(arg);
            if (arg != null)
            {
                List<object> list = (List<object>)arg;
                if (list[0] != null)
                {
                    this._dialogGroupId = (int)list[0];
                }
            }
            string[] uiName = new string[]
            {
                "PopDialog"
            };
            OnInstantiateOver[] onInstOvers = new OnInstantiateOver[]
            {
                new OnInstantiateOver(this.OnInstantiated)
            };
            UIManager.GetInstance().OpenUI(uiName, new OnOpenComplete(this.OnOpenCompleted), onInstOvers, this.anchor, true);
        }

        public override void ReopenUI(object arg)
        {
            if (arg != null)
            {
                List<object> list = (List<object>)arg;
                if (list[0] != null)
                {
                    this._dialogGroupId = (int)list[0];
                }
            }
            if (this._window != null)
            {
                this._window.SafeActive(true);
            }
            base.ReopenUI(arg);
            this.InitData();
        }

        public override void HideUI(object arg)
        {
            if (this._window != null)
            {
                this._window.SafeActive(false);
            }
            base.HideUI(arg);
        }

        private void OnInstantiated(GameObject window)
        {
            this._window = window;
            if (this._window != null)
            {
                this._menu = this._window.GetComponent<PopDialog>();
                this._menu.RegistOnCloseDialog(new Action(this.OnCloseDialog));
                Notifier.Register(103, this);
                Notifier.Register(104, this);
                this._MenuLoaded = true;
                this.InitData();
            }
        }

        private void OnOpenCompleted(GameObject go)
        {
            //UIKeyAndJoypadController component = this._window.GetComponent<UIKeyAndJoypadController>();
            //if (component != null)
            //{
            //    component.InitButtonsMaps(true);
            //}
        }

        private void InitData()
        {
            if (this._menu != null)
            {
                //this._isUsedKeyboard = (GameKey.style == KeyStyle.KEYBOARD || !GameKey.IsJoyPadHintMode);
                //this._menu.touchArea += clickDialog;
                //TODO:这一块这么弄就没有连点和键盘了，之后优化
                this._menu.touchArea.onClick.AddListener(() => clickDialog(false));
                this._menu.skipButton.onClick.AddListener(this.skipDialog);
                this._menu.logButton.onClick.AddListener(this.showLog);
                this._menu.autoButton.onClick.AddListener(this.switchAutoMode);
                this.shakeType = SHAKE_TYPE.NONE;
                this.shakeCount = 0;
                //this.cameraInitPos = new Vector3(UICamera.mainCamera.transform.localPosition.x, UICamera.mainCamera.transform.localPosition.y, UICamera.mainCamera.transform.localPosition.z);
                this.dialogInitPos = new Vector3(this._menu.root_dialog.transform.localPosition.x, this._menu.root_dialog.transform.localPosition.y, this._menu.root_dialog.transform.localPosition.z);
                this.skipOutCount = 0;
                this.skipOutSpeed = 0f;
                this.isDialogPressed = false;
                this.dialogPressCount = 0;
                this.textSpeed = 1;
                if (Time.time - DialogCloseTime > 5f)
                {
                    this.setAutoMode(false);
                }
                else
                {
                    this.setAutoMode(IsAutoMode);
                }
                DialogSegment curSegment = DialogManager.GetInstance().segmentsData[this._dialogGroupId];
                DialogManager.GetInstance().curSegment = curSegment;
                DialogManager.GetInstance().curDialogIndex = 0;
                Notifier.Notify(10104, new object[0]);
                this.startDialog();
            }
        }

        public void startDialog()
        {
            //this._menu.dialog_panel.alpha = 1f;
            DialogManager instance = DialogManager.GetInstance();
            DialogSegment segment = instance.curSegment;
            DialogData dialog = segment.dialogsData[DialogManager.GetInstance().curDialogIndex];
            for (int i = 0; i < this._menu.bg.transform.childCount; i++)
            {
                GameObject gameObject = this._menu.bg.transform.GetChild(i).gameObject;
                Destroy(gameObject);
            }
            if (!string.IsNullOrEmpty(segment.bgFilename))
            {
                //AssetBundleLoader.getInstance().loadAssetBundle("bg", delegate (AssetBundle bundle)
                //{
                //    GameObject original = bundle.LoadAsset<GameObject>(segment.bgFilename);
                //    this.bgObj = Instantiate<GameObject>(original);
                //    this.bgObj.transform.parent = this._menu.bg.transform;
                //    this.bgObj.transform.localPosition = Vector3.zero;
                //    this.bgObj.transform.localScale = Vector3.one;
                //}, false, ".assetbundle");

                Texture2D original = Resources.Load<Texture2D>("BG/" + segment.bgFilename);
                this.bgObj = new GameObject("BG");
                this.bgObj.transform.parent = this._menu.bg.transform;
                this.bgObj.AddComponent<Image>().sprite = Sprite.Create(original, new Rect(0, 0, original.width, original.height), Vector2.one * 0.5f);
                this.bgObj.GetComponent<Image>().SetNativeSize();
                this.bgObj.transform.localPosition = Vector3.zero;
                this.bgObj.transform.localScale = Vector3.one;
            }
            if (segment.bgmSoundId > 0)
            {
                //GameLogic.PlaySound(segment.bgmSoundId, true, false, false, null, null, false);
            }
            this.sfxInDialog.Clear();
            this.show(dialog);
        }

        public void clickDialog(bool state)
        {
            if (state)
            {
                if (!this.isDialogPressed)
                {
                    this.dialogPressCount = 0;
                    this.autoModeCount = 0;
                }
                this.isDialogPressed = true;
            }
            else
            {
                this.isDialogPressed = false;
                if (this.dialogPressCount < 30)
                {
                    this.doClick();
                    this.dialogPressCount = 0;
                }
            }
        }

        public void skipDialog()
        {
            if (this.skipOutCount > 0)
            {
                return;
            }
            //GameLogic.PlaySound(104, false, false, false, null, null, false);
            this.skipOutCount = 48;
            this.skipOutSpeed = 0.020833334f;
        }

        public void showLog()
        {
            //GameLogic.PlaySound(104, false, false, false, null, null, false);
            //MenuHelper.PopDialogLog();
        }

        public void switchAutoMode()
        {
            //GameLogic.PlaySound(104, false, false, false, null, null, false);
            if (this.skipOutCount > 0)
            {
                return;
            }
            this.setAutoMode(!PopDialogManager.IsAutoMode);
        }

        private void setAutoMode(bool isAuto)
        {
            //this.autoModeCount = 0;
            //PopDialogManager.IsAutoMode = isAuto;
            //this._menu.autoButton.SetButtonEnable(true, true);
            //if (PopDialogManager.IsAutoMode)
            //{
            //    this._menu.autoButton.GetComponent<UISprite>().spriteName = "dhk_24";
            //    this._menu.autoButton.OnHoverSpriteName = "dhk_24";
            //    this._menu.autoButton.OnNormalSpriteName = "dhk_24";
            //    this._menu.autoButton.OnDiableSpriteName = "dhk_24";
            //    this._menu.autoButton.transform.GetComponentInChildren<UILabel>().text = Singleton<LanguageHelper>.Instance.GetStaticString("UI_DIALOG_AUTO_PAGING");
            //    this._menu.autoButton.transform.Find("imgFlag").gameObject.SetActive(false);
            //}
            //else
            //{
            //    this._menu.autoButton.GetComponent<UISprite>().spriteName = "dhk_22";
            //    this._menu.autoButton.OnHoverSpriteName = "dhk_22";
            //    this._menu.autoButton.OnNormalSpriteName = "dhk_23";
            //    this._menu.autoButton.OnDiableSpriteName = "dhk_23";
            //    this._menu.autoButton.transform.GetComponentInChildren<UILabel>().text = Singleton<LanguageHelper>.Instance.GetStaticString("UI_DIALOG_AUTO_PAGE");
            //    this._menu.autoButton.transform.Find("imgFlag").gameObject.SetActive(true);
            //}
        }

        private void doClick()
        {
            if (this.textRoleUpCount > 0)
            {
                return;
            }
            for (int i = 0; i < this._menu.characters.Length; i++)
            {
                if (this._menu.characters[i].inoutState != CharacterIllustration.INOUT_STATE.NONE)
                {
                    return;
                }
            }
            DialogManager instance = DialogManager.GetInstance();
            if (instance.curSegment == null)
            {
                return;
            }
            if (this.showCount < this.context.Length)
            {
                this.textShowAll();
            }
            else
            {
                //GameLogic.PlaySound(100, false, false, false, null, null, false);
                this.textRollOut();
            }
        }

        private void textShowAll()
        {
            this.delay = 0;
            this._menu.lblText.text = this.context;
            this._menu.nextIcon.gameObject.SafeActive(true);
            this.showCount = this.context.Length;
        }

        private void textRollOut()
        {
            this.textRoleUpCount = 20;
        }

        public void onMessage(int notificationType, params object[] param)
        {
            if (notificationType != 103)
            {
                if (notificationType == 104)
                {
                    this.doNextDialog();
                }
            }
            else
            {
                DialogManager instance = DialogManager.GetInstance();
                DialogSegment curSegment = instance.curSegment;
                DialogData dialogData = curSegment.dialogsData[instance.curDialogIndex];
                this.formatText(string.Empty + dialogData.text);
                if (string.IsNullOrEmpty(string.Empty + dialogData.name))
                {
                    this._menu.nameBgLeft.gameObject.SafeActive(false);
                    this._menu.nameBgRight.gameObject.SafeActive(false);
                }
                else
                {
                    this._menu.nameBgLeft.gameObject.SafeActive(true);
                    this._menu.nameBgRight.gameObject.SafeActive(false);
                    this._menu.lblNameLeft.text = string.Empty + dialogData.name;
                }
            }
        }

        public void onRemoveNotify(int notificationType)
        {
        }

        private void doNextDialog()
        {
            DialogManager instance = DialogManager.GetInstance();
            DialogSegment curSegment = instance.curSegment;
            if (instance.curDialogIndex < curSegment.dialogsData.Count - 1)
            {
                instance.curDialogIndex++;
                DialogData dialog = curSegment.dialogsData[instance.curDialogIndex];
                this.show(dialog);
            }
            else
            {
                instance.curSegment = null;
                if (curSegment.keepOpen)
                {
                    Notifier.Notify(10105, new object[]
                    {
                        this._dialogGroupId
                    });
                }
                else
                {
                    this.hide();
                }
            }
        }

        public override bool DealKeyEvent(int keyCode, int keyState)
        {
            UIKeyAndJoypadController component = this._window.GetComponent<UIKeyAndJoypadController>();
            if (component != null && component.DealKeyEvent(keyCode, keyState))
            {
                return true;
            }
            //左键
            if (keyCode == 2001)
            {
                return true;
            }
            //回车同时按下或者持续按下
            if (keyCode == 1 && (keyState == 1 || keyState == 3))
            {
                this.clickDialog(true);
                return true;
            }
            //回车同时释放
            if (keyCode == 1 && keyState == 2)
            {
                this.clickDialog(false);
                return true;
            }
            if ((keyCode == 2 && keyState == 3) || (keyCode == 2002 & keyState == 1))
            {
                //this._menu.dialog_panel.alpha = 0.004f;
                return true;
            }
            if ((keyCode == 2 && keyState == 2) || (keyCode == 2002 & keyState == 2))
            {
                //this._menu.dialog_panel.alpha = 1f;
                return true;
            }
            if (keyCode == 11 && keyState == 2)
            {
                if (DialogManager.GetInstance().curSegment.isCanSkip)
                {
                    this.skipDialog();
                }
                return true;
            }
            if (keyCode == 9 && keyState == 2)
            {
                this.showLog();
                return true;
            }
            if (keyCode == 8 && keyState == 2)
            {
                this.switchAutoMode();
                return true;
            }
            return base.DealKeyEvent(keyCode, keyState);
        }

        public void show(DialogData dialog)
        {
            this._menu.root_dialog.SafeActive(true);
            //this._menu.root.alpha = 1f;
            this.skipOutCount = 0;
            this.skipOutSpeed = 0f;
            this._menu.nextIcon.gameObject.SafeActive(false);
            this.textRoleUpCount = 0;
            this._menu.lblText.transform.localPosition = Vector3.zero;
            if (MultiLanguage.Current == LANGUAGE.EN)
            {
                this._menu.lblText.fontSize = 29;
                //this._menu.lblText.width = 920;
                this._menu.lblNameLeft.fontSize = 30;
                //this._menu.lblNameLeft.width = 410;
                //this._menu.nameBgLeft.width = 430;
            }
            else
            {
                this._menu.lblText.fontSize = 32;
                //this._menu.lblText.width = 865;
                this._menu.lblNameLeft.fontSize = 32;
                //this._menu.lblNameLeft.width = 260;
                //this._menu.nameBgLeft.width = 290;
            }
            this._menu.skipButton.gameObject.SafeActive(DialogManager.GetInstance().curSegment.isCanSkip);
            DialogHeadData dialogHeadData = DialogManager.GetInstance().headsData[dialog.head];
            //num在0到bodyX的长度之间，这里是0-3
            int num = Mathf.Max(0, Mathf.Min(BODY_X.Length - 1, dialog.pos));
            int[] array = BODY_X[num];
            //这个应该是是否反转的
            bool flag = array[1] == -1;
            Vector3 pos = new Vector3((float)(array[0] + ((!flag) ? (-dialogHeadData.bodyX) : dialogHeadData.bodyX)), (float)dialogHeadData.bodyY, 0f);
            CharacterIllustration characterIllustration = this._menu.characters[num];
            characterIllustration.setData(dialogHeadData.bodyImgFilename, pos, array[1], true, dialogHeadData.faceImgFilename, dialogHeadData.faceX, dialogHeadData.faceY);
            switch (dialog.enterType)
            {
                case 2:
                    {
                        //int num2 = UIRoot.list[0].manualWidth / 2;
                        //float num3 = (float)(-(float)num2 - ((!flag) ? characterIllustration.bodyImg.width : 0));
                        characterIllustration.inoutState = CharacterIllustration.INOUT_STATE.MOVE_IN_LEFT;
                        //characterIllustration.offsetX = num3 - pos.x;
                        //characterIllustration.transform.localPosition = new Vector3(num3, pos.y, pos.z);
                        return;
                    }
                case 3:
                    {
                        //int num4 = UIRoot.list[0].manualWidth / 2;
                        //float num5 = (float)(num4 + ((!flag) ? 0 : characterIllustration.bodyImg.width));
                        characterIllustration.inoutState = CharacterIllustration.INOUT_STATE.MOVE_IN_RIGHT;
                        //characterIllustration.offsetX = num5 - pos.x;
                        //characterIllustration.transform.localPosition = new Vector3(num5, pos.y, pos.z);
                        return;
                    }
                case 4:
                    characterIllustration.bodyImg.color = new Color(1f, 1f, 1f, 0f);
                    characterIllustration.inoutState = CharacterIllustration.INOUT_STATE.FADE_IN;
                    return;
            }
            characterIllustration.inoutState = CharacterIllustration.INOUT_STATE.NONE;
            Notifier.Notify(103, new object[0]);
        }

        public void hide()
        {
            this.hideView();
            this.OnCloseDialog();
        }

        private void hideView()
        {
            if (this._menu != null)
            {
                for (int i = 0; i < this._menu.characters.Length; i++)
                {
                    this._menu.characters[i].gameObject.SafeActive(false);
                }
                this.context = string.Empty;
                this._menu.lblText.text = string.Empty;
                this._menu.nameBgLeft.gameObject.SafeActive(false);
                this._menu.nameBgRight.gameObject.SafeActive(false);
                this._menu.nextIcon.gameObject.SafeActive(false);
                this._menu.root_dialog.SafeActive(false);

                Destroy(this._menu.bg.transform.GetChild(0).gameObject);
            }
            for (int j = 0; j < this.sfxInDialog.Count; j++)
            {
                //GameLogic.StopSound(this.sfxInDialog[j], null);
            }
            this.sfxInDialog.Clear();
        }

        private void OnCloseDialog()
        {
            if (this._menu != null)
            {
                this._menu.root_dialog.transform.localPosition = this.dialogInitPos;
            }
            //UICamera.mainCamera.transform.localPosition = this.cameraInitPos;
            //PlayerData.AddDialogLog(new DialogLogData(DialogLogData.DIALOG_LOG_TYPE.DIALOG_SEGMENT, this._dialogGroupId));
            DialogCloseTime = Time.time;
            this.CloseUI(null);
            Notifier.Notify(10105, new object[]
            {
                this._dialogGroupId
            });
        }

        private void formatText(string text)
        {
            //TODO:回去找
            this.delayCount = 0;
            this.showCount = 0;
            this.placeholders.Clear();
            this.voicesInDialog.Clear();
            this.context = string.Empty;
            context = text;
        }

        private void setDelay(int delayFrame)
        {
            if (MultiLanguage.Current == LANGUAGE.EN)
            {
                this.delay = Mathf.CeilToInt((float)delayFrame / 2f);
            }
            else
            {
                this.delay = delayFrame;
            }
        }

        private void tagSoundInDialog(int soundId)
        {
            //if (ScriptControlDataManager.GetInstance().soundsData.ContainsKey(soundId))
            //{
            //    SoundInfoData soundInfoData = ScriptControlDataManager.GetInstance().soundsData[soundId];
            //    if (soundInfoData.type == 2)
            //    {
            //        this.voicesInDialog.Add(soundId);
            //    }
            //    else if (soundInfoData.type == 1)
            //    {
            //        this.sfxInDialog.Add(soundId);
            //    }
            //}
        }

        private void Update()
        {
            //if (PopDialogLogManager.IsVisible)
            //{
            //    return;
            //}
            if (this._MenuLoaded)
            {
                this.textSpeed = 1;
                if (this.isDialogPressed)
                {
                    this.dialogPressCount++;
                    if (this.dialogPressCount >= 30)
                    {
                        this.textSpeed = 3;
                    }
                }
                if (this.skipOutCount > 0)
                {
                    if (this.skipOutCount == 48)
                    {
                        //this.skipOutSpeed = this._menu.root.alpha / (float)(this.skipOutCount * this.textSpeed);
                    }
                    //this._menu.root.alpha = this._menu.root.alpha - this.skipOutSpeed;
                    this.skipOutCount -= this.textSpeed;
                    if (this.skipOutCount <= 0)
                    {
                        this.hide();
                    }
                    return;
                }
                if (this.textRoleUpCount > 0)
                {
                    this._menu.lblText.transform.localPosition = new Vector3(0f, 113f - (float)this.textRoleUpCount * 5f, 0f);
                    this.textRoleUpCount -= this.textSpeed;
                    if (this.textRoleUpCount <= 0)
                    {
                        this.textRoleUpCount = 0;
                        for (int i = 0; i < this.voicesInDialog.Count; i++)
                        {
                            //GameLogic.StopSound(this.voicesInDialog[i], null);
                        }
                        this.voicesInDialog.Clear();
                        this.nextDialog();
                    }
                    return;
                }
                if (this.showCount < this.context.Length)
                {
                    if (this.delay == 0)
                    {
                        this.textShowAll();
                    }
                    else
                    {
                        if (this.delayCount > 0)
                        {
                            this.delayCount -= this.textSpeed;
                        }
                        if (this.delayCount <= 0)
                        {
                            string empty = string.Empty;
                            for (int j = 0; j < this.placeholders.Count; j++)
                            {
                                PopDialogManager.PlaceHolder placeHolder = this.placeholders[j];
                                if (placeHolder.startOpenerIndex == this.showCount)
                                {
                                    this.showCount = placeHolder.startCloserIndex;
                                    switch (placeHolder.type)
                                    {
                                        case PopDialogManager.PlaceHolder.TYPE.Sound:
                                            //GameLogic.PlaySound(placeHolder.param, false, false, false, null, null, false);
                                            this.tagSoundInDialog(placeHolder.param);
                                            break;
                                        case PopDialogManager.PlaceHolder.TYPE.Delay:
                                            this.setDelay(placeHolder.param);
                                            break;
                                        case PopDialogManager.PlaceHolder.TYPE.LoopSound:
                                            //GameLogic.PlaySound(placeHolder.param, true, false, false, null, null, false);
                                            this.tagSoundInDialog(placeHolder.param);
                                            break;
                                        case PopDialogManager.PlaceHolder.TYPE.StopSound:
                                            //GameLogic.StopSound(placeHolder.param, null);
                                            break;
                                        case PopDialogManager.PlaceHolder.TYPE.Music:
                                            //GameLogic.PlaySound(placeHolder.param, true, false, false, null, null, false);
                                            break;
                                        case PopDialogManager.PlaceHolder.TYPE.ShakeDialog:
                                            this.shakeType = PopDialogManager.SHAKE_TYPE.DIALOG;
                                            this.shakeCount = placeHolder.param;
                                            break;
                                        case PopDialogManager.PlaceHolder.TYPE.ShakeUI:
                                            this.shakeType = PopDialogManager.SHAKE_TYPE.UI;
                                            this.shakeCount = placeHolder.param;
                                            break;
                                    }
                                }
                                else if (placeHolder.endOpenerIndex == this.showCount)
                                {
                                    this.showCount = placeHolder.endCloserIndex;
                                }
                            }
                            Debug.Log(1231231);
                            this._menu.lblText.text = this.context.Substring(0, Mathf.Min(this.showCount + 1, this.context.Length)) + empty;
                            int num = this.showCount + 1;
                            if (this.showCount >= 0 && num < this.context.Length)
                            {
                                char c = this.context[this.showCount];
                                char c2 = this.context[num];
                                //this._menu.lblText.UpdateNGUIText();
                                //string text = this._menu.lblText.processedText;
                                bool flag = false;
                                int k = this.showCount;
                                string str = string.Empty + c;
                                //if (MultiLanguage.Current == LANGUAGE.EN)
                                //{
                                //    if (!NGUIText.IsSpace((int)c) && !NGUIText.IsSpace((int)c2))
                                //    {
                                //        flag = true;
                                //        for (int l = num; l < this.context.Length; l++)
                                //        {
                                //            char c3 = this.context[l];
                                //            if (NGUIText.IsSpace((int)c3))
                                //            {
                                //                break;
                                //            }
                                //            text += c3;
                                //        }
                                //        for (k = this.showCount; k > 0; k--)
                                //        {
                                //            char ch = this.context[k - 1];
                                //            if (NGUIText.IsSpace((int)ch))
                                //            {
                                //                break;
                                //            }
                                //        }
                                //        str = this.context.Substring(k, this.showCount + 1 - k);
                                //    }
                                //}
                                //else if (!NGUIText.IsSpace((int)c) && NGUITools.InvalidHeadSym.Contains(c2))
                                //{
                                //    flag = true;
                                //    text += c2;
                                //}
                                if (flag)
                                {
                                    //float x = NGUIText.CalculatePrintedSize(text).x;
                                    //if (x > (float)this._menu.lblText.width)
                                    //{
                                    //    this._menu.lblText.text = this.context.Substring(0, k) + "\n" + str + empty;
                                    //}
                                }
                            }
                            this.showCount++;
                            this.delayCount = this.delay;
                            if (this.showCount >= this.context.Length)
                            {
                                this._menu.nextIcon.gameObject.SafeActive(true);
                            }
                        }
                    }
                }
                else
                {
                    bool flag2 = false;
                    for (int m = 0; m < this._menu.characters.Length; m++)
                    {
                        if (this._menu.characters[m].inoutState != CharacterIllustration.INOUT_STATE.NONE)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        if (this.textSpeed > 1)
                        {
                            this.autoModeCount++;
                            if (this.autoModeCount >= 15)
                            {
                                this.autoModeCount = 0;
                                this.doClick();
                            }
                        }
                        else if (PopDialogManager.IsAutoMode)
                        {
                            this.autoModeCount++;
                            if (this.autoModeCount >= 90)
                            {
                                this.autoModeCount = 0;
                                this.doClick();
                            }
                        }
                    }
                }
                if (this.shakeCount > 0)
                {
                    //int num2 = this.shakeCount % GCamera.SHAKE_OFFSET[0].Length;
                    //float num3 = GCamera.SHAKE_OFFSET[0][num2];
                    //float num4 = GCamera.SHAKE_OFFSET[1][num2];
                    PopDialogManager.SHAKE_TYPE shake_TYPE = this.shakeType;
                    if (shake_TYPE != PopDialogManager.SHAKE_TYPE.DIALOG)
                    {
                        if (shake_TYPE == PopDialogManager.SHAKE_TYPE.UI)
                        {
                            //UICamera.mainCamera.transform.localPosition = new Vector3(this.cameraInitPos.x + num3 * 100f, this.cameraInitPos.y + num4 * 100f, this.cameraInitPos.z);
                        }
                    }
                    else
                    {
                        //this._menu.root_dialog.transform.localPosition = new Vector3(this.dialogInitPos.x + num3 * 100f, this.dialogInitPos.y + num4 * 100f, this.dialogInitPos.z);
                    }
                    this.shakeCount--;
                } 
                else
                {
                    this._menu.root_dialog.transform.localPosition = this.dialogInitPos;
                    //UICamera.mainCamera.transform.localPosition = this.cameraInitPos;
                }
            }
        }

        public void nextDialog()
        {
            this._menu.lblText.text = string.Empty;
            this._menu.nameBgLeft.gameObject.SafeActive(false);
            this._menu.nameBgRight.gameObject.SafeActive(false);
            DialogManager instance = DialogManager.GetInstance();
            DialogSegment curSegment = instance.curSegment;
            DialogData dialogData = curSegment.dialogsData[instance.curDialogIndex];
            CharacterIllustration characterIllustration = this._menu.characters[dialogData.pos];
            Vector3 localPosition = characterIllustration.transform.localPosition;
            bool flag = characterIllustration.transform.localScale.x < 0f;
            switch (dialogData.exitType)
            {
                case 1:
                    Color color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
                    characterIllustration.bodyImg.color = color;
                    characterIllustration.faceImg.color = color;
                    Notifier.Notify(104, new object[0]);
                    return;
                case 2:
                    //int num = UIRoot.list[0].manualWidth / 2;
                    //float num2 = (float)(-(float)num - ((!flag) ? characterIllustration.bodyImg.width : 0));
                    characterIllustration.inoutState = CharacterIllustration.INOUT_STATE.MOVE_OUT_LEFT;
                    //characterIllustration.offsetX = localPosition.x - num2;
                    return;
                case 3:
                    //int num3 = UIRoot.list[0].manualWidth / 2;
                    //float num4 = (float)(num3 + ((!flag) ? 0 : characterIllustration.bodyImg.width));
                    characterIllustration.inoutState = CharacterIllustration.INOUT_STATE.MOVE_OUT_RIGHT;
                    //characterIllustration.offsetX = localPosition.x - num4;
                    return;
                case 4:
                    characterIllustration.inoutState = CharacterIllustration.INOUT_STATE.FADE_OUT;
                    return;
                case 5:
                    this._menu.characters[dialogData.pos].gameObject.SafeActive(false);
                    Notifier.Notify(104, new object[0]);
                    return;
            }
            Notifier.Notify(104, new object[0]);
        }

        public class PlaceHolder
        {
            public TYPE type;
            public int param;
            public int startOpenerIndex = -1;
            public int startCloserIndex = -1;
            public int endOpenerIndex = -1;
            public int endCloserIndex = -1;

            public PlaceHolder(TYPE type)
            {
                this.type = type;
                this.param = 0;
                this.startOpenerIndex = -1;
                this.startCloserIndex = -1;
                this.endOpenerIndex = -1;
                this.endCloserIndex = -1;
            }

            public enum TYPE
            {
                None,
                Color,
                Border,
                Italic,
                Underline,
                Strike,
                Sup,
                Sub,
                Emoji,
                Sound,
                Delay,
                LoopSound,
                StopSound,
                Music,
                ShakeDialog,
                ShakeUI
            }
        }
    }
}