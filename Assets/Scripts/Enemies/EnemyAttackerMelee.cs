using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which allows an enemy to make melee attacks
/// </summary>
public class EnemyAttackerMelee : EnemyAttacker
{
    [Header("Melee Settings")]
    [Tooltip("The list of colliders to turn on/off when making melee attacks")]
    public List<Collider> weaponColliders = new List<Collider>();

    /// <summary>
    /// Description:
    /// Coroutine which causes this script to enable colliders on weapons for the duration of an attack.
    /// Inputs: Vector3 position
    /// Outputs: IEnumerator
    /// </summary>
    /// <param name="position">The position to attack (unused here)</param>
    /// <returns>Coroutine</returns>
    protected override IEnumerator PerformAttack(Vector3 position)
    {
        OnAttackStart();
        SetWeaponColliders(true);
        float t = 0;
        while (t < attackDuration)
        {
            yield return null;
            t += Time.deltaTime;
        }
        SetWeaponColliders(false);
        OnAttackEnd();
    }

    /// <summary>
    /// Description:
    /// Turns on/off weapon colliders based on the activation parameter
    /// Inputs: bool activation
    /// Outputs: N/A
    /// </summary>
    /// <param name="activation">Whether to turn colliders on, or to turn them off</param>
    protected void SetWeaponColliders(bool activation)
    {
        foreach (Collider c in weaponColliders)
        {
            if (c != null)
            {
                c.enabled = activation;
            }
        }
    }
}
