using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

namespace XD
{
    public class PlayerInput : Singleton<PlayerInput>
    {
        [NonSerialized] public List<PlayerCharacter> local_players = new List<PlayerCharacter>(Constants.maxPlayers);
        [NonSerialized] public List<InputDevice> devices = new List<InputDevice>(Constants.maxPlayers);
        [NonSerialized] public List<int> selectedPlayers = new List<int>(Constants.maxPlayers);

        [NonSerialized, Tooltip("Used only for keyboard players")] public List<int> deviceIndices = new List<int>(Constants.maxPlayers);
        public bool inGameplayScene = false;
        public int CurrentPlayerCount { get { return devices.Count; } }
        public GameObject playerPrefab;
        public bool debugCreatePlayers = false;
        [Range(1, 4)] public int debugNumPlayers = 1;

        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InputSystem.onDeviceChange +=
            (device, change) =>
            {
                Debug.Log($"Device Change::device: {device.name} change: {change}");
                switch (change)
                {
                    case InputDeviceChange.Added:
                        // New Device.
                        break;
                    case InputDeviceChange.Disconnected:
                        // Device got unplugged.
                        break;
                    case InputDeviceChange.Reconnected:
                        // Plugged back in.
                        break;
                    case InputDeviceChange.Removed:
                        // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                        break;
                    default:
                        break;
                }
            };

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Init();
        }

        public bool AddPlayerAttempt(InputDevice device, int index)
        {
            if (local_players.Count >= Constants.maxPlayers)
            {
                Debug.Log($"AddPlayerAttempt but local players has allready: {local_players.Count} local players");
                return false;
            }

            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i] == device && index == deviceIndices[i])
                {
                    Debug.Log($"AddPlayerAttempt");
                    return false;
                }
            }

            LocalPlayerJoined(device, index);
            return true;
        }

        public void LocalPlayerJoined(InputDevice device, int index)
        {
            devices.Add(device);
            deviceIndices.Add(index);
            selectedPlayers.Add(0);
        }

    public void Init()
        {
            PlayerSpawnPoints spawnPoints = FindObjectOfType<PlayerSpawnPoints>();
            bool gameplayScene = spawnPoints != null;
            if (gameplayScene)
            {
                if (debugCreatePlayers)
                {
                    for (int i = 0; i < debugNumPlayers; i++)
                    {
                        Transform spawn = spawnPoints.spawns[i].transform;
                        devices.Add(Keyboard.current);
                        deviceIndices.Add(i);
                        GameObject p = Instantiate(playerPrefab, spawn.position, spawn.rotation);
                        PlayerCharacter pc = p.GetComponent<PlayerCharacter>();
                        pc.id = i;
                        local_players.Add(pc);
                        PlayerEvents.playerAddedEvnt.Invoke(pc);
                    }
                }
                else
                {
                    for (int i = 0; i < devices.Count; i++)
                    {
                        Transform spawn = spawnPoints.spawns[i].transform;
                        GameObject p = Instantiate(playerPrefab, spawn.position, spawn.rotation);
                        PlayerCharacter pc = p.GetComponent<PlayerCharacter>();
                        pc.id = i;
                        local_players.Add(pc);
                        PlayerEvents.playerAddedEvnt.Invoke(pc);
                    }
                }
            }
        }

        public void Update()
        {
            int playerCount = local_players.Count;
            for (int i = 0; i < playerCount; i++)
            {
                PlayerCharacter pc = local_players[i];
                InputDevice device = devices[i];
                if (pc == null || device == null) continue;
                UpdatePlayer(deviceIndices[i], pc, device);
            }
        }

        public void UpdatePlayer(int index, PlayerCharacter character, InputDevice device)
        {
            //update based on type of controller
            if (device is Gamepad gamepad)
            {
                character.moveVector = gamepad.GetMoveVec();
                character.interactPressedThisFrame = gamepad.xButton.wasPressedThisFrame;
                character.interactReleasedThisFrame = gamepad.xButton.wasReleasedThisFrame;
            }
            else if (device is Keyboard keyboard)
            {
                character.moveVector = keyboard.GetMoveVec(index, character.moveVector);
                keyboard.UpdateInteract(index, ref character.interactPressedThisFrame, ref character.interactReleasedThisFrame);
            }
            else
            {
                Debug.Log("Unsuported device on PlayerInput.UpdatePlayer");
            }

            character.ManualUpdate();
        }
    }   
}
