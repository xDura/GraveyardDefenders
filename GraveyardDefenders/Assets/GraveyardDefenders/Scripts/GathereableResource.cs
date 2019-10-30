using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class GathereableResource : BreakableObject
    {
        public GathereableSet set;
        public RESOURCE_TYPE type;

        public void OnEnable()
        {
            set.Add(this);    
        }

        public void OnDisable()
        {
            set.Remove(this);
        }

        public float Gather(float dmg)
        {
            float gathered = Hit(dmg);
            return gathered;
        }
    }
}
