using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which represents the capability of an enemy to see and hear a target
/// </summary>
public class EnemyAwareness : MonoBehaviour
{
    [Tooltip("The target to follow/look for")]
    public Transform target = null;
    [Tooltip("The position that the enemy expects to find the player at")]
    public Vector3 expectedPosition = Vector3.zero;
    [Tooltip("The angle of the vision cone of this enemy")]
    public float sightAngleRadius = 90.0f;
    [Tooltip("The maximum distance at which the enemy can spot the target")]
    public float sightDistance = 20.0f;
    [Tooltip("The maximum distance at which the enemy can hear the target moving")]
    public float hearingDistance = 10.0f;
    [Tooltip("The maximum speed at which the target can move within the hearing radius and avoid detection")]
    public float hearingSpeedThreshold = 5.0f;
    // The last position at which the target was heard
    private Vector3 lastHeardPosition = Vector3.zero;
    // The last time that the target was heard
    private float lastHeardTime = Mathf.NegativeInfinity;
    [Tooltip("A measure of how certain the enemy is of the target's presence and position")]
    [Range(0.0f, 1.0f)] public float certaintyOfPlayer = 0.0f;
    [Tooltip("The certainty threshold at which the enemy begins following it's senses")]
    public float followThreshold = 0.2f;
    [Tooltip("The certainty threshold at which the enemy is considered to have detected the target")]
    public float detectionThreshold = 0.5f;
    [Tooltip("The rate at which the enemy's certainty of the target's position and presence decays")]
    public float awarenessDecayRate = 2.0f;
    [Tooltip("The position that the enemy will return to if it loses track of it's target")]
    public Transform returnPositionTransform = null;
    // The position to follow currently
    public Vector3 followPosition
    {
        get
        {
            if (certaintyOfPlayer > followThreshold)
            {
                return expectedPosition;
            }
            else
            {
                if (returnPositionTransform != null)
                {
                    return returnPositionTransform.position;
                }
                return transform.position;
            }
        }
    }
    // Whether the enemy sees the player and has detected them
    public bool seesPlayer
    {
        get
        {
            return certaintyOfPlayer > detectionThreshold && CheckLineOfSight();
        }
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first call to Update
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void Start()
    {
        if (target == null && GameManager.instance != null)
        {
            target = GameManager.instance.player.transform;
        }
        else if (target == null)
        {
            target = FindObjectOfType<ThirdPersonCharacterController>().gameObject.transform;
        }
    }

    /// <summary>
    /// Description:
    /// Every update, check for the presence of the target
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    private void Update()
    {
        CheckForPlayer();
    }

    /// <summary>
    /// Description:
    /// Checks for the presence of the target
    /// Inputs: N/A
    /// Outputs: N/A
    /// </summary>
    public void CheckForPlayer()
    {
        float visionCertainty = GetVisionCertainty();
        float hearingCertainty = GetHearingCertainty();
        float totalCertainty = visionCertainty + hearingCertainty;
        if (totalCertainty == 0)
        {
            totalCertainty = -awarenessDecayRate * Time.deltaTime;
        }

        AddCertainty(totalCertainty);

        if (certaintyOfPlayer > followThreshold)
        {
            expectedPosition = target.position;
        }
    }

    /// <summary>
    /// Description:
    /// Adds a value to the certainty of this enemy that it knows where the target is
    /// Inputs: float amount
    /// Outputs: N/A
    /// </summary>
    /// <param name="amount">The amount to add</param>
    public void AddCertainty(float amount)
    {
        certaintyOfPlayer = Mathf.Clamp(certaintyOfPlayer + amount, 0, 1);
    }

    /// <summary>
    /// Description:
    /// Determines the change in certainty this frame due to the enemy's vision
    /// Inputs: N/A
    /// Outputs: float
    /// </summary>
    /// <returns>The change in certainty due to vision</returns>
    public float GetVisionCertainty()
    {
        if (CheckLineOfSight() && CheckVisionAngle())
        {
            return GetDistanceToTarget() / sightDistance * Time.deltaTime;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Description:
    /// Determines whether the enemy has line of sight to the target
    /// Inputs: N/A
    /// Outputs: bool
    /// </summary>
    /// <returns>Whether or not this enemy can see it's target</returns>
    public bool CheckLineOfSight()
    {
        Ray ray = new Ray(transform.position, target.position - transform.position);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray.origin, ray.direction, out hit, sightDistance))
        {
            if (hit.transform.IsChildOf(target))
            {
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Description:
    /// Determines the change in certainty this frame due to the enemy's hearing capabilities
    /// Inputs: N/A
    /// Outputs: float
    /// </summary>
    /// <returns>The change in certainty due to hearing the target</returns>
    public float GetHearingCertainty()
    {
        if (GetDistanceToTarget() < hearingDistance)
        {
            float speed = Mathf.Abs((target.position - lastHeardPosition).magnitude * Mathf.Pow(lastHeardTime - Time.timeSinceLevelLoad, -1.0f));
            float obstructionModifier = 1.0f;
            if (!CheckLineOfSight())
            {
                obstructionModifier = 0.25f;
            }
            lastHeardTime = Time.timeSinceLevelLoad;
            lastHeardPosition = target.position;
            if (speed > hearingSpeedThreshold)
            {
                return speed * obstructionModifier * Mathf.Pow((hearingDistance / GetDistanceToTarget()), 1) * Time.deltaTime;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Description:
    /// Checks if the target is within the enemy's frame of vision
    /// Inputs: N/A
    /// Outputs: bool
    /// </summary>
    /// <returns>Whether the enemy would be see it's target if there were no obstructions</returns>
    public bool CheckVisionAngle()
    {
        float angle = Vector3.Angle(transform.forward, target.position - transform.position);
        if (angle < sightAngleRadius)
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
    /// Determines the distance to the target
    /// Inputs: N/A
    /// Outputs: float
    /// </summary>
    /// <returns>The distance to the target</returns>
    public float GetDistanceToTarget()
    {
        return (target.position - transform.position).magnitude;
    }
}
