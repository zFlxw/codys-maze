using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [Header("Settings")]
    [Header("Intensity")]
    [SerializeField]
    private float sceneLightIntensity;

    [SerializeField]
    private float lanternIntensity;

    [SerializeField]
    private float playerLightIntensity;

    [Header("Radius")]
    [SerializeField]
    private float defaultRadius;

    [SerializeField]
    private float lanternRadius;


    [Header("Time")]
    [SerializeField]
    private float sceneLoadTime = 2f;

    [SerializeField]
    private float lanternActivationTime = 1.5f;

    [SerializeField]
    private float playerLightActivationTime = 0.5f;

    [SerializeField]
    private float flashlightActivationTime = 0.5f;

    [Header("Reference")]
    [SerializeField]
    private Light2D sceneLight;

    [SerializeField]
    private Light2D lantern;

    [SerializeField]
    private Light2D playerLight;

    [Space(10)]
    [SerializeField]
    private SpriteRenderer lanternOverlay;

    [SerializeField]
    private SpriteRenderer playerLightOverlay;

    private void Start()
    {
        sceneLight.intensity = 1.0f;
        lantern.intensity = 0.0f;

        playerLight.intensity = 0.0f;
        playerLight.pointLightOuterRadius = 0.0f;

        DOTween
            .Sequence()
            .Append(DOTween.To(() => sceneLight.intensity, x => sceneLight.intensity = x, sceneLightIntensity, sceneLoadTime))
            .OnComplete(() =>
            {
                DOTween
                    .Sequence()
                    .Append(DOTween.To(() => lantern.intensity, x => lantern.intensity = x, lanternIntensity, lanternActivationTime));

                DOTween
                    .Sequence()
                    .Append(DOTween.To(() => lantern.pointLightOuterRadius, x => lantern.pointLightOuterRadius = x, lanternRadius, lanternActivationTime));

                DOTween
                    .Sequence()
                    .Append(DOTween.To(() => lanternOverlay.color.a, x => lanternOverlay.color = new Color(0, 0, 0, x), 0.0f, lanternActivationTime));
            });
    }

    public void ActivatePlayerLight()
    {
        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLight.intensity, x => playerLight.intensity = x, playerLightIntensity, playerLightActivationTime));

        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLight.pointLightOuterRadius, x => playerLight.pointLightOuterRadius = x, defaultRadius, playerLightActivationTime));

        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLightOverlay.color.a, x => playerLightOverlay.color = new Color(255, 255, 255, x), 255.0f, playerLightActivationTime));
    }

    public void DeactivatePlayerLight()
    {
        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLight.intensity, x => playerLight.intensity = x, 0.0f, playerLightActivationTime));

        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLight.pointLightOuterRadius, x => playerLight.pointLightOuterRadius = x, 0.0f, playerLightActivationTime));

        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLightOverlay.color.a, x => playerLightOverlay.color = new Color(255, 255, 255, x), 0.0f, playerLightActivationTime));
    }

    public void ReducePlayerLight()
    {
        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLight.intensity, x => playerLight.intensity = x, playerLightIntensity / 2, playerLightActivationTime));

        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLight.pointLightOuterRadius, x => playerLight.pointLightOuterRadius = x, defaultRadius / 1.25f, playerLightActivationTime));

        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLightOverlay.color.a, x => playerLightOverlay.color = new Color(255 / 2, 255 / 2, 255/ 2, x), 255.0f, playerLightActivationTime));
    }

    public void ActivateFlashlight(float radius)
    {
        GameManager.Instance.Player.BatteryTimer.PauseTimer();
        GameManager.Instance.Player.FlashlightTimer.StartTimer();
        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLight.pointLightOuterRadius, x => playerLight.pointLightOuterRadius = x, radius, flashlightActivationTime));

        DOTween
            .Sequence()
            .Append(DOTween.To(() => playerLight.intensity, x => playerLight.intensity = x, playerLightIntensity, flashlightActivationTime))
            .OnComplete(() =>
            {
                GameManager.Instance.Player.FlashlightTimer.OnTimerStop += DeactivateFlashlight;
            });
    }

    private void DeactivateFlashlight(Timer timer)
    {
        if (GameManager.Instance.Player.BatteryTimer.RemainingTime > 0)
        {
            ActivatePlayerLight();

            GameManager.Instance.Player.BatteryTimer.ResumeTimer();
        }
        else
        {
            ReducePlayerLight();
        }
    }
}
