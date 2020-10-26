using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;
using XD.Net;
using System;

namespace XD
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Assignables")]
        public Canvas canvas;
        public CanvasGroup group;
        public GameObject mainMenuButtons;
        public GameObject multiMenuButtons;
        public GameObject gameBrowserMenu;
        public GameObject title;

        [Header("Variables")]
        public float fadeTime;
        public string sceneToLoadName;

        [Header("Events")]
        public static UnityEvent OnGameStart;

        void OnEnable()
        {
            MainMenuEvents.enteredLobby.AddListener(OnEnteredLobby);
            MainMenuEvents.leftLobby.AddListener(OnExitLobby);
            MainMenuEvents.onlineBrowseBackPressed.AddListener(OnlineBrowseBack);
        }

        void OnDisable()
        {
            MainMenuEvents.enteredLobby.RemoveListener(OnEnteredLobby);
            MainMenuEvents.leftLobby.RemoveListener(OnExitLobby);
            MainMenuEvents.onlineBrowseBackPressed.RemoveListener(OnlineBrowseBack);
        }

        void OnEnteredLobby()
        {
            title.gameObject.SetActive(false);
        }

        void OnExitLobby()
        {
            mainMenuButtons.SetActive(true);
            title.gameObject.SetActive(true);
        }

        #region MAIN_BUTTONS
        public void Couch()
        {
            MainMenuEvents.couchButtonPressed.Invoke();
            mainMenuButtons.SetActive(false);
        }

        public void Online()
        {
            MainMenuEvents.onlineButtonPressed.Invoke();
            NetManager.Instance.ConnectToPhoton();
            multiMenuButtons.SetActive(true);
            mainMenuButtons.SetActive(false);
        }

        public void Settings()
        {
            MainMenuEvents.settingsPressed.Invoke();
        }

        //called from button
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion

        #region MULTI_BUTTONS

        public void OnlineBack()
        {
            MainMenuEvents.onlineBackButtonPressed.Invoke();
            multiMenuButtons.SetActive(false);
            mainMenuButtons.SetActive(true);
            NetManager.Instance.Disconnect();
        }

        public void OnlineBrowse()
        {
            MainMenuEvents.onlineBrowsePressed.Invoke();
            NetManager.Instance.JoinLobby();
            multiMenuButtons.SetActive(false);
            gameBrowserMenu.SetActive(true);
        }

        public void OnlineBrowseBack()
        {
            gameBrowserMenu.SetActive(false);
            NetManager.Instance.LeaveLobby();
            multiMenuButtons.SetActive(true);
        }

        public void OnlineCreateGame()
        {
            MainMenuEvents.createRoomPressed.Invoke();
            NetManager.Instance.CreateRoom();
        }
        #endregion

        public void StartGame()
        {
            group.DOFade(0.0f, fadeTime).OnComplete(StartGameFadeEnded);
            SceneManager.LoadScene(sceneToLoadName);
        }

        private void StartGameFadeEnded()
        {
            OnGameStart.Invoke();
        }
    }
}
