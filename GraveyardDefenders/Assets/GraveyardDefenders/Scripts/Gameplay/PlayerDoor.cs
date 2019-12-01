using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace XD
{
    public class PlayerDoor : MonoBehaviour
    {
        [Header("Assignable")]
        public Transform door;
        public Collider doorCollider;

        [Header("Variables")]
        public float closedYRotation;
        public float openedYRotation;
        public float openingTime = 0.3f;

        [Header("Runtime")]
        public int playersInside = 0;
        public bool opened = false;
        private Vector3 helper_rotation;

        public void Awake()
        {
            playersInside = 0;
            opened = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerCharacter>())
            {
                playersInside++;
                if (playersInside > 0) OpenDoor();
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerCharacter>())
            {
                playersInside--;
                if (playersInside <= 0) CloseDoor();
            }
        }

        private void OpenDoor()
        {
            if (opened) return;

            opened = true;
            doorCollider.enabled = false;
            helper_rotation = Vector3.zero;
            helper_rotation.y = openedYRotation;
            door.transform.DOLocalRotate(helper_rotation, openingTime);
        }

        private void CloseDoor()
        {
            if (!opened) return;

            opened = false;
            doorCollider.enabled = true;
            helper_rotation = Vector3.zero;
            helper_rotation.y = closedYRotation;
            door.transform.DOLocalRotate(helper_rotation, openingTime);
        }
    }
}
