using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        public float maxShakeAmmount;
        public float mediumShakeAmmount;
        private Tweener shakeTween;
        public Transform visualsToShake;

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
        }

        void Shake(int shakeID)
        {
            if(shakeTween != null) shakeTween.Complete();
            switch (shakeID)
            {
                case 0:
                    break;
                case 1:
                    shakeTween = visualsToShake.DOShakeRotation(10.0f, mediumShakeAmmount);
                    break;
                case 2:
                    shakeTween = visualsToShake.DOShakeRotation(10.0f, maxShakeAmmount);
                    break;
            }
            lastShakeID = shakeID;
        }
    }
}
