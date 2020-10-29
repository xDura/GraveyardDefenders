using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace XD
{
    [CreateAssetMenu(menuName = "XD/Skeleton/SkeletonBoneNames")]
    public class SkeletonBoneNames : ScriptableObject
    {
        [System.Serializable] public class SkeletonBoneNamesDictionary : SerializableDictionaryBase<BONE_ID, string> { };
        public SkeletonBoneNamesDictionary boneNames = new SkeletonBoneNamesDictionary();
    }   
}
