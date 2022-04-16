using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class inherits from the Pickup class and will heal the player when picked up
/// </summary>
public class OxygenPickup : Pickup
{
    [Header("Oxygen Settings")]
    [Tooltip("The Oxygen to apply")]
    public int oxygenAmount = 30;

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
        if (collision.tag == "Player" && collision.gameObject.GetComponent<Oxygen>() != null)
        {
            Oxygen playerOxygen = collision.gameObject.GetComponent<Oxygen>();
            playerOxygen.AddOxygen(oxygenAmount);
        }
        base.DoOnPickup(collision);
    }
}
