using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Handles management of the cursor and its state
/// </summary>
public class CursorManager : MonoBehaviour {

    // An enum that defines the cursor state, used when setting the cursor state to be different values
    public enum CursorState { InGame, Menu };

    [Header("Settings")]
    [Tooltip("The state to start the cursor in in this scene")]
    public CursorState startState = CursorState.InGame;

    // An instance of this to be referenced by other scripts
    public static CursorManager instance;

    /// <summary>
    /// Description:
    /// Standard Unity function which is called when the script is loaded in
    /// Input:
    /// none
    /// Return:
    /// void
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ChangeCursorMode(startState);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Description:
    /// Changes cursor mode to match the desired state
    /// Input:
    /// CursorState cursorState
    /// Returns:
    /// void
    /// </summary>
    /// <param name="cursorState"></param>

    public void ChangeCursorMode(CursorState cursorState)
    {
        if (cursorState == CursorState.InGame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (cursorState == CursorState.Menu)
        {

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

    }
}
