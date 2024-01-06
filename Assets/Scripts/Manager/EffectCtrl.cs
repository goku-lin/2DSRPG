using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl
{
    public static EffectCtrl instance;
    private SpawnPool spawnPool;
    internal bool playeffect;

    public static void Init()
    {
        instance = new EffectCtrl();
        EventDispatcher.instance.Regist<Vector3>(GameEventType.showHitEffect, instance.showHitEffect);
        // EventDispatcherDemo.instance.showHitEffect += instance.showHitEffect;

        //优化后
        var i = ResourcesExt.Load<GameObject>("effect/hit-blue-1");

        var poolGo = new GameObject("hitEffect Pool");

        instance.spawnPool = poolGo.AddComponent<SpawnPool>();

        var prefabPool = new PrefabPool(i.transform);
        instance.spawnPool.CreatePrefabPool(prefabPool);

        //魔法特效
        i = ResourcesExt.Load<GameObject>("effect/MagicCircleSimpleGreen");
        prefabPool = new PrefabPool(i.transform);
        instance.spawnPool.CreatePrefabPool(prefabPool);

        i = ResourcesExt.Load<GameObject>("effect/HealBig");
        prefabPool = new PrefabPool(i.transform);
        instance.spawnPool.CreatePrefabPool(prefabPool);

        i = ResourcesExt.Load<GameObject>("effect/HealingWindZone");
        prefabPool = new PrefabPool(i.transform);
        instance.spawnPool.CreatePrefabPool(prefabPool);

        i = ResourcesExt.Load<GameObject>("effect/RocketMissileFire");
        prefabPool = new PrefabPool(i.transform);
        instance.spawnPool.CreatePrefabPool(prefabPool);

        i = ResourcesExt.Load<GameObject>("effect/MysticExplosionOrange");
        prefabPool = new PrefabPool(i.transform);
        instance.spawnPool.CreatePrefabPool(prefabPool);
    }

    public void ShowRestoreHealthBig(Character from)
    {
        // throw new NotImplementedException();
        var go = spawnPool.Spawn("HealingWindZone");

        go.transform.SetParent(from.transform, false);
        go.transform.localPosition = Vector3.zero;

        spawnPool.Despawn(go, 5F);

    }

    private void showHitEffect(Vector3 worldPos)
    {
        var go = spawnPool.Spawn("hit-blue-1");
        go.transform.position = worldPos;

        spawnPool.Despawn(go, 2F);
    }

    public void ShowMagicCircleSimpleGreen(Character player)
    {
        var go = spawnPool.Spawn("MagicCircleSimpleGreen");
        //不然删除角色时就出bug了
        //TODO:未来删除角色不destroy而隐藏时，记得可以来改
        //go.transform.SetParent(playerController.transform, false);
        //go.transform.localPosition = Vector3.zero;
        go.transform.position = player.transform.position;

        spawnPool.Despawn(go, 2F);
    }

    public void ShowRestoreHealth(Character player)
    {
        var go = spawnPool.Spawn("HealBig");

        //不然删除角色时就出bug了
        //TODO:未来删除角色不destroy而隐藏时，记得可以来改
        //go.transform.SetParent(player.transform, false);
        //go.transform.localPosition = Vector3.zero;
        go.transform.position = player.transform.position;

        spawnPool.Despawn(go, 2F);
    }


    public void ShowFireFall(Character player, float duration)
    {
        var p_transform = spawnPool.Spawn("RocketMissileFire");
        int rndvalue = UnityEngine.Random.Range(0, 10);
        rndvalue = rndvalue < 5 ? -1 : 1;

        var dir = CameraCtrl.Instance.transform.position - player.transform.position;
        dir.Normalize();

        var qua = Quaternion.Euler(0, rndvalue * 45, 0);

        //在摄像机的后方 5点和7点方向出现 特效
        var startPos = Vector3.up * 3 + player.transform.position + qua * (dir * 25);
        //这里是控制火球掉落的偏移值，不设置就正好在敌方脚下
        //var fireDir = player.transform.position - startPos;
        //fireDir.Normalize();

        var endPos = player.transform.position;// + fireDir * 2;

        spawnPool.Despawn(p_transform, 4);
        player.StartCoroutine(C_FireFall(p_transform, duration, startPos, endPos));
    }

    IEnumerator C_FireFall(Transform trs, float duration, Vector3 startPos, Vector3 endPos)
    {
        var startTime = Time.time;
        while (true)
        {
            var t = (Time.time - startTime) / duration;
            trs.position = Vector3.Lerp(startPos, endPos, t);
            if (t >= 1)
            {
                trs.position = endPos;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }


    public void ShowMysticExplosionOrange(Vector3 worldPos)
    {
        var p_transform = spawnPool.Spawn("MysticExplosionOrange");
        p_transform.position = worldPos;
        spawnPool.Despawn(p_transform, 4f);
    }

    public void ReMove()
    {
        EventDispatcher.instance.UnRegist<Vector3>(GameEventType.showHitEffect, this.showHitEffect);
    }
}
