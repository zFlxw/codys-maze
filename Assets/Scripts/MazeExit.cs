using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MazeExit : MonoBehaviour
{
    [SerializeField]
    private GameObject winUI;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player == null)
            {
                return;
            }

            player.BatteryTimer.PauseTimer();
            player.DisableMovement = true;
            winUI.SetActive(true);
        }
    }
}
