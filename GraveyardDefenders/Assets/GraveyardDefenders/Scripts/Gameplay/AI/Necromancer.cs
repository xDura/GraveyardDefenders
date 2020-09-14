using XD.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace XD
{
    public class Necromancer : Enemy
    {
        public float summon_cd;
        public float last_summon_time = float.NegativeInfinity;
        public float NormalizedSummonity => TimeUtils.TimeSince(last_summon_time) / summon_cd;
        public bool CanSpawn => TimeUtils.TimeSince(last_summon_time) > summon_cd;

        public override void Init()
        {
            base.Init();
            last_summon_time = TimeUtils.GetTime();
        }

        public override void UpdateEnemy(float deltaTime)
        {
            base.UpdateEnemy(deltaTime);
            UpdateNavigation();
            UpdateSummoning();
            UpdateAnimation();
        }

        private void UpdateSummoning()
        {
            if (CanSpawn)
            {
                if(FindSummonPos(out Vector3 summonPos))
                    Summon(summonPos);
            }
        }

        private bool FindSummonPos(out Vector3 pos)
        {
            Vector3 current_dir = transform.forward;
            float current_radius = 1.5f;
            float angle_one_step = 360f / 8f; 
            Quaternion rot = Quaternion.Euler(0.0f, angle_one_step, 0.0f);
            for (int i = 0; i < 8; i++)
            {
                Vector3 summon_sample_pos = transform.position + (current_dir.normalized * current_radius);
                DebugExtension.DebugArrow(transform.position, current_dir.normalized * current_radius, Color.red, 0.0f, false);
                current_dir = rot * current_dir; //prepare next dir

                if (!NavMesh.SamplePosition(summon_sample_pos, out NavMeshHit hit, 10.0f, NavMesh.AllAreas)) continue;
                if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), current_dir, current_radius)) continue; //something in the way

                pos = hit.position;
                return true;
            }

            Debug.LogError($"Necromancer {name} can't find position to spawn skeleton", this.gameObject);
            pos = Vector3.zero;
            return false;
        }

        private void Summon(Vector3 summon_pos)
        {
            Vector3 lookAtObjective = (currentObjective.transform.position - summon_pos).normalized;
            NPCManager.Instance.InstantiateSkeleton(summon_pos, Quaternion.LookRotation(lookAtObjective, Vector3.up), true);
            DebugExtension.DebugWireSphere(summon_pos, Color.red, 1.0f, 10.0f, false);
            last_summon_time = TimeUtils.GetTime();
        }

        private void UpdateNavigation()
        {
            if (!agent.hasPath)
            {
                currentClosestObjectivePoint = currentObjective.GetClosestPointFrom(transform.position);
                if (NavMesh.SamplePosition(currentClosestObjectivePoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    bool path_found = agent.CalculatePath(hit.position, path_helper);
                    if (path_found) agent.SetPath(path_helper);
                    else
                    {
                        DebugExtension.DebugArrow(transform.position, hit.position - transform.position, Color.red, 0.0f, false);
                        Debug.LogErrorFormat($"agent: {name}, didn't find a path", gameObject);
                    }
                }
                else
                {
                    DebugExtension.DebugArrow(transform.position, currentClosestObjectivePoint - transform.position, Color.red, 0.0f, false);
                    Debug.LogErrorFormat($"agent: {name}, didn't find a good position towards objective", gameObject);
                }
            }
            else
            {
                Vector3 last = transform.position;
                foreach (Vector3 corner in agent.path.corners)
                {
                    DebugExtension.DebugArrow(last, corner - last, Color.cyan, 0.1f, false);
                    last = corner;
                }
            }
        }

        private void UpdateAnimation()
        {
            animator.SetBool("Walk", agent.velocity != Vector3.zero);
            animator.SetFloat("Summonity", NormalizedSummonity);
        }
    }   
}
