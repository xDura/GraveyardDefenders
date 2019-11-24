using UnityEngine;

namespace XD.Audio
{
    public class SceneAudio : MonoBehaviour
    {
        public AUDIO_MUSICS music;
        public AUDIO_AMBIENCES ambience;

        public void Awake()
        {
            AudioManager.Instance.PlayMusic(music);
            AudioManager.Instance.PlayAmbience(ambience);
        }
    }   
}
