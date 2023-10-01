using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static PlacementSystem;

public class UIRuntime : MonoBehaviour
{   
    [SerializeField] private PlacementSystem ps;
    private VisualElement root;
    private VisualElement gameUI;
    private VisualElement menuUI;
    private Button buttonSettings;
    private VisualElement buttonWrapper;
    private GameObject PlacementSystem;
    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        //root.Q<Button>("ButtonStart");

        Button buttonPlay = root.Q<Button>("ButtonStart");
        Button buttonExit = root.Q<Button>("ButtonExit");
        buttonSettings = root.Q<Button>("ButtonSettings");
        buttonWrapper = root.Q<VisualElement>("MenuBar");
        //gameUI = root.Q<VisualElement>("MenuBar");
        Button buttonHowTo = root.Q<Button>("ButtonHowTo");

        buttonPlay.clicked += PlayGame;
        buttonExit.clicked += ExitApplicationClicked;
        buttonSettings.clicked += () => ps.StartPlacement(0);


        //buttonSettings.clicked += SettingsClicked;
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    private void SettingsClicked()
    {
        buttonWrapper.Clear();
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