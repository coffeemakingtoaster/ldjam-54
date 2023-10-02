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
    private VisualElement placementToolTip;
    private VisualElement deletionToolTip;
    private VisualElement tutorialPanel;
    private VisualElement settingsPanel;
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
    private Button buttonSettingsClose;
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

        placementToolTip = root.Q<VisualElement>("PlacementTooltip");
        deletionToolTip = root.Q<VisualElement>("DeletionTooltip");
        tutorialPanel = root.Q<VisualElement>("HowToOverlay");
        settingsPanel = root.Q<VisualElement>("SettingsOverlay");
        buttonExit = root.Q<Button>("ButtonExit");
        buttonSettings = root.Q<Button>("ButtonSettings");
        buttonHowTo = root.Q<Button>("ButtonHowTo");
        buttonHowToClose = root.Q<Button>("CloseTutorial");
        buttonSettingsClose = root.Q<Button>("CloseSettings");

        // -- ASSIGNMENT --
        // Devices
        buttonCuttingBoard.clicked += () => StartPlacement(0);
        buttonPlate.clicked += () => StartPlacement(1);
        buttonPan.clicked += () => StartPlacement(2);
        buttonPot.clicked += () => StartPlacement(3);
        buttonFry.clicked += () => StartPlacement(4);
        buttonMeat.clicked += () => StartPlacement(5);

        // Tracks
        trackStraight.clicked += () => StartPlacement(6);
        trackTurn.clicked += () => StartPlacement(7);
        trackSwitch.clicked += () => StartPlacement(8);

        // Tools
        deleteTool.clicked += () => StartDelete();
        buttonHowTo.clicked += () => OpenTutorial();
        buttonHowToClose.clicked += () => ClosePanel();
        buttonSettings.clicked += () => OpenSettings();
        buttonSettingsClose.clicked += () => ClosePanel();

        buttonExit.clicked += EndGameClicked;

        deletionToolTip.visible = false;
        placementToolTip.visible = false;
        tutorialPanel.visible = false;
    }

    private void StartPlacement(int i)
    {
        placementToolTip.visible = true;
        ps.StartPlacement(i);
        Debug.Log("StartPlacement " + i);
    }

    private void StartDelete()
    {
        deletionToolTip.visible = true;
        ps.StartDelete();
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
        placementToolTip.visible = false;
        deletionToolTip.visible = false;
        tutorialPanel.visible = false;
        settingsPanel.visible = false;
    }

    private void EndGameClicked()
    {
        SceneManager.LoadScene("GameMenu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

}
