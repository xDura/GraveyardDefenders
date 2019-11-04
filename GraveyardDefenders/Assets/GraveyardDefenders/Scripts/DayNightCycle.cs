using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace XD
{
    public enum DAY_NIGHT_PHASE
    {
        DAY,
        NIGHT
    }

    [System.Serializable]
    public class DayNightCycleLightAttributes
    {
        public Vector3 rotation;
        public Color color;
        public float intensity;
        public Color fogColor;
    }

    public class DayNightCycle : MonoBehaviour
    {
        [Header("Assignable")]
        public Light directionalLight;
        public Fog fog;

        [Header("Variables")]
        public float cycleTime = 120.0f;
        public float transitionTime = 2.0f;
        public List<DayNightCycleLightAttributes> dayNightCycleLightAttributes = new List<DayNightCycleLightAttributes>();

        [Header("Runtime")]
        public DAY_NIGHT_PHASE currentPhase = DAY_NIGHT_PHASE.NIGHT;
        public float lastPhaseStartTime = float.NegativeInfinity;
        public bool HasToSwapPhase
        {
            get { return (Time.timeSinceLevelLoad - lastPhaseStartTime) > cycleTime; }
        }
        public int daysSurvived = 0;

        public float CycleRemainingTimeNormalized
        {
            get { return (Time.timeSinceLevelLoad - lastPhaseStartTime) / cycleTime; }
        }

        void Start()
        {
            daysSurvived = 0;
            lastPhaseStartTime = Time.timeSinceLevelLoad;
        }

        void Update()
        {
            if (HasToSwapPhase)
                DoTransitionCycle();
        }

        public void DoTransitionCycle()
        {
            DAY_NIGHT_PHASE nextPhase = (DAY_NIGHT_PHASE)((int)(currentPhase + 1) % dayNightCycleLightAttributes.Count);
            if (nextPhase == DAY_NIGHT_PHASE.NIGHT) daysSurvived++;
            Debug.LogFormat($"DayNightCycle: Transition from {currentPhase.ToString()} to: {nextPhase.ToString()}");
            DayNightCycleLightAttributes nextPhaseAttribs = dayNightCycleLightAttributes[(int)nextPhase];
            directionalLight.DOIntensity(nextPhaseAttribs.intensity, transitionTime);
            directionalLight.DOColor(nextPhaseAttribs.color, transitionTime);
            directionalLight.transform.DORotate(nextPhaseAttribs.rotation, transitionTime);
            currentPhase = nextPhase;
            fog.meshRenderer.material.DOColor(nextPhaseAttribs.fogColor, transitionTime);
            lastPhaseStartTime = Time.timeSinceLevelLoad;
        }
    }
}
