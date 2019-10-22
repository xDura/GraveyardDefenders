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

        [Header("Shake")]
        public float maxShakeAmmount;
        public float mediumShakeAmmount;
        private Tweener shakeTween;
        public Transform visualsToShake;

        public int lastShakeID = 0;
        public int CurrentShakeID
        {
            get { return Mathf.FloorToInt(((Time.timeSinceLevelLoad - lastSpawnTime) * 3.0f )/ (timeToSpawn)); }
        }

        public bool HasToSpawn { get { return Time.timeSinceLevelLoad - lastSpawnTime > timeToSpawn; } }

        void Start()
        {
            lastSpawnTime = Time.timeSinceLevelLoad;
            lastShakeID = 0;
        }

        void Update()
        {
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
