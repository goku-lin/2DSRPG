using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum MulAttributeKey
{
    maxHp = 0,
    str,
    magic,
    def,
    mdef,
    quick,
    tech
}

[System.Serializable]
public class Role
{
    public int pid;
    public string unitName;
    public int lv;
    public float exp;
    public string jid;
    public int movePower;

    public int maxHp; // 血
    public int hp;
    public int mp;
    public int str; // 攻击，影响造成的伤害
    public int magic;
    public int def; // 防御，减少受到的伤害
    public int mdef;
    public int quick;   //速度，影响命中和闪避
    public int tech;    //技巧，影响必杀和必杀抵抗

    //倍率，用来加算运算的,用枚举
    public float[] multiAttribute = new float[7];

    public Job job;

    public Item[] items = new Item[6];
    public Item equip;

    public List<Skill> commonSkill = new List<Skill>();
    public List<Skill> equipedSkills = new List<Skill>();
    public Dictionary<string, Skill> learnedSkills = new Dictionary<string, Skill>();

    //public int killCount; //击杀次数，需要击杀得到属性改变时使用
    //public int deadCount; //死亡次数，需要死亡得到属性改变时使用

    private Role() { }

    public Role(int characterId)
    {
        this.pid = characterId;
        Person personInfo = this.getBasicInfo();
        this.unitName = personInfo.Name;
        this.jid = personInfo.Jid;
        job = this.getJobInfo();

        this.lv = personInfo.Lv;
        this.exp = 0f;
        this.movePower = personInfo.BaseMove + job.BaseMove;

        this.maxHp = personInfo.BaseHp + job.BaseHp;
        this.hp = this.maxHp;
        this.mp = 100;
        this.str = personInfo.BaseStr + job.BaseStr;
        this.magic = personInfo.BaseMagic + job.BaseMagic;
        this.def = personInfo.BaseDef + job.BaseDef;
        this.mdef = personInfo.BaseMdef + job.BaseMdef;
        this.quick = personInfo.BaseQuick + job.BaseQuick;
        this.tech = personInfo.BaseTech + job.BaseTech;

        string[] sids = personInfo.CommonSids.Split(';');
        for (int i = 0; i < sids.Length - 1; i++)
        {
            Skill skill = new Skill();
            skill.Info = getSkillInfo(sids[i]);
            commonSkill.Add(skill);
            equipedSkills.Add(skill);
            learnedSkills.Add(skill.Info.Sid, skill);
            InitSkillAction(skill);
        }

        sids = job.Skills.Split(';');
        for (int i = 0; i < sids.Length - 1; i++)
        {
            Skill skill = new Skill();
            skill.Info = getSkillInfo(sids[i]);
            learnedSkills.Add(skill.Info.Sid, skill);
            InitSkillAction(skill);
            //TODO:技能怎么处理在这里解决（包括上下代码）
            if (equipedSkills.Count < PlayerData.skillNumber)
                equipedSkills.Add(skill);
        }

        string[] initItems = personInfo.Items.Split(";");
        for (int j = 0; j < this.items.Length; j++)
        {
            if (j >= initItems.Length - 1)
            {
                break;
            }
            this.items[j] = PlayerData.CreateItem(initItems[j]);
            sids = items[j].info.EquipSids.Split(';');
            for (int i = 0; i < sids.Length - 1; i++)
            {
                Skill skill = new Skill();
                skill.Info = getSkillInfo(sids[i]);
                items[j].skills.Add(skill);
                InitSkillAction(skill);
            }
        }

        for (int i = 0; i < this.items.Length; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            if (CanEquip(job, items[i]))
            {
                this.equip = items[i];
                break;
            }
        }

        //this.curWeaponIndex = -1;
        //this.curActState = 0;

        //this.killCount = 0;
        //this.deadCount = 0;
    }

    #region 属性值计算
    public int GetAttack()
    {
        if (job.AttackType == 0)
        {
            return str;
        }
        else if (job.AttackType == 1)
        {
            return magic;
        }
        return 0;
    }

    public int GetDefend(Role attacker)
    {
        if (attacker.job.AttackType == 0)
        {
            return def;
        }
        else if (attacker.job.AttackType == 1)
        {
            return mdef;
        }
        return 0;
    }

    public int calcAtt()
    {
        int attackerAtk = 0;
        int equipPower = 0;
        if (job != null)
        {
            attackerAtk = GetAttack();
            if (equip != null)
            {
                equipPower = equip.info.Power;
            }
        }
        return attackerAtk + equipPower;
    }

    public int calcDef(Role attacker)
    {
        int defend = 0;
        if (attacker.job != null)
        {
            defend = GetDefend(attacker);
        }
        return defend;
    }
    #endregion

