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

        public List<AudioFXItem> usedEffectItems;
        public List<AudioFXItem> availableEffectItems;

        #region STARTUP_AND_TERMIATION
        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            //Debug.Log($"OnSingletonAwake");GlobalEvents.playAmbienceEvent
            GlobalEvents.audioAmbienceEvent.AddListener(PlayAmbience);
            GlobalEvents.audioFXEvent.AddListener(PlayFX);
            GlobalEvents.audioMusic.AddListener(PlayMusic);
        }

        public override void OnSingletonDestroy(bool isMainInstance)
        {
            base.OnSingletonDestroy(isMainInstance);
            //Debug.Log($"OnSingletonDestroy {isMainInstance}");
            GlobalEvents.audioAmbienceEvent.RemoveListener(PlayAmbience);
            GlobalEvents.audioFXEvent.RemoveListener(PlayFX);
            GlobalEvents.audioMusic.RemoveListener(PlayMusic);
        }

        public void Terminate()
        {
            ambienceSource.Stop();
            mainMusicSource.Stop();
            for (int i = 0; i < usedEffectItems.Count; i++)
            {
                AudioSource currentSource = usedEffectItems[i].source;
                currentSource.Stop();
                availableEffectItems.Add(usedEffectItems[i]);
            }
        }
        #endregion

        public void PlayAmbience(AUDIO_AMBIENCES id)
        {
            StopAmbience();
            AudioSourceData data = database.ambiences[id];
            ambienceSource.BuildFrom(data);
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
            mainMusicSource.BuildFrom(data);
            mainMusicSource.Play();
        }

        public void StopMusic()
        {
            mainMusicSource.Stop();
        }

        public void PlayFX(AUDIO_FX id, GameObject go)
        {
            if(availableEffectItems.Count > 0)
            {
                AudioSourceData data = database.effects[id];
                AudioFXItem itemToPlay = availableEffectItems[0];
                availableEffectItems.RemoveAt(0);
                usedEffectItems.Add(itemToPlay);

                itemToPlay.Init(data);
                itemToPlay.transform.position = go.transform.position;
                itemToPlay.source.Play();
            }
            else
                Debug.LogWarning($"Not enough FXitems: could not play {id.ToString()} on: {go?.name}");
        }

        public void Update()
        {
            for (int i = 0; i < usedEffectItems.Count; i++)
            {
                if(usedEffectItems[i].NeedsDespawn)
                {
                    AudioFXItem item = usedEffectItems[i];
                    item.source.Stop();
                    item.data = null;
                    usedEffectItems.Remove(item);
                    availableEffectItems.Add(item);
                }
            }
        }
    }   
}
