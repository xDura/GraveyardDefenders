#if UNITY_EDITOR
using UnityEngine;

namespace XD
{
    public class RectTransformInspector : MonoBehaviour
    {
        public RectTransform rT;
        public Vector2 anchoredPosition;
        public Vector2 offsetMax;
        public Vector2 offsetMin;
        public Vector3 anchoredPosition3D;
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 pivot;
        public Vector2 sizeDelta;

        void Awake()
        {
            rT = GetComponent<RectTransform>();    
        }

        void Update()
        {
            anchoredPosition = rT.anchoredPosition;
            offsetMax = rT.offsetMax;
            offsetMin = rT.offsetMin;
            anchoredPosition3D = rT.anchoredPosition3D;
            anchorMin = rT.anchorMin;
            anchorMax = rT.anchorMax;
            pivot = rT.pivot;
            sizeDelta = rT.sizeDelta;
        }
    }   
}
#endif
