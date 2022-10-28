using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class uses the game manager's reset score function to reset the score
/// It is meant to be used on buttons that launch a new game from any level
/// </summary>
public class ScoreReseter : MonoBehaviour
{

    /// <summary>
    /// Description:
    /// Calls the GameManger Reset Score function to reset the score player preference data
    /// Input:
    /// none
    /// Return:
    /// void
    /// </summary>
    public void ResetScore()
    {
        GameManager.ResetScore();
    }
}
