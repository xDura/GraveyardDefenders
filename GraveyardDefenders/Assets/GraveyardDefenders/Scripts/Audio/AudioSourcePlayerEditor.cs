#if UNITY_EDITOR
using UnityEngine;

namespace XD.Audio
{
    public class AudioSourcePlayerEditor : MonoBehaviour
    {
        public AUDIO_FX fxToPlay;
        public AudioSource source;

        [Button]
        void PlayAudioSource()
        {
            source.Play();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) AudioManager.Instance.PlayFX(fxToPlay, gameObject);
        }
    }   
}
#endif
