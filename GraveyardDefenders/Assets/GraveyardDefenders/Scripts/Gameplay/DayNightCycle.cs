using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XD.Audio;

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
        public static DAY_NIGHT_PHASE currentPhase_s = DAY_NIGHT_PHASE.NIGHT;
        public static float lastPhaseStartTime_s = float.NegativeInfinity;

        [Header("Assignable")]
        public Light directionalLight;
        public BreakableSet breakables;

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
            GlobalEvents.audioAmbienceEvent.Invoke(AUDIO_AMBIENCES.LEVEL_01_AMBIENCE_NIGHT);
        }

        void Update()
        {
            if (HasToSwapPhase)
                DoTransitionCycle();
        }

        public void DoTransitionCycle()
        {
            DAY_NIGHT_PHASE nextPhase = (DAY_NIGHT_PHASE)((int)(currentPhase + 1) % dayNightCycleLightAttributes.Count);
            if (currentPhase == DAY_NIGHT_PHASE.NIGHT)
                daysSurvived++;

            if(nextPhase == DAY_NIGHT_PHASE.DAY)
            {
                for (int i = 0; i < breakables.items.Count; i++)
                {
                    BreakableObject breakable = breakables.items[i];
                    if (breakable is GathereableResource)
                        ((GathereableResource)breakable).StartGrowing();                
                }

                GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.START_DAY, gameObject);
                GlobalEvents.audioAmbienceEvent.Invoke(AUDIO_AMBIENCES.LEVEL_01_AMBIENCE_DAY);
                GlobalEvents.newDayStarted.Invoke();
            }
            else
            {
                GlobalEvents.audioFXEvent.Invoke(AUDIO_FX.START_NIGHT, gameObject);
                GlobalEvents.audioAmbienceEvent.Invoke(AUDIO_AMBIENCES.LEVEL_01_AMBIENCE_NIGHT);
                GlobalEvents.newNightStared.Invoke();
            }

            DayNightCycleLightAttributes nextPhaseAttribs = dayNightCycleLightAttributes[(int)nextPhase];
            directionalLight.DOIntensity(nextPhaseAttribs.intensity, transitionTime);
            directionalLight.DOColor(nextPhaseAttribs.color, transitionTime);
            directionalLight.transform.DORotate(nextPhaseAttribs.rotation, transitionTime);

            currentPhase = nextPhase;
            currentPhase_s = nextPhase;
            lastPhaseStartTime = Time.timeSinceLevelLoad;
            lastPhaseStartTime_s = Time.timeSinceLevelLoad;
        }
    }
}
