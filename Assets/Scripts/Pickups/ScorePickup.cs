using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which represents a score pickup
/// </summary>
public class ScorePickup : Pickup
{
    [Header("Score Pickup Settings")]
    [Tooltip("The amount of score gained when picked up.")]
    public int scoreAmount = 1;

    /// <summary>
    /// Description:
    /// When picked up, add score to the player via the game manager
    /// Inputs: Collider collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The collider which caused this to be picked up</param>
    public override void DoOnPickup(Collider collision)
    {
        if (collision.tag == "Player" && GameManager.instance != null)
        {
            GameManager.AddScore(scoreAmount);
        }
        base.DoOnPickup(collision);
    }
}
