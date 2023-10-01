using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;

public class Utilitys
{
    public static readonly string SEPARATOR = string.Empty + Path.DirectorySeparatorChar;

    #region 保存和读取
    //填充Level类数据
    public static void FillLevel(string fileName, ref GLevel level)
    {
        FileInfo file = new FileInfo(fileName);
        StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument doc = new XmlDocument();
        doc.Load(sr);

        level.levelName = doc.SelectSingleNode("/Level/Name").InnerText;
        level.background = doc.SelectSingleNode("/Level/Background").InnerText;

        level.pos = new Vector3(float.Parse(doc.SelectSingleNode("/Level/Pos").Attributes["x"].Value),
            float.Parse(doc.SelectSingleNode("/Level/Pos").Attributes["y"].Value),
              float.Parse(doc.SelectSingleNode("/Level/Pos").Attributes["z"].Value));

        level.size = new Vector3(float.Parse(doc.SelectSingleNode("/Level/Size").Attributes["x"].Value),
            float.Parse(doc.SelectSingleNode("/Level/Size").Attributes["y"].Value),
              float.Parse(doc.SelectSingleNode("/Level/Size").Attributes["z"].Value));

        level.map = new GMap(
            int.Parse(doc.SelectSingleNode("/Level/Map/MapX").InnerText),
            int.Parse(doc.SelectSingleNode("/Level/Map/MapY").InnerText));

        XmlNodeList nodes;
        nodes = doc.SelectNodes("/Level/Map/Tiles/tile");
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            tile p = new tile(
                int.Parse(node.Attributes["index"].Value),
                int.Parse(node.Attributes["cost"].Value));

            level.map.tilesList.Add(p);
        }

