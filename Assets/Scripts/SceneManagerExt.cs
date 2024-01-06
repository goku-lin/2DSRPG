using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//不把所有事件清一下的话，反复进入一个场景会加载不出来
public class SceneManagerExt : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    static GameObject loadingGob;

    static SceneManagerExt _instance;
    public static SceneManagerExt instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<SceneManagerExt>();
            }
            return _instance;
        }
    }

    public void LoadSceneShowProgress(GameDefine.SceneType p_levelId)
    {
        var p_loadingGob = ResourcesExt.Load<GameObject>("Prefabs/Loading");
        loadingGob = MonoBehaviour.Instantiate(p_loadingGob);

        StartCoroutine(C_Update((int)p_levelId));
    }


    IEnumerator C_Update(int levelId)
    {
        var loadImage = GameObject.Find("load_front").GetComponent<Image>();

        loadImage.fillAmount = 0;

        yield return new WaitForEndOfFrame();
        //LoadSceneMode.Single 加载场景前销毁所有对象,除了DontDestroyOnLoad
        var asyncOperation = SceneManager.LoadSceneAsync(levelId, LoadSceneMode.Single);
        //不允许加载后自动激活
        //改成场景显示后，再进入场景后删除进条
        //防止场景还没显示出来，进度条已经被删除造成的黑屏
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            //UI显示加载进度
            loadImage.fillAmount = asyncOperation.progress;
            yield return new WaitForEndOfFrame();

            Debug.Log(asyncOperation.progress);
        }

        loadImage.fillAmount = 0.9f;

        SceneManager.sceneLoaded += OnsceneLoaded;
        asyncOperation.allowSceneActivation = true;


    }

    private void OnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //改成场景显示后，再进入场景后删除进条预制体
        SceneManager.sceneLoaded -= OnsceneLoaded;
        Destroy(loadingGob);
    }
}