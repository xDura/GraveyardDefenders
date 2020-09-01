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
        public override bool CanGather { get { return !destroyed; } }

        public float Gather(float dmg)
        {
            float gathered = Hit(dmg);
            return gathered;
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
    }
}
