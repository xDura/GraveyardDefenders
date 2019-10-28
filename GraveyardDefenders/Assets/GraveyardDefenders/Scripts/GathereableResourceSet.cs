using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "XD/GathereableResourceSet", fileName = "GathereableResourceSet", order = 1)]
    public class GathereableResourceSet : ScriptableObject
    {
        [SerializeField] public List<GameObject> gathereables;

        public void AddGathereable(GameObject resource)
        {
            if(!gathereables.Contains(resource))
                gathereables.Add(resource);
        }

        public void RemoveGathereable(GameObject resource)
        {
            if (gathereables.Contains(resource))
                gathereables.Remove(resource);
        }

        public void Clear()
        {
            gathereables.Clear();
        }
    }
}
