using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace XD
{
    public class GathereableResource : BreakableObject
    {
        public GathereableSet set;
        public RESOURCE_TYPE type;

        [Header("Grow")]
        public bool grows = true;
        public float growTime = 10.0f;
        public float growYOffset = -2.0f;
        public Vector3 growStartScale;
        public UnityEvent OnGrowDone;
        public UnityEvent OnGrowStart;
        public override bool CanGather => !destroyed;

        public float Gather(float dmg, Vector3 gatherPos)
        {
            float gathered = Hit(dmg);
            SpawnGatherEffects(gatherPos);
            return gathered;
        }

        void SpawnGatherEffects(Vector3 gatherPos)
        {
            switch (type)
            {
                case RESOURCE_TYPE.STONE:
                    {
                        ParticleSystemEvents.SpawnParticleEvent.Invoke(Constants.Instance.rockHitParticles, gatherPos, Quaternion.identity);
                        GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.MINING_STONE, this.gameObject);
                    }
                    break;
                case RESOURCE_TYPE.WOOD:
                    {
                        ParticleSystemEvents.SpawnParticleEvent.Invoke(Constants.Instance.woodHitParticles, gatherPos, Quaternion.identity);
                        GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.CHOP_WOOD, this.gameObject);
                    }
                    break;
                case RESOURCE_TYPE.CRYSTAL:
                    {
                        ParticleSystemEvents.SpawnParticleEvent.Invoke(Constants.Instance.rockHitParticles, gatherPos, Quaternion.identity);
                        GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.MINING_STONE, this.gameObject);
                    }
                    break;
            }
        }

        public void StartGrowing()
        {
            if (!destroyed) return;

            transform.localScale = growStartScale;
            float topPositionY = transform.localPosition.y;
            transform.localPosition = transform.localPosition + (Vector3.up * growYOffset);

            transform.DOScale(Vector3.one, growTime).OnComplete(OnGrowEnded).OnUpdate(GrowUpdate);
            transform.DOLocalMoveY(topPositionY, growTime);
            OnGrowStart.Invoke();
        }

        private void GrowUpdate() {}
        public void OnGrowEnded()
        {
            currentHP = maxHP;
            destroyed = false;
            OnGrowDone.Invoke();
        }

        #region IInteractable
        public override bool CanBeInteracted(ResourceInventory inventory) { return CanGather; } //TODO: future: check if the bag is full
        public override PLAYER_ACTIONS GetAction() { return PLAYER_ACTIONS.GATHER; }
        #endregion
    }
}
