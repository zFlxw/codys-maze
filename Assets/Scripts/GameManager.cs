using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Reference")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject barricade;
    [SerializeField] private List<ExitBarrier> possibleExits;

    public Player Player => player;
    public AudioSource SfxAudioSource => sfxAudioSource;

    public InputManager InputManager { get; private set; }
    public ItemManager ItemManager { get; private set; }
    public LightManager LightManager { get; private set; }
    public CanvasManager CanvasManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        InputManager = GetComponent<InputManager>();
        ItemManager = GetComponent<ItemManager>();
        ItemManager.LoadAllItems();

        LightManager = GetComponent<LightManager>();
        CanvasManager = GetComponent<CanvasManager>();
    }

    private void Start()
    {
        int openExit = Random.Range(0, possibleExits.Count);
        for (int i = 0; i < possibleExits.Count; i++)
        {
            if (i == openExit)
            {
                continue;
            }

            ExitBarrier exit = possibleExits[i];
            exit.SpawnBarricade(barricade);
        }
    }
}