using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace XD
{
    [System.Serializable] public class ResourceToTurretTypeDictionary : SerializableDictionaryBase<RESOURCE_TYPE, TurretTypeData> { }
    [CreateAssetMenu(menuName = "XD/Databases/TurretTypesDB")]
    public class TurretTypesDatabase : ScriptableObject
    {
        public ResourceToTurretTypeDictionary turretTypes;
        public TurretTypeData GetTurretTypeData(RESOURCE_TYPE type)
        {
            if(turretTypes.TryGetValue(type, out TurretTypeData value))
                return value;
            else
            {
                Debug.LogError($"No TurretTypeData for resource {type} in TurretTypesDatabase");
                return null;
            }
        }
    }   
}
