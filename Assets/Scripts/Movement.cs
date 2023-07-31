using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Player player;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] stepSounds;
       
    [Header("Tweaks")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float stepSpeed = 0.5f;

    private Rigidbody2D _rigidbody;

    private float _footstepTimer = 0.0f;

    private static readonly int IsMovingHash = Animator.StringToHash("IS_MOVING");
    private static readonly int HorizontalHash = Animator.StringToHash("HORIZONTAL");
    private static readonly int VerticalHash = Animator.StringToHash("VERTICAL");

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (player.DisableMovement)
        {
            if (_rigidbody.velocity != Vector2.zero)
            {
                _rigidbody.velocity = Vector2.zero;
                animator.SetBool(IsMovingHash, false);
            }

            return;
        }

        if (animator == null || GameManager.Instance.InputManager == null)
        {
            return;
        }

        if (GameManager.Instance.InputManager.Movement is { x: 0, y: 0 })
        {
            animator.SetBool(IsMovingHash, false);
            if (_rigidbody.velocity != Vector2.zero)
            {
                _rigidbody.velocity = Vector2.zero;
            }

            return;
        }

        var direction = new Vector2(GameManager.Instance.InputManager.Movement.x,
            GameManager.Instance.InputManager.Movement.y);
        animator.SetBool(IsMovingHash, true);
        animator.SetFloat(HorizontalHash, direction.x);
        animator.SetFloat(VerticalHash, direction.y);

        _rigidbody.velocity = direction * speed;

        _footstepTimer -= Time.deltaTime;
        if (_footstepTimer > 0)
        {
            return;
        }

        sfxSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)], 0.35f);
        _footstepTimer = stepSpeed;
    }
}
