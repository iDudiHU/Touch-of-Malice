﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class inherits from the Pickup class and will give the player extra lives when picked up
/// </summary>
public class ExtraLifePickup : Pickup
{
    [Header("Extra Life Settings")]
    [Tooltip("How many Lives to give")]
    public int extraLives = 1;

    /// <summary>
    /// Description:
    /// Function called when this pickup is picked up
    /// Gives the player that picks this up an extra life
    /// Inputs: Collider2D collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The collider that is picking up this pickup</param>
    public override void DoOnPickup(Collider collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<Health>() != null)
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            playerHealth.AddLives(extraLives);
        }
        base.DoOnPickup(collision);
    }
}
