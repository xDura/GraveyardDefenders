using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XD.Net;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;

namespace XD
{
    public class RoomBrowser : MonoBehaviour
    {
        [SerializeField] private List<Button> room_uis = new List<Button>();

        [SerializeField] private Button refreshButton = default;
        [SerializeField] private Button leftButton = default;
        [SerializeField] private Button rightButton = default;
        public GameObject multiplayerMenu;

        [SerializeField] private int currentPage = 0;
        private int roomsPerPage = 4;
        private List<RoomInfo> rooms = new List<RoomInfo>();

        int CurrentFirstIndex => currentPage * roomsPerPage;
        int CurrentLastIndex => CurrentFirstIndex + roomsPerPage;
        bool CanGoRight => CurrentLastIndex < rooms.Count;
        bool CanGoLeft => currentPage > 0;

        void UpdateRoomsCache() { rooms = NetManager.Instance.room_list; }

        public void OnEnable()
        {
            Init();
            UpdateRoomsCache();
            UpdateRoomListUI();
        }

        public void OnDisable()
        {
            Init();
        }

        private void Init()
        {
            currentPage = 0;
            roomsPerPage = room_uis.Count;
        }

        public void OnLeftButton()
        {
            currentPage--;
            currentPage = Mathf.Clamp(currentPage, 0, currentPage);
            UpdateRoomListUI();
        }

        public void OnRightButton()
        {
            currentPage++;
            UpdateRoomListUI();
        }

        public void OnRefreshButton()
        {
            Init();
            UpdateRoomsCache();
            UpdateRoomListUI();
        }

        public void OnRoomButton(int index)
        {
            int realIndex = CurrentFirstIndex + index;
            if (realIndex >= rooms.Count) 
            {
                Debug.LogError($"OnRoomButton out of range realIndex: {realIndex} CurrentFirstIndex: {CurrentFirstIndex} roomCount: {rooms.Count}");
                return;
            }

            this.gameObject.SetActive(false);
            NetManager.Instance.JoinRoom(rooms[realIndex].Name);
        }

        public void OnBackButton()
        {
            this.gameObject.SetActive(false);
            multiplayerMenu.SetActive(true);
            NetManager.Instance.LeaveLobby();
        }

        public void UpdateLeftRight()
        {
            leftButton.gameObject.SetActive(CanGoLeft);
            rightButton.gameObject.SetActive(CanGoRight);
        }

        public void UpdateRoomListUI()
        {
            if(CurrentFirstIndex >= rooms.Count)
            {
                Debug.LogError($"StartIndexOutOfBounds index: {CurrentFirstIndex} roomCount: {rooms.Count}");
                currentPage = 0;
            }

            UpdateLeftRight();

            for (int i = 0; i < roomsPerPage; i++)
            {
                int realIndex = CurrentFirstIndex + i;
                bool hasRoom = realIndex < rooms.Count;
                if(!hasRoom)
                {
                    room_uis[i].gameObject.SetActive(false);
                    continue;
                }

                room_uis[i].gameObject.SetActive(true);
                RoomInfo roomInfo = rooms[realIndex];
                Button ui = room_uis[i];
                ui.GetComponentInChildren<TextMeshProUGUI>().text = roomInfo.Name;
            }
        }
    }   
}
