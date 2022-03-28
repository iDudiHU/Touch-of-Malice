using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for enemies
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The base movement speed of the enemy.")]
    public float moveSpeed = 2.0f;
    [Tooltip("Whether the enemy can move.")]
    public bool canMove = true;
    [Tooltip("The component that tells this one where to move to")]
    public EnemyAwareness awareness = null;
    // Whether the enemy is in the middle of performing an action, such as an attack.
    protected bool isActing = false;
    // Whether this enemy is preparing to attack/explode/do an action/etc.
    protected bool isPreparing = false;
    // The rigidbody to be used to move this enemy
    protected Rigidbody enemyRigidbody = null;

    [Header("Behavior Settings")]
    [Tooltip("The shooter component that this enemy will use to attack, if it attacks")]
    public EnemyAttacker attacker = null;
    //[Tooltip("The target to move, aim, and/or shoot at.")]
    public Vector3 target
    {
        get
        {
            if (awareness != null)
            {
                return awareness.followPosition;
            }
            else
            {
                return transform.position;
            }
        }
    }
    [Tooltip("The maximum distance from the target that this enemy will begin to attack")]
    public float maximumAttackRange = 5.0f;
    [Tooltip("Whether or not this enemy will fire a gun if it has one.")]
    public bool doesAttack = false;
    [Tooltip("Whether this enemy requires line of sight to it's target to take action against it")]
    public bool lineOfSightToAttack = true;

    /// <summary>
    /// Enum to help track the movement state of this enemy
    /// </summary>
    public enum MovementStates
    {
        Idle, Moving
    }
    [Tooltip("The movement status of this enemy currently (used to help animate)")]
    public MovementStates movementState = MovementStates.Idle;
    /// <summary>
    /// Enum to help track the action state of this enemy
    /// </summary>
    public enum ActionStates
    {
        Idle, Preparing, Attacking
    }
    [Tooltip("The state of this enemy with respect to actions it can perform.")]
    public ActionStates actionState = ActionStates.Idle;

    /// <summary>
    /// Description:
    /// When this script starts up, set it up
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    private void Start()
    {
        Setup();
    }

    /// <summary>
    /// Description:
    /// Every frame, determine the correct movement, rotation, actions, etc. and cause the enemy to act accordingly
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    private void Update()
    {
        HandleMovement();
        HandleActions();
    }

    /// <summary>
    /// Description:
    /// Sets up this enemy component, specifically the rigidbody used to move it.
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    protected virtual void Setup()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        if (attacker == null)
        {
            attacker = GetComponent<EnemyAttacker>();
        }
    }

    /// <summary>
    /// Description:
    /// Handles the desired movement of this enemy
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    protected virtual void HandleMovement()
    {
        if (canMove && enemyRigidbody != null)
        {
            Vector3 desiredMovement = CalculateDesiredMovement();
            Quaternion desiredRotation = CalculateDesiredRotation();

            enemyRigidbody.velocity = Vector3.zero;
            enemyRigidbody.angularVelocity = Vector3.zero;

            enemyRigidbody.MovePosition(desiredMovement);
            enemyRigidbody.MoveRotation(desiredRotation);
        }
    }

    /// <summary>
    /// Description:
    /// Handles the desired actions of this enemy
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    protected virtual void HandleActions()
    {
        TryToAttack();
    }

    /// <summary>
    /// Description:
    /// Tries to attack using references to an attacker script
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    protected virtual void TryToAttack()
    {
        if (doesAttack && attacker != null && target != null && (target - transform.position).magnitude < maximumAttackRange)
        {
            if (!lineOfSightToAttack || (awareness != null && lineOfSightToAttack && awareness.seesPlayer))
            {
                attacker.Attack(target);
            }
        }
    }

    /// <summary>
    /// Descriptions:
    /// Calculates the movement that this enemy should make.
    /// Inputs: N/A
    /// Outputs: Vector3
    /// </summary>
    /// <returns>Vector3: The desired movement of this enemy</returns>
    protected virtual Vector3 CalculateDesiredMovement()
    {
        return transform.position;
    }

    /// <summary>
    /// Description:
    /// Calculates the rotation that this enemy should rotate to
    /// Inputs: N/A
    /// Outputs: Quaternion
    /// </summary>
    /// <returns>Quaternion: The desired rotation of this enemy</returns>
    protected virtual Quaternion CalculateDesiredRotation()
    {
        return transform.rotation;
    }

    /// <summary>
    /// Description:
    /// Determines the state of this enemy and sets the state variable(s) accordingly
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    protected void SetState()
    {
        if (isActing)
        {
            actionState = ActionStates.Attacking;
        }
        else if (isPreparing)
        {
            actionState = ActionStates.Preparing;
        }
        else
        {
            actionState = ActionStates.Idle;
        }
        if (canMove && enemyRigidbody != null && enemyRigidbody.velocity.magnitude > 0.1f)
        {
            movementState = MovementStates.Moving;
        }
        else
        {
            movementState = MovementStates.Idle;
        }
    }
}
