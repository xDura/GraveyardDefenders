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

        [Header("Runtime")]
        Camera cam;

        [Header("Variables")]
        public float moveSpeed;

        void Start()
        {
            if (!cam) cam = Camera.main;
            if (!animator) animator = GetComponent<Animator>();
            if (!characterController) characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
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
