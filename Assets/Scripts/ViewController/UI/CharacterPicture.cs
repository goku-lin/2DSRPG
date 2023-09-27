using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPicture : MonoBehaviour
{
    private Role role;
    private bool isPlaySound;

    public void Init(Role nowRole)
    {
        role = nowRole;
        isPlaySound = true;
        this.GetComponent<Button>().onClick.RemoveAllListeners();
        this.GetComponent<Button>().onClick.AddListener(ClickPicture);
    }

    private void ClickPicture()
    {
        if (isPlaySound)
        {
            Debug.Log(role.unitName);
            //TODO:对话完善
            Debug.Log(role.getDialogInfo(role.unitName + "_1").info);
            EventDispatcher.instance.DispatchEvent(GameEventType.playIdleVoice, role);
        }
    }

}
