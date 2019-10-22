using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XD
{
    public class BreakableObject : MonoBehaviour
    {
        public float currentHP;
        public float maxHP;
        public UnityEvent brokeEvent;

        public bool destroyed = false;

        void Start()
        {
            currentHP = maxHP;
            destroyed = false;
        }

        public void Hit(float dmg)
        {
            currentHP -= dmg;
            currentHP = Mathf.Clamp(currentHP, 0.0f, maxHP);
            if (currentHP == 0.0f)
            {
                Debug.LogFormat($"{name} has been broken");
                brokeEvent.Invoke();
                destroyed = true; 
            }
        }
    }
}
