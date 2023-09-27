using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static GameDefine;

public class BattleManager : SingletonMono<BattleManager>
{
    public bool isBeforeBattle;
    public List<Character> characters;  //所有角色
    public Character nowCharacter;  //当前控制角色
    public GMapTile[] mapTiles; //所有格子
    public Sect mySect = Sect.Blue;
    //寻路
    Dictionary<int, AStarNode> dic;
    Dictionary<int, AStarNode> attackDic;
    List<int> moveRangePath = new List<int>();
    List<int> attackRangePath = new List<int>();
    public List<int> skillRangePath = new List<int>();
    List<int> currentMovePath = new List<int>();
    public GMap map;

    public bool isShow = false; //移动范围显示
    public bool isAttack = false;   //是否正在攻击
    public int moveSpeed = 5;   //游戏物体移动速度
    public bool isMoving = false;  //是否正在移动

    private GameObject moveTilePrefab;
    private GameObject attackTilePrefab;
    public bool auto = false;

    //根据技能ID获取图片
    Dictionary<string, Sprite> skillTextureMap = new Dictionary<string, Sprite>();
    private int lastSkillId;
    private Skill usingSkill;
    private bool showskillReleaseRange;
    private Character skillTarget;
    private Sect orderSect = Sect.Red;

    protected override void Awake()
    {
        base.Awake();
        EffectCtrl.Init();
        SkillSystem.Instance.Init();
        BehaviorCtrl.Init();
        if (AudioCtrl.instance == null)
            AudioCtrl.Init();
        moveTilePrefab = ResourcesExt.Load<GameObject>("Prefabs/MoveGrid");
        attackTilePrefab = ResourcesExt.Load<GameObject>("Prefabs/AttackGrid");
        isBeforeBattle = true;
    }
    
    private void Start()
    {
        //获得地图
        map = MapManager.Instance.map;
        mapTiles = map.tiles;
        Character[] tempCharacter;
        tempCharacter = FindObjectsOfType<Character>();
        for (int i = 0; i < tempCharacter.Length; i++) //初始化所有角色
        {
            tempCharacter[i].uid = i;
            tempCharacter[i].Init();
            characters.Add(tempCharacter[i]);
        }

        UIManager.Instance.Init_HpImage();

        //Invoke(nameof(NextOrder), 0.1f);
        EventDispatcher.instance.Regist(GameEventType.GotoHomeClICK, this.OnGotoHomeClICK);
    }

    private void Update()
    {
        if (currentMovePath.Count != 0)
        {
            MoveTo();
        }
        //对话时不能动界面
        if (StoryManager.Instance.canTalk) return;
        //释放技能时不能操作
        if (nowCharacter != null && nowCharacter.state == PlayerSate.skill) return;
        //托管不与允许选择人物
        if (auto) return;
        if (BehaviorCtrl.instance.myAutomationBehavior.executting) return;
        if (isBeforeBattle)
        {
            MouseDownBeforeBattle();
        }
        else if (!isAttack && this.orderSect == mySect)
        {
            MouseDown();
        }
        DebugLvUp();
    }

    //TODO:debug的，以后记得删掉
    public void DebugLvUp()
    {
        if (Input.GetMouseButtonDown(2))
        {
            nowCharacter.getRole().gainExp(100);
        }
    }

    private void OnGotoHomeClICK()
    {
        SceneManagerExt.instance.LoadSceneShowProgress(SceneType.GameTilte);
    }

