using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will replace the cursor with a new texture you provide when the game starts
/// </summary>
public class CursorChanger : MonoBehaviour
{
    [Header("Settings:")]
    [Tooltip("The cursor to change to")]
    public Texture2D newCursorSprite;

    /// <summary>
    /// Description:
    /// Standard Unity function called once when the script is first loaded and before Update is called
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void Start()
    {
        ChangeCursor();
    }

    /// <summary>
    /// Description:
    /// Changes the cursor to the one set in the inspector
    /// Inputs:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    void ChangeCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;

        // The location that clicking actually hits, also positions the clicker
        Vector2 hotSpot = new Vector2();
        // Dividing the width and height by 2 will center it
        hotSpot.x = newCursorSprite.width / 2;
        hotSpot.y = newCursorSprite.height / 2;

        Cursor.SetCursor(newCursorSprite, hotSpot, CursorMode.Auto);
    }
}
