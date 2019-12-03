using UnityEngine;

namespace XD.Audio
{
    public class SceneAudio : MonoBehaviour
    {
        public AUDIO_MUSICS music;
        public AUDIO_AMBIENCES dayAmbience;
        public AUDIO_AMBIENCES nightAmbience;

        public void Awake()
        {
            AudioManager.Instance.PlayMusic(music);
        }
    }   
}
