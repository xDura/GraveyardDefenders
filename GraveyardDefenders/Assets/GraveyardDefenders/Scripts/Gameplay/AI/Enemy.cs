using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using XD.Utils;

namespace XD
{
    public class Enemy : MonoBehaviour
    {
        [Header("Assignables")]
        public NavMeshAgent agent;
        public Animator animator;
        public Image healthBar;
        public CapsuleCollider capsule;

        [Header("Runtime")]
        public float maxRayDistance = 100.0f;
        public NavMeshHit navmesh_hit_helper;
        public RaycastHit hit_helper;
        public BreakableObject currentTarget = default;
        protected Objective currentObjective = default;
        protected NavMeshPath path_helper = default;
        public Vector3 currentClosestObjectivePoint = Vector3.zero;
        public float currentHP;
        public float CurrentHPPercent => currentHP / maxHP;

        //TODO: move all info about types of enemies to scriptables
        //maxHP, damage they get from stuff etc
        [Header("Variables")]
        public float dayDamagePerSecond;
        public float maxHP;

        public void Start() { StartEnemy(); }

        void Update() 
        {
            UpdateEnemy(Time.deltaTime); // this should be called by NPCManager on all of them
        }

        public virtual void  StartEnemy()
        {
            if (!agent) agent = GetComponent<NavMeshAgent>();
            if (!animator) animator = GetComponent<Animator>();
            if (!capsule) capsule = GetComponent<CapsuleCollider>();
            Init();
        }

        public virtual void Init()
        {
            if (path_helper == null) path_helper = new NavMeshPath();
            if (!currentObjective) currentObjective = FindObjectOfType<Objective>();
            currentHP = maxHP;
            agent.autoTraverseOffMeshLink = false;
            currentTarget = null;
        } //once they are spawned
        public virtual void DeInit() {} //once they return to the pool

        public virtual void UpdateEnemy(float deltaTime) {}
    }   
}
