/*
 *           ~~ Screenshot Utility ~~ 
 *  Takes a screenshot of the game window with its
 *  current resolution. Should work in the editor 
 *  or on any platform.
 *  
 *  Created by Brian Winn, Michigan State University
 *  Games for Entertainment and Learning (GEL) Lab
 * 
 *  Notes:
 *    - Images are stored in a Screenshots folder within the Unity project directory.
 * 
 *    - Images will be copied over if player prefs are reset!
 * 
 *    - If the resolution is 1024x768, and the scale factor
 *      is 2, the screenshot will be saved as 2048x1536.
 * 
 *    - The mouse is not captured in the screenshot.
 * 
 * Last Updated: January 1, 2021
 */

using UnityEngine;
using System.Collections;
using System.IO; // included for access to File IO such as Directory class
using UnityEngine.InputSystem; // included for using the new Unity Input System

/// <summary>
/// Handles taking a screenshot of the game.
/// </summary>
public class ScreenshotUtility : MonoBehaviour
{
    // static reference to ScreenshotUtility so can be called from other scripts directly (not just through gameobject component)
    public static ScreenshotUtility screenShotUtility;

    #region Public Variables
    [Header("Settings")]
    [Tooltip("Should the screenshot utility run only in the editor.")]
    public bool runOnlyInEditor = true;
    [Tooltip("What key is mapped to take the screenshot.")]
    public string m_ScreenshotKey = "c";
    [Tooltip("What is the scale factor for the screenshot. Standard is 1, 2x size is 2, etc..")]
    public int m_ScaleFactor = 1;
    [Tooltip("Include image size in filename.")]
    public bool includeImageSizeInFilename = true;
    #endregion

    [Header("Private Variables")]
    #region Private Variables
    // The number of screenshots taken
    [Tooltip("Use the Reset Counter contextual menu item to reset this.")]
    [SerializeField]
    private int m_ImageCount = 0;
    #endregion

    #region Constants
    // The key used to get/set the number of images
    private const string ImageCntKey = "IMAGE_CNT";
    #endregion

    /// <summary>
    /// This sets up the screenshot utility and allows it to persist through scenes.
    /// </summary>
    void Awake()
    {
        if (screenShotUtility != null)
        { // this gameobject must already have been setup in a previous scene, so just destroy this game object
            Destroy(this.gameObject);
        }
        else if (runOnlyInEditor && !Application.isEditor)
        { // chose not to work if this is running outside the editor so destroy it
            Destroy(this.gameObject);
        }
        else
        { // this is the first time we are setting up the screenshot utility
          // setup reference to ScreenshotUtility class
            screenShotUtility = this.GetComponent<ScreenshotUtility>();

            // keep this gameobject around as new scenes load
            DontDestroyOnLoad(gameObject);

            // get image count from player prefs for indexing of filename
            m_ImageCount = PlayerPrefs.GetInt(ImageCntKey);

            // if there is not a "Screenshots" directory in the Project folder, create one
            if (!Directory.Exists("Screenshots"))
            {
                Directory.CreateDirectory("Screenshots");
            }
        }
    }

    /// <summary>
    /// Called once per frame. Handles the input to do TakeScreenshot action.
    /// </summary>
    void Update()
    {
        // this is the way to check for input using the old Unity Input
        //    if (Input.GetKeyDown(m_ScreenshotKey.ToLower()))

        // But, we will use the new Unity Input System to check for input on the Keyboard
        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout(m_ScreenshotKey).wasPressedThisFrame)
        {
            TakeScreenshot();
        }
    }

    /// <summary>
    /// This function will reset the screenshot image counter used in the filename. The function is available through the context menu on the component in the inspector.
    /// </summary>
    [ContextMenu("Reset Counter")]
    public void ResetCounter()
    {
        // reset counter to 0
        m_ImageCount = 0;
        // set player prefs to new value
        PlayerPrefs.SetInt(ImageCntKey, m_ImageCount);
    }

    /// <summary>
    /// Take the screenshot and save the PNG file to disk in the Screenshot folder of the project.
    /// </summary>
    public void TakeScreenshot()
    {
        // Saves the current image count
        PlayerPrefs.SetInt(ImageCntKey, ++m_ImageCount);

        // Adjusts the height and width for the file name
        int width = Screen.width * m_ScaleFactor;
        int height = Screen.height * m_ScaleFactor;

        // Determine the pathname/filename for the file
        string pathname = "Screenshots/Screenshot_";
        if (includeImageSizeInFilename)
        {
            pathname += width + "x" + height + "_";
        }
        pathname += m_ImageCount + ".png";

        // Take the actual screenshot and save it
        ScreenCapture.CaptureScreenshot(pathname, m_ScaleFactor);
        Debug.Log("Screenshot captured at " + pathname);
    }
}
