using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the bounce force of a jump pad that activates when the player's feet hit it
/// </summary>
public class JumpPad : MonoBehaviour
{
    [Header("Bounce Settings")]
    [Tooltip("The force multiplyer to bounce the player by when jump is not held")]
    public float regularBounceForceMultiplyer = 2f;
    [Tooltip("The force multiplyer to bounce the player by when jump is held")]
    public float jumpHeldBounceForceMultiplyer = 3f;

    [Header("Effects")]
    [Tooltip("The effect to create when the player uses the jump pad")]
    public GameObject jumpPadEffect;

    [Header("Animation")]
    public Animator jumpPadAnimator;

    /// <summary>
    /// Description:
    /// Standard Unity function that is called when a collider enters a trigger
    /// Input:
    /// Collision collision
    /// Return:
    /// void (no return)
    /// </summary>
    /// <param name="collision">The collision information of what has collided with the attached collider</param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Feet")
        {
            BouncePlayer();
        }
    }

    /// <summary>
    /// Description:
    /// Tells the player controller attatched to the player object collided with to bounce
    /// Inputs:
    /// none
    /// Returns:
    /// void
    /// </summary>
    private void BouncePlayer()
    {
        ThirdPersonCharacterController playerController = GameManager.instance.player.GetComponentInChildren<ThirdPersonCharacterController>();
        if (playerController != null)
        {
            playerController.ResetJumps();
            playerController.Bounce(regularBounceForceMultiplyer, jumpHeldBounceForceMultiplyer);
            if (jumpPadEffect != null)
            {
                Instantiate(jumpPadEffect, transform.position, transform.rotation, null);
            }
            if (jumpPadAnimator != null)
            {
                jumpPadAnimator.SetTrigger("Bounce");
            }
        }
    }
}