    //鼠标点击
    public void MouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving && !InteractWithUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                //点击到格子
                if (hitInfo.collider.CompareTag("Tile"))
                {
                    GMapTile tempMapTile = hitInfo.collider.gameObject.GetComponent<GMapTile>();
                    GetMouseDownBehavior(tempMapTile);
                }
            }
        }
        //显示敌人威胁范围
        if (Input.GetMouseButtonDown(1))
        {
            //ShowMoveRange(enemy);
        }
    }

    private void MouseDownBeforeBattle()
    {
        if (Input.GetMouseButtonDown(0) && !InteractWithUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                //点击到格子
                if (hitInfo.collider.CompareTag("Tile"))
                {
                    GMapTile tempMapTile = hitInfo.collider.gameObject.GetComponent<GMapTile>();
                    if (tempMapTile.character != null)
                    {
                        if (isShow)
                            CancelSelect();
                        nowCharacter = tempMapTile.character;
                        EventDispatcher.instance.DispatchEvent(GameEventType.playIdleVoice, nowCharacter.getRole());
                        this.UpdateActionPanel(nowCharacter);
                        ShowMoveRange(nowCharacter);
                    }
                    else
                    {
                        CancelSelect();
                    }
                }
            }
        }
    }

    public void GetMouseDownBehavior(GMapTile tempMapTile)
    {
        //要在技能范围内才能选择
        if (showskillReleaseRange)
        {
            if (skillRangePath.Contains(tempMapTile.index))
            {
                //选择释放技能的目标
                if (nowCharacter && tempMapTile.index == nowCharacter.tileIndex)
                {
                    SkillSelectionTarget(this.nowCharacter, nowCharacter);
                }
                else if (tempMapTile.character != nowCharacter)
                {
                    SkillSelectionTarget(this.nowCharacter, tempMapTile.character);
                }
            }
        }
        //当点击玩家时，显示移动范围和攻击范围
        else if (GetPlayers(mySect).Contains(tempMapTile.character))
        {
            if (nowCharacter == null || nowCharacter.tileIndex != tempMapTile.index)
            {
                if (isShow)
                    CancelSelect();
                nowCharacter = tempMapTile.character;
                EventDispatcher.instance.DispatchEvent(GameEventType.playIdleVoice, nowCharacter.getRole());
                this.UpdateActionPanel(nowCharacter);
                ShowMoveRange(nowCharacter);
            }
            else
            {
                bool temp = !UIManager.Instance.actionPanel.activeSelf;
                UIManager.Instance.actionPanel.SetActive(temp);
                UIManager.Instance.infoUI.SetActive(!temp);
            }
        }
        //点击可移动范围内时，进行移动
        else if (isShow && moveRangePath.Contains(tempMapTile.index) && (tempMapTile.character == null || tempMapTile.character == nowCharacter) && nowCharacter.state == PlayerSate.idle)
        {
            UIManager.Instance.actionPanel.SetActive(true);
            UIManager.Instance.infoUI.SetActive(false);
            if (nowCharacter.sect != mySect) return;
            isMoving = true;
            int tempIndex = tempMapTile.index;
            currentMovePath = AStar.FindPath(nowCharacter, nowCharacter.tileIndex, nowCharacter.tileIndex, tempIndex,
                        true, nowCharacter.movePower, nowCharacter.movePower, map, 0, 0,
                        true, true, null, null, null, moveRangePath, GetEnemyList(mySect));
        }
        //选择敌人
        else if (tempMapTile.character != null && GetEnemyList(mySect).Contains(tempMapTile.character.tileIndex))
        {
            UIManager.Instance.actionPanel.SetActive(true);
            UIManager.Instance.infoUI.SetActive(false);
            //移动范围和攻击范围内的敌人点击后移动到目标位置并打开攻击界面
            if (isShow && (moveRangePath.Contains(tempMapTile.index) || attackRangePath.Contains(tempMapTile.index)) && nowCharacter.sect == mySect && nowCharacter.state == PlayerSate.idle)
            {
                isMoving = true;
                isAttack = true;
                nowCharacter.target = tempMapTile.character;
                int tempIndex = tempMapTile.index;
                currentMovePath = AStar.FindPath(nowCharacter, nowCharacter.tileIndex, nowCharacter.tileIndex, tempIndex,
                            true, nowCharacter.movePower, nowCharacter.movePower, map, nowCharacter.min_AttackRange, nowCharacter.max_AttackRange,
                            true, true, null, null, null, moveRangePath, GetEnemyList(mySect));
            }
            //非攻击时显示敌人移动范围
            else
            {
                if (isShow)
                    CancelSelect();
                nowCharacter = tempMapTile.character;
                ShowMoveRange(nowCharacter);
            }
        }
        //取消显示
        else
        {
            CancelSelect();
        }
    }

    public void ShowMoveRange(Character unit)
    {
        isShow = true;
        //nowCharacter = unit;
        dic = new Dictionary<int, AStarNode>();
        attackDic = new Dictionary<int, AStarNode>();
        AStar.MoveableArea(unit, unit.startIndex, unit.movePower, map, dic, GetEnemyList(unit.sect));
        foreach (var i in dic.Keys)
        {
            moveRangePath.Add(i);
        }
        if (!moveRangePath.Contains(unit.tileIndex)) moveRangePath.Add(unit.tileIndex);
        //获得攻击范围和移动范围显示
        foreach (var i in moveRangePath)
        {
            if (mapTiles[i].character == null)
                AStar.AttackableArea(unit, i, nowCharacter.max_AttackRange, map, attackDic, moveRangePath);
            Instantiate(moveTilePrefab, mapTiles[i].transform);
        }

        foreach (var i in attackDic.Keys)
        {
            attackRangePath.Add(i);
        }
        //除开重合的部分
        attackRangePath = attackRangePath.FindAll(t => !moveRangePath.Contains(t));
        //攻击范围显示
        foreach (var i in attackRangePath)
        {
            Instantiate(attackTilePrefab, mapTiles[i].transform);
        }
    }

    public void UnShowMoveRange()
    {
        //移动到前不能取消
        if (isMoving) return;
        isShow = false;
        UIManager.Instance.actionPanel.SetActive(false);
        nowCharacter = null;
        //isMoving = false;
        ClearTile();
    }

    public void CancelSelect()
    {
        if (nowCharacter == null) return;
        nowCharacter.tileIndex = mapTiles[nowCharacter.startIndex].index;
        nowCharacter.transform.position = mapTiles[nowCharacter.startIndex].transform.position;
        UnShowMoveRange();
        currentMovePath.Clear();
    }

    /// <summary>
    /// 清除格子
    /// </summary>
    /// <param name="isComBack"></param>在使用技能时用的多，保证可以回到之前的显示状态
    public void ClearTile()
    {
        foreach (var i in moveRangePath)
        {
            //设置表现变化
            Destroy(mapTiles[i].transform.GetChild(0).gameObject);
        }
        foreach (var i in attackRangePath)
        {
            //设置表现变化
            Destroy(mapTiles[i].transform.GetChild(0).gameObject);
        }
        foreach (var i in skillRangePath)
        {
            //设置表现变化
            Destroy(mapTiles[i].transform.GetChild(0).gameObject);
        }
        moveRangePath.Clear();
        attackRangePath.Clear();
        skillRangePath.Clear();
    }

    public void ShowActiveSkill(Character unit, uint skillRange)
    {
        ClearTile();
        dic = new Dictionary<int, AStarNode>();
        AStar.AttackableArea(unit, unit.tileIndex, skillRange, map, dic, skillRangePath);
        //这个放在寻路上面就不能选择自己，因为上面的最后一个是排除掉了
        skillRangePath.Add(unit.tileIndex);
        foreach (var i in dic.Keys)
        {
            skillRangePath.Add(i);
        }
        //技能范围显示
        foreach (var i in skillRangePath)
        {
            Instantiate(moveTilePrefab, mapTiles[i].transform);
        }
    }

    public void SetDestination(List<int> currentMovePath, Character nowCharacter)
    {
        this.currentMovePath = currentMovePath;
        this.nowCharacter = nowCharacter;
        isMoving = true;
    }

    public void MoveTo()
    {
        if (Vector3.Distance(mapTiles[currentMovePath[0]].transform.position, nowCharacter.gameObject.transform.position) >= 0.1f)
        {
            Vector3 moveDir = (mapTiles[currentMovePath[0]].transform.position - nowCharacter.gameObject.transform.position).normalized;
            nowCharacter.transform.position += moveSpeed * Time.deltaTime * moveDir;
        }
        else
        {
            //每次到达新的点更新寻路位置，同时校准角色位置，不然会有一点点偏差
            nowCharacter.tileIndex = currentMovePath[0];
            nowCharacter.gameObject.transform.position = mapTiles[currentMovePath[0]].transform.position;
            currentMovePath.RemoveAt(0);
        }
        if (currentMovePath.Count == 0)
        {
            isMoving = false;
            if (isAttack)
            {
                UIManager.Instance.OpenAttackUI(true);
            }
        }
    }

    public void Wait()
    {
        if (isMoving || !nowCharacter) return;
        if (nowCharacter.state == PlayerSate.wait) return;
        //交换结点上的玩家位置
        mapTiles[nowCharacter.startIndex].character = null;
        mapTiles[nowCharacter.tileIndex].character = nowCharacter;
        //更新玩家的结点位置
        nowCharacter.startIndex = nowCharacter.tileIndex;
        nowCharacter.state = PlayerSate.wait;
        //技能CD减少1
        nowCharacter.Cd_Add(-1);

        UnShowMoveRange();
        currentMovePath.Clear();

        Character idlePlayer = null;
        if (orderSect == mySect)
            idlePlayer = IdlePlayer(mySect);
        else
            idlePlayer = IdlePlayer(Sect.Red);

        if (idlePlayer != null)
        {
            //UpdateSelect(idlePlayer);
        }
        else
        {
            Debug.Log("回合结束！");

            StartCoroutine(OrderOvery());
        }
    }

    public void Wait_AI(Character playerC)
    {
        this.nowCharacter = playerC;
        Wait();
    }

    private Character IdlePlayer(Sect sect)
    {
        foreach (Character player in this.characters)
        {
            if (player.sect == sect && player.state == PlayerSate.idle) return player;
        }

        return null;
    }

    IEnumerator C_EffectEnd(System.Action func)
    {
        //等待特效播放完成
        while (EffectCtrl.instance.playeffect)
        {
            yield return new WaitForEndOfFrame();
        }
        if (func != null) func.Invoke();
    }

    IEnumerator OrderOvery()
    {
        yield return StartCoroutine(C_EffectEnd(null));
        yield return new WaitForSeconds(1.5f);
        this.NextOrder();
    }

    public void Attack(Character attacker)
    {
        UIManager.Instance.OpenAttackUI(false);
        StartCoroutine(attacker.Attack());
        //BattleManager.Instance.Wait();
        isAttack = false;
    }

    private void UpdateActionPanel(Character player)
    {
        UIManager.Instance.actionPanel.SetActive(true);
        List<Skill> learnedSkill = player.getRole().equipedSkills;

        for (int i = 0; i < 3; i++)
        {
            string id;
            if (i >= learnedSkill.Count)
                id = "";
            else id = learnedSkill[i].Name;
            var showIcon = id != "";

            if (showIcon) UIManager.Instance.skill_slot[i].button.image.sprite = GetSkillTexture(learnedSkill[i].IconLabel);
            UIManager.Instance.skill_slot[i].button.gameObject.SetActive(showIcon);

            if (i >= learnedSkill.Count) continue;
            var skill = learnedSkill[i];

            if (skill.SkillType != 0)
            {
                var cd = skill.CD;
                UIManager.Instance.skill_slot[i].cdImage.gameObject.SetActive(cd > 0);
                UIManager.Instance.skill_slot[i].cdText.text = cd.ToString();
            }
            else
                UIManager.Instance.skill_slot[i].cdImage.gameObject.SetActive(false);
        }
    }

    //得到技能id对应的图片
    public Sprite GetSkillTexture(string iconLabel)
    {

        if (!skillTextureMap.ContainsKey(iconLabel))
        {
            var i = ResourcesExt.Load<Sprite>("Textures/skill/" + iconLabel);
            skillTextureMap.Add(iconLabel, i);
        }
        return skillTextureMap[iconLabel];
    }

    //角色战败时移除
    public void CharacterDie(Character character)
    {
        List<Character> players = GetPlayers(mySect);
        if (players.Contains(character))
        {
            CancelSelect();
        }
        RemoveCharacter(character);
        if (GetEnemy(mySect).Count == 0)
        {
            Debug.Log("you win");
            UIManager.Instance.BattleEnd(true);
        }
        if (players.Count == 0)
        {
            Debug.Log("you Lose");
            UIManager.Instance.BattleEnd(false);
        }
    }

    public void RemoveCharacter(Character character)
    {
        mapTiles[character.tileIndex].character = null;
        characters.Remove(character);
        //TODO:血条想个办法优化
        Destroy(character.gameObject, 0.5f);
        //character.gameObject.SetActive(false);
    }

    public void UseSkill(int id)
    {
        if (isMoving || isBeforeBattle) return;
        if (nowCharacter.getRole().equipedSkills[id].SkillType == 0)
        {
            Debug.Log("被动技能");
            return;
        }
        if (nowCharacter.getRole().equipedSkills[id].CD > 0)
        {
            Debug.Log("技能还没冷却");
            return;
        }
        if (nowCharacter.getRole().mp < nowCharacter.ShowActiveSkill_ReleaseRange(id).MPCost)
        {
            Debug.Log("MP不够");
            return;
        }
        //这个id是第几个按钮的技能，不是技能id
        lastSkillId = id;
        //TODO:实现多种技能放在一个技能槽选择，梦战那样，可以在这里再封装一个方法
        ShowActiveSkill_ReleaseRange2();
    }

    private void ShowActiveSkill_ReleaseRange2()
    {
        this.usingSkill = nowCharacter.ShowActiveSkill_ReleaseRange(lastSkillId);
        ShowActiveSkill(nowCharacter, (uint)usingSkill.RangeO);
        this.showskillReleaseRange = true;

        UIManager.Instance.ShowSkillSelectTarget(true);
    }

    private void ShowActiveSkill_ActionRange(Skill skill, Character target)
    {
        ShowActiveSkill(target, (uint)skill.ActionRangeO);
    }

    //等待人物选择释放范围内的目标
    public void SkillSelectionTarget(Character from, Character to)
    {
        if (to == null) return;
        skillTarget = to;

        if (to != null && canSelect(from, to, usingSkill))
        {
            ShowActiveSkill_ActionRange(this.usingSkill, skillTarget);
            UIManager.Instance.ShowSkillConfirm(true);
        }
    }

    public void SkillSelectionTarget(Character from, Character to, Skill skill)
    {
        if (to == null) return;
        skillTarget = to;
        this.usingSkill = skill;
        //技能选择判定
        if (to != null && canSelect(from, to, usingSkill))
        {
            ShowActiveSkill_ActionRange(this.usingSkill, skillTarget);
            UIManager.Instance.ShowSkillConfirm(true);
        }
    }

    public bool canSelect(Character from, Character to, Skill skill)
    {
        if (skill.RangeTarget == 1)
        {
            return from.sect == to.sect;
        }
        else
        {
            return from.sect != to.sect;
        }
    }

    public void ConfirmClick()
    {
        this.nowCharacter.Releaseskill(this.usingSkill, skillTarget);
        UIManager.Instance.actionPanel.SetActive(false);
    }

    public void confirmUseSkill_AI(Character from, Character to)
    {
        this.nowCharacter = from;
        skillTarget = to;
        this.nowCharacter.Releaseskill(this.usingSkill, to);

        //ClearTile();
    }

    internal void Cancel_skillSelectTarget()
    {
        showskillReleaseRange = false;
        this.ClearTile();
        UpdateActionPanel(nowCharacter);
        this.ShowMoveRange(nowCharacter);

        UIManager.Instance.ShowSkillSelectTarget(false);
    }

    internal void Cancel_SkillConfirm()
    {
        this.ShowActiveSkill_ReleaseRange2();

        UIManager.Instance.ShowSkillConfirm(false);

    }

    internal void ActionEnd()
    {
        this.showskillReleaseRange = false;
        //this.battle = false;
        //this.Wait(true);
        StartCoroutine(C_EffectEnd(() => { this.Wait(); }));
    }

    public void NextOrder()
    {
        if (orderSect == Sect.Blue)
            orderSect = Sect.Red;
        else if (orderSect == Sect.Red)
            orderSect = Sect.Blue;

        if (orderSect == mySect)
            UIManager.Instance.ShowOrderTip("orderTipOpen_My");
        else
            UIManager.Instance.ShowOrderTip("orderTipOpen_Enemy");

        //把所有玩家设置为可行动
        foreach (Character unit in GetPlayers(orderSect))
        {
            unit.state = PlayerSate.idle;
        }

        //if (orderSect == mySect)
        //    UpdateSelect(players[0]);

        if (orderSect == mySect)
        {
            if (this.auto) AutoMyScet();
        }
        else
        {
            //开始敌人回合
            EnemyOrder();
        }
    }

    void EnemyOrder()
    {
        //敌人由AI进行托管
        var enemys = GetEnemy(this.mySect);

        if (enemys.Count != 0)
            BehaviorCtrl.instance.enemyAutomationBehavior.Start_Automation(enemys);
        else
        {
            StartCoroutine(OrderOvery());
        }
    }

    void AutoMyScet()
    {
        var players = GetPlayersIdle(mySect);
        //要考虑
        if (auto)
        {
            BehaviorCtrl.instance.myAutomationBehavior.ReadyStop(false);
            //防止AI运行中重复执行出现不可控的情况
            if (BehaviorCtrl.instance.myAutomationBehavior.executting == false)
                BehaviorCtrl.instance.myAutomationBehavior.Start_Automation(players);
        }
        else
        {
            //防止AI执行到一半被停止
            //自动化进入准备停止阶段，直到当前的AI执行完毕才会真正停止
            BehaviorCtrl.instance.myAutomationBehavior.ReadyStop(true);
        }
    }

    public List<Character> GetPlayersIdle(Sect sect)
    {
        List<Character> i = new List<Character>();
        foreach (var item in this.GetPlayers(mySect))
        {
            if (item.sect == sect && item.state == PlayerSate.idle) i.Add(item);
        }
        return i;
    }

    public List<Character> GetEnemy(Sect sect)
    {
        List<Character> enemy = new List<Character>();
        foreach (var item in this.characters)
        {
            if (item.sect != sect) enemy.Add(item);
        }

        return enemy;
    }

    public List<Character> GetPlayers(Sect sect)
    {
        List<Character> i = new List<Character>();
        foreach (var item in this.characters)
        {
            if (item.sect == sect) i.Add(item);
        }
        return i;
    }

    public List<int> GetEnemyList(Sect sect)
    {
        List<int> enemyList = new List<int>();
        foreach (var item in this.characters)
        {
            if (item.sect != sect) enemyList.Add(item.tileIndex);
        }

        return enemyList;
    }

    public void AttackSelect_AI(Character from, Character to, List<int> range)
    {
        isMoving = true;
        nowCharacter = from;
        nowCharacter.target = to;
        currentMovePath = AStar.FindPath(nowCharacter, nowCharacter.tileIndex, nowCharacter.tileIndex, to.tileIndex,
                    true, nowCharacter.movePower, nowCharacter.movePower, map, nowCharacter.min_AttackRange, nowCharacter.max_AttackRange,
                    true, true, null, null, null, range, GetEnemyList(mySect));
    }

    public void ExchangeMyAutoAi()
    {
        auto = !auto;

        AutoMyScet();
    }

    /// <summary>
    /// 更新队伍的角色的应该
    /// </summary>
    private void updateArmyIndexes()
    {
        List<int> list = new List<int>();
        //foreach (Character gfightUnit in this.teams[0].actors.Values)
        //{
        //    int characterId = gfightUnit.getCharacterId();
        //    if (characterId != -1 && !list.Contains(characterId))
        //    {
        //        list.Add(characterId);
        //    }
        //}
        for (int i = 0; i < PlayerData.ArmyIndexes.Count; i++)
        {
            int item = PlayerData.ArmyIndexes[i];
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
        PlayerData.ArmyIndexes = list;
    }

    public void ExchangeBattleUnit(Role role)
    {
        nowCharacter.pid = role.pid;
        nowCharacter.Init();
    }

    bool InteractWithUI()
    {
        //EventSystem.current.currentSelectedGameObject 只作用于按钮 IsPointerOverGameObject 作用于全部
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            return true;
        }
        return false;
    }

    protected override void OnDestroy()
    {
        EventDispatcher.instance.ClearEventListener();
        base.OnDestroy();
    }
}
