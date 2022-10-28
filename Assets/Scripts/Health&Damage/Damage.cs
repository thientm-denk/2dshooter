using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the dealing of damage to health components.
/// </summary>
public class Damage : MonoBehaviour
{
    [Header("Team Settings")]
    [Tooltip("The team associated with this damage")]
    public int teamId = 0;

    [Header("Damage Settings")]
    [Tooltip("How much damage to deal")]
    public int damageAmount = 1;
    [Tooltip("Prefab to spawn after doing damage")]
    public GameObject hitEffect = null;
    [Tooltip("Whether or not to destroy the attached game object after dealing damage")]
    public bool destroyAfterDamage = true;
    [Tooltip("Whether or not to apply damage when triggers collide")]
    public bool dealDamageOnTriggerEnter = false;
    [Tooltip("Whether or not to apply damage when triggers stay, for damage over time")]
    public bool dealDamageOnTriggerStay = false;
    [Tooltip("Whether or not to apply damage on non-trigger collider collisions")]
    public bool dealDamageOnCollision = false;

    /// <summary>
    /// Description: 
    /// Standard Unity function called whenever a Collider2D enters any attached 2D trigger collider
    /// Inputs:
    /// Collider2D collision
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="collision">The Collider2D that set of the function call</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dealDamageOnTriggerEnter)
        {
            DealDamage(collision.gameObject);
        }
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called every frame a Collider2D stays in any attached 2D trigger collider
    /// Inputs:
    /// Collider2D collision
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="collision">The Collider2D that set of the function call</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (dealDamageOnTriggerStay)
        {
            DealDamage(collision.gameObject);
        }
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called when a Collider2D hits another Collider2D (non-triggers)
    /// Inputs:
    /// Collision2D collision
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="collision">The Collision2D that set of the function call</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dealDamageOnCollision)
        {
            DealDamage(collision.gameObject);
        }
    }

    /// <summary>
    /// Description:
    /// This function deals damage to a health component if the collided 
    /// with gameobject has a health component attached AND it is on a different team.
    /// Inputs:
    /// GameObject collisionGameObject
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="collisionGameObject">The game object that has been collided with</param>
    private void DealDamage(GameObject collisionGameObject)
    {
        Health collidedHealth = collisionGameObject.GetComponent<Health>();
        if (collidedHealth != null)
        {
            if (collidedHealth.teamId != this.teamId)
            {
                collidedHealth.TakeDamage(damageAmount);
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, transform.rotation, null);
                }
                if (destroyAfterDamage)
                {
                    if (gameObject.GetComponent<Enemy>() != null)
                    {
                        gameObject.GetComponent<Enemy>().DoBeforeDestroy();
                    }
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
