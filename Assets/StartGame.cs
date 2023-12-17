using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public string sceneToLoad = "SampleScene";
    public Button startButton;  // Reference to the start game button in the Unity Editor
    public Button exitButton;   // Reference to the exit game button in the Unity Editor

    void Start()
    {
        // Assuming you've already set up the button references in the Unity Editor
        if (startButton != null)
        {
            // Attach the LoadGame method to the start button's onClick event
            startButton.onClick.AddListener(LoadGame);
        }

        if (exitButton != null)
        {
            // Attach the QuitGame method to the exit button's onClick event
            exitButton.onClick.AddListener(QuitGame);
        }
    }

    // You can call these methods from other scripts, UI button clicks, or any other events

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}