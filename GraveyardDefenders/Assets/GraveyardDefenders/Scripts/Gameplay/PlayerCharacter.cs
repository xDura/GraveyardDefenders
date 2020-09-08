﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XD.Utils;

namespace XD
{
    [RequireComponent(typeof(InteractSystem), typeof(CharacterController), typeof(Animator))]
    public class PlayerCharacter : MonoBehaviour
    {
        [Header("Assignable")]
        public CharacterController characterController;
        public InteractSystem interactSystem;
        public Animator animator;
        public ResourceInventory inventory;
        public GameObject pickaxe;
        public GameObject axe;
        public GameObject hammer;
        public ParticleSystem walkDustParticles;

        [Header("Runtime")]
        Camera cam;
        [NonSerialized] public int id;
        public bool isLocal = false;

        #region SAFE_AREA
        public Vector3 lastSafeAreaExitPosition = Vector3.zero;
        private bool inSafeArea = false;
        private float lastTimeSafeAreaEnter = float.NegativeInfinity;
        private float lastTimeSafeAreaExit = float.NegativeInfinity;

        public float TimeInSafeArea => inSafeArea ? TimeUtils.TimeSince(lastTimeSafeAreaEnter) : 0f;
        public float TimeOutsideSafeArea => !inSafeArea ? TimeUtils.TimeSince(lastTimeSafeAreaExit) : 0f;
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

        [NonSerialized] public Vector2 moveVector;
        [NonSerialized] public bool interactPressedThisFrame = false;
        [NonSerialized] public bool interactReleasedThisFrame = false;
        public bool HasResource(RESOURCE_TYPE type) { return inventory.HasResource(type); }

        void Start()
        {
            inventory.Reset();
            pickaxe.SetActive(false);
            axe.SetActive(false);
            if (!cam) cam = Camera.main;
            if (!animator) animator = GetComponent<Animator>();
            if (!characterController) characterController = GetComponent<CharacterController>();
        }


        public void Respawn()
        {
            interactSystem.StopCurrentAction();
            PlayerSpawnPoints sp = FindObjectOfType<PlayerSpawnPoints>();
            Transform n = sp.spawns[id];
            transform.position = n.position;
            transform.rotation = n.rotation;
            ParticleSystemEvents.SpawnParticleEvent.Invoke(Constants.Instance.respawnParticles, n.position, Quaternion.LookRotation(Vector3.up));
        }

        public void OnActionEnded(PLAYER_ACTIONS action)
        {
            pickaxe.SetActive(false);
            axe.SetActive(false);
            hammer.SetActive(false);
        }

        public void RotateToWards(Vector3 worldPoint)
        {
            Vector3 playerPos = transform.position;
            Vector3 lookAtPos = worldPoint;
            lookAtPos.y = playerPos.y;
            Vector3 lookAtTargetDir = (lookAtPos - playerPos).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookAtTargetDir, Vector3.up), 8f * Time.deltaTime);
        }

        public void ManualUpdate()
        {
            Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
            Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;

            float horizontal = moveVector.x;
            float vertical = moveVector.y;

            Vector3 movement = (horizontal * right) + (vertical * forward);
            movement = movement.normalized * moveSpeed * Time.deltaTime;

            if (movement != Vector3.zero)
            {
                animator.SetBool("Walk", true);
                PlayWalkDust();
                transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
                characterController.Move(movement);
            }
            else
            {
                StopWalkDust();
                animator.SetBool("Walk", false);
            }
        }

        public void PlayWalkDust()
        {
            if(!walkDustParticles.isPlaying) walkDustParticles.Play();
        }

        public void StopWalkDust()
        {
            walkDustParticles.Stop();
        }
    }
}
