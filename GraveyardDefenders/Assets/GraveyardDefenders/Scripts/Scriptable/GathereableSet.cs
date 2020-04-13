using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "XD/Sets/GathereableSet")]
    public class GathereableSet : RuntimeSet<GathereableResource>
    {
        [ContextMenu("LogNames")]
        public void LogNames()
        {
            foreach (GathereableResource resource in items)
            {
                Debug.Log(resource.name);
            }
        }
    }
}