using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SPRG.Resource
{
    public class DataType : MonoBehaviour
    {
        public enum ACTOR_FLAG
        {
            // 表示角色处于未激活状态，可能是因为当前不可用或被暂时禁用。
            INACTIVE,

            // 表示角色处于激活状态，可以正常工作，参与游戏中的各种操作。
            ACTIVE,

            // 表示角色被封锁或阻止，可能无法移动或执行某些动作，受到一些限制。
            BLOCKADE,

            // 表示角色同时处于激活状态和被封锁状态，可能仍受到一些封锁或限制。
            ACTIVE_BLOCKADE
        }
    }
}
