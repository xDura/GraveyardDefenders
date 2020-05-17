using UnityEngine;
using XD.Events;
using XD.Audio;

namespace XD
{
    public class GlobalEvents
    {
        public static Evnt<AUDIO_AMBIENCES> audioAmbienceEvent = new Evnt<AUDIO_AMBIENCES>();
        public static Evnt<AUDIO_FX, GameObject> audioFXEvent = new Evnt<AUDIO_FX, GameObject>();
        public static Evnt<AUDIO_MUSICS> audioMusic = new Evnt<AUDIO_MUSICS>();

        public static Evnt newDayStarted = new Evnt();
        public static Evnt newNightStared = new Evnt();
    }

    public class NetEvents
    {
        public static Evnt boltStartDone = new Evnt();
    }

    public static class PlayerEvents
    {
        public static Evnt<PlayerCharacter> playerAddedEvnt = new Evnt<PlayerCharacter>();
    }

    //public class EventsManager : Singleton<EventsManager> { }   
}
