using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

namespace XD
{
    public class PlayerInput : Singleton<PlayerInput>
    {
        [NonSerialized] public List<PlayerCharacter> local_players;
        [NonSerialized] public List<InputDevice> devices;
        [NonSerialized] public List<int> selectedPlayers;

        [NonSerialized, Tooltip("Used only for keyboard players")] public List<int> deviceIndices;
        public bool inGameplayScene = false;
        public int CurrentPlayerCount => devices.Count;
        public List<GameObject> playerPrefabs;
        public bool debugCreatePlayers = false;
        public List<int> debugSelected = new List<int>();
        [Range(1, 4)] public int debugNumPlayers = 1;

        public override void OnSingletonAwake()
        {
            base.OnSingletonAwake();

            local_players = new List<PlayerCharacter>(Constants.maxPlayers);
            devices = new List<InputDevice>(Constants.maxPlayers);
            selectedPlayers = new List<int>(Constants.maxPlayers);
            deviceIndices = new List<int>(Constants.maxPlayers);

            MainMenuEvents.enteredLobby.AddListener(JoinLobby);
            MainMenuEvents.leftLobby.AddListener(LeaveLobby);
            MainMenuEvents.enteredLobby.AddListener(EnteredLobby);
            MainMenuEvents.allPlayersReadyLobby.AddListener(OnAllPlayersReadyLobby);

            InputSystem.onDeviceChange +=
            (device, change) =>
            {
                Debug.Log($"Device Change::device: {device.name} change: {change}");
                switch (change)
                {
                    case InputDeviceChange.Added: // New Device.
                        break;
                    case InputDeviceChange.Disconnected: // Device got unplugged.
                        break;
                    case InputDeviceChange.Reconnected: // Plugged back in.
                        break;
                    case InputDeviceChange.Removed: // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                        break;
                    default:
                        break;
                }
            };

            SceneManager.sceneLoaded += OnSceneLoaded;
#if UNITY_EDITOR
            //para testeo añadir algo Init();
#endif
        }

        public override void OnSingletonDestroy(bool isMainInstance)
        {
            base.OnSingletonDestroy(isMainInstance);
            Debug.Log("OnSingletonDestroy");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            MainMenuEvents.enteredLobby.RemoveListener(JoinLobby);
            MainMenuEvents.leftLobby.RemoveListener(LeaveLobby);
            MainMenuEvents.enteredLobby.RemoveListener(EnteredLobby);
            MainMenuEvents.allPlayersReadyLobby.RemoveListener(OnAllPlayersReadyLobby);
        }

        void OnAllPlayersReadyLobby()
        {
            DestroyAllPlayerCharacters();
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DestroyAllPlayerCharacters();
            if (FindObjectOfType<UIMainMenu>()) return;

            SpawnAllPlayersForDevices();
        }

        public bool AddPlayerAttempt(InputDevice device, int index)
        {
            if (CurrentPlayerCount >= Constants.maxPlayers)
            {
                Debug.Log($"AddPlayerAttempt but local players has allready: {CurrentPlayerCount} local players");
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
            SpawnPlayer(index, selectedPlayers[index]);
        }

        public GameObject GetPlayerPrefab(int id) { return Constants.Instance.GetPrefab(id); }

        public void JoinLobby()
        {
            int i = 0;
            foreach (Gamepad gamepad in Gamepad.all)
            {
                AddPlayerAttempt(gamepad, i);
                i++;
            }
        }

        void EnteredLobby()
        {
            SpawnAllPlayersForDevices();
        }

        void SpawnAllPlayersForDevices()
        {
            for (int i = 0; i < devices.Count; i++)
                SpawnPlayer(deviceIndices[i], selectedPlayers[i]);
        }

        void DestroyAllPlayerCharacters()
        {
            for (int i = 0; i < local_players.Count; i++)
            {
                if (local_players[i] == null) continue;

                Destroy(local_players[i].gameObject);
            }
            local_players.Clear();
        }

        public void LeaveLobby()
        {
            DestroyAllPlayerCharacters();
        }

        public PlayerCharacter SpawnPlayer(int index, int skin)
        {
            PlayerSpawnPoints spawnPoints = FindObjectOfType<PlayerSpawnPoints>();
            Transform spawn = spawnPoints.spawns[index].transform;
            GameObject p = Instantiate(GetPlayerPrefab(skin), spawn.position, spawn.rotation);
            PlayerCharacter pc = p.GetComponent<PlayerCharacter>();
            pc.SetSkin(skin);
            pc.id = local_players.Count;
            local_players.Add(pc);
            PlayerEvents.playerAddedEvnt.Invoke(pc);
            return pc;
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
