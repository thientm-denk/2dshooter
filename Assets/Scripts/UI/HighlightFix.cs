using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// This Script is from:
/// https://forum.unity.com/members/daterre.121542/
/// Found on the forum here:
/// https://forum.unity.com/threads/button-keyboard-and-mouse-highlighting.294147/
/// It fixes the annoying highlight / selection issue with buttons when using both keyboard/gamepad and mouse to interact with UI
/// It must be placed on the button / selectable UI in order to function
/// </summary>
[RequireComponent(typeof(Selectable))]
public class HighlightFix : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    /// <summary>
    /// Description:
    /// Standard Unity UI function called when the pointer enters the UI element
    /// Inputs:
    /// PointerEventData eventData
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="eventData">The PointerEventData that set off this function call</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!EventSystem.current.alreadySelecting)
            EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    /// <summary>
    /// Description:
    /// Standard Unity UI function called when the UI element is deselected
    /// Inputs:
    /// BaseEventData eventData
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="eventData">The BaseEventData that caused this function call</param>
    public void OnDeselect(BaseEventData eventData)
    {
        this.GetComponent<Selectable>().OnPointerExit(null);
    }
}