        sr.Close();
        sr.Dispose();
    }

    public static void SaveLevel(string fileName, GLevel level)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sb.AppendLine("<Level>");

        sb.AppendLine(string.Format("<Name>{0}</Name>", level.levelName));
        sb.AppendLine(string.Format("<Background>{0}</Background>", level.background));
        sb.AppendLine(string.Format("<Pos x=\"{0}\" y=\"{1}\" z=\"{0}\"/>", level.pos.x, level.pos.y, level.pos.z));
        sb.AppendLine(string.Format("<Size x=\"{0}\" y=\"{1}\" z=\"{0}\"/>", level.size.x, level.size.y, level.size.z));

        sb.AppendLine("<Map>");
        sb.AppendLine(string.Format("<MapX>{0}</MapX>", level.map.x));
        sb.AppendLine(string.Format("<MapY>{0}</MapY>", level.map.y));
        sb.AppendLine("<Tiles>");
        for (int i = 0; i < level.map.tilesList.Count; i++)
        {
            sb.AppendLine(string.Format("<tile index=\"{0}\" cost=\"{1}\"/>", level.map.tilesList[i].index, level.map.tilesList[i].moveCost));
        }
        sb.AppendLine("</Tiles>");
        sb.AppendLine("</Map>");
        sb.AppendLine("</Level>");

        string content = sb.ToString();

        StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);
        sw.Write(content);
        sw.Flush();
        sw.Dispose();
    }

    public static void SaveUnit(string fileName, GLevel level)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sb.AppendLine("<Level>");

        sb.AppendLine(string.Format("<Name>{0}</Name>", level.levelName));

        sb.AppendLine("<Units>");
        for (int i = 0; i < level.map.tilesList.Count; i++)
        {
            if (level.map.tiles[i].character != null)
                sb.AppendLine(string.Format("<unit name=\"{0}\" index=\"{1}\" isPlayer=\"{2}\"/>",
                    level.map.tiles[i].character.unitName,
                    level.map.tiles[i].character.tileIndex,
                    level.map.tiles[i].character.gameObject.CompareTag("Player")));
        }
        sb.AppendLine("</Units>");

        sb.AppendLine("</Level>");

        string content = sb.ToString();

        StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);
        sw.Write(content);
        sw.Flush();
        sw.Dispose();
    }

    //填充Level类数据
    public static List<UnitInfo> LoadUnit(string fileName)
    {
        FileInfo file = new FileInfo(fileName);
        StreamReader sr = new StreamReader(file.OpenRead(), Encoding.UTF8);

        XmlDocument doc = new XmlDocument();
        doc.Load(sr);
        XmlNodeList nodes;

        nodes = doc.SelectNodes("/Level/Units/unit");

        List<UnitInfo> characters = new List<UnitInfo>();
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            UnitInfo p = new UnitInfo(
                node.Attributes["name"].Value,
                int.Parse(node.Attributes["index"].Value),
                bool.Parse(node.Attributes["isPlayer"].Value));

            characters.Add(p);
        }

        sr.Close();
        sr.Dispose();
        return characters;
    }

    #endregion

    #region 可视化
    public const int sortingOrderDefault = 5000;
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        textMesh.characterSize = 0.1f;

        return textMesh;
    }

    public static IEnumerator LoadImage(string url, SpriteRenderer render)
    {
        WWW www = new WWW(url);

        while (!www.isDone)
            yield return www;

        Texture2D texture = www.texture;
        Sprite sp = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f), 128);
        render.sprite = sp;
    }

    #endregion

    #region 数据处理
    static Dictionary<string, Func<Role, int>> getters = new Dictionary<string, Func<Role, int>>()
        {
            { "MaxHp", role => role.maxHp },
            { "HP", role => role.hp },
            { "力", role => role.str },
            { "魔力", role => role.magic },
            { "防御", role => role.def },
            { "魔防", role => role.mdef },
            { "速度", role => role.quick },
            { "技巧", role => role.tech }
            // 添加其他属性的映射
        };

    static List<char> nums = new List<char>()
    {
        '+', '-', '*', '/', '>', '<', '='
    };

    public static string TranslateString(string operateString, Role nowRole, Role targetRole)
    {
        if (operateString == "") return true.ToString();
        Role role;
        //去掉空格
        string translateStr = operateString.Replace(" ", "");
        //替换开始点、替换结束点、是否需要替换
        int start = 0;
        int end = 0;
        bool needReplace = false;
        for (int i = 0; i < translateStr.Length; i++)
        {
            //如果是符号和最后就判断一次
            if (nums.Contains(translateStr[i]))
            {
                start = end;
                end = i;
                //Debug.Log(translateStr[start..end]);
                needReplace = true;
            }
            if (i == translateStr.Length - 1)
            {
                start = end;
                end = i + 1;
                //Debug.Log(translateStr[start..end]);
                needReplace = true;
            }
            if (needReplace)
            {
                needReplace = false;
                while (start < end && nums.Contains(translateStr[start]))
                {
                    start++;
                }
                //进行替换
                int temp = start + 3;
                if (temp < end && translateStr[start..temp] == "相手の")
                {
                    role = targetRole;
                    translateStr = translateStr[..start] + role.maxHp.ToString() + translateStr[temp..];
                    end -= 3;
                }
                else role = nowRole;
                //根据字典读出需要的数据
                if (getters.ContainsKey(translateStr[start..end]))
                {
                    i = start + 1 + getters[translateStr[start..end]](role).ToString().Length;
                    translateStr = translateStr[..start] + getters[translateStr[start..end]](role) + translateStr[end..];
                }
                //if (translateStr[start..end] == "MaxHp")
                //{
                //    translateStr = translateStr[..start] + role.maxHp.ToString() + translateStr[end..];
                //    i = start + 1 + role.maxHp.ToString().Length;
                //}
                end = i;
            }
        }
        return translateStr;
    }
    #endregion

    public static bool fileAccess
    {
        get
        {
            return Application.platform != RuntimePlatform.WebGLPlayer;
        }
    }

    public static bool FileExistsInPersistentDataPath(string fileName)
    {
        return File.Exists(Application.persistentDataPath + Utilitys.SEPARATOR + fileName);
    }

    public static bool Save(string fileName, byte[] bytes)
    {
        if (!Utilitys.fileAccess)
        {
            return false;
        }
        string text = Application.persistentDataPath + "/" + fileName;
        if (bytes == null)
        {
            if (File.Exists(text))
            {
                File.Delete(text);
            }
            return true;
        }
        string text2 = text + "_tmp";
        if (File.Exists(text2))
        {
            File.Delete(text2);
        }
        try
        {
            FileStream fileStream = File.Create(text2);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();
            fileStream.Close();
            if (File.Exists(text))
            {
                File.Delete(text);
            }
            File.Move(text2, text);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
        Debug.Log(text);
        return true;
    }

    public static byte[] Load(string fileName)
    {
        if (!Utilitys.fileAccess)
        {
            return null;
        }
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        return null;
    }
}
