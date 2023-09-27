using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomationBehavior
{
    List<Character> players;
    private bool stop = false;
    public bool executting { get; private set; }

    public void Start_Automation(List<Character> p_players)
    {
        stop = false;
        executting = true;
        Debug.Log("AI托管");
        players = p_players;
        Execute();
    }

    public void ReadyStop(bool b)
    {
        Debug.Log("等待停止AI托管");
        stop = b;
    }

    void Execute()
    {
        if (stop)
        {
            Debug.Log("停止AI托管");
            executting = false;
            return;
        }

        if (players.Count >= 1)
        {
            Character player = players[0];
            player.behaviorAI.behaviorEnd -= behaviorEnd;
            player.behaviorAI.behaviorEnd += behaviorEnd;
            //侠客主要攻击
            if (player.tempjob == GameDefine.AIType.Attack)
            {
                player.behaviorAI.ExcuteBehavior_Advanced();
            }//祝由职业主要辅助
            else if (player.tempjob == GameDefine.AIType.Heal)
            {
                player.behaviorAI.ExcuteBehavior_Auxiliary();
            }
        }
        else
        {
            executting = false;
        }
    }

    void behaviorEnd()
    {
        players.RemoveAt(0);
        Execute();
    }
}