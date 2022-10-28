using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class moves the attached object in the direction specified
/// </summary>
public class DirectionalMover : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The direction to move in")]
    public Vector3 direction = Vector3.down;
    [Tooltip("The speed to move at")]
    public float speed = 5.0f;

    /// <summary>
    /// Description:
    /// Standard Unity function called every frame
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Description:
    /// Moves this object the speed and in the direction specified
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void Move()
    {
        transform.position = transform.position + direction.normalized * speed * Time.deltaTime;
    }
}
