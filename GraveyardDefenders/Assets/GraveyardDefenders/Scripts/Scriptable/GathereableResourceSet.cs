using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "XD/GathereableResourceSet", fileName = "GathereableResourceSet", order = 1)]
    public class GathereableResourceSet : ScriptableObject
    {
        public List<GathereableResource> items = new List<GathereableResource>();

        public void AddGathereable(GathereableResource resource)
        {
            if(!items.Contains(resource))
                items.Add(resource);
        }

        public void RemoveGathereable(GathereableResource resource)
        {
            if (items.Contains(resource))
                items.Remove(resource);
        }

        public void Clear()
        {
            items.Clear();
        }
    }
}
