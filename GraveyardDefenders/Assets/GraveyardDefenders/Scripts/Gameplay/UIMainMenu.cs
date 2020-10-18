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

        [Header("Variables")]
        public float fadeTime;
        public string sceneToLoadName;

        [Header("Events")]
        public static UnityEvent OnGameStart;

        #region MAIN_BUTTONS
        public void Couch()
        {

        }

        public void Online()
        {
            NetManager.Instance.ConnectToPhoton();
            multiMenuButtons.SetActive(true);
            mainMenuButtons.SetActive(false);
        }

        public void Settings()
        {

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
            multiMenuButtons.SetActive(false);
            mainMenuButtons.SetActive(true);
            NetManager.Instance.Disconnect();
        }

        public void OnlineBrowse()
        {
            NetManager.Instance.JoinLobby();
            multiMenuButtons.SetActive(false);
            gameBrowserMenu.SetActive(true);
        }

        public void OnlineCreateGame()
        {
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
