using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Variables
    private bool isGameOver = false;
    private float elapsedTime = 0.0f;
    public int enemiesKilled = 0;

    // Game Objects
    private PlayerController playerController;
    public GameObject deadScreen;

    // UI elements
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI elapsedTimeText;
    public TextMeshProUGUI abilityCooldownText;

    public Button reloadGameButton;
    public Button exitToMainMenu;

    // Terrain variables
    public Terrain terrain;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        deadScreen.SetActive(false);

        playerController = FindObjectOfType<PlayerController>();

        terrain.treeDistance = 500;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Check your game over condition here
        if (playerController.playerHP <= 0)
        {
            GameOver();
        }

        // Update the cooldown of the ability
        abilityCooldownText.text = Mathf.RoundToInt(playerController.abilityCooldownTimer).ToString();

        if (playerController.abilityCooldownTimer <= 0)
        {
            abilityCooldownText.gameObject.SetActive(false);
        }
        else
        {
            abilityCooldownText.gameObject.SetActive(true);
        }
    }

    void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;

            // Set time scale to 0 to freeze the game
            Time.timeScale = 0f;

            deadScreen.SetActive(true);

            // Make the cursor visible and unlocked again
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            enemiesKilledText.text = "Enemies killed: " + enemiesKilled.ToString();
            levelText.text = "Level Obtained: " + playerController.playerLevel.ToString();
            elapsedTimeText.text = "You survived for: " + Mathf.RoundToInt(elapsedTime).ToString() + " seconds";

            reloadGameButton.onClick.AddListener(ReloadGame);

            exitToMainMenu.onClick.AddListener(ExitToMainMenu);
        }
    }

    // Example method to resume the game (called from a UI button or some other event)
    void ReloadGame()
    {
        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
