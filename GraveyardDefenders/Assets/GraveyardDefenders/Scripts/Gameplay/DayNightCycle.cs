using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XD.Audio;
using XD.Utils;

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
        public static int daysSurvived_s = 0;

        [Header("Assignable")]
        public Light directionalLight;
        public BreakableSet breakables;

        [Header("Variables")]
        public DAY_NIGHT_PHASE startingPhase;
        public float cycleTime = 120.0f;
        public float transitionTime = 2.0f;
        public List<DayNightCycleLightAttributes> dayNightCycleLightAttributes = new List<DayNightCycleLightAttributes>();

        [Header("Runtime")]
        public DAY_NIGHT_PHASE currentPhase = DAY_NIGHT_PHASE.NIGHT;
        public float lastPhaseStartTime = float.NegativeInfinity;
        public bool HasToSwapPhase
        {
            get { return TimeUtils.TimeSince(lastPhaseStartTime) > cycleTime; }
        }
        public int daysSurvived = 0;

        public float CycleRemainingTimeNormalized
        {
            get { return TimeUtils.TimeSince(lastPhaseStartTime) / cycleTime; }
        }

        void Start()
        {
            currentPhase = startingPhase;
            currentPhase_s = currentPhase;
            daysSurvived = 0;
            daysSurvived_s = 0;
            lastPhaseStartTime = TimeUtils.GetTime();
            lastPhaseStartTime_s = lastPhaseStartTime;

            if(currentPhase == DAY_NIGHT_PHASE.DAY)
                GlobalEvents.audioAmbienceEvent.Invoke(AUDIO_AMBIENCES.LEVEL_01_AMBIENCE_DAY);
            else
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
            {
                daysSurvived++;
                daysSurvived_s++;
            }

            DayNightCycleLightAttributes nextPhaseAttribs = dayNightCycleLightAttributes[(int)nextPhase];
            directionalLight.DOIntensity(nextPhaseAttribs.intensity, transitionTime);
            directionalLight.DOColor(nextPhaseAttribs.color, transitionTime);
            directionalLight.transform.DORotate(nextPhaseAttribs.rotation, transitionTime);

            currentPhase = nextPhase;
            currentPhase_s = nextPhase;
            lastPhaseStartTime = TimeUtils.GetTime();
            lastPhaseStartTime_s = TimeUtils.GetTime();

            if(nextPhase == DAY_NIGHT_PHASE.DAY)
            {
                //start growing trees and other materials that might be consumed
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

        }
    }
}
