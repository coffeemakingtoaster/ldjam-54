using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static PlacementSystem;

public class UIRuntime : MonoBehaviour
{   
    private VisualElement root;
    private VisualElement gameUI;
    private VisualElement menuUI;
    private VisualElement tutorialPanel;
    private VisualElement settingsPanel;
    private Button buttonPlay;
    private Button buttonExit;
    private Button buttonSettingsClose;
    private Button buttonHowTo;
    private Button buttonHowToClose;
    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        buttonPlay = root.Q<Button>("ButtonStart");
        buttonHowTo = root.Q<Button>("ButtonHowTo");
        buttonHowToClose = root.Q<Button>("CloseTutorial");
        buttonExit = root.Q<Button>("ButtonExit");

        settingsPanel = root.Q<VisualElement>("SettingsOverlay");
        tutorialPanel = root.Q<VisualElement>("HowToOverlay");

        buttonPlay.clicked += PlayGame;
        buttonExit.clicked += ExitApplicationClicked;
        buttonHowTo.clicked += () => OpenTutorial();
        buttonHowToClose.clicked += () => ClosePanel();

        tutorialPanel.visible = false;
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OpenTutorial()
    {
        tutorialPanel.visible = true;
    }

    private void OpenSettings()
    {
        settingsPanel.visible = true;
    }

    private void ClosePanel()
    {
        tutorialPanel.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    private void ExitApplicationClicked()
    {
        #if UNITY_EDITOR
        Debug.Log("Quitting game..");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Debug.Log("Quitting game..");
		Application.Quit ();
#endif
    }

}
