using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Assignable")]
        public CharacterController characterController;
        public Animator animator;
        public GathereableSet resources;
        public ResourceInventory inventory;

        [Header("Runtime")]
        Camera cam;
        public GathereableResource currentGathereable;
        public bool gathering = false;

        [Header("Variables")]
        public float moveSpeed;
        public float interactRadius;
        public float gatheringTime = 0.5f;
        public float lastGatherTime = float.NegativeInfinity;
        public float TimeSinceLastGather { get { return Time.timeSinceLevelLoad - lastGatherTime; } }

        void Start()
        {
            if (!cam) cam = Camera.main;
            if (!animator) animator = GetComponent<Animator>();
            if (!characterController) characterController = GetComponent<CharacterController>();
        }

        void UpdateCurrentResource()
        {
            DebugExtension.DebugCircle(transform.position, interactRadius, 0.0f, false);
            currentGathereable = null;

            float nearestDistance = float.PositiveInfinity;
            for (int i = 0; i < resources.items.Count; i++)
            {
                float currentDistance = Vector3.Distance(resources.items[i].transform.position, transform.position);
                if (currentDistance < nearestDistance && currentDistance < interactRadius)
                {
                    currentGathereable = resources.items[i].GetComponent<GathereableResource>();
                    nearestDistance = currentDistance;
                }
            }

            if (currentGathereable != null)
                DebugExtension.DebugPoint(currentGathereable.transform.position, 1.0f, 0.0f, false);
        }

        private void StartGathering()
        {
            transform.LookAt(currentGathereable.transform.position, Vector3.up);
            gathering = true;
            lastGatherTime = Time.timeSinceLevelLoad;
        }

        private void StopGathering()
        {
            gathering = false;
        }

        private void UpdateGathering()
        {
            if (!gathering) return;

            if (TimeSinceLastGather >= gatheringTime)
            {
                float gathered = currentGathereable.Gather(1.0f);
                inventory.AddResource(currentGathereable.type, gathered);
                lastGatherTime = Time.timeSinceLevelLoad;
            }
        }

        void Update()
        {
            UpdateCurrentResource();

            Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
            Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
            DebugExtension.DebugArrow(transform.position, right, Color.red, 0.0f, false);
            DebugExtension.DebugArrow(transform.position, forward, Color.blue, 0.0f, false);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (!gathering && currentGathereable && Input.GetKey(KeyCode.Space)) StartGathering();

            Vector3 movement = (horizontal * right) + (vertical * forward);
            movement = movement.normalized * moveSpeed * Time.deltaTime;

            if (movement != Vector3.zero)
            {
                if(gathering) StopGathering();
                animator.SetBool("Walk", true);
                transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
                characterController.Move(movement);
            }
            else
            {
                animator.SetBool("Walk", false);
            }

            animator.SetBool("Gathering", gathering);
            if (gathering) UpdateGathering();
        }
    }
}
