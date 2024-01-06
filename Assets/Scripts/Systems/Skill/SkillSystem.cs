using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExecuteTiming
{
    FightingStart,
    FightingEnd,
    CD_Down
}



public class SkillSystem : Singleton<SkillSystem>
{
    internal void Init()
    {
        EventDispatcher.instance.Regist<Character, Character>(GameEventType.battle_Start, this.Battle_Start);

        EventDispatcher.instance.Regist<Character, Character>(GameEventType.battle_End, this.Battle_End);
    }

    private void Battle_End(Character from, Character to)
    {
        from.getRole().recoverRole(from.beforeRole);
        to.getRole().recoverRole(to.beforeRole);

        //攻击者  执行犀甲 的完毕 功能
        var states1 = GetSkillState(from.getRole(), ExecuteTiming.FightingEnd);
        foreach (Skill state in states1)
        {
            CastSkill(state.Info, from.getRole(), to.getRole());
        }

        //考虑对方也存在被动技能
        var states2 = GetSkillState(to.getRole(), ExecuteTiming.FightingEnd);
        foreach (Skill state in states2)
        {
            CastSkill(state.Info, to.getRole(), from.getRole()); ;
        }
    }

    private void Battle_Start(Character unitfrom, Character unitto)
    {
        Role from = unitfrom.getRole();
        Role to = unitto.getRole();
        //战斗前 双方执行 被动技能的 节点逻辑
        var states1 = GetSkillState(from, ExecuteTiming.FightingStart);
        foreach (Skill state in states1)
        {
            CastSkill(state.Info, from, to);
        }

        //考虑对方也存在被动技能
        var states2 = GetSkillState(to, ExecuteTiming.FightingStart);
        foreach (Skill state in states2)
        {
            CastSkill(state.Info, to, from);;
        }

        for (int i = 0; i < from.multiAttribute.Length; i++)
        {
            from[i] = (int)(from[i] * (1 + from.multiAttribute[i]));
            to[i] = (int)(to[i] * (1 + to.multiAttribute[i]));
        }
    }

    Dictionary<string, Action<Role, int>> setters = new Dictionary<string, Action<Role, int>>()
        {
            { "MaxHp", (role, value) => role.maxHp = value },
            { "HP", (role, value) => role.hp = value },
            { "力", (role, value) => role.str = value },
            { "魔力", (role, value) => role.magic = value },
            { "防御", (role, value) => role.def = value },
            { "魔防", (role, value) => role.mdef = value },
            { "速度", (role, value) => role.quick = value },
            { "技巧", (role, value) => role.tech = value }
            // 添加其他属性的映射
        };

    Dictionary<string, Func<Role, int>> getters = new Dictionary<string, Func<Role, int>>()
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

    //加算赋值处
    Dictionary<string, Action<Role, float>> MulSetters = new Dictionary<string, Action<Role, float>>()
        {
            { "MaxHp", (role, value) => role.multiAttribute[0] = value },
            { "力", (role, value) => role.multiAttribute[1] = value },
            { "魔力", (role, value) => role.multiAttribute[2] = value },
            { "防御", (role, value) => role.multiAttribute[3] = value },
            { "魔防", (role, value) => role.multiAttribute[4] = value },
            { "速度", (role, value) => role.multiAttribute[5] = value },
            { "幸运", (role, value) => role.multiAttribute[6] = value },
            { "技巧", (role, value) => role.multiAttribute[7] = value }
            // 添加其他属性的映射
        };

    Dictionary<string, Func<Role, float>> MulGetters = new Dictionary<string, Func<Role, float>>()
        {
            { "MaxHp", role => role.multiAttribute[0] },
            { "力", role => role.multiAttribute[1] },
            { "魔力", role => role.multiAttribute[2] },
            { "防御", role => role.multiAttribute[3] },
            { "魔防", role => role.multiAttribute[4] },
            { "速度", role => role.multiAttribute[5] },
            { "幸运", role => role.multiAttribute[6] },
            { "技巧", role => role.multiAttribute[7] }
            // 添加其他属性的映射
        };

    public void CastSkill(SkillInfo skill, Role from, Role to)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        if ((bool)dt.Compute(Utilitys.TranslateString(skill.Condition, from, to), null))
        {
            string[] actNames = skill.ActNames.Split(';');
            string[] actOperations = skill.ActOperations.Split(';');
            string[] actValues = skill.ActValues.Split(';');
            //-1是因为都用分号的话，最后一个必为空
            for (int i = 0; i < actNames.Length - 1; i++)
            {
                Role target;
                if (actNames[i].Length > 3 && actNames[i][0..3] == "相手の")
                {
                    target = to;
                }
                else
                {
                    target = from;
                }
                switch (actOperations[i])
                {
                    case "=":
                        setters[actNames[i]](target, (int)dt.Compute(Utilitys.TranslateString(actValues[i], from, to), null));
                        Debug.Log(getters[actNames[i]](target));
                        break;
                    case "+":
                        setters[actNames[i]](target, getters[actNames[i]](target) + (int)dt.Compute(Utilitys.TranslateString(actValues[i], from, to), null));
                        Debug.Log(getters[actNames[i]](target));
                        break;
                    case "-":
                        setters[actNames[i]](target, getters[actNames[i]](target) - (int)dt.Compute(Utilitys.TranslateString(actValues[i], from, to), null));
                        Debug.Log(getters[actNames[i]](target));
                        break;
                    case "*":
                        int x = (int)(getters[actNames[i]](target) * Convert.ToDouble(dt.Compute(Utilitys.TranslateString(actValues[i], from, to), null).ToString()));
                        setters[actNames[i]](target, x);
                        Debug.Log(getters[actNames[i]](target));
                        break;
                    case "#":
                        MulSetters[actNames[i]](target, MulGetters[actNames[i]](target) + (float)Convert.ToDouble(dt.Compute(Utilitys.TranslateString(actValues[i], from, to), null)));
                        break;
                }
            }
        }
        if (skill.SkillType != 0)
        {
            if (skill.RangeTarget == 1)
            {

            }
            else if (skill.RangeTarget == 2)
            {
                
            }
        }
    }

    public List<Skill> GetSkillState(Role player, ExecuteTiming executeTiming)
    {
        //throw new NotImplementedException();
        //查找开始战斗执行时机的技能状态
        List<Skill> n = new List<Skill>();
        foreach (Skill skill in player.equipedSkills)
        {
            if (skill != null)
            {
                if ((ExecuteTiming)skill.Info.Timing == executeTiming)
                    n.Add(skill);
            }
        }
        return n;
    }

    public void Releaseskill(Skill usingSkill, Character from, List<Character> filterPlayers)
    {
        usingSkill.activeSkillAction.Releaseskill(from, filterPlayers, usingSkill);

        usingSkill.cd = usingSkill.Info.Cycle;
        from.getRole().mp -= usingSkill.Info.MPCost;
        Debug.Log(from.getRole().mp);
    }
}
