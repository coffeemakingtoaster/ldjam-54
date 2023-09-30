using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace UIToolkitDemo
{
    // non-UI logic for HomeScreen
    public class HomeScreenController : MonoBehaviour
    {
        public static event Action MainMenuExited;

        void OnEnable()
        {
            HomeScreen.PlayButtonClicked += OnPlayGameLevel;
        }

        void OnDisable()
        {
            HomeScreen.PlayButtonClicked -= OnPlayGameLevel;
        }

        void Start()
        {

        }

        // scene-management methods
        public void OnPlayGameLevel()
        {

            MainMenuExited?.Invoke();

#if UNITY_EDITOR
            if (Application.isPlaying);
#endif
                //SceneManager.LoadSceneAsync(m_LevelData.sceneName);
        }
    }
}
