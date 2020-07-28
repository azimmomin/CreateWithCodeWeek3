using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 backgroundStartPosition;
    private float backgroundWidth;

    private void Start()
    {
        backgroundStartPosition = transform.position;
        backgroundWidth = GetComponent<BoxCollider>().size.x;
    }

    private void Update()
    {
        // If the background has moved left such that half of it
        // has moved past the start position, reset the camera's
        // transform to the initial position.
        if (transform.position.x < backgroundStartPosition.x - (backgroundWidth / 2f))
            transform.position = backgroundStartPosition;
    }
}
