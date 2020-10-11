using UnityEngine;
using CommandTerminal;
using UnityEditor;
using UnityEngine.InputSystem;
using XD.Multiplayer;

namespace XD
{
    public static class ConsoleCommands
    {
        [RegisterCommand("TimeScale", Help = "Set time scale", MaxArgCount = 1, MinArgCount = 1)]
        static void SetTimeScale(CommandArg[] args)
        {
            Time.timeScale = args[0].Float;
        }

        [RegisterCommand("Server", Help = "Connect a server", MaxArgCount = 1, MinArgCount = 0)]
        static void CreateServer(CommandArg[] args)
        {
            //BoltLauncher.StartServer();
        }

        [RegisterCommand("Client", Help = "Connect a client", MaxArgCount = 1, MinArgCount = 0)]
        static void ConnectAsClient(CommandArg[] args)
        {
            //BoltLauncher.StartClient(UdpEndPoint.Any);
        }

    }   
}