    public void InitSkillAction(Skill skill)
    {
        if (skill.Info.SkillType == 1)
        {
            skill.activeSkillAction = new RestoreHealth();
        }
        else if (skill.Info.SkillType == 2)
        {
            skill.activeSkillAction = new DamageSkill();
        }
    }

    public Role clone()
    {
        Role role = new Role();
        role.pid = this.pid;
        role.jid = this.jid;
        role.lv = this.lv;
        role.exp = this.exp;
        role.movePower = this.movePower;

        role.maxHp = this.maxHp;
        role.hp = this.hp;
        role.mp = this.mp;
        role.str = this.str;
        role.magic = this.magic;
        role.def = this.def;
        role.mdef = this.mdef;
        role.quick = this.quick;
        role.tech = this.tech;

        //for (int j = 0; j < this.items.Length; j++)
        //{
        //    Item item = this.items[j];
        //    if (item != null)
        //    {
        //        role.items[j] = PlayerData.CreateItem(item.itemId, item.count);
        //    }
        //    else
        //    {
        //        role.items[j] = null;
        //    }
        //}
        //if (this.equip != null)
        //{
        //    role.equip = PlayerData.CreateItem(this.equip.itemId, this.equip.count);
        //}
        //else
        //{
        //    role.equip = null;
        //}
        //foreach (int key in this.learnedSkills.Keys)
        //{
        //    role.learnedSkills.Add(key, Skill.Clone(this.learnedSkills[key]));
        //}
        //for (int k = 0; k < 6; k++)
        //{
        //    role.equipedSkills[k] = Skill.Clone(this.equipedSkills[k]);
        //}
        //role.curWeaponIndex = this.curWeaponIndex;
        //role.curActState = this.curActState;

        //role.killCount = this.killCount;
        //role.deadCount = this.deadCount;
        return role;
    }

    public void recoverRole(Role role)
    {
        this.maxHp = role.maxHp;
        this.str = role.str;
        this.magic = role.magic;
        this.def = role.def;
        this.mdef = role.mdef;
        this.quick = role.quick;
        this.tech = role.tech;

        for (int i = 0; i < multiAttribute.Length; i++)
        {
            multiAttribute[i] = 0;
        }
    }

    public Person getBasicInfo()
    {
        if (!DataManager.GetInstance().personData.ContainsKey(this.pid))
        {
            Debug.Log("characterId " + this.pid + " not exist!");
        }
        return DataManager.GetInstance().personData[this.pid];
    }

    public Job getJobInfo()
    {
        if (!DataManager.GetInstance().jobData.ContainsKey(this.jid))
        {
            Debug.Log("jid " + this.jid + " not exist!");
        }
        return DataManager.GetInstance().jobData[this.jid];
    }

    public SkillInfo getSkillInfo(string sid)
    {
        if (!DataManager.GetInstance().skillData.ContainsKey(sid))
        {
            Debug.Log("sid " + sid + " not exist!");
        }
        return DataManager.GetInstance().skillData[sid];
    }

    public Dialog getDialogInfo(string did)
    {
        if (!DataManager.GetInstance().dialogData.ContainsKey(did))
        {
            Debug.Log("did " + did + " not exist!");
        }
        return DataManager.GetInstance().dialogData[did];
    }

    public bool CanEquip(Job job, Item item)
    {
        ItemInfo itemInfo = item.info;
        int value;
        switch (itemInfo.Kind)
        {
            case (int)ItemKind.Sword:
                value = job.WeaponSword;
                break;
            case (int)ItemKind.Lance:
                value = job.WeaponLance;
                break;
            case (int)ItemKind.Axe:
                value = job.WeaponAxe;
                break;
            case (int)ItemKind.Bow:
                value = job.WeaponBow;
                break;
            case (int)ItemKind.Dagger:
                value = job.WeaponDagger;
                break;
            case (int)ItemKind.Magic:
                value = job.WeaponMagic;
                break;
            case (int)ItemKind.Fist:
                value = job.WeaponFist;
                break;
            case (int)ItemKind.Special:
                value = job.WeaponSpecial;
                break;
            default:
                return true;
        }
        if (value >= itemInfo.WeaponLevel)
            return true;
        return false;
    }

    public void gainExp(int exp)
    {
        this.exp += (float)exp;
        //TODO:经验判断,在100那里判断
        if (this.exp >= 100 && this.lv < getJobInfo().MaxLevel)
        {
            this.lvUp();
        }
    }

