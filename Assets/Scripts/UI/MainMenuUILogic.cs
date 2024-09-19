using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUILogic : MonoBehaviour
{
    private const string LevelSelectorName = "LevelSelector";
    private const string StartButtonName = "StartButton";
    private const string StartingRoundSliderName = "StartingRoundSlider";
    private const string QuitButtonName = "QuitButton";

    public event EventHandler StartingRoundSliderChanged;
    
    protected virtual void OnStartingRoundSliderChanged(float value)
    {
        StartingRoundSliderChanged?.Invoke(this, EventArgs.Empty);
    }


    private UIDocument _mainMenuUIDocument;

    private void OnEnable()
    {
        _mainMenuUIDocument = GetComponent<UIDocument>();
        if (_mainMenuUIDocument == null)
        {
            Debug.LogError("Main Menu UI Document is not found");
            enabled = false;
            return;
        }

        _mainMenuUIDocument.rootVisualElement.Q<Button>(StartButtonName).clicked += () =>
        {
            Debug.Log("Start Button Pressed");
            int sceneNr = _mainMenuUIDocument.rootVisualElement.Q<DropdownField>(LevelSelectorName).index + 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNr);
        };

        var slider = _mainMenuUIDocument.rootVisualElement.Q<Slider>(StartingRoundSliderName);
        slider.RegisterValueChangedCallback(evt =>
        {
            Debug.Log("Slider value changed!");
            OnStartingRoundSliderChanged(evt.newValue);
        });
        
        _mainMenuUIDocument.rootVisualElement.Q<Button>(QuitButtonName).clicked += () =>
        {
#if !UNITY_EDITOR
            Application.Quit();
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        };
    }
}