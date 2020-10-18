using UnityEngine;
using CommandTerminal;
using UnityEditor;
using UnityEngine.InputSystem;
using XD.Net;

namespace XD
{
    public static class ConsoleCommands
    {
        [RegisterCommand("TimeScale", Help = "Set time scale", MaxArgCount = 1, MinArgCount = 1)]
        static void SetTimeScale(CommandArg[] args)
        {
            Time.timeScale = args[0].Float;
        }

        [RegisterCommand("net.connect", Help = "Connect to photon", MaxArgCount = 1, MinArgCount = 0)]
        static void Connect(CommandArg[] args)
        {
            NetManager.Instance.ConnectToPhoton();
        }

        [RegisterCommand("net.joinlobby", Help = "join the lobby to search for rooms", MaxArgCount = 1, MinArgCount = 0)]
        static void JoinLobby(CommandArg[] args)
        {
            NetManager.Instance.JoinLobby();
        }

        [RegisterCommand("net.disconnect", Help = "disconnect from photon", MaxArgCount = 1, MinArgCount = 0)]
        static void Disconnect(CommandArg[] args)
        {
            NetManager.Instance.Disconnect();
        }

        [RegisterCommand("net.createroom", Help = "create a photon room", MaxArgCount = 1, MinArgCount = 1)]
        static void CreateRoom(CommandArg[] args)
        {
            NetManager.Instance.CreateRoom(args[0].String);
        }

        [RegisterCommand("net.joinroom", Help = "join a photon room", MaxArgCount = 1, MinArgCount = 1)]
        static void JoinRoom(CommandArg[] args)
        {
            NetManager.Instance.JoinRoom(args[0].String);
        }

        [RegisterCommand("net.region", Help = "join a photon region", MaxArgCount = 1, MinArgCount = 1)]
        static void ConnectToRegion(CommandArg[] args)
        {
            NetManager.Instance.ConnectToRegion(args[0].String);
        }

        [RegisterCommand("net.state", Help = "net state window", MaxArgCount = 0, MinArgCount = 0)]
        static void NetStateWindow(CommandArg[] args)
        {
            NetStateDebugWindow ns = GameObject.FindObjectOfType<NetStateDebugWindow>();
            if(ns == null)
            {
                GameObject go = new GameObject("NetStateDebugWindow");
                go.AddComponent<NetStateDebugWindow>();
            }
            else
            {
                GameObject.Destroy(ns.gameObject);
            }
        }

        [RegisterCommand("net.lobbystate", Help = "lobby state window", MaxArgCount = 0, MinArgCount = 0)]
        static void LobbyStateWindow(CommandArg[] args)
        {
            LobbyStateDebugWindow ns = GameObject.FindObjectOfType<LobbyStateDebugWindow>();
            if (ns == null)
            {
                GameObject go = new GameObject("LobbyStateWidnow");
                go.AddComponent<LobbyStateDebugWindow>();
            }
            else
            {
                GameObject.Destroy(ns.gameObject);
            }
        }
    }   
}
