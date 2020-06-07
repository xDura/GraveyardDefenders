using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using XD.Utils;

namespace XD
{
    public class SkeletonController : MonoBehaviour
    {
        [Header("Assignables")]
        public NavMeshAgent agent;
        public Animator animator;
        public Image healthBar;

        [Header("Runtime")]
        public float maxRayDistance = 100.0f;
        public NavMeshHit navmesh_hit_helper;
        public RaycastHit hit_helper;
        public BreakableObject currentTarget = default;
        Objective currentObjective = default;
        NavMeshPath path_helper = default;
        public Vector3 currentClosestObjectivePoint = Vector3.zero;
        public float currentHP;

        [Header("Variables")]
        public float attackDamage = 1.0f;
        public float attackVelocity = 1.0f;
        public float lastAttackTime = float.NegativeInfinity;
        public float attackRange = 0.05f;
        public float dayDamagePerSecond;
        public float maxHP;
        public float TimeSinceLastAttack
        {
            get { return TimeUtils.TimeSince(lastAttackTime); }
        }

        public float CurrentHPPercent
        {
            get { return currentHP / maxHP; }
        }

        void Start()
        {
            if (!agent) agent = GetComponent<NavMeshAgent>();
            if (!animator) animator = GetComponent<Animator>();

            Init();
        }

        public void Init()
        {
            if(path_helper == null) path_helper = new NavMeshPath();
            if(!currentObjective) currentObjective = FindObjectOfType<Objective>();
            currentHP = maxHP;
            agent.autoTraverseOffMeshLink = false;
            currentTarget = null;
        }

        void Update()
        {
            UpdateNavigation();
            UpdateCombat();
            UpdateAnimation();
            UpdateHealthBar();

            if (DayNightCycle.currentPhase_s== DAY_NIGHT_PHASE.DAY)
            {
                currentHP -= dayDamagePerSecond * Time.deltaTime;
                if (currentHP <= 0) Die();
            }
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
            currentTarget.Hit(attackDamage);
            animator.SetTrigger("Attack");
            if (currentTarget.destroyed) agent.CompleteOffMeshLink();
            lastAttackTime = TimeUtils.GetTime();
        }
    }
}
