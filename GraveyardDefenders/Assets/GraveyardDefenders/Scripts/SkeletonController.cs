using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace XD
{
    public class SkeletonController : MonoBehaviour
    {
        public NavMeshAgent agent;
        public float maxRayDistance = 100.0f;
        public NavMeshHit navmesh_hit_helper;
        public RaycastHit hit_helper;

        public float attackDamage = 1.0f;
        public float attackVelocity = 1.0f;
        public float lastAttackTime = float.NegativeInfinity;
        public float TimeSinceLastAttack
        {
            get { return Time.timeSinceLevelLoad - lastAttackTime; }
        }

        BreakableObject currentTarget = default;

        void Start()
        {
            if (!agent) GetComponent<NavMeshAgent>();

            agent.autoTraverseOffMeshLink = false;
        }

        void Update()
        {
            Camera Camera = Camera.main;
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * maxRayDistance), Color.red);


            if (Input.GetMouseButtonDown(0))
            {
                if(Physics.Raycast(ray, out hit_helper, maxRayDistance))
                {
                    if (NavMesh.SamplePosition(hit_helper.point, out navmesh_hit_helper, 1.0f, NavMesh.AllAreas)) { agent.SetDestination(navmesh_hit_helper.position); }
                }
            }


            if (agent.nextOffMeshLinkData.valid && agent.nextOffMeshLinkData.offMeshLink)
            {
                Debug.Log("Has offmeshLink: " + agent.nextOffMeshLinkData.offMeshLink.name);
                currentTarget = agent.nextOffMeshLinkData.offMeshLink.gameObject.GetComponent<BreakableObject>();
            }

            if (currentTarget)
            {
                if (!currentTarget.destroyed && TimeSinceLastAttack >= attackVelocity)
                {
                    Attack();
                    DebugExtension.DebugArrow(transform.position, currentTarget.transform.position - transform.position, Color.red, 0.1f, false);
                }
                else if (currentTarget.destroyed)
                    currentTarget = null;
            }
        }

        void Attack()
        {
            Debug.Log(name + " is Attacking: " + currentTarget.name);
            currentTarget.Hit(attackDamage);
            if (currentTarget.destroyed) agent.CompleteOffMeshLink();
            lastAttackTime = Time.timeSinceLevelLoad;
        }
    }
}
