using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this; //保证是该类型的
        }
    }

    public static bool IsInitialized    //判断是否创建
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()  //当销毁时，设置会空
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}

public class Singleton<T> where T : class, new()
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

            }
            return _instance;
        }
    }



}