using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;

namespace XD
{
    public class BreakableObject : MonoBehaviour, IInteractable
    {
        [Header("Assignable")]
        public Image healthBar;
        public BreakableSet breakableSet;
        public Collider col;

        [Header("Variables")]
        public float maxHP;
        public bool isRepairable = true;
        public RESOURCE_TYPE repairResource;
        public bool isInfinite = false;
        public bool reflectDamage = false;

        [Header("Runtime")]
        public float currentHP; //TODO: Create a HealthComponent and use it for skeletons and other future enemies
        public bool destroyed = false;
        public bool CanHit => !destroyed;
        public bool CanRepair => isRepairable && currentHP < maxHP;
        public virtual bool CanGather => false;

        public Vector3 cached_center;

        [Header("Events")]
        public UnityEvent brokeEvent;
        public UnityEvent hitEvent;
        public UnityEvent onRepairEvent; //called when the breakable was broken and is repaired
        public UnityEvent onRepairHitEvent; //called each time the breakable is repaired

        public float CurrentHPPercent { get { return currentHP / maxHP; } }

        public void OnEnable()
        {
            col.enabled = true;
            cached_center = col.bounds.center;
            breakableSet.Add(this);
        }

        public void OnDisable() { breakableSet.Remove(this); }

        void Start() { Init(); }

        protected virtual void Init()
        {
            currentHP = maxHP;
            destroyed = false;
        }

        public float Hit(float dmg)
        {
            float oldHP = currentHP;
            currentHP -= dmg;
            currentHP = Mathf.Clamp(currentHP, 0.0f, maxHP);
            hitEvent.Invoke();

            if (currentHP == 0.0f)
            {
                brokeEvent.Invoke();
                destroyed = true; 
            }

            UpdateHealthBar();
            float totalDamageDone = oldHP - currentHP;
            if (isInfinite) currentHP = oldHP;
            return totalDamageDone;
        }

        public float Repair(float ammount)
        {
            if (!isRepairable)
            {
                Debug.LogError("Calling repair in non repairable breakableObject");
                return 0.0f;
            }

            float oldHP = currentHP;
            currentHP += ammount;
            currentHP = Mathf.Clamp(currentHP, 0.0f, maxHP);
            if (ammount > 0.0f)
                onRepairHitEvent.Invoke();
            if (oldHP == 0.0f && currentHP != 0.0f)
            {
                onRepairEvent.Invoke();
                destroyed = false;
            }

            UpdateHealthBar();
            return currentHP - oldHP;
        }

        public Vector3 GetClosestPoint(Vector3 source) { return col.ClosestPoint(source); }
        public Vector3 GetCenter() { return cached_center; }

        public void UpdateHealthBar()
        {
            if (healthBar != null) healthBar.fillAmount = CurrentHPPercent;
        }

        #region IInteractable
        public GameObject GetGO() { return gameObject; }
        public float GetDistance(Vector3 pos) { return Vector3.Distance(pos, GetClosestPoint(pos)); }
        public virtual bool CanBeInteracted(ResourceInventory inventory) { return CanRepair && inventory.HasResource(repairResource); } //in breakables interaction is repair
        public virtual PLAYER_ACTIONS GetAction() { return PLAYER_ACTIONS.REPAIR; }
        public Vector3 GetInteractLookAt() { return GetCenter(); }
        #endregion
    }
}
