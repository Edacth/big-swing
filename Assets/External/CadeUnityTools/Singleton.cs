using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static private bool errorShown;
    static private T s_instance;
    static public T instance
    {
        get
        {
            if (s_instance == null && !errorShown)
            {
                Debug.Log(typeof(T).ToString() + "Singleton script does not exist in the scene");
                errorShown = true;
            }

            return s_instance;
        } 
    }

    protected private void SingletonInit(T _instance, bool _persistScenes)
    {
        if (s_instance == null)
        {
            s_instance = _instance;
        }
        else
        {
            Destroy(gameObject);  
        }
        
        if (_persistScenes)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += SingletonUnload;
        }
    }

    protected void SingletonUnload(UnityEngine.SceneManagement.Scene _current)
    {
        // Unloads singleton
        s_instance = null;
    }

    void OnApplicationQuit()
    {
        s_instance = null;
    }
}
