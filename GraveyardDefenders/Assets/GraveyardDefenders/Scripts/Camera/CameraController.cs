using UnityEngine;
using System.Collections.Generic;

namespace XD
{
    public class CameraController : MonoBehaviour
    {
        public List<Transform> activeTransforms;
        public Bounds bounds = new Bounds();
        public float zOffset;
        public float xOffset;

        public bool debugBounds = false;

        private Vector3 helperVector = new Vector3();

        public void Awake()
        {
            PlayerEvents.playerAddedEvnt.AddListener(AddPlayer);
        }

        public void OnDestroy()
        {
            PlayerEvents.playerAddedEvnt.RemoveListener(AddPlayer);
        }

        public void AddPlayer(PlayerCharacter pc)
        {
            AddTransform(pc?.transform);
        }

        public void RemovePlayer(PlayerCharacter pc)
        {
            RemoveTransform(pc?.transform);
        }

        public void AddTransform(Transform t)
        {
            if (!activeTransforms.Contains(t)) activeTransforms.Add(t);
        }

        public void RemoveTransform(Transform t)
        {
            if (activeTransforms.Contains(t)) activeTransforms.Remove(t);
        }

        void UpdateBounds()
        {
            if (activeTransforms.Count == 0) return;

            bounds.center = activeTransforms[0].position;
            bounds.size = Vector3.zero;
            for (int i = 0; i < activeTransforms.Count; i++)
                bounds.Encapsulate(activeTransforms[i].position);
        }

        void LateUpdate()
        {
            DebugExtension.DebugBounds(bounds, Color.blue, 0.0f, false);
            UpdateBounds();
            helperVector.x = (bounds.center.x + xOffset);
            helperVector.z = (bounds.center.z + zOffset);
            helperVector.y = transform.position.y;

            transform.position = helperVector;
        }
    }   
}
