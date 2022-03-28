using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a pickup which marks the goal as complete
/// </summary>
public class GoalPickup : Pickup
{
    /// <summary>
    /// Description:
    /// When picked up, marks the goal as complete
    /// Inputs: Collider collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The collider that caused this to be picked up</param>
    public override void DoOnPickup(Collider collision)
    {
        if (collision.tag == "Player")
        {
            GoalManager.CompleteGoal();
        }
        base.DoOnPickup(collision);
    }
}
