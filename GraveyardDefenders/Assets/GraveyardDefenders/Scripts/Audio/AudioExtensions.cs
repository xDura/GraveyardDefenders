using UnityEngine;

namespace XD.Audio
{
    public static class AudioExtensions
    {
        public static void BuildFrom(this AudioSource target, AudioSourceData data)
        {
            target.time = data.time;
            target.outputAudioMixerGroup = data.outputAudioMixerGroup;
            target.loop = data.loop;
            target.ignoreListenerVolume = data.ignoreListenerVolume;
            target.playOnAwake = data.playOnAwake;
            target.ignoreListenerPause = data.ignoreListenerPause;
            target.velocityUpdateMode = data.velocityUpdateMode;
            target.panStereo = data.panStereo;
            target.spatialBlend = data.spatialBlend;
            target.spatialize = data.spatialize;
            target.spatializePostEffects = data.spatializePostEffects;
            target.reverbZoneMix = data.reverbZoneMix;
            target.bypassEffects = data.bypassEffects;
            target.bypassListenerEffects = data.bypassListenerEffects;
            target.bypassReverbZones = data.bypassReverbZones;
            target.dopplerLevel = data.dopplerLevel;
            target.spread = data.spread;
            target.priority = data.priority;
            target.mute = data.mute;
            target.minDistance = data.minDistance;
            target.maxDistance = data.maxDistance;
            target.rolloffMode = data.rolloffMode;
            target.clip = data.clip;
            target.timeSamples = data.timeSamples;
            target.pitch = data.pitch;
            target.volume = data.volume;

            if (data.useRandomPitch) { target.pitch = Random.Range(data.minRandomPitch, data.maxRandomPitch); }
        }

        public static void ToData(this AudioSourceData data, AudioSource sourceSource)
        {
            data.time = sourceSource.time;
            data.outputAudioMixerGroup = sourceSource.outputAudioMixerGroup;
            data.loop = sourceSource.loop;
            data.ignoreListenerVolume = sourceSource.ignoreListenerVolume;
            data.playOnAwake = sourceSource.playOnAwake;
            data.ignoreListenerPause = sourceSource.ignoreListenerPause;
            data.velocityUpdateMode = sourceSource.velocityUpdateMode;
            data.panStereo = sourceSource.panStereo;
            data.spatialBlend = sourceSource.spatialBlend;
            data.spatialize = sourceSource.spatialize;
            data.spatializePostEffects = sourceSource.spatializePostEffects;
            data.reverbZoneMix = sourceSource.reverbZoneMix;
            data.bypassEffects = sourceSource.bypassEffects;
            data.bypassListenerEffects = sourceSource.bypassListenerEffects;
            data.bypassReverbZones = sourceSource.bypassReverbZones;
            data.dopplerLevel = sourceSource.dopplerLevel;
            data.spread = sourceSource.spread;
            data.priority = sourceSource.priority;
            data.mute = sourceSource.mute;
            data.minDistance = sourceSource.minDistance;
            data.maxDistance = sourceSource.maxDistance;
            data.rolloffMode = sourceSource.rolloffMode;
            data.clip = sourceSource.clip;
            data.timeSamples = sourceSource.timeSamples;
            data.pitch = sourceSource.pitch;
            data.volume = sourceSource.volume;
        }
    }   
}
