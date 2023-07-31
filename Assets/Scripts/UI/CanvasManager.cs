using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Sound Options")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicSliderLabel;
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxSliderLabel;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image[] hudElements;

    private List<Timer> _startedTimers = new List<Timer>();

    private void Start()
    {
        foreach (Image hudElement in hudElements) {
            hudElement.color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        }

        musicAudioSource.enabled = true;
        musicAudioSource.Play();
        musicSlider.value = musicAudioSource.volume;
        musicSliderLabel.text = $"AMBIENT: {string.Format("{0:0.0}", musicSlider.value * 1000)}%";

        sfxAudioSource.enabled = true;
        sfxSlider.value = sfxAudioSource.volume;
        sfxSliderLabel.text = $"SFX: {string.Format("{0:0.0}", sfxSlider.value * 1000)}%";

        pauseMenu.SetActive(false);

        musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChange);
    }

    public void FadeInHUD()
    {
        foreach (Image hudElement in hudElements)
        {
            DOTween
            .Sequence()
            .Append(DOTween.To(() => hudElement.fillAmount, x => hudElement.fillAmount = x, 1.0f, 2.0f));
        }
        
    }

    public void ShowPauseMenu(InputAction.CallbackContext _)
    {
        pauseMenu.SetActive(true);
        GameManager.Instance.InputManager.SetActiveMap("PauseMenu");
        _startedTimers.Clear();

        if (GameManager.Instance.Player.BatteryTimer.IsRunning)
        {
            GameManager.Instance.Player.BatteryTimer.PauseTimer();
            _startedTimers.Add(GameManager.Instance.Player.BatteryTimer);
        }

        if (GameManager.Instance.Player.FlashlightTimer.IsRunning)
        {
            GameManager.Instance.Player.FlashlightTimer.PauseTimer();
            _startedTimers.Add(GameManager.Instance.Player.FlashlightTimer);
        }

        if (GameManager.Instance.Player.SpyglassTimer.IsRunning)
        {
            GameManager.Instance.Player.SpyglassTimer.PauseTimer();
            _startedTimers.Add(GameManager.Instance.Player.SpyglassTimer);
        }
    }

    public void HidePauseMenu(InputAction.CallbackContext _)
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.InputManager.SetActiveMap("Player");
        foreach (Timer timer in _startedTimers)
        {
            timer.ResumeTimer();
        }

        _startedTimers.Clear();
    }

    private void OnMusicSliderChange(float value)
    {
        musicAudioSource.volume = value;
        musicSliderLabel.text = $"AMBIENT: {string.Format("{0:0.0}", value * 1000)}%";
    }

    private void OnSfxSliderChange(float value)
    {
        sfxAudioSource.volume = value;
        sfxSliderLabel.text = $"SFX: {string.Format("{0:0.0}", value * 1000)}%";
    }
}
