using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which applies a force to any rigidbody that touches it.
/// </summary>
public class ForceField : MonoBehaviour
{
    [Tooltip("The direction that this force field applies a force in. (this vector will be normalized in code)")]
    [SerializeField] private Vector3 _forceDirection = Vector3.zero;
    [Tooltip("The magnitude of the force to be applied.")]
    public float forceMagnitude = 1.0f;
    [Tooltip("The mode in which the force will be applied")]
    public ForceMode forceMode = ForceMode.Force;
    /// <summary>
    /// Description:
    /// Accessor for the force direction of this force field, returns the normalized _force Direction
    /// </summary>
    public Vector3 forceDirection
    {
        get
        {
            return _forceDirection.normalized;
        }
        set
        {
            _forceDirection = value;
        }
    }

    /// <summary>
    /// Description:
    /// When drawing gizmos and this object is selected, indicate the direction and magnitude of the forces applied by this component
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + GetTotalForce());
    }

    /// <summary>
    /// Description:
    /// Collision function which causes forces to be applied
    /// Inputs: Collision collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The data pertaining to this collision</param>
    private void OnCollisionStay(Collision collision)
    {
        ApplyForceToGameObject(collision.gameObject);
    }

    /// <summary>
    /// Description:
    /// Collision function which causes forces to be applied
    /// Inputs: Collider other
    /// Outputs: N/A
    /// </summary>
    /// <param name="other">The collider causing the collision</param>
    private void OnTriggerStay(Collider other)
    {
        ApplyForceToGameObject(other.gameObject);
    }

    /// <summary>
    /// Description: 
    /// Collision function which causes forces to be applied
    /// Inputs: ControllerColliderHit hit
    /// Outputs: N/A
    /// </summary>
    /// <param name="hit">Collsion data</param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ApplyForceToGameObject(hit.gameObject);
    }

    /// <summary>
    /// Description:
    /// Applies forces to a gameobject if it has a rigidbody
    /// Inputs: GameObject target
    /// Outputs: N/A
    /// </summary>
    /// <param name="target">The game object to apply forces to</param>
    private void ApplyForceToGameObject(GameObject target)
    {
        Vector3 totalForce = GetTotalForce();
        Rigidbody rigidbody = target.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.AddForce(totalForce, forceMode);
        }
    }

    /// <summary>
    /// Description:
    /// Calculates and returns the total force applied by this component
    /// Inputs: N/A
    /// Outputs: Vector3
    /// </summary>
    /// <returns>The total force to apply from this component, as a vector with direction and magnitude</returns>
    private Vector3 GetTotalForce()
    {
        return forceDirection * forceMagnitude;
    }
}
