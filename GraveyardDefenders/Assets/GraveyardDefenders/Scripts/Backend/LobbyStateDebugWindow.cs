using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using XD.Net;

namespace XD
{
    public class LobbyStateDebugWindow : MonoBehaviour
    {
        private void OnGUI()
        {
            NetManager nm = NetManager.Instance;
            GUILayout.Toggle(nm.InLobby, "InLobby");


            if (nm.InLobby)
            {
                GUILayout.Label($"LobbyName {nm.CurrentLobby.Name}");
                GUILayout.Label($"LobbyType {nm.CurrentLobbyType}");
                for (int i = 0; i < nm.room_list.Count; i++)
                {
                    GUILayout.Label($"room index: {i},  name: {nm.room_list[i].Name}");
                }
            }
        }
    }   
}
