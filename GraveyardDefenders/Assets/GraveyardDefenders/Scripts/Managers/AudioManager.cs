using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace XD.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioDatabase database;

        public AudioSource ambienceSource;
        public AudioSource mainMusicSource;

        //TODO: make a pool implementation for this
        public List<AudioSource> usedEffectSources;
        public List<AudioSource> availableEffectSources;

        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log($"OnSingletonAwake");
        }

        public override void OnSingletonDestroy(bool isMainInstance)
        {
            base.OnSingletonDestroy(isMainInstance);
            Debug.Log($"OnSingletonDestroy {isMainInstance}");
        }

        public void PlayAmbience(AUDIO_AMBIENCES id)
        {
            StopAmbience();
            AudioSourceData data = database.ambiences[id];
            data.Populate(ambienceSource);
            ambienceSource.Play();
        }

        public void StopAmbience()
        {
            ambienceSource.Stop();
        }

        public void PlayMusic(AUDIO_MUSICS id)
        {
            StopMusic();
            AudioSourceData data = database.musics[id];
            data.Populate(mainMusicSource);
            mainMusicSource.Play();
        }

        public void StopMusic()
        {
            mainMusicSource.Stop();
        }

        public void PlayFX(AUDIO_FX id, GameObject go)
        {

        }

        public void Terminate()
        {
            ambienceSource.Stop();
            mainMusicSource.Stop();
            for (int i = 0; i < usedEffectSources.Count; i++)
            {
                AudioSource currentSource = usedEffectSources[i];
                currentSource.Stop();
                availableEffectSources.Add(currentSource);
            }
        }
    }   
}
