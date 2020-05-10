using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XD.Utils;

namespace XD
{
    public enum PLAYER_ACTIONS
    {
        NONE,
        GATHER,
        REPAIR,
        BREAK,
        UPGRADE,
    }

    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Assignable")]
        public CharacterController characterController;
        public Animator animator;
        public BreakableSet resources;
        public ResourceInventory inventory;
        public GameObject pickaxe;
        public GameObject axe;
        public GameObject hammer;
        public ParticleSystem walkDustParticles;
        public GameObject rockHitParticles;
        public GameObject woodHitParticles;
        public GameObject respawnParticles;

        [Header("Runtime")]
        Camera cam;
        [NonSerialized] public BreakableObject currentBreakable;
        [NonSerialized] public bool doingAction = false;
        [NonSerialized] public PLAYER_ACTIONS current_action = PLAYER_ACTIONS.NONE;
        [NonSerialized] public int id;
        [NonSerialized] public List<Upgradeable> nearbyUpgradeables = new List<Upgradeable>();
        [NonSerialized] public Upgradeable bestUpgradeable;

        #region SAFE_AREA
        public Vector3 lastSafeAreaExitPosition = Vector3.zero;
        private bool inSafeArea = false;
        private float lastTimeSafeAreaEnter = float.NegativeInfinity;
        private float lastTimeSafeAreaExit = float.NegativeInfinity;

        public float TimeInSafeArea
        {
            get 
            {
                if (inSafeArea) return TimeUtils.TimeSince(lastTimeSafeAreaEnter);
                else return 0f;
            } 
        }
        public float TImeOutsideSafeArea 
        {
            get
            {
                if (!inSafeArea) return TimeUtils.TimeSince(lastTimeSafeAreaExit);
                else return 0;
            }
        }

        public void SetSafeArea(bool isInSafeArea)
        {
            if (isInSafeArea) lastTimeSafeAreaEnter = TimeUtils.GetTime();
            else
            {
                lastSafeAreaExitPosition = transform.position;
                lastTimeSafeAreaExit = TimeUtils.GetTime();
            }
            inSafeArea = isInSafeArea;
        }
        #endregion

        [Header("Variables")]
        public float moveSpeed;
        public float interactRadius;
        public float interactHitTime = 0.5f;
        [Tooltip("Time it takes at the start of interaction to start it usefull to match animation")]
        public float interactStartHitTimeOffset = 0.25f;
        public float lastInteractHitTime = float.NegativeInfinity;
        public float TimeSinceLastInteractHit { get { return TimeUtils.TimeSince(lastInteractHitTime); } }
        public bool IsHitReady { get { return TimeSinceLastInteractHit >= interactHitTime; } }

        [NonSerialized] public Vector2 moveVector;
        [NonSerialized] public bool interactPressedThisFrame = false;
        [NonSerialized] public bool interactReleasedThisFrame = false;

        void Start()
        {
            current_action = PLAYER_ACTIONS.NONE;
            doingAction = false;
            currentBreakable = null;
            inventory.Reset();
            pickaxe.SetActive(false);
            axe.SetActive(false);
            if (!cam) cam = Camera.main;
            if (!animator) animator = GetComponent<Animator>();
            if (!characterController) characterController = GetComponent<CharacterController>();
        }

        public void Respawn()
        {
            PlayerSpawnPoints sp = FindObjectOfType<PlayerSpawnPoints>();
            Transform n = sp.spawns[id];
            transform.position = n.position;
            transform.rotation = n.rotation;
            ParticleSystemEvents.SpawnParticleEvent.Invoke(respawnParticles, n.position, Quaternion.LookRotation(Vector3.up));
        }

        void UpdateCurrentBreakable()
        {
            DebugExtension.DebugCircle(transform.position, interactRadius, 0.0f, false);
            currentBreakable = null;

            float nearestDistance = float.PositiveInfinity;
            for (int i = 0; i < resources.items.Count; i++)
            {
                BreakableObject breakable = resources.items[i];
                Vector3 pos = breakable.GetClosestPoint(transform.position);
                float currentDistance = Vector3.Distance(pos, transform.position);
                DebugExtension.DebugPoint(pos, Color.white, 1.0f, 0.0f, false);
                if ((currentDistance < interactRadius) && (currentDistance < nearestDistance) && (breakable.CanGather || (breakable.CanRepair && inventory.HasResource(breakable.repairResource))))
                {
                    DebugExtension.DebugArrow(transform.position, pos - transform.position, Color.white, 0.0f, false);
                    currentBreakable = breakable;
                    nearestDistance = currentDistance;
                }
            }

            if (currentBreakable != null)
                DebugExtension.DebugPoint(currentBreakable.GetCenter(), Color.blue, 1.0f, 0.0f, false);
            else if (doingAction)
                StopInteracting();
        }

        private void AttemptInteraction()
        {
            if (currentBreakable.CanGather) current_action = PLAYER_ACTIONS.GATHER;
            else if (currentBreakable.isRepairable && inventory.HasResource(currentBreakable.repairResource)) current_action = PLAYER_ACTIONS.REPAIR;
            else
            {
                current_action = PLAYER_ACTIONS.NONE;
                return;
            }

            StartInteraction();
        }

