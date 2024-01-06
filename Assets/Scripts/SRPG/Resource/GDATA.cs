using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SPRG.Resource
{
    public class GDATA
    {
        public enum ACTOR_TYPE
        {
            Decoration, //装饰物
            FightUnit,  //战斗单位
            MovingObj,  //移动物体
            Barrier,
            Terrian,
            Interact, //互动
            SelfDestruction,    //自毁，可能表示具有自毁功能的角色或物体
            Sfx3d,          //3D音效，可能表示游戏中的音效对象
            BlockMainmenu,  //阻止主菜单，可能用于在游戏中阻止或屏蔽主菜单的特定元
        }

        public static readonly Dictionary<ACTOR_TYPE, int> PARAM_COUNTS_ACTOR = new Dictionary<ACTOR_TYPE, int>
        {
            { ACTOR_TYPE.Decoration, 1 },
            { ACTOR_TYPE.FightUnit, 14 },
            { ACTOR_TYPE.MovingObj, 7 },
            { ACTOR_TYPE.Barrier, 7 },
            { ACTOR_TYPE.Terrian, 7 },
            { ACTOR_TYPE.Interact, 7 },
            { ACTOR_TYPE.SelfDestruction, 2 },
            { ACTOR_TYPE.Sfx3d, 1 },
            { ACTOR_TYPE.BlockMainmenu, 0 },
        };
    }

}