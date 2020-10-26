using UnityEngine;

namespace XD
{
    public class AnimationSceneBaker : MonoBehaviour
    {
        public Animator animator;

        [Button]
        void UpdateAnimator(string stateName, int layer, float normalizedTime)
        {
            animator.Play(stateName, 0, normalizedTime);
            animator.Update(0.1f);
        }
    }   
}
