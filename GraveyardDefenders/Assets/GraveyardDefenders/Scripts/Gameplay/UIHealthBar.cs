using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [ExecuteInEditMode]
    public class UIHealthBar : MonoBehaviour
    {
        Camera cam;

        void Awake() { cam = Camera.main; }

        void LateUpdate()
        {
            transform.LookAt(cam.transform.position, cam.transform.up);
        }
    }
}
