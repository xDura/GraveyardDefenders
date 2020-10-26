using UnityEngine;
using XD.Events;
using Photon.Realtime;
using System.Collections.Generic;
using Photon.Pun;

namespace XD
{
    public class GlobalEvents
    {
        public static Evnt<AUDIO_AMBIENCES> audioAmbienceEvent = new Evnt<AUDIO_AMBIENCES>();
        public static Evnt<AUDIO_FX, GameObject> audioFXEvent = new Evnt<AUDIO_FX, GameObject>();
        public static Evnt<AUDIO_MUSICS> audioMusic = new Evnt<AUDIO_MUSICS>();

        public static Evnt newDayStarted = new Evnt();
        public static Evnt newNightStared = new Evnt();

        public static Evnt<Upgradeable> upgradeableUpgraded = new Evnt<Upgradeable>();
        public static Evnt<Skeleton> skeletonDespawned = new Evnt<Skeleton>();
    }

    public class UIEvents
    {
        public static Evnt<PlayerCharacter, Upgradeable> showUpgradeableEvnt = new Evnt<PlayerCharacter, Upgradeable>();
        public static Evnt<PlayerCharacter, Upgradeable> hideUpgradeableEvnt = new Evnt<PlayerCharacter, Upgradeable>();
    }

    public class MainMenuEvents
    {
        public static Evnt couchButtonPressed = new Evnt();
        public static Evnt onlineButtonPressed = new Evnt();
        public static Evnt onlineBackButtonPressed = new Evnt();
        public static Evnt createRoomPressed = new Evnt();
        public static Evnt onlineBrowsePressed = new Evnt();
        public static Evnt settingsPressed = new Evnt();
        public static Evnt settingsBackPressed = new Evnt();
        public static Evnt onlineBrowseBackPressed = new Evnt();
        //public static Evnt lobbyBackPressed = new Evnt();
        public static Evnt enteredLobby = new Evnt();
        public static Evnt leftLobby = new Evnt();
        public static Evnt allPlayersReadyLobby = new Evnt();
    }

    namespace Net
    {
        public class NetEvents
        {
            public static Evnt OnConnected = new Evnt();
            public static Evnt OnConnectedToMaster = new Evnt();
            public static Evnt<string> OnCustomAuthenticationFailed = new Evnt<string>();
            public static Evnt<Dictionary<string, object>> OnCustomAuthenticationResponse = new Evnt<Dictionary<string, object>>();
            public static Evnt<DisconnectCause> OnDisconnected = new Evnt<DisconnectCause>();
            public static Evnt<RegionHandler> OnRegionListReceived = new Evnt<RegionHandler>();
            public static Evnt<Player> OnMasterClientSwitched = new Evnt<Player>();
            public static Evnt<Player> OnPlayerEnteredRoom = new Evnt<Player>();
            public static Evnt<Player> OnPlayerLeftRoom = new Evnt<Player>();
            public static Evnt<Player, ExitGames.Client.Photon.Hashtable> OnPlayerPropertiesUpdate = new Evnt<Player, ExitGames.Client.Photon.Hashtable>();
            public static Evnt<ExitGames.Client.Photon.Hashtable> OnRoomPropertiesUpdate = new Evnt<ExitGames.Client.Photon.Hashtable>();
            public static Evnt OnJoinedLobby = new Evnt();
            public static Evnt OnLeftLobby = new Evnt();
            public static Evnt<List<RoomInfo>> OnRoomListUpdate = new Evnt<List<RoomInfo>>();
            public static Evnt<List<TypedLobbyInfo>> OnLobbyStatisticsUpdate = new Evnt<List<TypedLobbyInfo>>();
            public static Evnt<List<FriendInfo>> OnFriendListUpdate = new Evnt<List<FriendInfo>>();
            public static Evnt OnCreatedRoom = new Evnt(); //only the host will call this
            public static Evnt<short, string> OnCreateRoomFailed = new Evnt<short, string>();
            public static Evnt OnJoinedRoom = new Evnt(); //remember that the host does also call this one
            public static Evnt<short, string> OnJoinRoomFailed = new Evnt< short, string>();
            public static Evnt<short, string> OnJoinRandomFailed = new Evnt<short, string>();
            public static Evnt OnLeftRoom = new Evnt();
            public static Evnt<ExitGames.Client.Photon.EventData> OnEvent = new Evnt<ExitGames.Client.Photon.EventData>();
        }
    }

    public static class PlayerEvents
    {
        public static Evnt<PlayerCharacter> playerAddedEvnt = new Evnt<PlayerCharacter>();
    }
}
