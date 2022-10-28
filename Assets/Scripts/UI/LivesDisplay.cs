using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : UIelement
{
    [Tooltip("The test UI for display")]
    public Text displayText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateUI();

        DisplayLives();
    }


    public void DisplayLives()
    {
        try
        {
            GameObject player = GameObject.FindWithTag("Player");
            Health health = player.GetComponent<Health>();
            if (displayText != null && health != null)
            {
                displayText.text = "Lives: " + health.currentLives.ToString();
            }
        }
        catch
        {
            displayText.text = "Lives: 0";
        }
        
    }
}
