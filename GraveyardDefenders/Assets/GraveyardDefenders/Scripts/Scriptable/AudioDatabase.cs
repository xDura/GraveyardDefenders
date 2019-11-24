using UnityEngine;
using UnityEngine.Audio;
using RotaryHeart.Lib.SerializableDictionary;

namespace XD.Audio
{
    #region AUDIO_DATA
    public enum AUDIO_AMBIENCES
    {
        LEVEL_01_AMBIENCE,
    }

    public enum AUDIO_FX
    {
        DAMAGE_WOOD,
        CHOP_WOOD,
        DAMAGE_STONE,
        MINING_STONE,
        SKELETON_SPAWN,
        SKELETON_DIE,
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

        public void PopulateFrom(AudioSource sourceSource)
        {
            time = sourceSource.time;
            outputAudioMixerGroup = sourceSource.outputAudioMixerGroup;
            loop = sourceSource.loop;
            ignoreListenerVolume = sourceSource.ignoreListenerVolume;
            playOnAwake = sourceSource.playOnAwake;
            ignoreListenerPause = sourceSource.ignoreListenerPause;
            velocityUpdateMode = sourceSource.velocityUpdateMode;
            panStereo = sourceSource.panStereo;
            spatialBlend = sourceSource.spatialBlend;
            spatialize = sourceSource.spatialize;
            spatializePostEffects = sourceSource.spatializePostEffects;
            reverbZoneMix = sourceSource.reverbZoneMix;
            bypassEffects = sourceSource.bypassEffects;
            bypassListenerEffects = sourceSource.bypassListenerEffects;
            bypassReverbZones = sourceSource.bypassReverbZones;
            dopplerLevel = sourceSource.dopplerLevel;
            spread = sourceSource.spread;
            priority = sourceSource.priority;
            mute = sourceSource.mute;
            minDistance = sourceSource.minDistance;
            maxDistance = sourceSource.maxDistance;
            rolloffMode = sourceSource.rolloffMode;
            clip = sourceSource.clip;
            timeSamples = sourceSource.timeSamples;
            pitch = sourceSource.pitch;
            volume = sourceSource.volume;
        }

        public void Populate(AudioSource target)
        {
            target.time = time;
            target.outputAudioMixerGroup = outputAudioMixerGroup;
            target.loop = loop;
            target.ignoreListenerVolume = ignoreListenerVolume;
            target.playOnAwake = playOnAwake;
            target.ignoreListenerPause = ignoreListenerPause;
            target.velocityUpdateMode = velocityUpdateMode;
            target.panStereo = panStereo;
            target.spatialBlend = spatialBlend;
            target.spatialize = spatialize;
            target.spatializePostEffects = spatializePostEffects;
            target.reverbZoneMix = reverbZoneMix;
            target.bypassEffects = bypassEffects;
            target.bypassListenerEffects = bypassListenerEffects;
            target.bypassReverbZones = bypassReverbZones;
            target.dopplerLevel = dopplerLevel;
            target.spread = spread;
            target.priority = priority;
            target.mute = mute;
            target.minDistance = minDistance;
            target.maxDistance = maxDistance;
            target.rolloffMode = rolloffMode;
            target.clip = clip;
            target.timeSamples = timeSamples;
            target.pitch = pitch;
            target.volume = volume;
        }
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
