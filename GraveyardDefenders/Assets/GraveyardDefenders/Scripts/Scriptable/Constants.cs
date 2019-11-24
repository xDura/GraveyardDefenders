using UnityEngine;

namespace XD
{
    [CreateAssetMenu(menuName = "XD/Constants")]
    public class Constants : ScriptableObject
    {
        public static Constants Instance
        {
            get { return ConstantsManager.Instance.constants; }
        }
    }   
}
