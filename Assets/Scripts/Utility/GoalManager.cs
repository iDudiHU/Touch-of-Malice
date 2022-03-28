using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class which tracks when the player has acheived their objective
/// </summary>
public static class GoalManager
{
    // Boolean which tracks if the objective has been completed
    public static bool goalAcheived = false;

    /// <summary>
    /// Description:
    /// Marks the goal as acheived
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    public static void CompleteGoal()
    {
        goalAcheived = true;
        if (GameManager.instance != null)
        {
            GameManager.instance.LevelCleared();
        }
    }

    /// <summary>
    /// Description:
    /// Resets goal completion (meant to be done on player death)
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    public static void ResetGoal()
    {
        goalAcheived = false;
    }
}
