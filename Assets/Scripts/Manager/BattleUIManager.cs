using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : SingletonMono<BattleUIManager>
{
    public GameObject actionPanel;
    public GameObject Panel_Attack;
    public Button attackBtn;
    public Button cancelAttackBtn;

    private SpawnPool spawnPool;
    private CanvasRenderer hudCanvas;

    [HideInInspector] public Button auto_AI;
    private GameObject skillSelectTarget;
    private GameObject skillConfirm;
    private Animator orderTip;

    private Text auto_AI_Text;
    private Button settingBtn;
    //名字，武器名字，HP，攻击，命中，必杀
    private Transform battleUI;
    private Text[] playBattleUI = new Text[4];
    private Text[] targetBattleUI = new Text[4];
    private Image playerPicture;
    private Image targetPicture;

    public Transform battleReadyUI;
    private Transform selectUnitPanel;
    private GameObject characterFace;

    private GameObject bag;
    public GameObject infoUI;

    private Transform battleEndPanel;

    public ActionOrder actionOrder;

    Dictionary<string, GameObject> faceDic = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        InitializeUIComponents();
        InitializeEventListeners();
        InitializeObjectPool();
    }

    private void InitializeUIComponents()
    {
        hudCanvas = transform.Find("hudCanvas").GetComponent<CanvasRenderer>();
        infoUI = transform.Find("InfoUI").gameObject;
        actionPanel = transform.Find("ActionUI/ActionPanel").gameObject;
        // ... 初始化其他UI组件 ...
    }

    private void InitializeEventListeners()
    {
        EventDispatcher.instance.Regist<int, Vector3>(GameEventType.showHudDamage, this.ShowHudDamage);
        // ... 添加其他事件监听 ...
    }

    private void InitializeObjectPool()
    {
        spawnPool = hudCanvas.gameObject.AddComponent<SpawnPool>();
        CreateObjectPool("ui/hudItem");
        CreateObjectPool("ui/hudText_green");
        // ... 创建其他对象池 ...
    }

    private void CreateObjectPool(string resourcePath)
    {
        var prefab = ResourcesExt.Load<GameObject>(resourcePath);
        var prefabPool = new PrefabPool(prefab.transform);
        spawnPool.CreatePrefabPool(prefabPool);
    }

    private void Start()
    {
        infoUI = transform.Find("InfoUI").gameObject;

        actionPanel = this.transform.Find("ActionUI/ActionPanel").gameObject;
        var waitBtn = actionPanel.transform.Find("wait").GetComponent<Button>();
        waitBtn.onClick.AddListener(BattleManager.Instance.Wait);
        actionPanel.SetActive(false);

        attackBtn.onClick.AddListener(() => BattleManager.Instance.Attack(BattleManager.Instance.nowCharacter));
        cancelAttackBtn.onClick.AddListener(CloseAttackUI);

        bag = transform.Find("Bag").gameObject;
        //this.transform.Find("ActionUI/BagBtn").GetComponent<Button>().onClick.AddListener(OpenBag);

        this.skillSelectTarget = this.transform.Find("ActionUI/skillSelectTarget").gameObject;

        this.skillConfirm = this.transform.Find("ActionUI/skillConfirm").gameObject;
        //千万记住，此时按钮的拖拽赋值要去掉，不然一次触发两次函数，debug都不知道怎么弄
        this.skillConfirm.transform.Find("confirm").GetComponent<Button>().onClick.AddListener(this.ConfirmClick);

        this.skillConfirm.transform.Find("cancel").GetComponent<Button>().onClick.AddListener(Cancel_SkillConfirm);

        this.skillSelectTarget.transform.Find("cancel").GetComponent<Button>().onClick.AddListener(Cancel_skillSelectTarget);

        this.auto_AI = transform.Find("ActionUI/auto_AI").GetComponent<Button>();
        auto_AI_Text = this.auto_AI.GetComponentInChildren<Text>();
        auto_AI.onClick.AddListener(onClick_auto_AI);

        this.settingBtn = transform.Find("ActionUI/setting").GetComponent<Button>();
        settingBtn.onClick.AddListener(OnSettingBtnClick);

        battleUI = transform.Find("BattleUI");
        InitBattle(playBattleUI, out playerPicture, "PlayerUI");
        InitBattle(targetBattleUI, out targetPicture, "TargetUI");

        battleReadyUI = transform.Find("BattleReadyUI");
        battleReadyUI.Find("StartBattle").GetComponent<Button>().onClick.AddListener(StartBattle);
        battleReadyUI.Find("ChangeBattleUnit").GetComponent<Button>().onClick.AddListener(ChangeBattleUnit);
        selectUnitPanel = battleReadyUI.Find("SelectUnitPanel");
        characterFace = ResourcesExt.Load<GameObject>("Prefabs/CharacterFace");
        battleReadyUI.Find("ChangePos").GetComponent<Button>().onClick.AddListener(ChangePos);

        battleEndPanel = transform.Find("BattleEndPanel");
        battleEndPanel.Find("ReturnWorld").GetComponent<Button>().onClick.AddListener(() => SceneManagerExt.instance.LoadSceneShowProgress(GameDefine.SceneType.GameWorld));

        actionOrder = transform.Find("ActionOrder").GetComponent<ActionOrder>();
    }

    public void BattleEnd(bool isVictory)
    {
        battleEndPanel.gameObject.SetActive(true);
        if (isVictory)
        {
            battleEndPanel.Find("Victory").gameObject.SetActive(true);
        }
        else
        {
            battleEndPanel.Find("Fail").gameObject.SetActive(true);
        }
    }

    public void OpenBag()
    {
        bag.SetActive(true);
        if (bag.GetComponent<Bag>().character != BattleManager.Instance.nowCharacter)
        {
            bag.GetComponent<Bag>().InitOpenBag();
        }
    }

    /// <summary>
    /// 开始战斗
    /// </summary>
    private void StartBattle()
    {
        BattleManager.Instance.isBeforeBattle = false;
        BattleManager.Instance.NextOrder();
        BattleManager.Instance.CancelSelect();
        battleReadyUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 战斗前选择出战单位
    /// </summary>
    private void ChangeBattleUnit()
    {
        if (BattleManager.Instance.nowCharacter == null) return;

        selectUnitPanel.gameObject.SetActive(true);
        if (selectUnitPanel.childCount == 0)
        {
            foreach (var role in PlayerData.Army.Values)
            {
                GameObject face = Instantiate(characterFace, selectUnitPanel);
                face.transform.GetComponent<Image>().sprite = ResourcesExt.Load<Sprite>("Face/" + role.unitName);
                face.GetComponent<Button>().onClick.AddListener(() => { ExchangeBattleUnit(role); });
                faceDic.Add(role.unitName, face);
                if (BattleManager.Instance.isEmbattle(role)) face.SetActive(false);
            }
        }
        else
        {
            foreach (var role in PlayerData.Army.Values)
            {
                if (BattleManager.Instance.isEmbattle(role)) faceDic[role.unitName].SetActive(false);
                else faceDic[role.unitName].SetActive(true);
            }
        }
    }

    private void ChangePos()
    {
        BattleManager.Instance.isChangePos = true;
        BattleManager.Instance.CancelSelect();
        battleReadyUI.gameObject.SetActive(false);
    }

    private void ExchangeBattleUnit(Role role)
    {
        BattleManager.Instance.ExchangeBattleUnit(role);
        selectUnitPanel.gameObject.SetActive(false);
        //battleReadyUI.gameObject.SetActive(true);
    }

    private void InitBattle(Text[] tempBattleUI, out Image picture, string unitBattleUIName)
    {
        Transform temp = battleUI.Find(unitBattleUIName);
        tempBattleUI[0] = temp.Find("NameBG/CharacterName").GetComponent<Text>();
        tempBattleUI[1] = temp.Find("NameBG/WeaponName").GetComponent<Text>();
        tempBattleUI[2] = temp.Find("DataBG/HP/HP_Text").GetComponent<Text>();
        tempBattleUI[3] = temp.Find("DataBG/Attack/Attack_Text").GetComponent<Text>();
        picture = temp.Find("Picture").GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selectUnitPanel.gameObject.activeSelf)
                selectUnitPanel.gameObject.SetActive(false);
        }
    }

    //显示行动命令指令
    public void OpenActionOrder(Character character)
    {
        actionOrder.gameObject.SetActive(true);
        actionOrder.Init(character.getRole());
        UpdateActionPanel(BattleManager.Instance.nowCharacter);
    }

    private void UpdateActionPanel(Character player)
    {
        actionPanel.SetActive(true);
        List<Skill> learnedSkill = player.getRole().equipedSkills;

        for (int i = 0; i < 3; i++)
        {
            if (i >= learnedSkill.Count) continue;
            var skill = learnedSkill[i];

            //TODO:给自己技能写上冷却
            if (skill.Info.SkillType != 0)
            {
                var cd = skill.cd;
                Transform cdTrans = actionOrder.skillList[i].transform.Find("cd");
                cdTrans.gameObject.SetActive(cd != 0);
                if (cd == 0) continue;
                cdTrans.GetComponentInChildren<Text>().text = cd.ToString();
                //BattleUIManager.Instance.skill_slot[i].cdImage.gameObject.SetActive(cd > 0);
                //BattleUIManager.Instance.skill_slot[i].cdText.text = cd.ToString();
            }
        }
    }

    //战斗UI和行动UI的变化
    public void OpenAttackUI(bool open)
    {
        actionPanel.SetActive(!open);
        actionOrder.gameObject.SetActive(false);
        Panel_Attack.SetActive(open);
        OpenBattleInfo(BattleManager.Instance.nowCharacter, BattleManager.Instance.nowCharacter.target, open);
    }

    public void CloseAttackUI()
    {
        OpenAttackUI(false);
        BattleManager.Instance.isAttack = false;
        EventDispatcher.instance.DispatchEvent<Character, Character>(GameEventType.battle_End, BattleManager.Instance.nowCharacter, BattleManager.Instance.nowCharacter.target);
    }

    //// 将精灵的世界坐标转换成屏幕坐标
    private Vector3 GetScreenPos(Camera cam, Vector3 worldPos)
    {
        var screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
        return new Vector3(screenPos.x, screenPos.y, 0);

    }

    //伤害数字显示
    private void ShowHudDamage(int damage, Vector3 worldPos)
    {
        var hudItem = spawnPool.Spawn("hudItem", this.hudCanvas.transform);

        var screenPos = GetScreenPos(Camera.main, worldPos);
        hudItem.position = screenPos;

        StartCoroutine(FloatUI(hudItem.gameObject));

        var text = hudItem.Find("Text").GetComponent<Text>();
        text.text = damage.ToString();
        spawnPool.Despawn(hudItem, 1.3f);
    }

    //回血显示
    public void ShowRestoreHealth(int hp, Character player)
    {
        var hudItem = spawnPool.Spawn("hudText_green", this.hudCanvas.transform);
        var screenPos = GetScreenPos(Camera.main, player.transform.position + Vector3.up * 3);
        hudItem.position = screenPos;
        //设置最后渲染
        hudItem.transform.SetAsLastSibling();

        StartCoroutine(FloatUI(hudItem.gameObject));

        var text = hudItem.Find("Text").GetComponent<Text>();
        text.text = "+" + hp.ToString();
        spawnPool.Despawn(hudItem, 1.3f);
    }

    //物体浮动
    IEnumerator FloatUI(GameObject go)
    {
        //1.2秒 升高180 米
        var duration = 1.2f;
        var startTime = Time.time;

        var startPos = go.transform.position;
        var y_offset = 180;
        float t1 = 0;
        while (t1 < 1)
        {
            t1 = (Time.time - startTime) / duration;

            if (t1 >= 1f) t1 = 1;

            yield return new WaitForEndOfFrame();

            var y = Mathf.Lerp(0, y_offset, t1);

            go.transform.position = startPos + new Vector3(0, y, 0);
        }
    }

    //技能使用确认
    private void ConfirmClick()
    {
        BattleManager.Instance.ConfirmClick();
        skillConfirm.SetActive(false);

        EventDispatcher.instance.DispatchEvent(GameEventType.playButtonUiSound);
    }

    public void ShowSkillSelectTarget(bool show)
    {
        skillSelectTarget.SetActive(show);
        actionOrder.gameObject.SetActive(!show);
        this.actionPanel.SetActive(!show);

    }

    public void ShowSkillConfirm(bool show)
    {
        skillConfirm.SetActive(show);
        skillSelectTarget.SetActive(!show);

    }

    private void Cancel_SkillConfirm()
    {
        BattleManager.Instance.Cancel_SkillConfirm();
        EventDispatcher.instance.DispatchEvent(GameEventType.playButtonUiSound);
    }

    private void Cancel_skillSelectTarget()
    {
        BattleManager.Instance.Cancel_skillSelectTarget();
        EventDispatcher.instance.DispatchEvent(GameEventType.playButtonUiSound);
    }

    public void ShowOrderTip(string animationName)
    {
        if (orderTip == null) orderTip = this.transform.Find("ActionUI/orderTip").GetComponent<Animator>();

        orderTip.gameObject.SetActive(true);

        orderTip.Play(animationName);
    }

    void onClick_auto_AI()
    {
        BattleManager.Instance.ExchangeMyAutoAi();
        EventDispatcher.instance.DispatchEvent(GameEventType.playButtonUiSound);
        if (!BattleManager.Instance.auto)
        {
            auto_AI_Text.text = "AI托管";
        }
        else
        {
            auto_AI_Text.text = "关闭托管";
        }
    }

    private void OnSettingBtnClick()
    {
        var go = ResourcesExt.Load<GameObject>("UI/settingPanel");

        var rgo = MonoBehaviour.Instantiate(go);
        rgo.transform.SetParent(this.transform, false);
    }

    public void OpenBattleInfo(Character player, Character target, bool open)
    {
        battleUI.gameObject.SetActive(open);
        if (open)
        {
            player.BeforeBattle();
            LoadBattleInfoData(playBattleUI, playerPicture, player, target);
            LoadBattleInfoData(targetBattleUI, targetPicture, target, player);
        }
    }

    private void LoadBattleInfoData(Text[] unitBattleUI, Image unitPicture, Character player, Character target)
    {
        unitBattleUI[0].text = player.getRole().unitName;
        unitBattleUI[1].text = player.getRole().equip.info.Name;
        unitBattleUI[2].text = player.getRole().hp.ToString();
        unitBattleUI[3].text = (Math.Max(player.model.calcAtt() - target.model.calcDef(player.getRole()), 1)).ToString();
        unitPicture.sprite = ResourcesExt.Load<Sprite>("Picture/" + player.getRole().unitName);
        if (unitPicture.sprite == null)
        {
            unitPicture.sprite = ResourcesExt.Load<Sprite>("Picture/temp");
        }
        unitPicture.SetNativeSize();
    }

}
