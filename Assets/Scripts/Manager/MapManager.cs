using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class MapManager : SingletonMono<MapManager>
{
    public GMap map;
    private GameObject mapTile;
    private GameObject character;

    protected override void Awake()
    {
        base.Awake();
        Init(GameManager.Instance.nowLevel);
    }

    public void Init(string nowLevel)
    {
        mapTile = ResourcesExt.Load<GameObject>("Prefabs/MapTile");
        character = ResourcesExt.Load<GameObject>("Prefabs/Enemy");

        string file = Application.streamingAssetsPath + "/Level/" + nowLevel + ".xml";
        GLevel level = new GLevel();
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
        List<UnitInfo> players = new List<UnitInfo>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].isPlayer)
            {
                players.Add(characters[i]);
                continue;
            }
            Character unit = Instantiate(character).GetComponent<Character>();
            unit.tileIndex = characters[i].tileIndex;
            unit.startIndex = unit.tileIndex;
            unit.transform.position = map.tiles[unit.tileIndex].transform.position;
            map.tiles[unit.tileIndex].character = unit;
        }

        int j = 0;
        foreach (var item in PlayerData.Army.Values)
        {
            if (j >= players.Count) break;
            GameObject tempUnit = ResourcesExt.Load<GameObject>("Prefabs/" + item.unitName);
            Character unit = Instantiate(tempUnit).GetComponent<Character>();
            unit.tileIndex = players[j++].tileIndex;
            unit.startIndex = unit.tileIndex;
            unit.transform.position = map.tiles[unit.tileIndex].transform.position;
            map.tiles[unit.tileIndex].character = unit;
        }
    }
}
