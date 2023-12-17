using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerController playerController;
    public float moveSpeed = 3f;
    public float runSpeed = 6f;
    private CharacterController characterController;
    public Animator playerAnimator;

    private bool isWalkingShooting;
    private bool isRunningShooting;

    public float gravity = 9.8f;
    public float jumpHeight = 2f;

    private float verticalVelocity;

    public Transform playerBody;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // Apply gravity
        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            // Reset vertical velocity if grounded
            verticalVelocity = -0.5f;
        }

        // Handle jumping
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
            playerAnimator.SetBool("rifle_jump", true);
        } else
        {
            playerAnimator.SetBool("rifle_jump", false);
        }

        // Combine movement with vertical velocity
        Vector3 finalMove = moveDirection * (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? runSpeed : moveSpeed);
        finalMove.y = verticalVelocity;

        // Move the character controller
        characterController.Move(finalMove * Time.deltaTime);

        // Update isWalking and isRunning based on input
        bool isWalking = finalMove.magnitude > 0f &&
                  !(Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) &&
                  (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);

        bool isRunning = finalMove.magnitude > 0f &&
                         (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
                         (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);

        // Set animator parameters based on the current animation state
        playerAnimator.SetBool("walking_rifle", isWalking);
        playerAnimator.SetBool("running_rifle", isRunning);

        if (isWalking && playerController.isShooting)
        {
            isWalkingShooting = true;
        }
        else
        {
            isWalkingShooting = false;
        }

        if (isRunning && playerController.isShooting)
        {
            isRunningShooting = true;
        }
        else
        {
            isRunningShooting = false;
        }

        if(!isWalking && !isRunning && !isRunningShooting && !isWalkingShooting && playerController.isShooting)
        {
            playerAnimator.SetBool("idle_rifle_fire", true);
        }
        else
        {
            playerAnimator.SetBool("idle_rifle_fire", false);
        }

        if (!isWalking && !isRunning && !isRunningShooting && !isWalkingShooting && !playerController.isShooting)
        {
            playerAnimator.SetBool("idle_rifle", true);
        }
        else
        {
            playerAnimator.SetBool("idle_rifle", false);
        }

        playerAnimator.SetBool("walking_rifle_fire", isWalkingShooting);
        playerAnimator.SetBool("running_rifle_fire", isRunningShooting);
    }
}