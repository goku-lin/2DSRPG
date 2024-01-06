using France.Game.model.level;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
                LoadPlayer(j++, characters[i], i);
                continue;
            }
            GActor gactor = GActor.CreateActor(i, 1);
            level.actors[i] = gactor;
            GameObject tempUnit = Instantiate(characterPrafab);
            Character unit = tempUnit.GetComponent<Character>();

            unit.model = (GFightUnit)gactor;
            //TODO:敌方的名字想办法处理
            unit.model.pid = characters[i].pid;

            Role role = unit.getRole();

            unit.tileIndex = characters[i].tileIndex;
            unit.startIndex = unit.tileIndex;
            unit.transform.position = map.tiles[unit.tileIndex].transform.position;
            map.tiles[unit.tileIndex].character = unit;

            unit.sect = GameDefine.Sect.Red;
            unit.model.teamId = 1;
            tempUnit.name = "Character:" + role.unitName;
            tempUnit.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Picture/" + role.unitName);
            //TODO:这里没弄pid和init
            unit.model.actorObject = tempUnit;
            gactor.createObj();
        }
    }

    public void LoadPlayer(int playerNum, UnitInfo player, int uid)
    {
        Role role = PlayerData.Army.ElementAt(playerNum).Value;
        GActor gactor = GActor.CreateActor(uid, 1);
        level.actors[uid] = gactor;

        GameObject tempUnit = Instantiate(characterPrafab);
        Character unit = tempUnit.GetComponent<Character>();

        unit.tileIndex = player.tileIndex;
        unit.startIndex = unit.tileIndex;
        unit.transform.position = map.tiles[unit.tileIndex].transform.position;
        map.tiles[unit.tileIndex].character = unit;

        unit.sect = GameDefine.Sect.Blue;
        tempUnit.name = "Character:" + role.unitName;
        tempUnit.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Picture/" + role.unitName);

        unit.model = (GFightUnit)gactor;
        unit.model.teamId = 0;
        unit.model.pid = role.pid;
        unit.model.actorObject = tempUnit;
        gactor.createObj();
        //unit.Init();
    }
}