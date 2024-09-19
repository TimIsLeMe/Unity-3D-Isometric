using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayPauseUILogic : MonoBehaviour
{
    private const string MainMenuButtonName = "MainMenu";
    private const string UnpauseButtonName = "Unpause";
    
        
    private UIDocument _overlayDocument;
    
    public event EventHandler UnpauseButtonPressed;
    protected virtual void OnUnpauseButtonPressed()
    {
        UnpauseButtonPressed?.Invoke(this, EventArgs.Empty);
        Debug.Log("OnUnpauseButtonPressed OverlayPauseUILogic");

    }

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

        _overlayDocument.rootVisualElement.Q<Button>(UnpauseButtonName).clicked += () =>
        {
            Debug.Log("Unpause button clicked!");
            OnUnpauseButtonPressed();
        };
    }
}
