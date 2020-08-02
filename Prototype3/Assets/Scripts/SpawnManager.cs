using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 3f;

    private bool isSpawningActive = false;

    private void Awake()
    {
        PlayerController.OnGameStarted += OnGameStarted;
        PlayerController.OnGameOver += OnGameOver;
    }

    private void OnGameStarted()
    {
        isSpawningActive = true;
    }

    private void OnGameOver()
    {
        isSpawningActive = false;
    }

    private void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, spawnInterval);
    }

    private void SpawnObstacle()
    {
        if (isSpawningActive)
        {
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, objectToSpawn.transform.rotation);
            spawnedObject.GetComponent<MoveLeft>().IsMovementActive = true;
        }
    }

    private void OnDestroy()
    {
        PlayerController.OnGameStarted -= OnGameStarted;
        PlayerController.OnGameOver -= OnGameOver;
    }
}
