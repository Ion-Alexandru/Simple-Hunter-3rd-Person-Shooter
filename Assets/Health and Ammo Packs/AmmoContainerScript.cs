using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoContainerScript : MonoBehaviour
{
    public GameObject ammoContainerPrefab;

    public int ammoToAdd = 100; // Set the amount of ammo to add

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player (you may need to adjust the tag)
        if (other.CompareTag("Player"))
        {
            // Get the player's ammo script (replace "PlayerAmmoScript" with the actual name of your player's ammo script)
            PlayerController playerController = other.GetComponent<PlayerController>();

            // Check if the player's ammo script is found
            if (playerController != null)
            {
                // Add ammo to the player
                playerController.ammoCount += ammoToAdd;
                playerController.ammoText.text = playerController.ammoCount.ToString();


                // Disable the ammo container (make it disappear)
                gameObject.SetActive(false);
            }
        }
    }
}
