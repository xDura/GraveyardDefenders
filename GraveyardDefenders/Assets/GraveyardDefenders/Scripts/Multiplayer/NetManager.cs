using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;

namespace XD.Net
{
    public class NetManager : Singleton<NetManager>, IConnectionCallbacks, IInRoomCallbacks, ILobbyCallbacks, IMatchmakingCallbacks, IOnEventCallback
    {
        public ClientState NetClientState => PhotonNetwork.NetworkClientState;
        public bool IsConnected => PhotonNetwork.IsConnected;
        public bool IsConnectedAndReady => PhotonNetwork.IsConnectedAndReady;
        public Room CurrentRoom => PhotonNetwork.CurrentRoom;
        public bool InRoom => IsConnectedAndReady && CurrentRoom != null;
        public bool IsMulti => InRoom;
        public bool IsMaster => InRoom && PhotonNetwork.IsMasterClient;
        public bool IsClient => InRoom && !PhotonNetwork.IsMasterClient;
        public const byte instantiationEventCode = 1;  

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

        public void JoinRoom(string roomId)
        {
            PhotonNetwork.JoinRoom(roomId);
        }

        public void CreateRoom(string roomId)
        {
            if (IsConnectedAndReady) PhotonNetwork.CreateRoom(roomId);
            else Debug.LogError($"CreateRoom {roomId} and still not connectedandeady");
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

        public void AttachPhotonView(Transform t, PhotonView pView, int prefabId)
        {
            if (PhotonNetwork.AllocateViewID(pView))
            {
                object[] data = new object[]
                {
                    t.position, t.rotation, pView.ViewID, prefabId,
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };
                SendOptions sendOptions = new SendOptions { Reliability = true };

                PhotonNetwork.RaiseEvent(instantiationEventCode, data, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewId.");
            }
        }

        public void OnReceivedAttachedPhotonView(EventData photonEvent)
        {
            Debug.Log($"Received instantiation {photonEvent.CustomData}");
            if (photonEvent.Parameters != null)
            {
                foreach (KeyValuePair<byte, object> pair in photonEvent.Parameters)
                    Debug.Log($"instantiation data {pair.Key} {pair.Value}");
            }
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
            NetEvents.OnConnected.Invoke();
            DebugLog("OnConnected");
        }

        public void OnConnectedToMaster()
        {
            NetEvents.OnConnectedToMaster.Invoke();
            DebugLog("OnConnectedToMaster");
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            NetEvents.OnCustomAuthenticationFailed.Invoke(debugMessage);
            DebugLog($"OnCustomAuthenticationFailed debugMessage:{debugMessage}");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            NetEvents.OnCustomAuthenticationResponse.Invoke(data);
            DebugLog($"OnCustomAuthenticationResponse");
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            NetEvents.OnDisconnected.Invoke(cause);
            DebugLog($"OnDisconnected: {cause}");
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            NetEvents.OnRegionListReceived.Invoke(regionHandler);
            DebugLog($"OnRegionListReceived {regionHandler.BestRegion}");
        }
        #endregion

        #region PUN_IInRoomCallbacks
        public void OnMasterClientSwitched(Player newMasterClient)
        {
            NetEvents.OnMasterClientSwitched.Invoke(newMasterClient);
            DebugLog($"OnMasterClientSwitched {newMasterClient.NickName}");
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            NetEvents.OnPlayerEnteredRoom.Invoke(newPlayer);
            DebugLog($"OnPlayerEnteredRoom {newPlayer.NickName}");
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            NetEvents.OnPlayerLeftRoom.Invoke(otherPlayer);
            DebugLog($"OnPlayerLeftRoom {otherPlayer.NickName}");
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            NetEvents.OnPlayerPropertiesUpdate.Invoke(targetPlayer, changedProps);
            DebugLog($"OnPlayerPropertiesUpdate {targetPlayer.NickName}");
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            NetEvents.OnRoomPropertiesUpdate.Invoke(propertiesThatChanged);
            DebugLog($"OnRoomPropertiesUpdate");
        }
        #endregion

        #region PUN_ILobbyCallbacks

        public void OnJoinedLobby()
        {
            NetEvents.OnJoinedLobby.Invoke();
            DebugLog($"OnJoinedLobby");
        }

        public void OnLeftLobby()
        {
            NetEvents.OnLeftLobby.Invoke();
            DebugLog($"OnLeftLobby");
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            NetEvents.OnRoomListUpdate.Invoke(roomList);
            DebugLog($"OnRoomListUpdate");
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            NetEvents.OnLobbyStatisticsUpdate.Invoke(lobbyStatistics);
            DebugLog($"OnLobbyStatisticsUpdate");
        }
        #endregion

        #region PUN_IMatchmakingCallbacks

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            NetEvents.OnFriendListUpdate.Invoke(friendList);
            DebugLog($"OnFriendListUpdate");
        }

        public void OnCreatedRoom() //only the host will call this
        {
            NetEvents.OnCreatedRoom.Invoke();
            DebugLog($"OnCreatedRoom name:{CurrentRoom.Name}, playerCount:{CurrentRoom.PlayerCount}");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            NetEvents.OnCreateRoomFailed.Invoke(returnCode, message);
            DebugLog($"OnCreateRoomFailed {returnCode} {message}");
        }

        public void OnJoinedRoom() //remember that the host does also call this one
        {
            NetEvents.OnJoinedRoom.Invoke();
            DebugLog($"OnJoinedRoom {CurrentRoom.Name}, playerCount:{CurrentRoom.PlayerCount}");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            NetEvents.OnJoinRoomFailed.Invoke(returnCode, message);
            DebugLog($"OnJoinRoomFailed {returnCode} {message}");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            NetEvents.OnJoinRandomFailed.Invoke(returnCode, message);
            DebugLog($"OnJoinRandomFailed {returnCode} {message}");
        }

        public void OnLeftRoom()
        {
            NetEvents.OnLeftRoom.Invoke();
            DebugLog($"OnLeftRoom");
        }
        #endregion

        #region PUN_IOnEventCallback
        public void OnEvent(EventData photonEvent)
        {
            NetEvents.OnEvent.Invoke(photonEvent);
            if (photonEvent.Code == instantiationEventCode) OnReceivedAttachedPhotonView(photonEvent);
            DebugLog($"OnEvent {photonEvent.Code}");
        }
        #endregion

        public void DebugLog(string s)
        {
            Debug.Log(s);
        }
    }
}
