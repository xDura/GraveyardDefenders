using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    [ExecuteInEditMode]
    public class UIHealthBar : MonoBehaviour
    {
        Camera cam;
        Camera Cam { get { if (!cam) cam = Camera.main; return cam; } }

        void LateUpdate()
        {
            transform.LookAt(Cam.transform.position, Cam.transform.up);
        }
    }
}
