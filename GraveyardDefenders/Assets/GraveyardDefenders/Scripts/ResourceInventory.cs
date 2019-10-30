using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public enum RESOURCE_TYPE
    {
        WOOD,
        METAL,
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
            Debug.LogFormat($" AddingResource: {resourceType.ToString()}, ammount: {ammount}");
            resources[(int)resourceType] += ammount;
        }
    }
}
