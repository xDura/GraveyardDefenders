using Photon.Pun;
using UnityEngine;
using XD.Net;
using Photon.Realtime;

namespace XD
{
    public class NetStateDebugWindow : MonoBehaviour
    {
        private void OnGUI()
        {
            NetManager nm = NetManager.Instance;
            GUILayout.Toggle(nm.IsConnected, "IsConnected");
            GUILayout.Toggle(nm.IsConnectedAndReady, "IsConnectedAndReady");
            GUILayout.Label($"NetClientState {nm.NetClientState}");
            if (nm.InRoom)
            {
                GUILayout.Toggle(nm.IsMaster, "IsMaster");
                GUILayout.Toggle(nm.IsClient, "isClient");
                GUILayout.Label($"LocalName {PhotonNetwork.NickName}");
                GUILayout.Label($"RoomName {nm.CurrentRoom.Name}");

                GUILayout.Label($"PlayerCount {nm.CurrentRoom.PlayerCount}");
                GUILayout.Label($"MaxPlayers {nm.CurrentRoom.MaxPlayers}");
                //for (int i = 0; i < nm.CurrentRoom.PlayerCount; i++)
                //{
                //    Player p = nm.CurrentRoom.GetPlayer(i);
                //    GUILayout.Label($"Player: id: {p.UserId}, name: {p.NickName}");
                //}
            }
        }
    }   
}
