using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] private uint pointsPerSecond = 10;

    private ulong currentScore = 0;
    private uint scoreMultipler = 1;

    private void Awake()
    {
        PlayerController.OnDoubleSpeedActive += SetDoubleSpeedActive;
        PlayerController.OnGameOver += OnGameOver;
    }

    private void SetDoubleSpeedActive(bool isActive)
    {
        scoreMultipler = isActive ? 2U : 1U;
    }

    private void OnGameOver()
    {
        StopAllCoroutines();
        Debug.Log($"Final Score: {currentScore}");
    }

    private void Start()
    {
        StartCoroutine(TrackScore());
    }

    private IEnumerator TrackScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            currentScore += (pointsPerSecond * scoreMultipler);
            Debug.Log($"Score: {currentScore}");
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        PlayerController.OnDoubleSpeedActive -= SetDoubleSpeedActive;
    }
}
