using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XD
{
    public class Objective : MonoBehaviour
    {
        public Collider objectiveCollider;
        public UnityEvent onObjectiveFailed;
        public UnityEvent onObjectiveCompleted;

        void Awake()
        {
            if (!objectiveCollider) GetComponent<Collider>();
        }

        public Vector3 GetClosestPointFrom(Vector3 position)
        {
            //TODO: remove this phyisics call and make something fully deterministic
            return objectiveCollider.ClosestPoint(position);
        }

        public void Fail()
        {
            onObjectiveFailed.Invoke();
        }

        public void Complete()
        {
            onObjectiveCompleted.Invoke();
        }
    }
}

