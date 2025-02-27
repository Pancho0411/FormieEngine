using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Formie Engine/Game/Objects Manager")]
public class ObjectsManager : MonoBehaviour
{
    private static ObjectsManager instance;

    public static ObjectsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<ObjectsManager>();
                instance.StartSingleton();
            }

            return instance;
        }
    }

    private readonly List<FormieObject> objects = new List<FormieObject>();

    private void StartSingleton()
    {
        GetFormieObjects();
    }

    private void GetFormieObjects()
    {
        foreach (FormieObject formieObject in FindObjectsByType<FormieObject>(FindObjectsSortMode.None))
        {
            objects.Add(formieObject);
        }
    }

    public void RespawnFormieObjects()
    {
        foreach (FormieObject formieObject in objects)
        {
            formieObject.OnRespawn();
        }
    }
}
