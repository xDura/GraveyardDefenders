using UnityEngine;
using DG.Tweening;

namespace XD
{
    public class Shaker : MonoBehaviour
    {
        Tweener currentTeen = default;

        public float duration = 1.0f;
        public Vector3 axisAmmount = Vector3.one;
        public int vibratio = 1;
        public float randomness = 1.0f;
        public bool fadeOut = true;

        public void Kill()
        {
            if (currentTeen != null) currentTeen.Kill();
        }

        public void Complete()
        {
            if (currentTeen != null) currentTeen.Complete();
        }

        public void Shake(float a_duration, Vector3 a_axisAmmount, int a_vibratio, float a_randomness, bool a_fadeout)
        {
            currentTeen = transform.DOShakeRotation(a_duration, a_axisAmmount, a_vibratio, a_randomness, a_fadeout);
        }

        public void Shake()
        {
            currentTeen = transform.DOShakeRotation(duration, axisAmmount, vibratio, randomness, fadeOut);
        }
    }   
}
