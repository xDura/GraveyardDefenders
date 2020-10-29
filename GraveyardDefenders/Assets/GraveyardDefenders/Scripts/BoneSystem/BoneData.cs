using UnityEngine;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using System;

namespace XD
{
    public enum BONE_ID
    {
        ROOT,
        HIP,
        SPINE,
        SPINE2,
        CHEST,
        SHOULDER_R,
        UPPER_ARM_R,
        LOWER_ARM_R,
        HAND_R,
        SHOULDER_L,
        UPPER_ARM_L,
        LOWER_ARM_L,
        HAND_L,
        NECK,
        HEAD,
        EYES,
        UPPER_LEG_R,
        KNEE_R,
        FOOT_R,
        UPPER_LEG_L,
        KNEE_L,
        FOOT_L,
    }

    public class BoneData : MonoBehaviour
    {
        [Serializable] public class BoneDictionary : SerializableDictionaryBase<BONE_ID, Transform> { };

        public BoneDictionary bones = new BoneDictionary();
        public SkeletonBoneNames boneNames;

        public Transform GetBone(BONE_ID id)
        {
            if (bones.TryGetValue(id, out Transform bone)) return bone;
            else return null;
        }

        [ContextMenu("CollectBones")]
        public void CollectBones()
        {
            bones.Clear();
            foreach (KeyValuePair<BONE_ID, string> pair in boneNames.boneNames)
            {
                Transform t = transform.FindChildRecursive(pair.Value);
                if (t) bones.Add(pair.Key, t);
            }
        }
    }   
}
