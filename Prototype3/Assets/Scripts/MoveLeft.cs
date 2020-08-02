using UnityEngine;

/// <summary>
/// This class moves an object left at a fixed speed.
/// If the object loses contact with the ground and
/// it is an obstacle, we'll destroy it in order to
/// prevent them from lingering in the scene.
/// </summary>
public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    public bool IsMovementActive { get; set; }
    private float currentSpeed;

    private void Awake()
    {
        PlayerController.OnGameStarted += OnGameStarted;
        PlayerController.OnGameOver += OnGameOver;
        PlayerController.OnDoubleSpeedActive += SetDoubleSpeedActive;
    }

    private void OnGameStarted()
    {
        IsMovementActive = true;
    }

    private void OnGameOver()
    {
        IsMovementActive = false;
    }

    private void SetDoubleSpeedActive(bool isActive)
    {
        currentSpeed = isActive ? currentSpeed * 2f : speed;
    }

    private void Start()
    {
        currentSpeed = speed;
    }

    private void Update()
    {
        if (IsMovementActive)
            transform.Translate(Vector3.left * (currentSpeed * Time.deltaTime));
    }

    private void OnCollisionExit(Collision collision)
    {
        if (gameObject.CompareTag("Obstacle") && collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PlayerController.OnGameStarted -= OnGameStarted;
        PlayerController.OnGameOver -= OnGameOver;
        PlayerController.OnDoubleSpeedActive -= SetDoubleSpeedActive;
    }
}