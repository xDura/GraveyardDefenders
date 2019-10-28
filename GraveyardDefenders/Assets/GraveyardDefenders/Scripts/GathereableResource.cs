using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [System.Serializable]
    public class GathereableResource : BreakableObject
    {
        public GathereableResourceSet set;

        public void OnEnable()
        {
            set.AddGathereable(this.gameObject);    
        }

        public void OnDisable()
        {
            set.RemoveGathereable(this.gameObject);
        }

        public void Gather(float dmg)
        {
            Hit(dmg);
        }
    }
}
