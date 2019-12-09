using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "XD/Constants")]
    public class Constants : ScriptableObject
    {
        [System.Flags]
        public enum DEBUG_FLAGS
        {
            PLAYER,
            GATHEREABLE_OBJECTS,
            RESOURCES_INVENTORY,
            AI,
            DAY_NIGHT_CYCLE,
            AUDIO,
            EVENTS,
        }

        public static Constants Instance
        {
            get { return ConstantsManager.Instance.constants; }
        }
    }   
}
