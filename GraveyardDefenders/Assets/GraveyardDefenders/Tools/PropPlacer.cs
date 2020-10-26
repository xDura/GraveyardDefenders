#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace XD
{
    public class PropPlacer : MonoBehaviour
    {
        public List<GameObject> prefabs;
        public Transform start;
        public Vector3 dir;

        [Button]
        public void Spawn()
        {
            Vector3 lastPos = start.transform.position;
            float lastSize = GetMaxValue(prefabs[0].GetComponent<MeshRenderer>().bounds.size);
            for (int i = 0; i < prefabs.Count; i++)
            {
                Bounds b = prefabs[i].GetComponent<MeshRenderer>().bounds;
                lastPos += Vector3.Project(b.size, dir);
                GameObject go = UnityEditor.PrefabUtility.InstantiatePrefab(prefabs[i], gameObject.scene) as GameObject;
                go.transform.position = lastPos;
            }
        }

        public float GetMaxValue(Vector3 vec)
        {
            if (vec.x >= vec.y && vec.x >= vec.z) return vec.x;
            if (vec.y >= vec.x && vec.y >= vec.z) return vec.y;
            return vec.z;
        }
    }   
}
#endif
