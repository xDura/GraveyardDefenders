using UnityEngine;
using XD.Net;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

namespace XD
{
    public class LobbyScene : MonoBehaviour
    {
        public Camera lobbyCamera;
        public CustomPlayersTrigger playerReadyTrigger;
        public CustomPlayersTrigger playerExitTrigger;
        public bool allPlayersInside = false;

        public void OnEnable()
        {
            MainMenuEvents.couchButtonPressed.AddListener(Enter);
            MainMenuEvents.createRoomPressed.AddListener(Enter);
            NetEvents.OnJoinedRoom.AddListener(OnJoinedRoom);
            NetEvents.OnLeftRoom.AddListener(Exit);

            playerReadyTrigger.onAllPlayersInside.AddListener(AllPlayersInsideReady);
            playerReadyTrigger.onPlayerExit.AddListener(OnPlayerExitReadyTrigger);
            playerExitTrigger.onPlayerEnter.AddListener(OnPlayerEnteredExitTrigger);
        }

        public void OnDisable()
        {
            MainMenuEvents.couchButtonPressed.RemoveListener(Enter);
            MainMenuEvents.createRoomPressed.RemoveListener(Enter);
            NetEvents.OnJoinedRoom.RemoveListener(OnJoinedRoom);
            NetEvents.OnLeftRoom.RemoveListener(Exit);

            playerReadyTrigger.onAllPlayersInside.RemoveListener(AllPlayersInsideReady);
            playerReadyTrigger.onPlayerExit.RemoveListener(OnPlayerExitReadyTrigger);
            playerExitTrigger.onPlayerEnter.RemoveListener(OnPlayerEnteredExitTrigger);
        }

        void OnPlayerEnteredExitTrigger(PlayerCharacter pc)
        {
            if (!pc.isLocal) return;
            Exit();
        }

        void AllPlayersInsideReady()
        {
            allPlayersInside = true;
            MainMenuEvents.allPlayersReadyLobby.Invoke();
            SceneManager.LoadScene("Level_01");
        }

        void OnPlayerExitReadyTrigger(PlayerCharacter pc)
        {
            allPlayersInside = false;
        }

        void Update()
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
                PlayerInput.Instance.AddPlayerAttempt(Keyboard.current, 0);
            if (Keyboard.current.lKey.wasPressedThisFrame)
                PlayerInput.Instance.AddPlayerAttempt(Keyboard.current, 1);
            if (Keyboard.current.numpad0Key.wasPressedThisFrame)
                PlayerInput.Instance.AddPlayerAttempt(Keyboard.current, 2);
            if (Keyboard.current.numpadPlusKey.wasPressedThisFrame)
                PlayerInput.Instance.AddPlayerAttempt(Keyboard.current, 3);
            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                Gamepad currentPad = Gamepad.all[i];
                if (currentPad.xButton.wasPressedThisFrame) PlayerInput.Instance.AddPlayerAttempt(currentPad, 0);
            }
        }

        void OnJoinedRoom()
        {
            if(NetManager.Instance.IsClient) Enter();
        }

        void Enter()
        {
            MainMenuEvents.enteredLobby.Invoke();
            lobbyCamera.gameObject.SetActive(true);
        }

        void Exit()
        {
            MainMenuEvents.leftLobby.Invoke();
            lobbyCamera.gameObject.SetActive(false);
            if (NetManager.Instance.IsClient) NetManager.Instance.LeaveRoom();
        }
    }   
}
