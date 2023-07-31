using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip[] pickupSounds;
    [SerializeField] private AudioClip[] useSounds;

    [SerializeField] private float value;
    [SerializeField] private ItemType itemType;

    [Tooltip("Whether the player should be able to pick the item up in the inventory or if it should be consumed immediately")]
    [SerializeField] private bool pickable = true;
    [SerializeField] private Sprite itemIcon;

    public ItemType ItemType { get => itemType; }
    public Sprite ItemIcon => itemIcon;
    public bool Pickable => pickable;


    public void PickupItem(Player player)
    {
        if (!pickable)
        {
            return;
        }

        player.InventoryManager.AddItem(itemType);
        GameManager.Instance.SfxAudioSource.pitch = Random.Range(0.95f, 1.15f);
        GameManager.Instance.SfxAudioSource.PlayOneShot(pickupSounds[Random.Range(0, pickupSounds.Length)], 0.25f);

        Destroy(this.gameObject);
    }

    public bool UseItem(Player player)
    {
        if (player.InSpawnArea)
        {
            return false;
        }
        GameManager.Instance.SfxAudioSource.pitch = Random.Range(0.95f, 1.15f);
        GameManager.Instance.SfxAudioSource.PlayOneShot(useSounds[Random.Range(0, useSounds.Length)], 0.25f);
        switch (itemType)
        {
            case ItemType.BATTERY:
                player.BatteryTimer.ResetTimer();
                if (!player.BatteryTimer.IsRunning && !player.SpyglassTimer.IsRunning && !player.FlashlightTimer.IsRunning)
                {
                    GameManager.Instance.LightManager.ActivatePlayerLight();
                    player.BatteryTimer.ResumeTimer();
                }

                break;

            case ItemType.FLASHLIGHT:
                GameManager.Instance.LightManager.ActivateFlashlight(value);

                break;

            case ItemType.SPYGLASS:
                GameManager.Instance.LightManager.ActivateFlashlight(value);
                player.BatteryTimer.PauseTimer();
                player.BlockMovement(3);
                player.IncreaseFOV();
                player.SpyglassTimer.StartTimer();

                break;
        }

        if (!this.pickable)
        {
            Destroy(this.gameObject);
        }

        return true;
    }
}
