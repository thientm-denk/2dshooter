using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which destroys it's gameobject after a certain amount of time
/// </summary>
public class TimedObjectDestroyer : MonoBehaviour
{
    [Tooltip("The lifetime of this gameobject")]
    public float lifetime = 5.0f;

    // The amount of time this gameobject has already existed in play mode
    private float timeAlive = 0.0f;

    [Tooltip("Whether to destroy child gameobjects when this gameobject is destroyed")]
    public bool destroyChildrenOnDeath = true;

    // Flag which tells whether the application is shutting down (helps avoid errors)
    public static bool quitting = false;

    /// <summary>
    /// Description:
    /// Standard Unity function called when the application quits
    /// 
    /// Ensures that the quitting flag gets set correctly to avoid work as the application quits
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void OnApplicationQuit()
    {
        quitting = true;
        DestroyImmediate(this.gameObject);
    }

    /// <summary>
    /// Description:
    /// Every frame, increment the amount of time that this gameobject has been alive,
    /// or if it has exceeded it's maximum lifetime, destroy it
    /// Inputs: none
    /// Returns: void (no return)
    /// </summary>
    void Update()
    {
        if (timeAlive > lifetime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            timeAlive += Time.deltaTime;
        }
    }

    /// <summary>
    /// Description:
    /// Behavior which triggers when this component is destroyed
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void OnDestroy()
    {
        if (destroyChildrenOnDeath && !quitting && Application.isPlaying)
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject childObject = transform.GetChild(i).gameObject;
                if (childObject != null)
                {
                    DestroyImmediate(childObject);
                }
            }
        }
        transform.DetachChildren();
    }
}
