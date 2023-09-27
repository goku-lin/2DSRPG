using UnityEngine;

public class ResourcesExt
{

    public static T Load<T>(string path) where T : Object
    {

        return Resources.Load<T>(path);
    }
}