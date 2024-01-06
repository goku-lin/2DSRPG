using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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

public class Singleton<T> where T : new()
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Activator.CreateInstance<T>();
                if (instance == null)
                {
                    Debug.Log("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}

public abstract class SingletonMonoBehavior<T, P> : MonoBehaviour where T : MonoBehaviour where P : MonoBehaviour
{
    /// <summary>
    /// 获取单例实例。
    /// </summary>
    public static T Instance
    {
        get
        {
            return SingletonMonoBehavior<T, P>.GetInstance();
        }
    }

    /// <summary>
    /// 创建单例实例。
    /// </summary>
    private static void CreateInstance()
    {
        GameObject gameObject;

        // 根据泛型参数 P 类型创建 GameObject
        if (typeof(P) == typeof(MonoBehaviour))
        {
            gameObject = new GameObject();
            gameObject.name = typeof(T).Name;
        }
        else
        {
            P p = UnityEngine.Object.FindObjectOfType<P>();
            if (!p)
            {
                Debug.LogError("Could not find object with required component " + typeof(P).Name);
                return;
            }
            gameObject = p.gameObject;
        }

        Debug.Log("Creating instance of singleton component " + typeof(T).Name);
        SingletonMonoBehavior<T, P>.instance = gameObject.AddComponent<T>();
        SingletonMonoBehavior<T, P>.hasInstance = true;
    }

    /// <summary>
    /// 获取或创建单例实例。
    /// </summary>
    private static T GetInstance()
    {
        object obj = SingletonMonoBehavior<T, P>.lockObject;
        T result;
        lock (obj)
        {
            if (SingletonMonoBehavior<T, P>.hasInstance)
            {
                result = SingletonMonoBehavior<T, P>.instance;
            }
            else
            {
                Type typeFromHandle = typeof(T);
                T[] array = UnityEngine.Object.FindObjectsOfType<T>();
                if (array.Length > 0)
                {
                    SingletonMonoBehavior<T, P>.instance = array[0];
                    SingletonMonoBehavior<T, P>.hasInstance = true;
                    if (array.Length > 1)
                    {
                        Debug.LogWarning("Multiple instances of singleton " + typeFromHandle + " found; destroying all but the first.");
                        for (int i = 1; i < array.Length; i++)
                        {
                            UnityEngine.Object.DestroyImmediate(array[i].gameObject);
                        }
                    }
                    result = SingletonMonoBehavior<T, P>.instance;
                }
                else
                {
                    SingletonPrefabAttribute singletonPrefabAttribute = Attribute.GetCustomAttribute(typeFromHandle, typeof(SingletonPrefabAttribute)) as SingletonPrefabAttribute;
                    if (singletonPrefabAttribute == null)
                    {
                        SingletonMonoBehavior<T, P>.CreateInstance();
                    }
                    else
                    {
                        string name = singletonPrefabAttribute.Name;
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(name));
                        if (gameObject == null)
                        {
                            Debug.LogError(string.Concat(new object[]
                            {
                                    "Could not find prefab ",
                                    name,
                                    " for singleton of type ",
                                    typeFromHandle,
                                    "."
                            }));
                            SingletonMonoBehavior<T, P>.CreateInstance();
                        }
                        else
                        {
                            gameObject.name = name;
                            SingletonMonoBehavior<T, P>.instance = gameObject.GetComponent<T>();
                            if (SingletonMonoBehavior<T, P>.instance == null)
                            {
                                Debug.LogWarning(string.Concat(new object[]
                                {
                                        "There wasn't a component of type \"",
                                        typeFromHandle,
                                        "\" inside prefab \"",
                                        name,
                                        "\"; creating one now."
                                }));
                                SingletonMonoBehavior<T, P>.instance = gameObject.AddComponent<T>();
                                SingletonMonoBehavior<T, P>.hasInstance = true;
                            }
                        }
                    }
                    result = SingletonMonoBehavior<T, P>.instance;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 强制确保只存在一个单例。
    /// </summary>
    protected bool EnforceSingleton()
    {
        object obj = SingletonMonoBehavior<T, P>.lockObject;
        lock (obj)
        {
            if (SingletonMonoBehavior<T, P>.hasInstance)
            {
                T[] array = UnityEngine.Object.FindObjectsOfType<T>();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].GetInstanceID() != SingletonMonoBehavior<T, P>.instance.GetInstanceID())
                    {
                        UnityEngine.Object.DestroyImmediate(array[i].gameObject);
                    }
                }
            }
        }
        int instanceID = base.GetInstanceID();
        T t = SingletonMonoBehavior<T, P>.Instance;
        return instanceID == t.GetInstanceID();
    }

    /// <summary>
    /// 强制确保只存在一个单例组件。
    /// </summary>
    protected bool EnforceSingletonComponent()
    {
        object obj = SingletonMonoBehavior<T, P>.lockObject;
        lock (obj)
        {
            if (SingletonMonoBehavior<T, P>.hasInstance && base.GetInstanceID() != SingletonMonoBehavior<T, P>.instance.GetInstanceID())
            {
                UnityEngine.Object.DestroyImmediate(this);
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 当销毁对象时调用。
    /// </summary>
    private void OnDestroy()
    {
        SingletonMonoBehavior<T, P>.hasInstance = false;
    }

    private static T instance;
    private static bool hasInstance;
    private static object lockObject = new object();
}

/// <summary>
/// 单例预制体属性。用于标记单例类所需的预制体名称。
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class SingletonPrefabAttribute : Attribute
{
    /// <summary>
    /// 创建一个具有指定名称的单例预制体属性。
    /// </summary>
    /// <param name="name">单例预制体的名称。</param>
    public SingletonPrefabAttribute(string name)
    {
        this.Name = name;
    }

    /// <summary>
    /// 获取单例预制体的名称。
    /// </summary>
    public readonly string Name;
}