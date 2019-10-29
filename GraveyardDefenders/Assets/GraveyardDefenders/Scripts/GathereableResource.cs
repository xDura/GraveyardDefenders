using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class GathereableResource : BreakableObject
    {
        public GathereableSet set;

        public void OnEnable()
        {
            set.Add(this);    
        }

        public void OnDisable()
        {
            set.Remove(this);
        }

        public void Gather(float dmg)
        {
            Debug.Log("Gather");
            Hit(dmg);
        }
    }
}