        private void StartInteraction()
        {
            Vector3 center = currentBreakable.GetCenter();
            center.y = transform.position.y;
            transform.LookAt(center, Vector3.up);
            doingAction = true;
            lastInteractHitTime = TimeUtils.GetTime() - interactStartHitTimeOffset;
            if(current_action == PLAYER_ACTIONS.GATHER)
            {
                GathereableResource resource = currentBreakable as GathereableResource;
                if (resource)
                {
                    if (resource.type == RESOURCE_TYPE.STONE)
                        pickaxe.SetActive(true);
                    else if (resource.type == RESOURCE_TYPE.WOOD)
                        axe.SetActive(true);
                }
            }
            else if (current_action == PLAYER_ACTIONS.REPAIR)
            {
                hammer.SetActive(true);
            }
        }

        private void StopInteracting()
        {
            pickaxe.SetActive(false);
            axe.SetActive(false);
            hammer.SetActive(false);
            doingAction = false;
            current_action = PLAYER_ACTIONS.NONE;
        }

        private void UpdateAction()
        {
            if (!doingAction) return;

            if (IsHitReady)
            {
                switch (current_action)
                {
                    case PLAYER_ACTIONS.GATHER:
                        GathereableResource gathereable = currentBreakable as GathereableResource;
                        float gathered = gathereable.Gather(1.0f);
                        if (gathereable.type == RESOURCE_TYPE.STONE)
                        {
                            Vector3 pos = transform.position + (Vector3.up * 0.5f) + (transform.forward * 0.5f);
                            ParticleSystemEvents.SpawnParticleEvent.Invoke(rockHitParticles, pos, Quaternion.identity);
                            GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.MINING_STONE, this.gameObject);
                        }
                        else
                        {
                            Vector3 pos = transform.position + (Vector3.up * 0.5f) + (transform.forward * 0.5f);
                            ParticleSystemEvents.SpawnParticleEvent.Invoke(woodHitParticles, pos, Quaternion.identity);
                            GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.CHOP_WOOD, this.gameObject);
                        }
                        inventory.AddResource(gathereable.type, gathered);
                        lastInteractHitTime = TimeUtils.GetTime();
                        break;
                    case PLAYER_ACTIONS.REPAIR:
                        float repairedAmmount = currentBreakable.Repair(2.0f);
                        inventory.SubstractResource(currentBreakable.repairResource, 1.0f);
                        GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.REPAIR_WOOD, this.gameObject);
                        lastInteractHitTime = TimeUtils.GetTime();
                        if (!inventory.HasResource(currentBreakable.repairResource)) StopInteracting();
                        break;
                    case PLAYER_ACTIONS.BREAK:
                        float hitAmmount = currentBreakable.Hit(1.0f);
                        lastInteractHitTime = TimeUtils.GetTime();
                        break;
                    case PLAYER_ACTIONS.NONE:
                        break;
                }
            }
        }

        public void UpdateCurrentUpgradeable()
        {
            float bestDist = float.PositiveInfinity;
            if (bestUpgradeable) bestUpgradeable.HidePrompt();
            bestUpgradeable = null;

            for (int i = 0; i < nearbyUpgradeables.Count; i++)
            {
                Upgradeable u = nearbyUpgradeables[i];
                float currentDist = Vector3.Distance(u.transform.position, transform.position);
                if(currentDist < bestDist && !u.IsMaxLevel)
                {
                    bestDist = currentDist;
                    bestUpgradeable = u;
                }
            }

            if (bestUpgradeable != null) bestUpgradeable.ShowPrompt();
        }

        public void ManualUpdate()
        {
            UpdateCurrentBreakable();
            UpdateCurrentUpgradeable();

            Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
            Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;

            float horizontal = moveVector.x;
            float vertical = moveVector.y;
            if (!doingAction && interactPressedThisFrame && (currentBreakable /*|| bestUpgradeable*/))
            {
                AttemptInteraction();
            }
            //TODO: refactor this and put upgradeables inside the action system
            else if (!doingAction && interactPressedThisFrame)
            {
                if (bestUpgradeable && bestUpgradeable.CanBeUpgraded(inventory))
                {
                    bestUpgradeable.Upgrade();
                    bestUpgradeable = null;
                }
            }


            Vector3 movement = (horizontal * right) + (vertical * forward);
            movement = movement.normalized * moveSpeed * Time.deltaTime;

            if (movement != Vector3.zero)
            {
                if(doingAction) StopInteracting();
                animator.SetBool("Walk", true);
                if(!walkDustParticles.isPlaying) walkDustParticles.Play();
                transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
                characterController.Move(movement);
            }
            else
            {
                walkDustParticles.Stop();
                animator.SetBool("Walk", false);
            }

            if (doingAction)
            {
                if (current_action == PLAYER_ACTIONS.BREAK || current_action == PLAYER_ACTIONS.REPAIR || (current_action == PLAYER_ACTIONS.GATHER && (currentBreakable as GathereableResource).type == RESOURCE_TYPE.WOOD))
                    animator.SetBool("ChopWood", doingAction);
                else
                    animator.SetBool("Minning", true);
            }
            else
            {
                animator.SetBool("Minning", false);
                animator.SetBool("ChopWood", false);
            }

            if (doingAction) UpdateAction();
        }
    }
}
