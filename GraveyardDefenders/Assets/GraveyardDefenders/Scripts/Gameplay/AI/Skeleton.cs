using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using XD.Utils;

namespace XD
{
    public class Skeleton : Enemy
    {
        [Header("Variables")]
        public float attackDamage = 1.0f;
        public float attackVelocity = 1.0f;
        public float lastAttackTime = float.NegativeInfinity;
        public float attackRange = 0.05f;
        public float TimeSinceLastAttack => TimeUtils.TimeSince(lastAttackTime); 

        public override void UpdateEnemy(float deltaTime)
        {
            base.UpdateEnemy(deltaTime);
            UpdateNavigation();
            UpdateCombat();
            UpdateAnimation();
            UpdateHealthBar();

            if (DayNightCycle.currentPhase_s== DAY_NIGHT_PHASE.DAY) ReceiveHit(dayDamagePerSecond * deltaTime);
            if (currentHP <= 0) Die();
        }

        public void ReceiveHit(float damage)
        {
            currentHP -= damage;
            currentHP = Mathf.Clamp(currentHP, 0.0f, maxHP);
        }

        public void UpdateHealthBar()
        {
            if (healthBar != null) healthBar.fillAmount = CurrentHPPercent;
        }

        public void Die()
        {
            NPCManager.Instance.RemoveSkeleton(this);
        }

        private void UpdateNavigation()
        {
            if (!agent.hasPath)
            {
                currentClosestObjectivePoint = currentObjective.GetClosestPointFrom(transform.position);
                if(NavMesh.SamplePosition(currentClosestObjectivePoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
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

        private void UpdateCombat()
        {
            DebugExtension.DebugCircle(transform.position, Color.yellow, attackRange, 0.0f, false);

            //attack obstacles found in the way
            if (agent.nextOffMeshLinkData.valid && agent.nextOffMeshLinkData.offMeshLink)
            {
                BreakableObject obj = agent.nextOffMeshLinkData.offMeshLink.gameObject.GetComponent<BreakableObject>();
                if (obj && (Vector3.Distance(obj.GetClosestPoint(transform.position), transform.position) <= attackRange))
                    currentTarget = obj;
            }
            else if (agent.isOnOffMeshLink && agent.currentOffMeshLinkData.valid && currentTarget == null)
            {
                BreakableObject obj = agent.currentOffMeshLinkData.offMeshLink.gameObject.GetComponent<BreakableObject>();
                if (obj && (Vector3.Distance(obj.GetClosestPoint(transform.position), transform.position) <= attackRange))
                    currentTarget = obj;
            }

            currentClosestObjectivePoint = currentObjective.GetClosestPointFrom(transform.position);
            //attack the objective if its at range
            if (Vector3.Distance(currentClosestObjectivePoint, transform.position) <= attackRange)
            {
                agent.isStopped = true;
                transform.LookAt(currentObjective.transform.position);
                currentTarget = currentObjective.GetComponent<BreakableObject>();
            }
            else
            {
                DebugExtension.DebugArrow(transform.position, currentClosestObjectivePoint - transform.position, Color.red, 0.0f, false);
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

        private void UpdateAnimation()
        {
            animator.SetBool("Walk", agent.velocity != Vector3.zero);
        }

        private void Attack()
        {
            //Debug.Log(name + " is Attacking: " + currentTarget.name);
            float damagedealt = currentTarget.Hit(attackDamage);
            if (currentTarget.reflectDamage && damagedealt > 0.0f) ReceiveHit(damagedealt);

            animator.SetTrigger("Attack");
            if (currentTarget.destroyed) agent.CompleteOffMeshLink();
            lastAttackTime = TimeUtils.GetTime();
        }
    }
}
