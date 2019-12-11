using UnityEngine;
using UnityEngine.SceneManagement;

namespace XD
{
    public class UIPauseMenu : MonoBehaviour
    {
        public bool isOpen;
        public Canvas canvas;

        public void Awake()
        {
            Close();
        }

        private void OnDestroy()
        {
            Close();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOpen) Close();
                else Open();
            }
        }

        public void Open()
        {
            canvas.enabled = true;
            isOpen = true;
            Time.timeScale = 0.0f;
        }

        public void Close()
        {
            canvas.enabled = false;
            isOpen = false;
            Time.timeScale = 1.0f;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene("Level_01");
        }

        public void Surrender()
        {
            Close();
            FindObjectOfType<Objective>().Fail();
        }

        //Unify this with UIMainMenu
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }   
}
