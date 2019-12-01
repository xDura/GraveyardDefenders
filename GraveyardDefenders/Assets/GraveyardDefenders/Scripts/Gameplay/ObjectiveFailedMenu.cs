using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace XD
{
    public class ObjectiveFailedMenu : MonoBehaviour
    {
        [Header("Assignable")]
        public CanvasGroup canvas_group;

        [Header("Variables")]
        public float fadeTime = 0.2f;
        public float timeToLoad = 5.0f;
        public string sceneToLoad;

        private void OnEnable()
        {
            canvas_group.DOFade(1.0f, fadeTime);
            InvokeSceneLoad();
        }

        private void InvokeSceneLoad()
        {
            Invoke("SceneLoad", timeToLoad);
        }

        private void SceneLoad()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
