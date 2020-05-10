using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using XD.Events;

namespace XD
{

    public static class ParticleSystemEvents
    {
        public static Evnt<GameObject, Vector3, Quaternion> SpawnParticleEvent = new Evnt<GameObject, Vector3, Quaternion>();
    }

    [System.Serializable] public class ParticleSystemPoolsDictionary : SerializableDictionaryBase<GameObject, Pool> { }
    public class ParticleSystemManager : Singleton<ParticleSystemManager>
    {
        public ParticleSystemPoolsDictionary pools;

        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            ParticleSystemEvents.SpawnParticleEvent.AddListener(OnSpawnEvent);
        }

        private void OnSpawnEvent(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            SpawnParticleSystem(prefab, pos, rot);
        }

        public GameObject SpawnParticleSystem(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            if (!pools.ContainsKey(prefab))
            {
                Debug.LogError($"No pool found for prefab: {prefab.name}");
                return null;
            }

            Pool pool = pools[prefab];
            GameObject spawned = pool.Spawn(pos, rot);
            ParticleSystemController psc = spawned.GetComponent<ParticleSystemController>();
            if(psc)
            {
                psc.pool = pool;
                psc.Play();
            }
            return spawned;
        }

        //TODO: implement parented Spawn ones

#if UNITY_EDITOR
        public void AddPool(GameObject prefab, int size)
        {
            if (!prefab) return;
            if (pools.ContainsKey(prefab)) return;

            GameObject poolObject = new GameObject(prefab.name);
            poolObject.transform.SetParent(transform);
            Pool p = poolObject.AddComponent<Pool>();
            p.prefab = prefab;
            p.size = size;
            p.Init();
            pools.Add(prefab, p);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
