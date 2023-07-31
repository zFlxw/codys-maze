using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Player player;

    [Header("Player Map")]
    [SerializeField] private string movementActionName = "RUN";
    [SerializeField] private string inventoryActionName = "INVENTORY";
    [SerializeField] private string openPauseMenuActionName = "PAUSE_MENU";

    [Header("PauseMenu Map")]
    [SerializeField] private string closePauseMenuActionName = "CLOSE";

    public Vector2 Movement { get; private set; }
    public InputActionMap CurrentMap { get; private set; }
    private PlayerInput _playerInput;
    
    private InputAction _movementAction;
    private InputAction _inventoryAction;
    private InputAction _openPauseMenuAction;

    private InputAction _closePauseMenuAction;

    private void Awake()
    {
        _playerInput = player.GetComponent<PlayerInput>();
        CurrentMap = _playerInput.currentActionMap;
    }

    private void Start()
    {
        CurrentMap = _playerInput.currentActionMap;
        switch (CurrentMap.name)
        {
            case "Player":
                LoadPlayerMap();
                break;

            case "PauseMenu":
                LoadPauseMenuMap();
                break;
        }
    }

    public void SetActiveMap(string mapName)
    {
        _playerInput.SwitchCurrentActionMap(mapName);
        Start();
    }

    private void LoadPlayerMap()
    {
        _movementAction = CurrentMap.FindAction(movementActionName);
        _inventoryAction = CurrentMap.FindAction(inventoryActionName);
        _openPauseMenuAction = CurrentMap.FindAction(openPauseMenuActionName);

        _movementAction.performed += OnMove;
        _inventoryAction.performed += player.InventoryManager.UseItemInSlot;
        _openPauseMenuAction.performed += GameManager.Instance.CanvasManager.ShowPauseMenu;

        _movementAction.canceled += OnMove;
    }

    private void LoadPauseMenuMap()
    {
        _closePauseMenuAction = CurrentMap.FindAction(closePauseMenuActionName);

        _closePauseMenuAction.performed += GameManager.Instance.CanvasManager.HidePauseMenu;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        CurrentMap.Enable();
    }

    private void OnDisable()
    {
        CurrentMap.Disable();
    }
}
