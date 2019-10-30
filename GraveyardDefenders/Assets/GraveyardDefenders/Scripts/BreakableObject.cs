using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XD
{
    public class BreakableObject : MonoBehaviour
    {
        [Header("Variables")]
        public float maxHP;

        [Header("Runtime")]
        public float currentHP;
        public bool destroyed = false;

        [Header("Events")]
        public UnityEvent brokeEvent;
        public UnityEvent hitEvent;

        [Header("HealthBar")]
        public Image healthBar;

        public float CurrentHPPercent { get { return currentHP / maxHP; } }

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

        public void UpdateHealthBar()
        {
            if (healthBar != null) healthBar.fillAmount = CurrentHPPercent;
        }
    }
}
