using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public enum PLAYER_ACTIONS
    {
        NONE,
        GATHER,
        REPAIR,
        BREAK,
    }

    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Assignable")]
        public CharacterController characterController;
        public Animator animator;
        public BreakableSet resources;
        public ResourceInventory inventory;

        [Header("Runtime")]
        Camera cam;
        public BreakableObject currentBreakable;
        public bool doingAction = false;
        public PLAYER_ACTIONS current_action = PLAYER_ACTIONS.NONE;

        [Header("Variables")]
        public float moveSpeed;
        public float interactRadius;
        public float interactHitTime = 0.5f;
        public float lastInteractHitTime = float.NegativeInfinity;
        public float TimeSinceLastInteractHit { get { return Time.timeSinceLevelLoad - lastInteractHitTime; } }

        [Header("Input")]
        public string HorizontalAxisName = "Horizontal";
        public string VerticalAxisName = "Vertical";
        public KeyCode interactKey = KeyCode.F;

        void Start()
        {
            current_action = PLAYER_ACTIONS.NONE;
            doingAction = false;
            currentBreakable = null;
            inventory.Reset();
            if (!cam) cam = Camera.main;
            if (!animator) animator = GetComponent<Animator>();
            if (!characterController) characterController = GetComponent<CharacterController>();
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
            lastInteractHitTime = Time.timeSinceLevelLoad;
        }

        private void StopInteracting()
        {
            doingAction = false;
            current_action = PLAYER_ACTIONS.NONE;
        }

        private void UpdateAction()
        {
            if (!doingAction) return;

            if (TimeSinceLastInteractHit >= interactHitTime)
            {
                switch (current_action)
                {
                    case PLAYER_ACTIONS.GATHER:
                        GathereableResource gathereable = currentBreakable as GathereableResource;
                        float gathered = gathereable.Gather(1.0f);
                        inventory.AddResource(gathereable.type, gathered);
                        lastInteractHitTime = Time.timeSinceLevelLoad;
                        break;
                    case PLAYER_ACTIONS.REPAIR:
                        float repairedAmmount = currentBreakable.Repair(2.0f);
                        inventory.SubstractResource(currentBreakable.repairResource, 1.0f);
                        lastInteractHitTime = Time.timeSinceLevelLoad;
                        if (!inventory.HasResource(currentBreakable.repairResource)) StopInteracting();
                        break;
                    case PLAYER_ACTIONS.BREAK:
                        float hitAmmount = currentBreakable.Hit(1.0f);
                        lastInteractHitTime = Time.timeSinceLevelLoad;
                        break;
                    case PLAYER_ACTIONS.NONE:
                        break;
                }
            }
        }

        void Update()
        {
            UpdateCurrentBreakable();

            Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
            Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
            DebugExtension.DebugArrow(transform.position, right, Color.red, 0.0f, false);
            DebugExtension.DebugArrow(transform.position, forward, Color.blue, 0.0f, false);

            float horizontal = Input.GetAxis(HorizontalAxisName);
            float vertical = Input.GetAxis(VerticalAxisName);
            if (!doingAction && currentBreakable && Input.GetKey(interactKey))
                AttemptInteraction();


            Vector3 movement = (horizontal * right) + (vertical * forward);
            movement = movement.normalized * moveSpeed * Time.deltaTime;

            if (movement != Vector3.zero)
            {
                if(doingAction) StopInteracting();
                animator.SetBool("Walk", true);
                transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
                characterController.Move(movement);
            }
            else
            {
                animator.SetBool("Walk", false);
            }

            animator.SetBool("Gathering", doingAction);
            if (doingAction) UpdateAction();
        }
    }
}
