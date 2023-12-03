using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float triggerDistance = 15;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float pathLatency = 1.0f;
    [SerializeField] private Vector2 errorDistributionX = Vector2.zero;
    [SerializeField] private Vector2 errorDistributionY = Vector2.zero;
    
    private PlayerController[] players;
    private EnemyAI[] enemies;

    private Vector2 updatePosition = Vector3.zero;
    private Vector2 targetPosition = Vector3.zero;

    private bool isEnemyTriggered = false;
    private bool isPlayerTriggered = false;
    private bool isPlayerStopTriggered = false;

    private float lastPathTime = 0.0f;
    private float finalErrorDistributionX;
    private float finalErrorDistributionY;

    private void Start()
    {
        players = FindObjectsByType<PlayerController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        enemies = FindObjectsByType<EnemyAI>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        updatePosition = transform.position;
    }

    private void Update()
    {
        lastPathTime += Time.deltaTime;


        foreach (var player in players)
        {
            if (player == null)
                continue;

            var distanceX = transform.position.x - player.transform.position.x;
            var distanceY = transform.position.y - player.transform.position.y;


            if (lastPathTime > pathLatency)
            {
                lastPathTime = 0.0f;

                finalErrorDistributionX = Random.Range(errorDistributionX.x, errorDistributionX.y);
                finalErrorDistributionY = Random.Range(errorDistributionY.x, errorDistributionY.y);

                targetPosition = new Vector2(player.transform.position.x + finalErrorDistributionX,
                    player.transform.position.y + finalErrorDistributionY);
            }

            // Trigger zone
            if (isPlayerTriggered)
            {
                updatePosition = new Vector2(
                    Mathf.MoveTowards(transform.position.x, targetPosition.x, speed),
                    Mathf.MoveTowards(transform.position.y, targetPosition.y, speed)
                );

                if (isPlayerStopTriggered)
                {
                    updatePosition.x = transform.position.x;
                    updatePosition.y = Mathf.MoveTowards(transform.position.y, targetPosition.y, speed);
                }
            }
        }

        transform.position = updatePosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyTriggerDistance"))
            isEnemyTriggered = true;
        if (other.CompareTag("PlayerTriggerDistance"))
            isPlayerTriggered = true;
        if (other.CompareTag("PlayerStopDistance"))
            isPlayerStopTriggered = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("EnemyTriggerDistance"))
            isEnemyTriggered = true;
        if (other.CompareTag("PlayerTriggerDistance"))
            isPlayerTriggered = true;
        if (other.CompareTag("PlayerStopDistance"))
            isPlayerStopTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("EnemyTriggerDistance"))
            isEnemyTriggered = false;
        if (other.CompareTag("PlayerTriggerDistance"))
            isPlayerTriggered = false;
        if (other.CompareTag("PlayerStopDistance"))
            isPlayerStopTriggered = false;
    }
}