using UnityEngine;
using DG.Tweening;

namespace XD
{
    public class Shaker : MonoBehaviour
    {
        Tweener currentTeen = default;

        public void Kill()
        {
            if (currentTeen != null) currentTeen.Kill();
        }

        public void Complete()
        {
            if (currentTeen != null) currentTeen.Complete();
        }

        public void Shake(float duration, Vector3 axisAmmount, int vibratio, float randomness, bool fadeout)
        {
            currentTeen = transform.DOShakeRotation(duration, axisAmmount, vibratio, randomness, fadeout);
        }
    }   
}
