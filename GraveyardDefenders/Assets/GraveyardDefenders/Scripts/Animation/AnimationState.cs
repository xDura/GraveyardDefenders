using UnityEngine;

namespace XD
{
    public class AnimationState : MonoBehaviour
    {
        public Animator Animator;

        int currentState;
        float normalizedTime;

        int nextState;
        float nextNormalized;

        public void FixedUpdate()
        {
            AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);
            currentState = info.fullPathHash;
            normalizedTime = info.normalizedTime;
            if (normalizedTime > 1.0f)
                normalizedTime %= 1.0f;

            AnimatorStateInfo nextInfo = Animator.GetNextAnimatorStateInfo(0);
            nextState = nextInfo.fullPathHash;
            nextNormalized = nextInfo.fullPathHash;
        }

        public void OnGUI()
        {
            GUILayout.Label($"CurrentState {currentState}  {normalizedTime}");
            GUILayout.Label($"CurrentState {nextState}  {nextNormalized}");
        }
    }   
}
