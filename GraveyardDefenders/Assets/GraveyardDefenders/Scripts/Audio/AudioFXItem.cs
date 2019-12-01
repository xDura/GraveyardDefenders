using UnityEngine;

namespace XD.Audio
{
    public class AudioFXItem : MonoBehaviour
    {
        public AudioSource source;
        public AudioSourceData data;
        //TODO: handle pauses here
        public bool NeedsDespawn { get { return data.despawnOnEnd && !source.isPlaying; } }

        public void Init(AudioSourceData a_data)
        {
            data = a_data;
            source.BuildFrom(data);
        }
    }   
}
