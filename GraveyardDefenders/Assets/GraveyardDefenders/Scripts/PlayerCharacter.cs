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

        public GathereableResourceSet resources;

        [Header("Runtime")]
        Camera cam;
        public GathereableResource currentGathereable;

        [Header("Variables")]
        public float moveSpeed;
        public float interactRadius;

        void Start()
        {
            if (!cam) cam = Camera.main;
            if (!animator) animator = GetComponent<Animator>();
            if (!characterController) characterController = GetComponent<CharacterController>();
        }

        void UpdateResources()
        {
            DebugExtension.DebugCircle(transform.position, interactRadius, 0.0f, false);
            currentGathereable = null;

            float nearestDistance = float.PositiveInfinity;
            for (int i = 0; i < resources.gathereables.Count; i++)
            {
                float currentDistance = Vector3.Distance(resources.gathereables[i].transform.position, transform.position);
                if (currentDistance < nearestDistance && currentDistance < interactRadius)
                {
                    currentGathereable = resources.gathereables[i].GetComponent<GathereableResource>();
                    nearestDistance = currentDistance;
                }
            }

            if (currentGathereable != null)
                DebugExtension.DebugPoint(currentGathereable.transform.position, 1.0f, 0.0f, false);
        }

        void Update()
        {
            UpdateResources();

            Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
            Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
            DebugExtension.DebugArrow(transform.position, right, Color.red, 0.0f, false);
            DebugExtension.DebugArrow(transform.position, forward, Color.blue, 0.0f, false);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = (horizontal * right) + (vertical * forward);
            movement = movement.normalized * moveSpeed * Time.deltaTime;

            if (movement != Vector3.zero)
            {
                animator.SetBool("Walk", true);
                transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
                characterController.Move(movement);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
        }
    }
}
