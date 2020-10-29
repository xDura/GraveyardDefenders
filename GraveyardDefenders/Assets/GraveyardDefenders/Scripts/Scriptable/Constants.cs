using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;

namespace XD
{
    public enum PREFAB_ID
    {
        PLAYER_CHARACTER = 0,
        //---
        //---
        SKELETON = 20,
        NECROMANCER = 21,
        //---
        //---
        TREE_00 = 80,
        TREE_01 = 81,
        //---
        //---
        ROCK_00 = 90,
        ROCK_01 = 91,
        //---
        //---
        CRYSTAL_00 = 100,
        CRYSTAL_01 = 101,
        CRYSTAL_02 = 102,
    }

    public enum SKIN_ID
    {
        PLAYER_SKIN_0 = 0,
        PLAYER_SKIN_1 = 1,
        PLAYER_SKIN_2 = 2,
    }

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

        public PrefabDB prefabDB;
        public GameObject GetPrefab(int id) { return prefabDB.TryGetValue((PREFAB_ID)id, out GameObject prefab) ? prefab : null; }
        public GameObject GetPrefab(PREFAB_ID id) { return prefabDB.TryGetValue(id, out GameObject prefab) ? prefab : null; }

        public SkinsDB skinsDB;
        public GameObject GetSkin(int id) { return skinsDB.TryGetValue((SKIN_ID)id, out GameObject skin) ? skin : null; }
        public GameObject GetSkin(SKIN_ID id) { return skinsDB.TryGetValue(id, out GameObject skin) ? skin : null; }
        public int SkinCount => skinsDB.Count;
        public AvatarsDB avatarsDB;
        public Avatar GetAvatar(int id) { return avatarsDB.TryGetValue((SKIN_ID)id, out Avatar avatar) ? avatar : null; }
        public Avatar GetAvatar(SKIN_ID id) { return avatarsDB.TryGetValue(id, out Avatar avatar) ? avatar : null; }

        public GameObject rockHitParticles;
        public GameObject woodHitParticles;
        public GameObject respawnParticles;
        public GameObject changeSkinParticles;
        #endregion

        public TurretTypesDatabase turretTypesDB;

        public static Constants Instance => ConstantsManager.Instance.constants; 
        public bool HasDebugFlag(DEBUG_FLAGS flag)
        {
            return (debugFlags & flag) != 0;
        }

        [System.Serializable] public class PrefabDB : SerializableDictionaryBase<PREFAB_ID, GameObject> { }
        [System.Serializable] public class SkinsDB : SerializableDictionaryBase<SKIN_ID, GameObject> { }
        [System.Serializable] public class AvatarsDB : SerializableDictionaryBase<SKIN_ID, Avatar> { }
    }   
}
