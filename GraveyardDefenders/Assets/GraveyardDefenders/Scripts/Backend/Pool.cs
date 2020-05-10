using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XD
{
    public class Pool : MonoBehaviour
    {
        public int size;
        public List<GameObject> available;
        public GameObject prefab;

        [ContextMenu("Init")]
        public void Init()
        {
            available = new List<GameObject>(size);
            for (int i = 0; i < size; i++) InstantiateElement();
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            for (int i = 0; i < available.Count; i++)
            {
                if (Application.isEditor && !Application.isPlaying) DestroyImmediate(available[i]);
                else Destroy(available[i]);
            }
            available.Clear();
        }

        private GameObject InstantiateElement()
        {
#if UNITY_EDITOR
            GameObject go = null;
            if (Application.isEditor && !Application.isPlaying) go = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
            else go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
#else
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
#endif
                go.SetActive(false);
            available.Add(go);
            return go;
        }

        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject spawned;
            if(available.Count > 0)
            {
                spawned = available[available.Count - 1];
                available.RemoveAt(available.Count - 1);
            }
            else
            {
                spawned = InstantiateElement();
                Debug.LogError($"Pool overflow: item:{prefab.name}, originalSize: {size}");
            }

            spawned.transform.position = pos;
            spawned.transform.rotation = rot;
            spawned.SetActive(true);
            return spawned;
        }
        //TODO: implement parented Spawn ones

        public void Despawn(GameObject go)
        {
            go.transform.parent = this.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.SetActive(false);
            available.Add(go);
        }
    }   
}
