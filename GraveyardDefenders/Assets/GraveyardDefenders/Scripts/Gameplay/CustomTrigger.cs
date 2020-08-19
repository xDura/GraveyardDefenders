using UnityEngine;
using XD.Events;

namespace XD
{
    public class CustomTrigger : MonoBehaviour
    {
        public Evnt<Collider> onTriggerEnter = new Evnt<Collider>();
        public Evnt<Collider> onTriggerExit = new Evnt<Collider>();
        public Evnt<Collider> onTriggerStay = new Evnt<Collider>();

        private void OnTriggerEnter(Collider other) { onTriggerEnter.Invoke(other); }
        private void OnTriggerExit(Collider other) { onTriggerExit.Invoke(other); }
        private void OnTriggerStay(Collider other) { onTriggerStay.Invoke(other); }
    }   
}
