using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.UI;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public string levelName;
    public string backGround;
    public int mapX;
    public int mapY;
    public int tileCost = 1;
    public GameObject editorTile;
    private bool mCanChange;
    public GMap map;
    public string loadFile;
    public GameObject character;
    public GameObject player;

    private Transform backGroundTrans;
    private TextMesh[] debugTextArray;
    public event EventHandler<int> OnGridObjectChanged;
    Transform tileParent;
    Transform unitParent;
    public bool isPlayer;
    public bool isUnit;

    public GLevel Level
    {
        get
        {
            return new GLevel(levelName, map, backGround, backGroundTrans.position, backGroundTrans.localScale);
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
            mCanChange = true;
        else
            mCanChange = false;

        if (Input.GetMouseButtonDown(0) && mCanChange)
        {
            // 增加是否可以绘制的判断
            if (isUnit)
            {
                GMapTile tempTile = hitInfo.collider.GetComponent<GMapTile>();
                if (tempTile.character != null) return;
                Character unit;
                if (isPlayer)
                {
                    unit = Instantiate(player, unitParent).AddComponent<Character>();
                }
                else
                {
                    unit = Instantiate(character, unitParent).GetComponent<Character>();
                }
                tempTile.character = unit;
                unit.transform.position = tempTile.transform.position;
                unit.tileIndex = tempTile.index;
            }
            else
            {
                GMapTile tempTile = hitInfo.collider.GetComponent<GMapTile>();
                tempTile.moveCost = tileCost;
                OnGridObjectChanged(tempTile.index, tileCost);
            }
        }
        if (Input.GetMouseButtonDown(1) && mCanChange)
        {
            if (isUnit)
            {
                GMapTile tempTile = hitInfo.collider.GetComponent<GMapTile>();
                if (tempTile.character == null) return;
                Destroy(tempTile.character.gameObject);
            }
            else
            {
                GMapTile tempTile = hitInfo.collider.GetComponent<GMapTile>();
                tempTile.moveCost = 1;
                OnGridObjectChanged(tempTile.index, 1);
            }
        }
    }

    //初始化，生成格子
    public void Init()
    {
        ResetMap();
        backGroundTrans = this.transform.Find("BackGround");
        map = new GMap(mapX, mapY);
        debugTextArray = new TextMesh[mapY * mapX];
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                GameObject tile = Instantiate(editorTile, tileParent);
                tile.transform.position = new Vector3(j, i, 0);
                tile.GetComponent<GMapTile>().index = i * mapX + j;
                map.tiles[i] = tile.GetComponent<GMapTile>();
                map.tilesList.Add(new tile(i, map.tiles[i].moveCost));
                debugTextArray[i * mapX + j] = Utilitys.CreateWorldText(1.ToString(), tileParent, tile.transform.position, 40, Color.white, TextAnchor.MiddleCenter);
            }
        }
        InitEvent();
    }

    //重置地图
    private void ResetMap()
    {
        ResetTrans(ref tileParent, "TileParent");
        ResetTrans(ref unitParent, "UnitParent");
    }

    private void ResetTrans(ref Transform tempParent, string gameObjectName)
    {
        if (tempParent != null)
            Destroy(tempParent.gameObject);
        tempParent = new GameObject(gameObjectName).transform;
        tempParent.transform.parent = transform;
    }

    private void InitEvent()
    {
        OnGridObjectChanged += (object sender, int eventArgs) =>
        {
            debugTextArray[(int)sender].text = eventArgs.ToString();
            map.tilesList[(int)sender].moveCost = eventArgs;
        };
    }

    //加载格子
    public void Load()
    {
        ResetMap();
        GLevel level = new GLevel();
        Utilitys.FillLevel(Application.streamingAssetsPath + "/Level/" + loadFile, ref level);
        debugTextArray = new TextMesh[mapY * mapX];
        if (backGroundTrans == null) backGroundTrans = this.transform.Find("BackGround");
        backGroundTrans.position = level.pos;
        backGroundTrans.localScale = level.size;
        SpriteRenderer bgRenderer = backGroundTrans.GetComponent<SpriteRenderer>();
        string value = Application.dataPath + @"\Resources\Map" + "/" + level.background;
        StartCoroutine(Utilitys.LoadImage(value, bgRenderer));

        map = level.map;
        map.tiles = new GMapTile[map.y * map.x];
        for (int i = 0; i < map.y; i++)
        {
            for (int j = 0; j < map.x; j++)
            {
                int num = i * map.x + j;
                GameObject tile = Instantiate(editorTile, tileParent);
                tile.transform.position = new Vector3(j, i, 0);
                tile.GetComponent<GMapTile>().index = num;
                map.tiles[num] = tile.GetComponent<GMapTile>();
                map.tiles[num].moveCost = map.tilesList[num].moveCost;

                debugTextArray[num] = Utilitys.CreateWorldText(map.tiles[num].moveCost.ToString(), tileParent, tile.transform.position, 40, Color.white, TextAnchor.MiddleCenter);
            }
        }
        InitEvent();
    }

    public void LoadUnit()
    {
        List<UnitInfo> characters = Utilitys.LoadUnit(Application.streamingAssetsPath + "/Character/" + loadFile);
        for (int i = 0; i < characters.Count; i++)
        {
            Character unit;
            if (characters[i].isPlayer)
                unit = Instantiate(player, unitParent).AddComponent<Character>();
            else
                unit = Instantiate(character, unitParent).GetComponent<Character>();
            unit.unitName = characters[i].unitName;
            unit.tileIndex = characters[i].tileIndex;
            unit.startIndex = unit.tileIndex;
            unit.transform.position = map.tiles[unit.tileIndex].transform.position;
            map.tiles[unit.tileIndex].character = unit;
        }
    }

}
