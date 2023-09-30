using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class main_menu : MonoBehaviour
{
    

    // Start is called before the first frame update
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button ButtonStart = root.Q<Button>("ButtonStart");
        Button ButtonExit = root.Q<Button>("ButtonExit");

        ButtonStart.clicked += () => SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        ButtonExit.clicked += () => UnityEditor.EditorApplication.isPlaying = false;
        
    }
}
