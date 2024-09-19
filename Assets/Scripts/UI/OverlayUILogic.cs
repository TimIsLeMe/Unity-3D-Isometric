using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayUILogic : MonoBehaviour
{
    private const string MainMenuButtonName = "MainMenu";
    private const string EditCartButtonName = "EditCart";

    public event EventHandler EditCartButtonPressed;
    protected virtual void OnEditCartButtonPressed()
    {
        EditCartButtonPressed?.Invoke(this, EventArgs.Empty);
    }
        
    private UIDocument _overlayDocument;

    private void OnEnable()
    {
        _overlayDocument = GetComponent<UIDocument>();
        if (_overlayDocument == null)
        {
            Debug.LogError("No UIDocument found on OverlayManager object! Disabling OverlayManager script.");
            enabled = false;
            return;
        }
        _overlayDocument.rootVisualElement.Q<Button>(MainMenuButtonName).clicked += () =>
        {
            Debug.Log("MainMenu button clicked!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        };
        _overlayDocument.rootVisualElement.Q<Button>(EditCartButtonName).clicked += () =>
        {
            Debug.Log("EditCart button clicked!");
            OnEditCartButtonPressed();
        };
    }
}
