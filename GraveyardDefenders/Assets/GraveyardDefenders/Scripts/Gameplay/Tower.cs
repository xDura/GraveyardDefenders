using UnityEngine;
using System.Collections.Generic;

namespace XD
{
    public class Tower : MonoBehaviour
    {
        [System.Serializable]
        public class TurretCharge
        {
            public TurretTypeData type;
            public int current_charges;
            public GameObject Projectile => type.projectile;
            public GameObject VisualsPrefab => type.turretVisuals;
            public float ThrowPeriode => type.throw_periode;
            public TurretCharge(TurretTypeData data)
            {
                type = data;
                current_charges = data.projectiles_per_charge;
            }
        }
        [Header("Assignable")]
        public Transform throwTransform;
        public CustomTrigger trigger;

        [Header("Variables")]
        public int maxCharges = 4;

        //runtime
        float lastHitTime = 0.0f;
        SkeletonController target = null; //TODO: this needs to be a more generic enemy class
        [System.NonSerialized] public List<SkeletonController> targets;
        [System.NonSerialized] public List<TurretCharge> charges;
        public bool HasCharge => charges.Count > 0;
        public TurretCharge CurrentCharge => charges[0];
        public GameObject currentVisuals = null;

        void OnEnable()
        {
            if (!trigger) trigger = GetComponentInChildren<CustomTrigger>();
            targets = new List<SkeletonController>();
            charges = new List<TurretCharge>();
            trigger.onTriggerEnter.AddListener(TriggerEnter);
            trigger.onTriggerExit.AddListener(TriggerExit);
            GlobalEvents.skeletonDespawned.AddListener(OnSkeletonDespawned);
        }

        void OnDisable()
        {
            trigger.onTriggerEnter.RemoveListener(TriggerEnter);
            trigger.onTriggerExit.RemoveListener(TriggerExit);
            GlobalEvents.skeletonDespawned.RemoveListener(OnSkeletonDespawned);
        }

        #region TRIGGER_HANDLING
        void TriggerEnter(Collider col)
        {
            SkeletonController sc = col.GetComponent<SkeletonController>();
            if (sc && !targets.Contains(sc)) targets.Add(sc);
        }

        void TriggerExit(Collider col)
        {
            SkeletonController sc = col.GetComponent<SkeletonController>();
            if (sc && targets.Contains(sc)) targets.Remove(sc);
        }

        void OnSkeletonDespawned(SkeletonController controller)
        {
            targets.Remove(controller);
        }
        #endregion

        #region SHOOTING_AND TARGET_HANDLING
        void ResetTarget()
        {
            target = null;
            lastHitTime = float.PositiveInfinity;
        }

        void SetTarget(SkeletonController controller)
        {
            target = controller;
            lastHitTime = Time.timeSinceLevelLoad;
        }

        void Shoot()
        {
            GameObject new_projectile_GO = Instantiate(CurrentCharge.Projectile, throwTransform.position, throwTransform.rotation);
            Projectile new_projectile = new_projectile_GO.GetComponent<Projectile>();
            new_projectile.Init(target);
            CurrentCharge.current_charges--;
            if(CurrentCharge.current_charges <= 0)
            {
                charges.RemoveAt(0);
                SelectedChargeChanged();
            }
        }

        void CheckCurrentTargetValid()
        {
            bool currentTargetDisabled = target != null && !target.gameObject.activeInHierarchy;
            bool targetOutOfList = !targets.Contains(target);
            if (targetOutOfList || currentTargetDisabled) ResetTarget();
        }

        void UpdateBestTarget()
        {
            float bestDistance = float.PositiveInfinity;
            SkeletonController bestTarget = null;
            for (int i = 0; i < targets.Count; i++)
            {
                SkeletonController current = targets[i];
                float currentDist = Vector3.Distance(current.transform.position, transform.position);
                if(currentDist < bestDistance)
                {
                    bestTarget = current;
                    bestDistance = currentDist;
                }
            }

            if (bestTarget != null) SetTarget(bestTarget);
        }
        #endregion

        #region CHARGES_HANDLING
        public void AddCharge(TurretTypeData type)
        {
            charges.Insert(0, new TurretCharge(type));
            if (charges.Count > maxCharges) charges.RemoveAt(charges.Count - 1);
            SelectedChargeChanged();
        }

        public void SelectedChargeChanged()
        {
            //TODO: Remove instantiates here: pool, easy to pool
            if (currentVisuals) Destroy(currentVisuals);
            currentVisuals = Instantiate(CurrentCharge.VisualsPrefab, throwTransform);
        }
        #endregion

        void Update()
        {
            if (!HasCharge)
            {
                ResetTarget();
                return;
            }
            if (target == null) UpdateBestTarget();
            else CheckCurrentTargetValid();
            if (target == null) return;

            if(Utils.TimeUtils.TimeSince(lastHitTime) >= CurrentCharge.ThrowPeriode)
            {
                Shoot();
                lastHitTime = Time.timeSinceLevelLoad;
            }
        }
    }
}
