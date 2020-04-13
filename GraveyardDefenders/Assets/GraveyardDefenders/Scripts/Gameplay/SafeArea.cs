using UnityEngine;

namespace XD
{
    public class SafeArea : MonoBehaviour
    {
        public LayerMask layer;

        public void OnTriggerEnter(Collider other)
        {
            if ((layer.value & (1 << other.gameObject.layer)) == 0) return;

            other.GetComponent<PlayerCharacter>().SetSafeArea(true);
        }

        public void OnTriggerExit(Collider other)
        {
            if ((layer.value & (1 << other.gameObject.layer)) == 0) return;

            other.GetComponent<PlayerCharacter>().SetSafeArea(false);
        }
    }   
}
