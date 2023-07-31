using Assets;
using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InventoryManager))]
public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Transform spawnLocation;

    [SerializeField]
    private float defaultFov;

    [SerializeField]
    private float increasedFov;

    [SerializeField]
    private float fovTransitionTime = 0.25f;

    [SerializeField]
    private bool bypassBatteryTimer = false;

    [Header("References")]
    [SerializeField] private Timer batteryTimer;
    [SerializeField] private Timer flashlightTimer;
    [SerializeField] private Timer spyglassTimer;

    private Camera _camera;
    public Timer BatteryTimer { get => batteryTimer; }
    public Timer FlashlightTimer { get => flashlightTimer; }
    public Timer SpyglassTimer { get => spyglassTimer; }

    public InventoryManager InventoryManager { get; private set; }

    public bool DisableMovement { get; set; } = false;
    public bool InSpawnArea { get; private set; } = true;

    private void Awake()
    {
        _camera = Camera.main;
        _camera.orthographicSize = defaultFov;

        InventoryManager = GetComponent<InventoryManager>();

        batteryTimer.OnTimerStop += HandleBatteryTimerStop;
        transform.position = spawnLocation.position;
    }

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            if (collision.TryGetComponent<Item>(out Item item))
            {
                if (item.Pickable)
                {
                    item.PickupItem(this);
                }
                else
                {
                    item.UseItem(this);
                }
            }
        }
        else if (collision.CompareTag("SpawnArea"))
        {
            InSpawnArea = true;
            GameManager.Instance.LightManager.DeactivatePlayerLight();
            if (batteryTimer.IsRunning)
            {
                batteryTimer.PauseTimer();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnArea"))
        {
            InSpawnArea = false;
            GameManager.Instance.LightManager.ActivatePlayerLight();
            if (bypassBatteryTimer)
            {
                return;
            }

            if (batteryTimer.RemainingTime != batteryTimer.DefaultTime)
            {
                batteryTimer.ResumeTimer();
            }
            else
            {
                batteryTimer.StartTimer();
            }
        }
    }


    public void IncreaseFOV()
    {
        DOTween.Sequence()
            .Append(DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, increasedFov, fovTransitionTime))
            .OnComplete(() =>
            {
                StartCoroutine(ResetFOV());
            });
    }

    public void BlockMovement(int time)
    {
        DisableMovement = true;
        StartCoroutine(EnableMovement(time));
    }

    private IEnumerator ResetFOV()
    {
        yield return new WaitForSeconds(3);

        DOTween.Sequence()
            .Append(DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, defaultFov, fovTransitionTime));

        BatteryTimer.ResumeTimer();
    }

    private IEnumerator EnableMovement(int time)
    {
        yield return new WaitForSeconds(time);

        DisableMovement = false;
    }

    
    private void HandleBatteryTimerStop(Timer timer)
    {
        timer.PauseTimer();
        GameManager.Instance.LightManager.ReducePlayerLight();
    }
}
