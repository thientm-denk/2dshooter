using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesDisplay : UIelement
{
    [Tooltip("Text display")]
    public Text displayText;


    

    // Update is called once per frame
    void Update()
    {
        base.UpdateUI();

        DisplayScore();
    }

    public void DisplayScore()
    {
        if (displayText != null)
        {
            displayText.text = "Enemies: " + GameManager.CountEnemies().ToString();
        }
    }
}
