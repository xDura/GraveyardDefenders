using UnityEngine;

namespace XD.Utils
{
    public static class TimeUtils
    {
        public static float TimeSince(float timeSinceLevelLoad)
        {
            return Time.timeSinceLevelLoad - timeSinceLevelLoad;
        }

        public static float GetTime()
        {
            return Time.timeSinceLevelLoad;
        }
    }

    public static class PSUtils
    {
        public static void SetEmission(ParticleSystem ps, bool emisionToSet)
        {
            ParticleSystem.EmissionModule em = ps.emission;
            em.enabled = emisionToSet;
        }
    }
}
