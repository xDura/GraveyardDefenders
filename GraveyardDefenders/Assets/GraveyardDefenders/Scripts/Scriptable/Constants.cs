using UnityEngine;
using XD.Events;

namespace XD
{
    [CreateAssetMenu(menuName = "XD/Constants")]
    public class Constants : ScriptableObject
    {
        public const int maxPlayers = 4;

        [System.Flags]
        public enum DEBUG_FLAGS
        {
            NONE = 0,
            PLAYER = 1 << 1,
            GATHEREABLE_OBJECTS = 1 << 2,
            RESOURCES_INVENTORY = 1 << 3,
            AI = 1 << 4,
            DAY_NIGHT_CYCLE = 1 << 5,
            AUDIO = 1 << 6,
            EVENTS = 1 << 7,
        }

        public bool noEnemiesMode = false;
        public DEBUG_FLAGS debugFlags;

        //TODO: move this prefabs to another place
        #region PREFABS
        public GameObject rockHitParticles;
        public GameObject woodHitParticles;
        public GameObject respawnParticles;
        #endregion

        public TurretTypesDatabase turretTypesDB;

        public static Constants Instance => ConstantsManager.Instance.constants; 
        public bool HasDebugFlag(DEBUG_FLAGS flag)
        {
            return (debugFlags & flag) != 0;
        }
    }   
}
