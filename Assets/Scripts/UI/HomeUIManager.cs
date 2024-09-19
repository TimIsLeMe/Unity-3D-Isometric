using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] private MainMenuUILogic mainMenuPanelPrefab;
    private MainMenuUILogic _mainMenuPanel;

    private void Awake()
    {
        _mainMenuPanel = Instantiate(mainMenuPanelPrefab, transform);
    }
    
}