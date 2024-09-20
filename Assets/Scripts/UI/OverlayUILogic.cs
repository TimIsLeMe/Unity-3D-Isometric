using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayUILogic : MonoBehaviour
{
    private const string MainMenuButtonName = "MainMenu";
    private const string PauseButtonName = "Pause";
    
        
    private UIDocument _overlayDocument;
    
    public event EventHandler PauseButtonPressed;
    protected virtual void OnPauseButtonPressed()
    {
        PauseButtonPressed?.Invoke(this, EventArgs.Empty);
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

        _overlayDocument.rootVisualElement.Q<Button>(PauseButtonName).clicked += () =>
        {
            Debug.Log("Pause button clicked!");
            OnPauseButtonPressed();
        };
    }
}
