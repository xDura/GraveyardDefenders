﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using WebSocketSharp;

namespace XD.Net
{
    public class NetManager : Singleton<NetManager>, IConnectionCallbacks, IInRoomCallbacks, ILobbyCallbacks, IMatchmakingCallbacks, IOnEventCallback
    {
        public ClientState NetClientState => PhotonNetwork.NetworkClientState;
        public bool IsConnected => PhotonNetwork.IsConnected;
        public bool IsConnectedAndReady => PhotonNetwork.IsConnectedAndReady;
        public bool IsConnectedAndReadyTrue => NetClientState == ClientState.ConnectedToMasterServer;
        public string CurrentRegionCode => PhotonNetwork.CloudRegion;
        public Room CurrentRoom => PhotonNetwork.CurrentRoom;
        public bool InRoom => IsConnectedAndReady && CurrentRoom != null;
        public bool InLobby => PhotonNetwork.InLobby;
        public TypedLobby CurrentLobby => PhotonNetwork.CurrentLobby;
        public LobbyType CurrentLobbyType => CurrentLobby.Type;
        public bool IsMulti => InRoom;
        public bool IsMaster => InRoom && PhotonNetwork.IsMasterClient;
        public bool IsClient => InRoom && !PhotonNetwork.IsMasterClient;
        public const byte instantiationEventCode = 1;

        public List<RoomInfo> room_list = new List<RoomInfo>();

        public bool IsInRegion(string regionCode) { return IsConnected && CurrentRegionCode == regionCode; }

        public void ConnectToPhoton()
        {
            //TODO: add settings in a scriptableObject easy to modify 
            //and then call the other version of ConnectUsingSettings
            if (!IsConnected) PhotonNetwork.ConnectUsingSettings();
            else DebugLog("ConnectToPhoton and allready connected");
        }

        public void ConnectToRegion(string regionCode)
        {
            if (IsInRegion(regionCode)) return;

            if (IsConnected) Disconnect();
            PhotonNetwork.ConnectToRegion(regionCode);
        }

        public void JoinRoom(string roomId)
        {
            PhotonNetwork.JoinRoom(roomId);
        }

        public void LeaveRoom()
        {
            if (InRoom) PhotonNetwork.LeaveRoom();
        }

        public void CreateRoom(string roomId = "")
        {
            if (roomId.IsNullOrEmpty()) roomId = GenRoomId();
            if (IsConnectedAndReady) PhotonNetwork.CreateRoom(roomId);
            else Debug.LogError($"CreateRoom {roomId} and still not connectedandeady");
        }

        string GenRoomId()
        {
            System.Guid guid = System.Guid.NewGuid();
            //choke the guid into parts
            string guid_str = guid.ToString();
            string yourPlatformName = "MyName";
            return $"{yourPlatformName}_{guid_str.Split('-')[0]}";
        }

        public void JoinLobby()
        {
            if (!IsConnected) Debug.LogError($"Trying to joinlobby and state is {NetClientState}");
            if (InRoom) { PhotonNetwork.LeaveRoom(); }
            PhotonNetwork.JoinLobby();
        }

        public void LeaveLobby()
        {
            if (InLobby) PhotonNetwork.LeaveLobby();
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void AttachPhotonView(Transform t, PhotonView pView, PREFAB_ID prefabId, bool sceneEntity)
        {
            DebugLog($"Attaching {t.name} {prefabId} {sceneEntity}");
            bool isAllocated;
            if (sceneEntity) isAllocated = PhotonNetwork.AllocateSceneViewID(pView);
            else isAllocated = PhotonNetwork.AllocateViewID(pView);

            if (isAllocated)
            {
                object[] data = new object[]
                {
                    t.position, t.rotation, pView.ViewID, (int)prefabId,
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
            DebugLog($"Received attachment {photonEvent.CustomData}");
            object[] data = (object[])photonEvent.CustomData;
            Vector3 pos = (Vector3)data[0];
            Quaternion rot = (Quaternion)data[1];
            int viewID = (int)data[2];
            int prefabID = (int)data[3];

            GameObject prefab = Constants.Instance.GetPrefab(prefabID);
            GameObject instantiated = Instantiate(prefab, pos, rot);
            PhotonView photonView = instantiated.GetComponent<PhotonView>();
            NetworkEntity entity = instantiated.GetComponent<NetworkEntity>();
            photonView.ViewID = viewID;
            Player sender = CurrentRoom.GetPlayer(photonEvent.Sender);
            entity.OnAtachedNonOwner(sender);
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
            room_list.Clear();
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
            room_list.AddRange(roomList);
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
