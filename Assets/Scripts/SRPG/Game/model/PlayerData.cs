using Game;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData
{
    public static Dictionary<string, Role> Army = new Dictionary<string, Role>();
    /// <summary>
    /// 临时队伍，战斗的时候用
    /// </summary>
    public static Dictionary<int, Role> TempArmy = new Dictionary<int, Role>();
    public static List<int> ArmyIndexes = new List<int>();
    //已阵亡角色
    public static Dictionary<string, Role> Graveyard = new Dictionary<string, Role>();
    /// <summary>仓库</summary>
    public static SortedDictionary<long, Item> Warehouse = new SortedDictionary<long, Item>();
    public static Dictionary<int, int> ShopStorage = new Dictionary<int, int>();
    private static long GeneratedItemUID = 0L;
    public static long Money;
    public const int skillNumber = 3;
    public static int CurChapter;
    public static int CurLoc;
    public static long TotalGameTime;
    public static int CurDifficulty = 0;
    public static byte[] recordLoaderArray = null;

    public static void Init()
    {
        PlayerData.Army = new Dictionary<string, Role>();
        PlayerData.ArmyIndexes = new List<int>();
        PlayerData.TempArmy = new Dictionary<int, Role>();
        PlayerData.Warehouse = new SortedDictionary<long, Item>();
        PlayerData.ShopStorage = new Dictionary<int, int>();
        PlayerData.GeneratedItemUID = 0L;
        PlayerData.Money = 0L;
        //TODO:后面找找怎么加角色好
        Army.Add("PID_遠坂凛", new Role("PID_遠坂凛"));
        Army.Add("PID_遠坂桜", new Role("PID_遠坂桜"));
        Army.Add("PID_锦木千束", new Role("PID_锦木千束"));
        Army.Add("PID_井上泷奈", new Role("PID_井上泷奈"));
    }

    public static Role GetRole(string pid, int uid)
    {
        if (pid == null)
        {
            return null;
        }
        if (PlayerData.Army.ContainsKey(pid))
        {
            return PlayerData.Army[pid];
        }
        //临时队伍
        if (!PlayerData.TempArmy.ContainsKey(uid))
        {
            Role value = new Role(pid);
            PlayerData.TempArmy[uid] = value;
        }
        return PlayerData.TempArmy[uid];
    }

    /// <summary>
    /// 创立物品
    /// </summary>
    /// <param name="itemId">id</param>
    /// <param name="count">剩余使用次数</param>
    /// <returns></returns>
    public static Item CreateItem(string iid)
    {
        long generatedItemUID = PlayerData.GeneratedItemUID;
        PlayerData.GeneratedItemUID = generatedItemUID + 1L;
        Item item = new Item();
        item.uid = generatedItemUID;
        item.info = getItemInfo(iid);
        item.count = item.info.Endurance;

        return item;
    }

    public static ItemInfo getItemInfo(string iid)
    {
        if (!DataManager.GetInstance().itemData.ContainsKey(iid))
        {
            Debug.Log("iid " + iid + " not exist!");
        }
        return DataManager.GetInstance().itemData[iid];
    }

    public static bool SaveRecord(int recordIndex, RecordInfo info, bool saveLevelRecord, int beginEmbattleState)
    {
        string recordFilename = info.getFilename();
        if (recordIndex == -1)
        {
            recordFilename = "Rah";
        }
        else if (recordIndex == -2)
        {
            recordFilename = "Ral";
        }
        bool flag = PlayerData.Save(recordFilename, saveLevelRecord, beginEmbattleState);
        //if (flag)
        //{
        //    if (recordIndex == -1)
        //    {
        //        PlayerSetting.AutoSaveHome = info;
        //    }
        //    else if (recordIndex == -2)
        //    {
        //        PlayerSetting.AutoSaveLevel = info;
        //    }
        //}
        //else if (recordIndex == -1)
        //{
        //    PlayerSetting.AutoSaveHome = null;
        //}
        //else if (recordIndex == -2)
        //{
        //    PlayerSetting.AutoSaveLevel = null;
        //}
        //else
        //{
        //    PlayerSetting.Records.RemoveAt(recordIndex);
        //}
        //PlayerData.CheckCleanUp();
        //bool flag2 = PlayerSetting.SaveSetting();
        //return flag && flag2;
        return flag;
    }

    private static bool Save(string recordFilename, bool saveLevelRecord, int beginEmbattleState)
    {
        bool result = false;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8);
            binaryWriter.Write(65);
            PlayerData.WriteRecord(binaryWriter, beginEmbattleState);
            GameLogic.WriteRecord(binaryWriter);
            binaryWriter.Write(saveLevelRecord);
            if (saveLevelRecord)
            {
                GLevel curLevel = GameLogic.CurLevel;
                binaryWriter.Write(curLevel.levelName);
                //if (PlayerData.BattleRestartRecordArray == null)
                //{
                //    binaryWriter.Write(0);
                //}
                //else
                //{
                //    binaryWriter.Write(PlayerData.BattleRestartRecordArray.Length);
                //    binaryWriter.Write(PlayerData.BattleRestartRecordArray);
                //}
                //BattleSceneController.instance.writeRecord(binaryWriter);
            }
            result = Utilitys.Save(recordFilename, memoryStream.ToArray());
            binaryWriter.Close();
            memoryStream.Close();
        }
        return result;
    }

    public static void WriteRecord(BinaryWriter writer, int beginEmbattleState)
    {
        writer.Write(PlayerData.Army.Count);
        foreach (Role role in PlayerData.Army.Values)
        {
            role.writeRecord(writer);
        }
        writer.Write(PlayerData.ArmyIndexes.Count);
        for (int i = 0; i < PlayerData.ArmyIndexes.Count; i++)
        {
            writer.Write(PlayerData.ArmyIndexes[i]);
        }
        writer.Write(PlayerData.Graveyard.Count);
        foreach (Role role2 in PlayerData.Graveyard.Values)
        {
            role2.writeRecord(writer);
        }
        writer.Write(PlayerData.GeneratedItemUID);
        //writer.Write(PlayerData.CurDifficulty);
        writer.Write(PlayerData.Money);
        //writer.Write(PlayerData.CurChapter);
        //writer.Write(PlayerData.CurLoc);
        //writer.Write(PlayerData.TotalGameTime);
        writer.Write(PlayerData.Warehouse.Count);
        foreach (Item item in PlayerData.Warehouse.Values)
        {
            item.saveRecord(writer);
        }
    }

    public static bool LoadRecord(int recordIndex, string recordFilename)
    {
        if (!Utilitys.FileExistsInPersistentDataPath(recordFilename))
        {
            //MenuHelper.PopCommonInfo("Record File Missing!");
            Debug.Log("文件不存在");
            return false;
        }
        PlayerData.recordLoaderArray = Utilitys.Load(recordFilename);
        if (PlayerData.recordLoaderArray == null)
        {
            //MenuHelper.PopCommonInfo("Record File Missing!");
            Debug.Log("Record File Missing!");
            return false;
        }
        //PlayerData.recordVersion = -1;
        //PlayerData.AutoSaveBufferInfo = null;
        //PlayerData.AutoSaveBufferArray = null;
        using (MemoryStream memoryStream = new MemoryStream(PlayerData.recordLoaderArray))
        {
            BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);
            int num = binaryReader.ReadInt32();
            if (num < 51 || num > 65)
            {
                Debug.Log("Load wrong record version!!");
                binaryReader.Close();
                memoryStream.Close();
                //if (num < 51)
                //{
                //    PlayerSetting.DeleteRecord(recordIndex, true);
                //    MenuHelper.PopCommonInfo("DELETE WRONG VERSION RECORD!");
                //}
                //else
                //{
                //    MenuHelper.PopCommonInfo(Singleton<LanguageHelper>.Instance.GetStaticString("UI_MSG_HINT_WRONG_RECORD_VERSION"));
                //}
                return false;
            }
            //PlayerData.recordVersion = num;
            PlayerData.ReadRecord(binaryReader);
            GameLogic.ReadRecord(binaryReader);
            //    //GameLogic.ReplayMusic();
            //    //GameLogic.AutoEmptScene = GameLogic.curEmptyScene;
            //    bool flag = binaryReader.ReadBoolean();
            //    if (flag && levelLoader != null)
            //    {
            //        string text = binaryReader.ReadString();
            //        GLevelInfoData levelInfoData = LevelGlobalManager.GetInstance().getLevelInfoData(text);
            //        if (!PlayerData.IsDataAvailableDLC(levelInfoData.DLCId))
            //        {
            //            binaryReader.Close();
            //            memoryStream.Close();
            //            MenuHelper.PopGetInfo(GET_INFO_TYPE.HINT, string.Empty, LevelGlobalManager.GetInstance().getTextData(800016).text.ToString(), string.Empty, LevelGlobalManager.GetInstance().getTextData(800027).text.ToString());
            //            return false;
            //        }
            //        int num2 = binaryReader.ReadInt32();
            //        PlayerData.BattleRestartRecordArray = ((num2 <= 0) ? null : binaryReader.ReadBytes(num2));
            //        PlayerData.recordLevelStartIndex = memoryStream.Position;
            //        binaryReader.Close();
            //        memoryStream.Close();
            //        levelLoader.GetComponentInChildren<LevelLoaderView>().levelName = text;
            //        levelLoader.show();
            //    }
            //    else
            //    {
            //        foreach (Role role in PlayerData.Army.Values)
            //        {
            //            role.dataLock = false;
            //        }
            //        binaryReader.Close();
            //        memoryStream.Close();
            //        if (homeLoader != null)
            //        {
            //            homeLoader.show();
            //            GameLogic.AutoClickButton = MenuHelper.GetButtonIDByLocID(PlayerData.CurLoc);
            //        }
            //        GameLogic.CurLevel = null;
            //    }
        }
        //PlayerData.VerifyData();
        return true;
    }

    public static void ReadRecord(BinaryReader reader)
    {
        PlayerData.Init();
        int num3 = reader.ReadInt32();
        for (int k = 0; k < num3; k++)
        {
            Role role = Role.readRecord(reader);
            PlayerData.Army[role.pid] = role;
        }
        int num4 = reader.ReadInt32();
        for (int l = 0; l < num4; l++)
        {
            int item = reader.ReadInt32();
            PlayerData.ArmyIndexes.Add(item);
        }
        int num5 = reader.ReadInt32();
        for (int m = 0; m < num5; m++)
        {
            Role role2 = Role.readRecord(reader);
            PlayerData.Graveyard[role2.pid] = role2;
        }
        PlayerData.GeneratedItemUID = reader.ReadInt64();
        //PlayerData.CurDifficulty = reader.ReadInt32();
        PlayerData.Money = reader.ReadInt64();
        //PlayerData.CurChapter = reader.ReadInt32();
        //PlayerData.SetCurLoc(reader.ReadInt32());
        //PlayerData.TotalGameTime = reader.ReadInt64();
        int num6 = reader.ReadInt32();
        for (int n = 0; n < num6; n++)
        {
            Item item2 = Item.readRecord(reader);
            PlayerData.Warehouse[item2.uid] = item2;
        }
    }

}
