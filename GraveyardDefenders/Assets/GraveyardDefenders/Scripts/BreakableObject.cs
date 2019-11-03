using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;

namespace XD
{
    public class BreakableObject : MonoBehaviour
    {
        [Header("Assignable")]
        public Image healthBar;
        public BreakableSet breakableSet;
        public Collider col;

        [Header("Variables")]
        public float maxHP;
        public bool isRepairable = true;

        [Header("Runtime")]
        public float currentHP;
        public bool destroyed = false;
        public bool CanHit { get { return !destroyed; } }
        public bool CanRepair { get { return isRepairable && currentHP < maxHP; } }
        public Vector3 cached_center;

        [Header("Events")]
        public UnityEvent brokeEvent;
        public UnityEvent hitEvent;
        //called when the breakable was broken and is repaired
        public UnityEvent onRepairEvent;
        //called each time the breakable is repaired
        public UnityEvent onRepairHitEvent;

        public float CurrentHPPercent { get { return currentHP / maxHP; } }

        public void OnEnable()
        {
            col.enabled = true;
            cached_center = col.bounds.center;
            breakableSet.Add(this);
        }

        public void OnDisable()
        {
            breakableSet.Remove(this);
        }

        void Start()
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
                Debug.LogFormat($"{name} has been broken");
                brokeEvent.Invoke();
                destroyed = true; 
            }

            UpdateHealthBar();
            return oldHP - currentHP;
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

        public Vector3 GetClosestPoint(Vector3 source)
        {
            return col.ClosestPoint(source);
        }

        public Vector3 GetCenter()
        {
            return cached_center;
        }

        public void UpdateHealthBar()
        {
            if (healthBar != null) healthBar.fillAmount = CurrentHPPercent;
        }

        [ContextMenu("Checkbounds")]
        private void ShowBounds()
        {
            DebugExtension.DebugPoint(GetCenter(), Color.red, 10.0f, 10.0f, false);
        }
    }
}
