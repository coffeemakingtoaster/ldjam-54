using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIRuntime : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonPlay = root.Q<Button>("ButtonStart");

        buttonPlay.clicked += () => SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);

    }

}
