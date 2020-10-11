using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;

namespace XD.Multiplayer
{
    public class NetManager : Singleton<NetManager>, IConnectionCallbacks, IInRoomCallbacks, ILobbyCallbacks, IMatchmakingCallbacks, IOnEventCallback
    {
        public ClientState NetClientState => PhotonNetwork.NetworkClientState;
        public bool IsConnected => PhotonNetwork.IsConnected;
        public bool IsConnectedAndReady => PhotonNetwork.IsConnectedAndReady;
        public Room CurrentRoom => PhotonNetwork.CurrentRoom;

        public void ConnectToPhoton()
        {
            //TODO: add settings in a scriptableObject easy to modify 
            //and then call the other version of ConnectUsingSettings
            if (!IsConnected) PhotonNetwork.ConnectUsingSettings();
            else DebugLog("ConnectToPhoton and allready connected");
        }

        //public void ConnectToRegion(string regionCode)
        //{
        //    if (IsConnected) Disconnect();
        //    PhotonNetwork.ConnectToRegion(regionCode);
        //}

        public void CreateRoom(string id)
        {
            if (IsConnectedAndReady) PhotonNetwork.CreateRoom(id);
            else Debug.LogError($"CreateRoom {id} and still not connectedandeady");
        }

        public void JoinLobby()
        {
            if (!IsConnected) Debug.LogError($"Trying to joinlobby and state is {NetClientState}");
            PhotonNetwork.JoinLobby();
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        #region SINGLETON_STUFF
        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnSingletonDestroy(bool isMainInstance)
        {
            base.OnSingletonDestroy(isMainInstance);
            if (isMainInstance)
                PhotonNetwork.RemoveCallbackTarget(this);
        }
        #endregion

        #region PUN_IConnectionCallbacks
        public void OnConnected()
        {
            DebugLog("OnConnected");
        }

        public void OnConnectedToMaster()
        {
            DebugLog("OnConnectedToMaster");
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            DebugLog($"OnCustomAuthenticationFailed debugMessage:{debugMessage}");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            DebugLog($"OnCustomAuthenticationResponse");
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            DebugLog($"OnDisconnected: {cause}");
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            DebugLog($"OnRegionListReceived {regionHandler.BestRegion}");
        }
        #endregion

        #region PUN_IInRoomCallbacks
        public void OnMasterClientSwitched(Player newMasterClient)
        {
            DebugLog($"OnMasterClientSwitched {newMasterClient.NickName}");
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            DebugLog($"OnPlayerEnteredRoom {newPlayer.NickName}");
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            DebugLog($"OnPlayerLeftRoom {otherPlayer.NickName}");
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            DebugLog($"OnPlayerPropertiesUpdate {targetPlayer.NickName}");
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            DebugLog($"OnRoomPropertiesUpdate");
        }
        #endregion

        #region PUN_ILobbyCallbacks

        public void OnJoinedLobby()
        {
            DebugLog($"OnJoinedLobby");
        }

        public void OnLeftLobby()
        {
            DebugLog($"OnLeftLobby");
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            DebugLog($"OnRoomListUpdate");
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            DebugLog($"OnLobbyStatisticsUpdate");
        }
        #endregion

        #region PUN_IMatchmakingCallbacks

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            DebugLog($"OnFriendListUpdate");
        }

        public void OnCreatedRoom() //only the host will call this
        {
            DebugLog($"OnCreatedRoom name:{CurrentRoom.Name}, playerCount:{CurrentRoom.PlayerCount}");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            DebugLog($"OnCreateRoomFailed {returnCode} {message}");
        }

        public void OnJoinedRoom() //remember that the host does also call this one
        {
            DebugLog($"OnJoinedRoom {CurrentRoom.Name}, playerCount:{CurrentRoom.PlayerCount}");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            DebugLog($"OnJoinRoomFailed {returnCode} {message}");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            DebugLog($"OnJoinRandomFailed {returnCode} {message}");
        }

        public void OnLeftRoom()
        {
            DebugLog($"OnLeftRoom");
        }
        #endregion

        #region PUN_IOnEventCallback
        public void OnEvent(EventData photonEvent)
        {
            DebugLog($"OnEvent {photonEvent.Code}");
        }
        #endregion

        public void DebugLog(string s)
        {
            Debug.Log(s);
        }
    }
}
