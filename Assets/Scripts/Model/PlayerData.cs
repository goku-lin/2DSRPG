using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerData
{
    public static SortedDictionary<int, Role> Army = new SortedDictionary<int, Role>();
    public static Dictionary<int, Role> TempArmy = new Dictionary<int, Role>();
    public static List<int> ArmyIndexes = new List<int>();
    /// <summary>仓库</summary>
    public static SortedDictionary<long, Item> Warehouse = new SortedDictionary<long, Item>();
    public static Dictionary<int, int> ShopStorage = new Dictionary<int, int>();
    private static long GeneratedItemUID = 0L;
    public static long Money;
    public const int skillNumber = 3;

    public static void Init()
    {
        PlayerData.Army = new SortedDictionary<int, Role>();
        PlayerData.ArmyIndexes = new List<int>();
        PlayerData.TempArmy = new Dictionary<int, Role>();
        PlayerData.Warehouse = new SortedDictionary<long, Item>();
        PlayerData.ShopStorage = new Dictionary<int, int>();
        PlayerData.GeneratedItemUID = 0L;
        PlayerData.Money = 0L;
        //TODO:后面找找怎么加角色好
        Army.Add(1, new Role(1));
        Army.Add(2, new Role(2));
    }

    public static Role GetRole(int characterId, int actorId)
    {
        if (characterId < 0)
        {
            return null;
        }
        if (PlayerData.Army.ContainsKey(characterId))
        {
            return PlayerData.Army[characterId];
        }
        if (!PlayerData.TempArmy.ContainsKey(actorId))
        {
            Role value = new Role(characterId);
            PlayerData.TempArmy[actorId] = value;
        }
        return PlayerData.TempArmy[actorId];
    }

    /// <summary>
    /// 创立物品
    /// </summary>
    /// <param name="itemId">id</param>
    /// <param name="count">剩余使用次数</param>
    /// <returns></returns>
    public static Item CreateItem(string iid, int count = 1)
    {
        long generatedItemUID = PlayerData.GeneratedItemUID;
        PlayerData.GeneratedItemUID = generatedItemUID + 1L;
        Item item = new Item();
        item.uid = generatedItemUID;
        item.info = getItemInfo(iid);
        item.info.Endurance = count;

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

}
