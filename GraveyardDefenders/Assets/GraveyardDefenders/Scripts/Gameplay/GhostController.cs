using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace XD
{
    public class GhostController : MonoBehaviour
    {
        public PlayerCharacter target;
        public List<Material> materials; //TODO: make this one material when the model is updated on blender
        public float fadeTime;
        public float followVelocity;

        [Header("SineWaveInYAxis")]
        public float sineYFreq;
        public float sineYAmplitude;
        public float offset;

        private string fadeId;
        bool despawning = false;

        [Header("Hit")]
        float timeEnteredHitRange = Mathf.NegativeInfinity;
        public float hitRange = 1.0f;
        public float timeToHit = 1.5f;
        bool playerInHitRange = false;

        void Awake()
        {
            fadeId = GetInstanceID() + "Appear";
            CollectMaterials();
            DisappearInstant();
            Appear();
        }

        public void Init(PlayerCharacter character)
        {
            despawning = false;
            target = character;
            DisappearInstant();
            Appear();
        }

        private void CollectMaterials()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
                materials.AddRange(renderers[i].materials);
        }

        [ContextMenu("Appear")]
        public void Appear()
        {
            for (int i = 0; i < materials.Count; i++) materials[i].DOFade(1.0f, fadeTime).SetId(fadeId);
        }

        [ContextMenu("Disappear")]
        public void Disappear()
        {
            for (int i = 0; i < materials.Count; i++) materials[i].DOFade(0.0f, fadeTime).SetId(fadeId);
        }

        public void DisappearAndDespawn()
        {
            if (despawning) return;
            despawning = true;
            for (int i = 0; i < materials.Count; i++)
            {
                if (i == 0)
                    materials[i].DOFade(0.0f, fadeTime).SetId(fadeId).OnComplete(Despawn);
                else
                    materials[i].DOFade(0.0f, fadeTime).SetId(fadeId);
            }
        }

        public void Despawn()
        {
            DOTween.Kill(fadeId);
            NPCManager.Instance.RemoveGhost(this);
        }

        private void DisappearInstant()
        {
            Color auxC;
            for (int i = 0; i < materials.Count; i++)
            {
                auxC = materials[i].color;
                auxC.a = 0.0f;  
                materials[i].color = auxC;
            }
        }

        void Update()
        {
            if (target.TimeInSafeArea >= 0.5f) DisappearAndDespawn();
            if (despawning) return;

            Vector3 auxPos = transform.position;
            if (target)
            {
                Vector3 targetPos = target.transform.position;
                Vector3 toPlayer = targetPos - auxPos;
                toPlayer.y = 0.0f;

                Vector3 velocity = (toPlayer.normalized * (followVelocity * Time.deltaTime));
                auxPos += velocity;

                transform.rotation = Quaternion.LookRotation(toPlayer, Vector3.up);
            }

            auxPos.y = (Mathf.Sin(Time.time * sineYFreq) * sineYAmplitude) + offset;
            transform.position = auxPos;

            UpdateChaseHit();
        }

        #region HITS

        float TimePlayerHasBeenInsideHitRange
        {
            get
            {
                if (!playerInHitRange) return 0f;
                else return Time.timeSinceLevelLoad - timeEnteredHitRange;
            }
        }

        void UpdateChaseHit()
        {
            //check if we are in rangeXZ
            Vector3 positionOnXZ = transform.position;
            positionOnXZ.y = target.transform.position.y;
            float distanceToPlayer = Vector3.Distance(positionOnXZ, target.transform.position);
            if (distanceToPlayer <= hitRange)
            {
                if (!playerInHitRange) PlayerEnteredHitRange();
                if (TimePlayerHasBeenInsideHitRange >= timeToHit) HitPlayer();
            }
            else
            {
                if (playerInHitRange) PlayerExitHitRange();
            }
        }

        void PlayerEnteredHitRange()
        {
            playerInHitRange = true;
            timeEnteredHitRange = Time.timeSinceLevelLoad;
            //TODO: particles? feedback?   
        }

        void PlayerExitHitRange()
        {
            playerInHitRange = false;
            //TODO: particles? feedback?
        }

        void HitPlayer()
        {
            playerInHitRange = false;
            target.Respawn();
            //target.respawn
            //TODO: particles? feedback?
        }

        #endregion
    }
}
