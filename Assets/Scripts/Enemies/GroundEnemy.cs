using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class which derives from the Enemy base class. Handles ground enemy movement
/// </summary>
public class GroundEnemy : Enemy
{
    [Header("Ground Enemy Settings")]
    [Tooltip("The nav mesh agent used to move this enemy")]
    public NavMeshAgent agent = null;
    [Tooltip("The distance at which to stop before reaching the objective")]
    public float stopDistance = 2.0f;
    [Tooltip("Whether this enemy can stop if it is within it's stop distance but does not have line of sight to it's target")]
    public bool lineOfSightToStop = true;
    [Tooltip("Whether this enemy should always face the player, or face in the direction it is moving")]
    public bool alwaysFacePlayer = true;

    /// <summary>
    /// Description:
    /// Overrides base function setup, sets up nav mesh agent reference
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    protected override void Setup()
    {
        base.Setup();
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if (agent != null)
        {
            agent.updateRotation = !alwaysFacePlayer;
        }
    }

    /// <summary>
    /// Description:
    /// Handles movement for this enemy
    /// Rotates the enemy to face it's target and sets navmesh destination
    /// Inputs:
    /// none
    /// Outputs:
    /// void
    /// </summary>
    protected override void HandleMovement()
    {
        if (enemyRigidbody != null)
        {
            enemyRigidbody.velocity = Vector3.zero;
            enemyRigidbody.angularVelocity = Vector3.zero;
        }

        Quaternion desiredRotation = CalculateDesiredRotation();
        transform.rotation = desiredRotation;

        /// <summary>
        /// Description:
        /// Small function which determines whether or not this enemy should move
        /// Inputs: N/A
        /// Outputs: bool
        /// </summary>
        /// <returns>bool: whether or not this enemy should move</returns>
        bool ShouldMove()
        {
            if (agent != null && target != null && canMove)
            {
                if ((target - transform.position).magnitude > stopDistance)
                {
                    return true;
                }
                else if (awareness != null && lineOfSightToStop && !awareness.CheckLineOfSight())
                {
                    return true;
                }
            }
            return false;
        }

        if (ShouldMove())
        {
            agent.SetDestination(target);
        }
        else if (agent != null)
        {
            agent.SetDestination(transform.position);
        }
    }

    /// <summary>
    /// Description:
    /// Calculates the movement that this enemy is/will make this frame.
    /// Inputs: N/A
    /// Outputs: Vector3
    /// </summary>
    /// <returns>The desired movement of this enemy</returns>
    protected override Vector3 CalculateDesiredMovement()
    {
        if (agent != null)
        {
            return agent.desiredVelocity * Time.deltaTime;
        }
        return base.CalculateDesiredMovement();
    }

    /// <summary>
    /// Description:
    /// Caclulates the desired rotation of this enemy this frame
    /// Inputs: N/A
    /// Outputs: Quaternion
    /// </summary>
    /// <returns>The desired rotation of the enemy</returns>
    protected override Quaternion CalculateDesiredRotation()
    {
        if (alwaysFacePlayer)
        {
            if (awareness != null && awareness.certaintyOfPlayer > awareness.followThreshold)
            {
                Vector3 toTarget = target - transform.position;
                toTarget.y = 0;
                Quaternion result = Quaternion.LookRotation(toTarget, transform.up);
                return result;
            }
            else
            {
                return transform.rotation;
            }
        }
        return base.CalculateDesiredRotation();
    }
}
