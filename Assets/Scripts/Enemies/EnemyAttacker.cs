using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which handles enemy attacks
/// </summary>
public abstract class EnemyAttacker : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("The amount of time needed to complete an attack.")]
    public float attackDuration = 0.5f;
    [Tooltip("The minimum amount of time between attacks.")]
    public float cooldownDuration = 1.0f;
    // Whether or not the enemy can attack
    private bool canAttack = true;

    /// <summary>
    /// Description:
    /// Determines whether an attack is allowed currently
    /// Inputs: N/A
    /// Outputs: bool
    /// </summary>
    /// <returns>Whether or not an attack is possible currently</returns>
    protected virtual bool AttackPossible()
    {
        return canAttack;
    }

    /// <summary>
    /// Description:
    /// Function to be called by other scripts to start an attack if possible
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    public void Attack(Vector3 position)
    {
        if (AttackPossible())
        {
            StartCoroutine("PerformAttack", position);
        }
    }

    /// <summary>
    /// Description:
    /// Coroutine which actually performs an attack.
    /// Inputs: Vector3 position
    /// Outputs: IEnumerator
    /// </summary>
    /// <param name="position">The position to attack (unused here)</param>
    /// <returns>Coroutine</returns>
    protected virtual IEnumerator PerformAttack(Vector3 position)
    {
        OnAttackStart();
        yield return null;
        Debug.Log("Attack Made");
        OnAttackEnd();
    }

    /// <summary>
    /// Description:
    /// Coroutine which handles the cooldown on this enemy's attacks
    /// Inputs: N/A
    /// Outputs: IEnumerator
    /// </summary>
    /// <returns>Coroutine</returns>
    protected IEnumerator Cooldown()
    {
        float t = 0;
        while (t < cooldownDuration)
        {
            yield return null;
            t += Time.deltaTime;
        }
        canAttack = true;
    }

    /// <summary>
    /// Description:
    /// Called when an attack is started, and prevents multiple attacks at once
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    protected void OnAttackStart()
    {
        canAttack = false;
    }

    /// <summary>
    /// Description:
    /// Called when an attack finishes, starts the cooldown
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    protected void OnAttackEnd()
    {
        StartCoroutine("Cooldown");
    }
}
