﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class inherits from the Pickup class and will heal the player when picked up
/// </summary>
public class HealthPickup : Pickup
{
    [Header("Healing Settings")]
    [Tooltip("The healing to apply")]
    public int healingAmount = 1;

    /// <summary>
    /// Description:
    /// Function called when this pickup is picked up
    /// Heals the health attatched to the collider that picks this up
    /// Inputs: Collider2D collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The collider that is picking up this pickup</param>
    public override void DoOnPickup(Collider collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<Health>() != null)
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            playerHealth.ReceiveHealing(healingAmount);
        }
        base.DoOnPickup(collision);
    }
}
