using System.Collections;
using System.Collections.Generic;

public enum ItemKind
{
    None = 0,
    Sword = 1,
    Lance = 2,
    Axe = 3,
    Bow = 4,
    Dagger = 5,
    Magic = 6,
    Fist = 7,
    Special = 8,
    Tool = 9,
    Exp = 10,
}

public class Item
{
    public ItemInfo info;
    public long uid;     // 装备编号
    public List<Skill> skills = new List<Skill>();
    //TODO:强化等级，实现思路：在itemInfo加入要改变的对应等级数值,与下面的数值相加，调用时加入这些数值即可
    public int forgeLevel = 0;
    public int addPower;
    public int addWeight;
    public int addRangeI;
    public int addRangeO;
    public int addHit;
    public int addCritical;
    public int addAvoid;
    public int addSecure;

}
