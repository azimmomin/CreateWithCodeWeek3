using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 3f;

    private void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, spawnInterval);
    }

    private void SpawnObstacle()
    {
        Instantiate(objectToSpawn, spawnPoint.position, objectToSpawn.transform.rotation);
    }
}
