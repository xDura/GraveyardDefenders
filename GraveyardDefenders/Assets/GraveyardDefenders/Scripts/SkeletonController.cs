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

        void Start()
        {
            if (!agent) GetComponent<NavMeshAgent>();
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
                    if (NavMesh.SamplePosition(hit_helper.point, out navmesh_hit_helper, 1.0f, NavMesh.AllAreas))
                        agent.SetDestination(navmesh_hit_helper.position);

                }
            }
        }
    }
}
