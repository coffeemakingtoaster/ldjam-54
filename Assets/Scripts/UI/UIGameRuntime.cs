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
    private Button buttonSettings;
    private Button buttonCuttingBoard;
    private Button buttonPlate;
    private Button buttonPan;
    private Button buttonPot;
    private Button buttonFry;
    private Button buttonMeat;
    private Button buttonExtra;
    private Button trackStraight;
    private Button trackTurn;
    private Button trackSwitch;
    private Label cash;
    private VisualElement buttonWrapper;
    private GameObject PlacementSystem;
    private void Awake()
    {
        // -- INITIALIZATION --
        root = GetComponent<UIDocument>().rootVisualElement;

        buttonCuttingBoard = root.Q<Button>("CuttingBoard");
        buttonPlate = root.Q<Button>("Plate");
        trackTurn = root.Q<Button>("TrackTurn");

        
        
        trackToolTip = root.Q<VisualElement>("TrackTooltip");
        Button buttonExit = root.Q<Button>("ButtonExit");
        buttonSettings = root.Q<Button>("ButtonSettings");
        buttonWrapper = root.Q<VisualElement>("MenuBar");
        //gameUI = root.Q<VisualElement>("MenuBar");
        Button buttonHowTo = root.Q<Button>("ButtonHowTo");

        //buttonSchneidebrett.clicked += () => ps.StartPlacement(2);
        buttonExit.clicked += ExitApplicationClicked;
        trackTurn.clicked += TrackTurnClicked;
        buttonPlate.clicked += () => Debug.Log(buttonPlate);
        //buttonSettings.clicked += () => ps.StartPlacement(0);


        //buttonSettings.clicked += SettingsClicked;
    }
    
    private void SettingsClicked()
    {
        buttonWrapper.Clear();
    }

    private void TrackTurnClicked()
    {
        trackToolTip.visible = true;
        Debug.Log("TrackTurn");
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
