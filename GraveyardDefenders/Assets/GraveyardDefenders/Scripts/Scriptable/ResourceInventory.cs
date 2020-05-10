using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public enum RESOURCE_TYPE
    {
        WOOD,
        STONE,
    }

    [CreateAssetMenu(menuName = "XD/Resources")]
    public class ResourceInventory : ScriptableObject
    {
        public float[] resources;

        public float GetResourceCount(RESOURCE_TYPE resourceType)
        {
            return resources[(int)resourceType];
        }

        public void AddResource(RESOURCE_TYPE resourceType, float ammount)
        {
            resources[(int)resourceType] += ammount;
        }

        public void SubstractResource(RESOURCE_TYPE resourceType, float ammount)
        {
            resources[(int)resourceType] -= ammount;
        }

        public bool HasResource(RESOURCE_TYPE resourceType, float ammount = 1.0f)
        {
            return resources[(int)resourceType] >= ammount;
        }

        public void Reset()
        {
            for (int i = 0; i < resources.Length; i++)
                resources[i] = 0.0f;
        }
    }
}
