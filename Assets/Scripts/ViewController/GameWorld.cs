using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 世界的基础逻辑
/// </summary>
public class GameWorld : MonoBehaviour
{
    private GameObject characterUI;

    private void Start()
    {
        var d = GameObject.Find("LevelButton").GetComponent<Button>();
        d.onClick.AddListener(LevelButton);

        d = GameObject.Find("CharacterBtn").GetComponent<Button>();
        d.onClick.AddListener(CharacterBtn);

        characterUI = transform.Find("CharacterUI").gameObject;

        d = characterUI.transform.Find("Cancel").GetComponent<Button>();
        d.onClick.AddListener(() => characterUI.SetActive(false));
    }

    private void CharacterBtn()
    {
        characterUI.SetActive(true);
    }

    private void LevelButton()
    {
        SceneManagerExt.instance.LoadSceneShowProgress(GameDefine.SceneType.GameLevel);
        GameManager.Instance.SetLevel("BG001");
    }
}
