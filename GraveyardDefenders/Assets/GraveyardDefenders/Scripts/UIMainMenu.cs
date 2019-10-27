using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace XD
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Assignables")]
        public Canvas canvas;
        public CanvasGroup group;

        [Header("Variables")]
        public float fadeTime;
        public string sceneToLoadName;

        [Header("Events")]
        public static UnityEvent OnGameStart;

        public void StartGame()
        {
            group.DOFade(0.0f, fadeTime).OnComplete(StartGameFadeEnded);
            SceneManager.LoadScene(sceneToLoadName);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void StartGameFadeEnded()
        {
            OnGameStart.Invoke();
        }
    }
}
