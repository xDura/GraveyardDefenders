using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

namespace XD
{
    public class NightLights : MonoBehaviour
    {
        public List<Light> lights;
        public List<float> cached_intensities;
        public List<CFX_LightFlicker> cached_flickers;
        public float fadeTime;
        public float delay;

        private bool currentEnabled;
        private float lastEnabledDesiredChangeTime = float.NegativeInfinity;
        private bool HasToChangeEnabled => (currentEnabled != desiredEnabled) && ((Time.timeSinceLevelLoad - lastEnabledDesiredChangeTime) >= delay);
        private bool desiredEnabled;

        public void Start()
        {
            Cache();
            if (DayNightCycle.currentPhase_s == DAY_NIGHT_PHASE.DAY) DisableLights();
            else EnableLights();

            desiredEnabled = currentEnabled;

            GlobalEvents.newDayStarted.AddListener(TransitionToDisabled);
            GlobalEvents.newNightStared.AddListener(TransitionToEnabled);
        }

        public void TransitionToEnabled()
        {
            desiredEnabled = true;
            lastEnabledDesiredChangeTime = Time.timeSinceLevelLoad;
        }

        public void TransitionToDisabled()
        {
            desiredEnabled = false;
            lastEnabledDesiredChangeTime = Time.timeSinceLevelLoad;
        }

        void Update()
        {
            if (HasToChangeEnabled)
            {
                if (desiredEnabled)
                    EnableLights();
                else 
                    DisableLights();
            }
        }

        void Cache()
        {
            cached_intensities = new List<float>(lights.Count);
            for (int i = 0; i < lights.Count; i++)
            {
                cached_intensities.Add(lights[i].intensity);
                CFX_LightFlicker flicker = lights[i].GetComponent<CFX_LightFlicker>();
                if (flicker)
                {
                    cached_flickers.Add(flicker);
                    flicker.offset = Random.Range(0.0f, 2.0f);
                }
            }
        }

        public void EnableLights()
        {
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].enabled = true;
                if (i == 0)
                    lights[i].DOIntensity(cached_intensities[i], fadeTime).SetDelay(Random.Range(0.0f, 1.0f)).OnComplete(EnableLightsEnd);
                else
                    lights[i].DOIntensity(cached_intensities[i], fadeTime).SetDelay(Random.Range(0.0f, 0.5f));
            }
            currentEnabled = true;
        }
        public void DisableLights()
        {
            EnableFlickers(false);
            for (int i = 0; i < lights.Count; i++)
            {
                if (i == 0)
                    lights[i].DOIntensity(0f, fadeTime).OnComplete(DisableLightsEnd);
                else
                    lights[i].DOIntensity(0f, fadeTime);
            }
            currentEnabled = false;
        }

        void EnableLightsEnd()
        {
            EnableFlickers(true);
        }

        void EnableFlickers(bool enable)
        {
            for (int i = 0; i < cached_flickers.Count; i++)
                cached_flickers[i].enabled = enable;
        }


        void DisableLightsEnd()
        {
            for (int i = 0; i < lights.Count; i++)
                lights[i].enabled = false;
        }
    }   
}
