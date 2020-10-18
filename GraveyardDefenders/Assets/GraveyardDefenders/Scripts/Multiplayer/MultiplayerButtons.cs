using UnityEngine;
using UnityEngine.UI;
using XD.Net;

namespace XD
{
    public class MultiplayerButtons : MonoBehaviour
    {
        public Button buttonCreateRoom;
        public Button buttonBrowseGames;

        public void OnEnable() { SetInteractable(false); }

        public void OnDisable() { SetInteractable(false); }

        public void Update()
        {
            bool canUseButtons = NetManager.Instance.IsConnectedAndReadyTrue && !NetManager.Instance.InRoom;
            SetInteractable(canUseButtons);
        }

        public void SetInteractable(bool interactable)
        {
            buttonCreateRoom.interactable = interactable;
            buttonBrowseGames.interactable = interactable;
        }
    }   
}
