using System;
using UnityEngine;

public static class GameObjectExtension
{
    public static GameObject FindObject(this GameObject parent, string name)
    {
        Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform transform in componentsInChildren)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }

    public static T SafeAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        return (!(component == null)) ? component : gameObject.AddComponent<T>();
    }

    public static void SafeActive(this GameObject gameObject, bool active)
    {
        if (gameObject != null && gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }
    }
}
