using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn = null;
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 3f;

    private bool isSpawningActive = true;

    private void Awake()
    {
        PlayerController.OnGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        isSpawningActive = false;
    }

    private void Start()
    {
        isSpawningActive = true;
        InvokeRepeating("SpawnObstacle", startDelay, spawnInterval);
    }

    private void SpawnObstacle()
    {
        if (isSpawningActive)
        {
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            Instantiate(objectToSpawn, spawnPoint.position, objectToSpawn.transform.rotation);
        }
    }

    private void OnDestroy()
    {
        PlayerController.OnGameOver -= OnGameOver;
    }
}
