using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public string levelName;
    public string backGround;
    public int mapX;
    public int mapY;
    public int tileCost = 1;
    public string pid;

    public GameObject editorTile;
    public GameObject character;
    public GameObject player;

    private Transform backGroundTrans;
    private Transform tileParent;
    private Transform unitParent;
    private TextMesh[] debugTextArray;
    public bool isPlayer;
    public bool isUnit;

    private GMap map;
    public event EventHandler<int> OnGridObjectChanged;
    public string loadFile;

    public GLevel Level
    {
        get
        {
            return new GLevel(levelName, map, backGround, backGroundTrans.position, backGroundTrans.localScale);
        }
    }

    private void Awake()
    {
        backGroundTrans = this.transform.Find("BackGround");
        ResetMap();
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (!Camera.main) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (!Physics.Raycast(ray, out hitInfo)) return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick(hitInfo);
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick(hitInfo);
        }
    }

    private void HandleLeftClick(RaycastHit hitInfo)
    {
        GMapTile tempTile = hitInfo.collider.GetComponent<GMapTile>();
        if (tempTile == null) return;

        if (isUnit)
        {
            if (tempTile.unitMono == null)
            {
                CreateUnitAtTile(tempTile);
            }
        }
        else
        {
            tempTile.moveCost = tileCost;
            UpdateDebugText(tempTile.index, tileCost);
        }
    }

    private void HandleRightClick(RaycastHit hitInfo)
    {
        GMapTile tempTile = hitInfo.collider.GetComponent<GMapTile>();
        if (tempTile == null) return;

        if (isUnit && tempTile.unitMono != null)
        {
            Destroy(tempTile.unitMono.gameObject);
        }
        else
        {
            tempTile.moveCost = 1;
            UpdateDebugText(tempTile.index, 1);
        }
    }

    private void CreateUnitAtTile(GMapTile tile)
    {
        GameObject unitPrefab = isPlayer ? player : character;
        GameObject unitObj = Instantiate(unitPrefab, unitParent);
        UnitMono unitMono = unitObj.AddComponent<UnitMono>();

        if (char.ToUpper(pid[0]) == 'O')
        {
            unitMono.GetComponent<SpriteRenderer>().color = Color.gray;
        }

        unitMono.pid = pid;
        unitMono.tileIndex = tile.index;
        unitMono.transform.position = tile.transform.position;
        tile.unitMono = unitMono;
    }

    private void UpdateDebugText(int index, int moveCost)
    {
        if (debugTextArray == null || debugTextArray.Length <= index) return;
        debugTextArray[index].text = moveCost.ToString();
    }

    public void Init()
    {
        ResetMap();
        map = new GMap(mapX, mapY);
        GenerateMapTiles();
    }

    private void ResetMap()
    {
        ResetTransform(ref tileParent, "TileParent");
        ResetTransform(ref unitParent, "UnitParent");
        debugTextArray = new TextMesh[mapY * mapX];
    }

    private void ResetTransform(ref Transform parent, string name)
    {
        if (parent != null)
            Destroy(parent.gameObject);

        GameObject newParent = new GameObject(name);
        newParent.transform.parent = transform;
        parent = newParent.transform;
    }

    private void GenerateMapTiles()
    {
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                Vector3 tilePosition = new Vector3(j, i, 0);
                GameObject tileObj = Instantiate(editorTile, tileParent);
                tileObj.transform.position = tilePosition;

                GMapTile tile = tileObj.GetComponent<GMapTile>();
                if (tile == null) continue;

                tile.index = i * mapX + j;
                debugTextArray[i * mapX + j] = Utilitys.CreateWorldText("1", tileParent, tilePosition, 40, Color.white, TextAnchor.MiddleCenter);
            }
        }
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
            UnitMono unit;
            if (characters[i].isPlayer)
                unit = Instantiate(player, unitParent).AddComponent<UnitMono>();
            else
                unit = Instantiate(character, unitParent).GetComponent<UnitMono>();
            unit.pid = characters[i].pid;

            if (char.ToUpper(unit.pid[0]) == 'O')
            {
                unit.GetComponent<SpriteRenderer>().color = Color.gray;
            }
            
            unit.tileIndex = characters[i].tileIndex;
            unit.transform.position = map.tiles[unit.tileIndex].transform.position;
            map.tiles[unit.tileIndex].unitMono = unit;
        }
    }

    private void InitEvent()
    {
        OnGridObjectChanged += (object sender, int eventArgs) =>
        {
            debugTextArray[(int)sender].text = eventArgs.ToString();
            map.tilesList[(int)sender].moveCost = eventArgs;
        };
    }
}
