using UnityEngine;
using XD.Utils;

namespace XD
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn")]
        public float timeToSpawn;
        public float lastSpawnTime;
        public Transform spawnTransform;
        public bool HasToSpawn { get { return TimeUtils.TimeSince(lastSpawnTime) > timeToSpawn; } }

        [Header("Shake")]
        public Shaker shaker;
        public Vector3 maxShakeAxis;
        public Vector3 mediumShakeAxis;
        public int maxShakeVibratio = 5;
        public int mediumShakeVibratio = 15;
        public float randomness = 90.0f;
        public bool fadeout = false;

        public int lastShakeID = 0;
        public int CurrentShakeID { get { return Mathf.FloorToInt(((TimeUtils.TimeSince(lastSpawnTime)) * 3.0f )/ (timeToSpawn)); } }

        public const int randomSeed = 200;

        private bool needsInit = false;

        void Start()
        {
            needsInit = true;
        }

        public void Init()
        {
            shaker.Kill();
            needsInit = false;
            lastShakeID = 0;

            //TODO Remove this random
            lastSpawnTime = TimeUtils.GetTime() + Random.Range(0.0f, timeToSpawn / 2.0f);
        }

        void Update()
        {
            if (needsInit) Init();

            if(DayNightCycle.currentPhase_s == DAY_NIGHT_PHASE.DAY || NPCManager.Instance.skeletonsLeft <= 0)
            {
                lastSpawnTime += Time.deltaTime;
                Shake(0);
            }
            else
            {
                if (HasToSpawn)
                    SpawnEnemy();

                if(CurrentShakeID != lastShakeID)
                    Shake(CurrentShakeID);
            }
        }

        void SpawnEnemy()
        {
            NPCManager.Instance.InstantiateSkeleton(spawnTransform.position, spawnTransform.rotation);
            lastSpawnTime = TimeUtils.GetTime();
            Shake(0);
            GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.SKELETON_SPAWN, this.gameObject);
        }

        void Shake(int shakeID)
        {
            shaker.Complete();
            switch (shakeID)
            {
                case 0:
                    break;
                case 1:
                    shaker.Shake(timeToSpawn, mediumShakeAxis, mediumShakeVibratio, randomness, fadeout);
                    break;
                case 2:
                    shaker.Shake(timeToSpawn, maxShakeAxis, maxShakeVibratio, randomness, fadeout);
                    break;
            }
            lastShakeID = shakeID;
        }
    }
}
