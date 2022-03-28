using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which calls unity events on collisions
/// </summary>
public class CollisionEventCaller : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The event to call on collisions")]
    public UnityEvent collisionEvent = new UnityEvent();
    [Tooltip("The tag needed to activate the collision event")]
    public string requiredTag = ""; // a value of "" makes all tags valid including a lack of a tag
    [Tooltip("The layers on which gameobjects will activate the collision event")]
    public LayerMask requiredLayers = -1; // setting it to -1 makes all layers valid by default

    /// <summary>
    /// Description:
    /// 3D collision function, tries to call the collision event
    /// Inputs: Collision collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The collision data</param>
    private void OnCollisionEnter(Collision collision)
    {
        TryCallEvent(collision.gameObject);
    }

    /// <summary>
    /// Description:
    /// 2D collision function, tries to call the collision event
    /// Inputs: Collision2D collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The collision data</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryCallEvent(collision.gameObject);
    }

    /// <summary>
    /// Description:
    /// 3D trigger function, tries to call the collision event
    /// Inputs: Collider other
    /// Outputs: N/A
    /// </summary>
    /// <param name="other">The collider that caused the trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        TryCallEvent(other.gameObject);
    }

    /// <summary>
    /// Description:
    /// 2D trigger function, triees to call the collision event
    /// Inputs: Collider2D collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The collider that caused the trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryCallEvent(collision.gameObject);
    }

    /// <summary>
    /// Description:
    /// Tests if a layer is valid for the purposes of the collision event
    /// Inputs: int layer
    /// Outputs: bool
    /// </summary>
    /// <param name="layer">The layer to test</param>
    /// <returns>Whether or not the layer is valid</returns>
    private bool TestLayer(int layer)
    {
        if ((LayerMask.GetMask(LayerMask.LayerToName(layer)) & requiredLayers.value) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Description:
    /// Tests if a tag is valid for the purposes of the collision event
    /// Inputs: string tag
    /// Outputs: bool
    /// </summary>
    /// <param name="tag">The tag to test</param>
    /// <returns>Whether the tag is valid</returns>
    private bool TestTag(string tag)
    {
        if (requiredTag != "")
        {
            return tag == requiredTag;
        }
        return true;
    }

    /// <summary>
    /// Description:
    /// Tests if the paramter game object is valid, and if it is, calls the collision event
    /// Inputs: GameObject caller
    /// Outputs: N/A
    /// </summary>
    /// <param name="caller">The gameobject that caused a collision or trigger</param>
    private void TryCallEvent(GameObject caller)
    {
        if (TestLayer(caller.layer) && TestTag(caller.tag))
        {
            collisionEvent.Invoke();
        }
    }
}
