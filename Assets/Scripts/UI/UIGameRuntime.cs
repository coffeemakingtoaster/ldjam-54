using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIGameRuntime : MonoBehaviour
{   
    [SerializeField] private PlacementSystem ps;
    private VisualElement root;
    private VisualElement gameUI;
    private VisualElement menuUI;
    private VisualElement trackToolTip;
    private VisualElement tutorialPanel;
    private Button buttonCuttingBoard;
    private Button buttonPlate;
    private Button buttonPan;
    private Button buttonPot;
    private Button buttonFry;
    private Button buttonMeat;
    private Button trackStraight;
    private Button trackTurn;
    private Button trackSwitch;
    private Button deleteTool;
    private Button buttonSettings;
    private Button buttonExit;
    private Button buttonHowTo;
    private Button buttonHowToClose;
    private Label cash;

    private GameObject PlacementSystem;
    private void Awake()
    {
        // -- INITIALIZATION --
        root = GetComponent<UIDocument>().rootVisualElement;

        buttonCuttingBoard = root.Q<Button>("CuttingBoard");
        buttonPlate = root.Q<Button>("Plate");
        buttonPan = root.Q<Button>("Pan");
        buttonPot = root.Q<Button>("Pot");
        buttonFry = root.Q<Button>("Fry");
        buttonMeat = root.Q<Button>("Meat");

        trackStraight = root.Q<Button>("TrackStraight");
        trackTurn = root.Q<Button>("TrackTurn");
        trackSwitch = root.Q<Button>("TrackSwitch");

        deleteTool = root.Q<Button>("DeleteTool");

        trackToolTip = root.Q<VisualElement>("TrackTooltip");
        tutorialPanel = root.Q<VisualElement>("HowToOverlay");
        buttonExit = root.Q<Button>("ButtonExit");
        buttonSettings = root.Q<Button>("ButtonSettings");
        buttonHowTo = root.Q<Button>("ButtonHowTo");
        buttonHowToClose = root.Q<Button>("CloseTutorial");

        // -- ASSIGNMENT --
        // Devices
        buttonCuttingBoard.clicked += () => StartPlacement(2);
        buttonPlate.clicked += () => StartPlacement(2);
        buttonPan.clicked += () => StartPlacement(2);
        buttonPot.clicked += () => StartPlacement(2);
        buttonFry.clicked += () => StartPlacement(2);
        buttonMeat.clicked += () => StartPlacement(2);

        // Tracks
        trackStraight.clicked += () => TrackClicked(1);
        trackTurn.clicked += () => TrackClicked(4);
        trackSwitch.clicked += () => TrackClicked(3);

        // Tools
        deleteTool.clicked += () => StartDelete();
        buttonHowTo.clicked += () => OpenTutorial();
        buttonHowToClose.clicked += () => CloseTutorial();

        buttonExit.clicked += ExitApplicationClicked;

        trackToolTip.visible = false;
        tutorialPanel.visible = false;
    }
    
    private void SettingsClicked()
    {
        //buttonWrapper.Clear();
    }

    private void TrackClicked(int i)
    {
        trackToolTip.visible = true;
        ps.StartPlacement(i);
        Debug.Log("Track " + i);
    }

    private void StartPlacement(int i)
    {
        ps.StartPlacement(i);
        Debug.Log("StartPlacement " + i);
    }

    private void StartDelete()
    {
        ps.StartDelete();
        Debug.Log("StartDelete");
    }

    private void OpenTutorial()
    {
        tutorialPanel.visible = true;
    }

    private void CloseTutorial()
    {
        tutorialPanel.visible = false;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            trackToolTip.visible = false;
        }
    }

}
