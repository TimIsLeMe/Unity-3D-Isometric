using System;
using UnityEngine;

public class SceneUIManager : MonoBehaviour
{
    [SerializeField] private OverlayUILogic overlayUILogic;
    [SerializeField] private OverlayPauseUILogic overlayPauseUILogic;
    private OverlayUILogic _overlayPanel;
    private OverlayPauseUILogic _overlayPausePanel;
    private float _timeScale;
    private AudioSource[] _playingAudios;
    private void Awake()
    {
        _overlayPanel = Instantiate(overlayUILogic, transform);
        _overlayPausePanel = Instantiate(overlayPauseUILogic, transform);
    }

    private void Start()
    {
        overlayPauseUILogic.gameObject.SetActive(false);
        _overlayPanel.PauseButtonPressed += OnPauseButtonPressed;
        _overlayPausePanel.UnpauseButtonPressed += OnUnpauseButtonPressed;
    }
        
    private void OnPauseButtonPressed(object sender, EventArgs e)
    {
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
        _overlayPanel.gameObject.SetActive(false);
        _overlayPausePanel.gameObject.SetActive(true);
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        _playingAudios = new AudioSource[audioSources.Length];
        int j = 0;
        for(int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].isPlaying)
            {
                _playingAudios[j] = audioSources[i];
                _playingAudios[j].Pause();
                j++;
            }
        }
    }
    
    private void OnUnpauseButtonPressed(object sender, EventArgs e)
    {
        Debug.Log("OnUnpauseButtonPressed SceneManager");
        Time.timeScale = 1f;
        _overlayPanel.gameObject.SetActive(true);
        _overlayPausePanel.gameObject.SetActive(false);
      
        foreach(AudioSource audioSource in _playingAudios)
        {
            if (audioSource == null) break;
            audioSource.UnPause();
        }
        _playingAudios = null;
    }
    
}
