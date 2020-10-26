using UnityEngine;
using System.Collections.Generic;
using XD.Utils;

namespace XD
{
    public class ParticleSystemController : MonoBehaviour
    {
        public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        private float largestDuration;
        private float lastSpawnTime = float.NegativeInfinity;
        public float despawnDelay = 0.5f;
        private bool startedDespawning = false;
        [HideInInspector] public Pool pool;

        //TODO: some randomnes stuff? (ie: same particles can end with different offsets)

        [Button]
        void CollectParticleSystemsRecursive()
        {
            CollectTransform(transform);
        }

        void CollectTransform(Transform t)
        {
            ParticleSystem ps = t.GetComponent<ParticleSystem>();
            if(ps) particleSystems.Add(ps);

            for (int i = 0; i < t.childCount; i++) CollectTransform(t.GetChild(i));
        }

        [Button]
        public void Play()
        {
            startedDespawning = false;
            largestDuration = 0.0f;
            for (int i = 0; i < particleSystems.Count; i++)
            {
                ParticleSystem s = particleSystems[i];
                float currentDuration = s.main.duration;
                if (currentDuration >= largestDuration) largestDuration = currentDuration;
            }
            lastSpawnTime = TimeUtils.GetTime();
        }

        public void Update()
        {
            float timeSinceLastSpawn = TimeUtils.TimeSince(lastSpawnTime);
            if (timeSinceLastSpawn >= largestDuration) StartDespawn();
            if (timeSinceLastSpawn >= (largestDuration + despawnDelay)) EndDespawn();
        }

        public void StartDespawn()
        {
            if (!startedDespawning)
                startedDespawning = true;
        }

        public void EndDespawn()
        {
            if (pool) pool.Despawn(this.gameObject);
        }
    }   
}
