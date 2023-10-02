using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIGameRuntime : MonoBehaviour
{   
    [SerializeField] private PlacementSystem ps;
    [SerializeField] private GameStats gs;
    private VisualElement root;
    private VisualElement gameUI;
    private VisualElement menuUI;
    private VisualElement placementToolTip;
    private VisualElement deletionToolTip;
    private VisualElement controlsToolTip;
    private VisualElement tutorialPanel;
    private Button buttonCuttingBoard;
    private Button buttonPan;
    private Button buttonPot;
    private Button buttonFry;
    private Button buttonMeat;
    private Button trackStraight;
    private Button trackTurn;
    private Button trackSwitch;
    private Button deleteTool;
    private Button buttonExit;
    private Button buttonMute;
    private Button buttonHowTo;
    private Button buttonHowToClose;
    private Label cash;
    private bool muteSound;

    private GameObject PlacementSystem;
    private void Awake()
    {
        // -- INITIALIZATION --
        root = GetComponent<UIDocument>().rootVisualElement;

        buttonCuttingBoard = root.Q<Button>("CuttingBoard");
        buttonPan = root.Q<Button>("Pan");
        buttonPot = root.Q<Button>("Pot");
        buttonFry = root.Q<Button>("Fry");
        buttonMeat = root.Q<Button>("Meat");

        trackStraight = root.Q<Button>("TrackStraight");
        trackTurn = root.Q<Button>("TrackTurn");
        trackSwitch = root.Q<Button>("TrackSwitch");

        deleteTool = root.Q<Button>("DeleteTool");

        placementToolTip = root.Q<VisualElement>("PlacementTooltip");
        deletionToolTip = root.Q<VisualElement>("DeletionTooltip");
        controlsToolTip = root.Q<VisualElement>("ControlsTooltip");
        tutorialPanel = root.Q<VisualElement>("HowToOverlay");
        buttonExit = root.Q<Button>("ButtonExit");
        buttonMute = root.Q<Button>("ButtonMute");
        buttonHowTo = root.Q<Button>("ButtonHowTo");
        buttonHowToClose = root.Q<Button>("CloseTutorial");

        cash = root.Q<Label>("Cash");

        // -- ASSIGNMENT --
        // Devices
        buttonCuttingBoard.clicked += () => StartPlacement(3);
        buttonPan.clicked += () => StartPlacement(5);
        buttonPot.clicked += () => StartPlacement(4);
        buttonFry.clicked += () => StartPlacement(7);
        buttonMeat.clicked += () => StartPlacement(6);

        // Tracks
        trackStraight.clicked += () => StartPlacement(0);
        trackTurn.clicked += () => StartPlacement(1);
        trackSwitch.clicked += () => StartPlacement(2);

        // Tools
        deleteTool.clicked += () => StartDelete();
        buttonHowTo.clicked += () => OpenTutorial();
        buttonHowToClose.clicked += () => ClosePanel();
        buttonExit.clicked += EndGameClicked;
        buttonMute.clicked += ToggleAllSound;

        deletionToolTip.visible = false;
        controlsToolTip.visible = true;
        placementToolTip.visible = false;
        tutorialPanel.visible = false;

        muteSound = false;
        AudioListener.volume = 0.4f;
        buttonMute.AddToClassList(".soundon");
    }

    private void StartPlacement(int i)
    {
        controlsToolTip.visible = false;
        deletionToolTip.visible = false;
        placementToolTip.visible = true;
        ps.StartPlacement(i);
    }

    private void StartDelete()
    {
        controlsToolTip.visible = false;
        placementToolTip.visible = false;
        deletionToolTip.visible = true;
        ps.StartDelete();
    }

    private void OpenTutorial()
    {
        tutorialPanel.visible = true;
    }

    private void ClosePanel()
    {
        placementToolTip.visible = false;
        deletionToolTip.visible = false;
        controlsToolTip.visible = false;
        tutorialPanel.visible = false;
    }

    private void EndGameClicked()
    {
        SceneManager.LoadScene("GameMenu");
    }

    private void DisplayCurrentFunds()
    {
        cash.text = "$ " + gs.getCurrentFunds();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }

        DisplayCurrentFunds();
    }

    public void ToggleAllSound()
{
    muteSound = !muteSound;

    if (muteSound)
    {
        AudioListener.volume = 0;
    } else {
        AudioListener.volume = 0.4f;
    }

}

}
