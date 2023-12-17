using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthContainerScript : MonoBehaviour
{
    public GameObject healthContainerPrefab;

    public int healthToAdd = 50; // Set the amount of ammo to add

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player (you may need to adjust the tag)
        if (other.CompareTag("Player"))
        {
            // Get the player's ammo script (replace "PlayerAmmoScript" with the actual name of your player's ammo script)
            PlayerController playerController = other.GetComponent<PlayerController>();

            // Check if the player's ammo script is found
            if (playerController != null && playerController.playerHP < playerController.playerMaxHP)
            {
                // Calculate the remaining space in HP before reaching playerMaxHP
                int healthRemained = playerController.playerMaxHP - playerController.playerHP;

                // Add the minimum value between healthToAdd and the remaining space to playerHP
                playerController.playerHP += Mathf.Min(healthToAdd, healthRemained);

                // Update the playerHPBar text
                playerController.playerHPBar.text = playerController.playerHP.ToString();

                // Disable the health container (make it disappear)
                gameObject.SetActive(false);
            }
        }
    }
}
