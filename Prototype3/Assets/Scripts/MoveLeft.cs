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

    private bool isMovementActive = true;

    private void Awake()
    {
        PlayerController.OnGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        isMovementActive = false;
    }

    private void Start()
    {   
        isMovementActive = true;
    }

    private void Update()
    {
        if (isMovementActive)
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }

    private void OnDestroy()
    {
        PlayerController.OnGameOver -= OnGameOver;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (gameObject.CompareTag("Obstacle") && collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }
}