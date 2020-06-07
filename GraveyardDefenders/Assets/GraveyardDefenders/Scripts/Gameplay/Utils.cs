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

    public static class ProgressionUtils
    {
        public static int DayToProgression(int day)
        {
            if ((day % 5) == 0) return 0; //every 5 days we have a rest day
            return day * day;
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
