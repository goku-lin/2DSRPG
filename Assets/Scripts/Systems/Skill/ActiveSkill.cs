using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum SkillTarget
{
    //盟军
    OnlyFrom = 0,
    Friendly = 1,
    Enemy = 2,
    Tile,
    All,

}

//UI选择技能，到battle manager调用选择范围，选择释放后在character里面选择目标单位

public abstract class ActiveSkillAction
{
    public DataTable dt = new DataTable();

    public abstract void Releaseskill(Character from, List<Character> to, Skill skill);


    public abstract void BeforReleaseskill(Character from, List<Character> to);
}

public class RestoreHealth : ActiveSkillAction
{
    public override void BeforReleaseskill(Character from, List<Character> to)
    {
        EffectCtrl.instance.ShowMagicCircleSimpleGreen(from);
    }

    public override void Releaseskill(Character from, List<Character> to, Skill skill)
    {
        string[] actvalues = skill.Info.ActValues.Split(';');
        foreach (var player in to)
        {
            int addHp = (int)dt.Compute(Utilitys.TranslateString(actvalues[0], from.getRole(), player.getRole()), null);
            player.RestoreHealth(addHp);
            EffectCtrl.instance.ShowRestoreHealth(player);
        }
    }
}

public class AddBuff : ActiveSkillAction
{
    public override void BeforReleaseskill(Character from, List<Character> to)
    {
        EffectCtrl.instance.ShowMagicCircleSimpleGreen(from);
    }

    public override void Releaseskill(Character from, List<Character> to, Skill skill)
    {
        var addHp = 0;
        foreach (var player in to)
        {
            player.RestoreHealth(addHp);
            EffectCtrl.instance.ShowRestoreHealth(player);
        }
    }
}


public class RestoreHealthEffectBig : RestoreHealth
{
    public override void Releaseskill(Character from, List<Character> to, Skill skill)
    {
        base.Releaseskill(from, to, skill);

        EffectCtrl.instance.ShowRestoreHealthBig(from);

    }
}


public enum SkillType
{
    None,
    RestoreHealth,
    Attack,
}


public class ActiveSkillConfig
{
    //以人物为中心的方范围
    public uint releaseRange = 1;

    //作用范围
    public uint actionRange = 1;

    public SkillTarget skillTarget;

    public ActiveSkillAction activeSkillAction;
    internal int cd_config;
    internal int cd=0;

    public SkillType skillType;


    public bool canSelect(Character from, Character to)
    {
        if (skillTarget == SkillTarget.Friendly)
        {
            return from.sect == to.sect;
        }
        else
        {
            return from.sect != to.sect;
        }
    }
}


public class DamageSkill : ActiveSkillAction
{
    public override void BeforReleaseskill(Character from, List<Character> to)
    {
        EffectCtrl.instance.playeffect = true;
        EffectCtrl.instance.ShowMagicCircleSimpleGreen(from);
    }

    public override void Releaseskill(Character from, List<Character> to, Skill skill)
    {
        BattleManager.Instance.StartCoroutine(C_ShowTime(from, to, skill.Info));
    }

    IEnumerator C_ShowTime(Character from, List<Character> to, SkillInfo skill)
    {
        //镜头震动
        CameraCtrl.Instance.Shake(0.5f, 0.3f);
        string[] actvalues = skill.ActValues.Split(';');
        foreach (Character player in to)
        {
            int damage = (int)dt.Compute(Utilitys.TranslateString(actvalues[0], from.getRole(), player.getRole()), null);
            BattleManager.Instance.StartCoroutine(C_showTime_1(damage, player));
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1.3f);
        //特效播放完才可以操作
        EffectCtrl.instance.playeffect = false;
    }

    IEnumerator C_showTime_1(int damage, Character to)
    {
        EffectCtrl.instance.ShowFireFall(to, 1.5f);

        yield return new WaitForSeconds(1.3f);
        EffectCtrl.instance.ShowMysticExplosionOrange(to.transform.position);
        to.DamgeBySkill(damage);
        CameraCtrl.Instance.Shake(1, 1);
    }
}