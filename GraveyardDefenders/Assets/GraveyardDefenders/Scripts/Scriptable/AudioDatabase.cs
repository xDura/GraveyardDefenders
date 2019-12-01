using UnityEngine;
using UnityEngine.Audio;
using RotaryHeart.Lib.SerializableDictionary;

namespace XD.Audio
{
    #region AUDIO_DATA
    public enum AUDIO_AMBIENCES
    {
        LEVEL_01_AMBIENCE_DAY,
        LEVEL_01_AMBIENCE_NIGHT,
    }

    public enum AUDIO_FX
    {
        DAMAGE_WOOD,
        CHOP_WOOD,
        DAMAGE_STONE,
        MINING_STONE,
        SKELETON_SPAWN,
        SKELETON_DIE,
        REPAIR_WOOD,
    }

    public enum AUDIO_MUSICS
    {
        LEVEL_01_MUSIC,
        MAINMENU_MUSIC,
    }

    [System.Serializable]
    public class AudioSourceData
    {
        public float time;
        public AudioMixerGroup outputAudioMixerGroup;
        public bool loop;
        public bool ignoreListenerVolume;
        public bool playOnAwake;
        public bool ignoreListenerPause;
        public AudioVelocityUpdateMode velocityUpdateMode;
        public float panStereo;
        public float spatialBlend;
        public bool spatialize;
        public bool spatializePostEffects;
        public float reverbZoneMix;
        public bool bypassEffects;
        public bool bypassListenerEffects;
        public bool bypassReverbZones;
        public float dopplerLevel;
        public float spread;
        public int priority;
        public bool mute;
        public float minDistance;
        public float maxDistance;
        public AudioRolloffMode rolloffMode;
        public AudioClip clip;
        public int timeSamples;
        public float pitch;
        public float volume;
        public bool useRandomPitch = false;
        [Range(0.0f, 10.0f)] public float maxRandomPitch = 1.0f;
        [Range(0.0f, 10.0f)] public float minRandomPitch = 1.0f;
        public bool despawnOnEnd = false;
    }
    #endregion

    #region DICTIONARIES
    [System.Serializable] public class AmbiencesDictionary : SerializableDictionaryBase<AUDIO_AMBIENCES, AudioSourceData> { }
    [System.Serializable] public class EffectsDictionary : SerializableDictionaryBase<AUDIO_FX, AudioSourceData> { }
    [System.Serializable] public class MusicsDictionary : SerializableDictionaryBase<AUDIO_MUSICS, AudioSourceData> { }
    #endregion

    [CreateAssetMenu(menuName = "XD/AudioDatabase")]
    public class AudioDatabase : ScriptableObject
    {
        public AmbiencesDictionary ambiences;
        public EffectsDictionary effects;
        public MusicsDictionary musics;
    }   
}
