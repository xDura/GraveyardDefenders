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
            GlobalEvents.audioMusic.Invoke(music);
        }
    }   
}
