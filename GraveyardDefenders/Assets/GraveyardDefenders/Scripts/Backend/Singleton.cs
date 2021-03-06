﻿using UnityEngine;

namespace XD
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool dontDestroyOnLoad = true;
        static string TypeString => typeof(T).Name;
        static private T instance;
        static public T Instance
        {
            get
            {
                if (instance == null)
                {
                    Object found = Resources.Load(TypeString);
                    if (found == null)
                    {
                        Debug.LogError($"Singleton instance not found on Resources: type: {TypeString}, instances: {found}: consider placing singletons on scenes");
                        return default;
                    }
                    else
                    {
                        Debug.Log($"Instantiating Singleton on runtime: consider placing this on a scene {TypeString}");
                        GameObject g = Instantiate(found as GameObject);
                        instance = g.GetComponent<T>();
                        return instance;
                    }
                }
                else return instance;
            }
        }

        public static bool Exists() { return instance != null; }

        public void Awake()
        {
            if (!Exists()) instance = (this as T);
            else
            {
                if (instance != this)
                {
                    Debug.Log($"Destroying {gameObject.name}");
                    Destroy(this.gameObject);
                    return;
                }
            }
            OnSingletonAwake();
        }

        public virtual void OnSingletonAwake()
        {
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        public void OnDestroy()
        {
            OnSingletonDestroy(this == instance);
        }

        public virtual void OnSingletonDestroy(bool isMainInstance) {}
    }

}
