#if UNITY_EDITOR
using UnityEngine;

namespace XD
{
    public class AudioSourcePlayerEditor : MonoBehaviour
    {
        public AudioSource source;

        [ContextMenu("PlayAudioSource")]
        void PlayAudioSource()
        {
            source.Play();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) source.Play();
        }
    }   
}
#endif
