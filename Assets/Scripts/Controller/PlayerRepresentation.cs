using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles moving the player's representation which is detached from the movement controls
/// to make the math related to movement easier.
/// 
/// This script will make the player's representation (model) face in the direction the player controller is moving
/// </summary>
public class PlayerRepresentation : MonoBehaviour {

    // The input manager to read input from
    InputManager inputManager;

    [Header("Needed References")]
    [Tooltip("The player gameobject that has the third person character controller attached to it")]
    public ThirdPersonCharacterController playerCharacterController;
    [Tooltip("The animator that animates the visual representation of the player")]
    public Animator representationAnimator;

    // Use this for initialization
    void Start() {
        inputManager = InputManager.instance;
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once per frame
    /// Inputs:
    /// none
    /// Retuns:
    /// void
    /// </summary>
    void Update() {
        FollowTransformLikeParent();
        CentralizedControl(inputManager.horizontalMoveAxis, inputManager.verticalMoveAxis);
        ApplyAnimation();
    }

    /// <summary>
    /// Description:
    /// Handles swithing between control styles if more than one is coded in
    /// Inputs:
    /// float leftCharacterMovement | float rightCharacterMovement
    /// Returns:
    /// void
    /// </summary>
    /// <param name="leftCharacterMovement">The movement input along the horizontal</param>
    /// <param name="rightCharacterMovement">The movement input along the vertical</param>
    void CentralizedControl(float leftCharacterMovement, float rightCharacterMovement)
    {
        if (playerCharacterController.playerState == ThirdPersonCharacterController.PlayerState.Dead)
        {
            // Do nothing when the player is dead
        }
        else
        {
            NormalControl(leftCharacterMovement, rightCharacterMovement);
        }
    }

    /// <summary>
    /// Description:
    /// Handles motion of the player representation under the average or normal use case
    /// Inputs:
    /// float leftCharacterMovement | float rightCharacterMovement
    /// Returns:
    /// void
    /// </summary>
    /// <param name="leftCharacterMovement">The movement input along the horizontal</param>
    /// <param name="rightCharacterMovement">The movement input along the vertical</param>
    void NormalControl(float leftCharacterMovement, float rightCharacterMovement)
    {
        Vector3 movementDirection = new Vector3(leftCharacterMovement, 0, rightCharacterMovement);
        if (movementDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            rotation = Quaternion.Euler(rotation.eulerAngles.x, playerCharacterController.transform.rotation.eulerAngles.y + rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;
        }
    }

    [Tooltip("A 'Parent' object to follow as if this was a child without rotation following")]
    public Transform parent;

    [Header("Following Settings")]
    [Tooltip("An offeset to follow at")]
    public Vector3 offset;

    /// <summary>
    /// Description:
    /// Follows a transform as if it were the parent except it does not follow the rotation
    /// Input:
    /// none
    /// Return:
    /// void
    /// </summary>
    public void FollowTransformLikeParent()
    {
        this.gameObject.transform.position = parent.position + offset;
    }

    /// <summary>
    /// Description:
    /// Attempts to apply animation to the animator according to the player controllers state
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void ApplyAnimation()
    {
        if (representationAnimator == null)
        {
            return;
        }

        // Get the player state
        ThirdPersonCharacterController.PlayerState currentPlayerState = playerCharacterController.playerState;

        // Idle
        if (currentPlayerState == ThirdPersonCharacterController.PlayerState.Idle)
        {
            representationAnimator.SetBool("Idle", true);
        }
        else
        {
            representationAnimator.SetBool("Idle", false);
        }

        // Jumping
        if (currentPlayerState == ThirdPersonCharacterController.PlayerState.Jumping)
        {
            representationAnimator.SetBool("Jumping", true);
        }
        else
        {
            representationAnimator.SetBool("Jumping", false);
        }

        // Falling
        if (currentPlayerState == ThirdPersonCharacterController.PlayerState.Falling)
        {
            representationAnimator.SetBool("Falling", true);
        }
        else
        {
            representationAnimator.SetBool("Falling", false);
        }

        // Double Jumping
        if (currentPlayerState == ThirdPersonCharacterController.PlayerState.DoubleJumping)
        {
            representationAnimator.SetBool("DoubleJumping", true);
        }
        else
        {
            representationAnimator.SetBool("DoubleJumping", false);
        }

        // Running
        if (currentPlayerState == ThirdPersonCharacterController.PlayerState.Moving)
        {
            representationAnimator.SetBool("Running", true);
        }
        else
        {
            representationAnimator.SetBool("Running", false);
        }

        // Dead
        if (currentPlayerState == ThirdPersonCharacterController.PlayerState.Dead)
        {
            representationAnimator.SetBool("Dead", true);
        }
        else
        {
            representationAnimator.SetBool("Dead", false);
        }
    }
}
