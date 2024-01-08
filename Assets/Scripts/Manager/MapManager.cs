using France.Game.model.level;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : SingletonMono<MapManager>
{
    private GLevel level;
    public GMap map;
    private GameObject mapTile;
    private GameObject characterPrafab;

    protected override void Awake()
    {
        base.Awake();
        Init(GameManager.Instance.nowLevel);
    }

    public void Init(string nowLevel)
    {
        mapTile = ResourcesExt.Load<GameObject>("Prefabs/MapTile");
        characterPrafab = Resources.Load<GameObject>("Prefabs/Character");

        string file = Application.streamingAssetsPath + "/Level/" + nowLevel + ".xml";
        level = new GLevel();
        Utilitys.FillLevel(file, ref level);
        Transform bgTrans = this.transform.Find("BackGround");
        bgTrans.position = level.pos;
        bgTrans.localScale = level.size;
        SpriteRenderer bgRenderer = bgTrans.GetComponent<SpriteRenderer>();
        string value = Application.dataPath + @"\Resources\Map" + "/" + level.background;
        StartCoroutine(Utilitys.LoadImage(value, bgRenderer));

        map = level.map;
        map.tiles = new GMapTile[map.y * map.x];
        for (int i = 0; i < map.y; i++)
        {
            for (int j = 0; j < map.x; j++)
            {
                int num = i * map.x + j;
                GameObject tile = Instantiate(mapTile, transform);
                tile.transform.position = new Vector3(j, i, 0);
                tile.GetComponent<GMapTile>().index = num;
                map.tiles[num] = tile.GetComponent<GMapTile>();
                map.tiles[num].moveCost = map.tilesList[num].moveCost;
            }
        }
        LoadUnit(nowLevel);
    }

    public void LoadUnit(string nowLevel)
    {
        List<UnitInfo> characters = Utilitys.LoadUnit(Application.streamingAssetsPath + "/Character/" + nowLevel + ".xml");
        int j = 0;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].isPlayer)
            {
                CreateCharacter(characters[i], i, true, PlayerData.Army.ElementAt(j++).Value);
                continue;
            }
            else
            {
                CreateCharacter(characters[i], i, false, null);
                continue;
            }
        }
    }

    private void CreateCharacter(UnitInfo unitInfo, int uid, bool isPlayer, Role role)
    {
        // 共通的角色创建逻辑
        GameObject unitGO = Instantiate(characterPrafab);
        Character unit = unitGO.GetComponent<Character>();
        //地图位置
        unit.tileIndex = unitInfo.tileIndex;
        unit.startIndex = unit.tileIndex;
        unit.transform.position = map.tiles[unit.tileIndex].transform.position;
        map.tiles[unit.tileIndex].character = unit;
        //角色模型
        GActor gactor = GActor.CreateActor(uid, 1);
        level.actors[uid] = gactor;
        //角色model参数
        unit.model = (GFightUnit)gactor;
        unit.model.actorObject = unitGO;

        if (isPlayer)
        {
            unit.model.pid = role.pid;
            unit.sect = GameDefine.Sect.Blue;
            unit.model.teamId = (int)GameDefine.Sect.Blue;
        }
        else if (char.ToUpper(unitInfo.pid[0]) == 'O')
        {
            unit.model.pid = unitInfo.pid;
            role = unit.getRole();
            unit.sect = GameDefine.Sect.Grey;
            unit.model.teamId = (int)GameDefine.Sect.Grey;
        }
        else
        {
            unit.model.pid = unitInfo.pid;
            role = unit.getRole();
            unit.sect = GameDefine.Sect.Red;
            unit.model.teamId = (int)GameDefine.Sect.Red;
        }
        unitGO.name = "Character:" + role.unitName;
        unitGO.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Picture/" + role.unitName);

        gactor.createObj();
    }
}

/// <summary>
/// 专门用在地图编辑保存单位数据
/// </summary>
public class UnitInfo
{
    public string pid;
    public int tileIndex;
    public bool isPlayer;

    public UnitInfo(string pid, int tileIndex, bool isPlayer)
    {
        this.pid = pid;
        this.tileIndex = tileIndex;
        this.isPlayer = isPlayer;
    }
}