    public void lvUp()
    {
        //TODO:经验可以看看这个
        //this.exp -= (float)CharacterDataManager.GetInstance().lvDatas[this.lv].exp;
        this.exp -= 100;
        this.lv++;
        Person basicInfo = this.getBasicInfo();
        Job jobInfo = this.getJobInfo();

        this.maxHp += (int)(basicInfo.GrowHp + jobInfo.GrowHp);
        this.hp = this.maxHp;
        this.str += (int)(basicInfo.GrowStr + jobInfo.GrowStr);
        this.magic += (int)(basicInfo.GrowMagic + jobInfo.GrowMagic);
        this.def += (int)(basicInfo.GrowDef + jobInfo.GrowDef);
        this.mdef += (int)(basicInfo.GrowMdef + jobInfo.GrowMdef);
        this.quick += (int)(basicInfo.GrowQuick + jobInfo.GrowQuick);
        this.tech += (int)(basicInfo.GrowTech + jobInfo.GrowTech);

        //for (int i = 0; i < classInfo.learnableSkills.Count; i++)
        //{
        //    int num = classInfo.learnableSkills[i];
        //    if (!this.learnedSkills.ContainsKey(num))
        //    {
        //        Skill skill = new Skill(num);
        //        SkillData data = skill.getData();
        //        if (data.learnType == 1 && (float)this.lv >= data.learnParams[0])
        //        {
        //            this.learnNewSkill(skill, true, false);
        //        }
        //    }
        //}
        //PlayerData.TestAchievement(2, this.lv);
    }

    public void writeRecord(BinaryWriter writer)
    {
        writer.Write(this.pid);
        writer.Write(this.lv);
        writer.Write(this.exp);
        writer.Write(this.jid);

        for (int i = 0; i < multiAttribute.Length; i++)
        {
            writer.Write(this[i]);
        }

        for (int j = 0; j < this.items.Length; j++)
        {
            bool flag = this.items[j] != null;
            writer.Write(flag);
            if (flag)
            {
                this.items[j].saveRecord(writer);
            }
        }
        bool flag2 = this.equip != null;
        writer.Write(flag2);
        if (flag2)
        {
            this.equip.saveRecord(writer);
        }
        //技能保存得多写一个类，和item一样
        writer.Write(this.learnedSkills.Count);
        foreach (Skill skill in this.learnedSkills.Values)
        {
            skill.saveRecord(writer);
        }
        writer.Write(this.equipedSkills.Count);
        for (int k = 0; k < this.equipedSkills.Count; k++)
        {
            Skill skill2 = this.equipedSkills[k];
            skill2.saveRecord(writer);
        }

        //writer.Write(this.killCount);
        //writer.Write(this.deadCount);
    }

    public static Role readRecord(BinaryReader reader)
    {
        int tempPid = reader.ReadInt32();
        Role role = new Role(tempPid);
        role.lv = reader.ReadInt32();
        role.exp = reader.ReadSingle();
        role.jid = reader.ReadString();
        //TODO:这里数字7，因为不能用非静态的字段
        for (int i = 0; i < 7; i++)
        {
            role[i] = reader.ReadInt32();
        }

        for (int j = 0; j < role.items.Length; j++)
        {
            bool flag = reader.ReadBoolean();
            role.items[j] = ((!flag) ? null : Item.readRecord(reader));
        }
        bool flag2 = reader.ReadBoolean();
        role.equip = ((!flag2) ? null : Item.readRecord(reader));

        int num2 = reader.ReadInt32();
        role.learnedSkills = new Dictionary<string, Skill>();
        for (int k = 0; k < num2; k++)
        {
            Skill skill = Skill.readRecord(reader);
            role.learnedSkills[skill.Info.Sid] = skill;
        }
        int num3 = reader.ReadInt32();
        role.equipedSkills = new List<Skill>();
        for (int l = 0; l < num3; l++)
        {
            Skill skill2 = Skill.readRecord(reader);
            role.equipedSkills.Add(skill2);
        }

        //role.killCount = reader.ReadInt32();
        //role.deadCount = reader.ReadInt32();
        //role.dataLock = true;
        return role;
    }


    public int this[int index]
    {
        get
        {
            if (index == (int)MulAttributeKey.maxHp) return this.maxHp;
            else if (index == (int)MulAttributeKey.str) return this.str;
            else if (index == (int)MulAttributeKey.magic) return this.magic;
            else if (index == (int)MulAttributeKey.def) return this.def;
            else if (index == (int)MulAttributeKey.mdef) return this.mdef;
            else if (index == (int)MulAttributeKey.quick) return this.quick;
            else if (index == (int)MulAttributeKey.tech) return this.tech;
            else
            {
                throw new System.IndexOutOfRangeException();
            }
        }
        set
        {
            if (index == (int)MulAttributeKey.maxHp) this.maxHp = value;
            else if (index == (int)MulAttributeKey.str) this.str = value;
            else if (index == (int)MulAttributeKey.magic) this.magic = value;
            else if (index == (int)MulAttributeKey.def) this.def = value;
            else if (index == (int)MulAttributeKey.mdef) this.mdef = value;
            else if (index == (int)MulAttributeKey.quick) this.quick = value;
            else if (index == (int)MulAttributeKey.tech) this.tech = value;
            else
            {
                throw new System.IndexOutOfRangeException();
            }
        }
    }
}
