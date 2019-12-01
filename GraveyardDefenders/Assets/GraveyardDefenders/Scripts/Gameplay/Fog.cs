using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class Fog : MonoBehaviour
    {
        public float uv_scrollSpeed;
        public float rotationSpeed;
        public MeshRenderer meshRenderer;
        private Vector2 helper_v2 = Vector2.zero;

        void Update()
        {
            var offset = Time.time * uv_scrollSpeed;
            helper_v2.y = offset % 1.0f;
            helper_v2.x = offset % 1.0f;
            meshRenderer.material.mainTextureOffset = helper_v2;

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
