using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool flipX;

    [SerializeField] private Transform playerSprites;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private float speed = 5.0f;

    private Rigidbody2D playerRigidbody2D;

    private Vector2 playerPosition = Vector2.zero;
    private Vector2 playerVelocity = Vector2.zero;

    private static readonly int IsRun = Animator.StringToHash("isRun");

    private void Start()
    {
        TryGetComponent(out playerRigidbody2D);
    }

    private void Update()
    {
        playerVelocity = playerPosition * speed;

        if (playerVelocity.x > 0)
        {
            playerSprites.localScale = Vector3.one;
        }
        else if (playerVelocity.x < 0)
        {
            playerSprites.localScale = new Vector3(-1, 1, 1);
        }

        if (playerVelocity.x != 0 || playerVelocity.y != 0)
            playerAnimator.SetBool(IsRun, true);
        else
            playerAnimator.SetBool(IsRun, false);
    }

    private void FixedUpdate()
    {
        playerRigidbody2D.MovePosition(playerRigidbody2D.position + playerVelocity * Time.fixedDeltaTime);
    }

    public void OnMovements(InputValue value)
    {
        playerPosition = value.Get<Vector2>();
    }
}