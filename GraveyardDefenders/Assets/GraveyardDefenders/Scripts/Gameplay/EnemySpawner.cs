using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XD.Audio;

namespace XD
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn")]
        public float timeToSpawn;
        public float lastSpawnTime;
        public GameObject enemyPrefab;
        public Transform spawnTransform;
        public bool HasToSpawn { get { return Time.timeSinceLevelLoad - lastSpawnTime > timeToSpawn; } }

        [Header("Shake")]
        public Shaker shaker;
        public Vector3 maxShakeAxis;
        public Vector3 mediumShakeAxis;
        public int maxShakeVibratio = 5;
        public int mediumShakeVibratio = 15;
        public float randomness = 90.0f;
        public bool fadeout = false;

        public int lastShakeID = 0;
        public int CurrentShakeID
        {
            get
            {
                return Mathf.FloorToInt(((Time.timeSinceLevelLoad - lastSpawnTime) * 3.0f )/ (timeToSpawn));
            }
        }

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
            lastSpawnTime = Time.timeSinceLevelLoad + Random.Range(0.0f, timeToSpawn / 2.0f);
            Debug.LogFormat($"lastSpawnTime {lastSpawnTime}");
        }

        void Update()
        {
            if (needsInit) Init();

            if (HasToSpawn)
                SpawnEnemy();

            if(CurrentShakeID != lastShakeID)
                Shake(CurrentShakeID);
        }

        void SpawnEnemy()
        {
            Instantiate(enemyPrefab, spawnTransform.position, spawnTransform.rotation);
            lastSpawnTime = Time.timeSinceLevelLoad;
            Shake(0);
            AudioManager.Instance.PlayFX(AUDIO_FX.SKELETON_SPAWN, this.gameObject);
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
