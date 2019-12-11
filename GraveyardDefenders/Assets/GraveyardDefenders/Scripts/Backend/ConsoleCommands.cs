using UnityEngine;
using CommandTerminal;

namespace XD
{
    public static class ConsoleCommands
    {
        [RegisterCommand("TimeScale", Help = "Set time scale", MaxArgCount = 1, MinArgCount = 1)]
        static void SetTimeScale(CommandArg[] args)
        {
            Time.timeScale = args[0].Float;
        }
    }   
}
