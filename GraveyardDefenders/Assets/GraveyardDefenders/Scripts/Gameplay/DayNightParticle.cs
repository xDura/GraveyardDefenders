using System.Collections.Generic;
using UnityEngine;
using XD.Utils;

namespace XD
{
    public class DayNightParticle : MonoBehaviour
    {
        public DAY_NIGHT_PHASE activePhase;
        public List<ParticleSystem> particleSystems;

        void OnEnable()
        {
            GlobalEvents.newDayStarted.AddListener(OnPhaseChange);
            GlobalEvents.newNightStared.AddListener(OnPhaseChange);
            OnPhaseChange();
        }

        void OnDisable()
        {
            GlobalEvents.newDayStarted.RemoveListener(OnPhaseChange);
            GlobalEvents.newNightStared.RemoveListener(OnPhaseChange);
        }

        public void OnPhaseChange()
        {
            for (int i = 0; i < particleSystems.Count; i++)
            {
                ParticleSystem ps = particleSystems[i];
                if (DayNightCycle.currentPhase_s == activePhase) PSUtils.SetEmission(ps, true);
                else PSUtils.SetEmission(ps, false);
            }
        }
    }   
}
