using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is a base class meant to be inherited from so multiple differetn UpdateUI functions can be called
/// to handle various cases
/// </summary>
public class UIelement : MonoBehaviour
{
    /// <summary>
    /// Description:
    /// Updates the UI elements UI accordingly
    /// This is a "virtual" function so it can be overridden by classes that inherit from the UIelement class
    /// Inputs:
    /// None
    /// Retuns:
    /// void (no return)
    /// </summary>
    public virtual void UpdateUI()
    {

    }
}
