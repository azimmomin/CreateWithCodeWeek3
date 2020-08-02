using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnGameStarted;
    public static Action OnGameOver;

    public static Action<bool> OnDoubleSpeedActive;

    [SerializeField] private Vector3 playerStartPoint = Vector3.zero;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float gravityModifier = 1.5f;
    [SerializeField] private ParticleSystem explosionParticle = null;
    [SerializeField] private ParticleSystem dirtParticle = null;
    [SerializeField] private AudioClip jumpSound = null;
    [SerializeField] private AudioClip crashSound = null;

    private Rigidbody playerRigidbody = null;
    private Animator playerAnimator = null;
    private AudioSource playerAudioSource = null;

    private bool isOnGround = true;
    private int jumpCount = 0;
    private bool isDoubleSpeedActive = false;

    private float introWalkOnTime = 3f;

    private bool isGameActive = false;

    private void Start()
    {
        isGameActive = false;
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;

        StartCoroutine(PlayGameStartAnimation());
    }

    ///<summary>
    /// This coroutine will have the player walk forward into the scene
    /// before starting the game.
    ///</summary>
    private IEnumerator PlayGameStartAnimation()
    {

        // Set up start and end position for the player.
        Vector3 startPos = transform.position;
        Vector3 endPos = playerStartPoint;

        // Initialize time elapsed since player started moving.
        float elapsedTime = 0f;

        // Before starting interpolation, set player's movement animation speed.
        playerAnimator.SetFloat("Speed_f", 0.3f);

        // Linearly interpolate the player forward for the duration of 'introWalkOnTime'
        // When we exit this loop, the player's position should be at 'playerStartPoint'
        while (elapsedTime < introWalkOnTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / introWalkOnTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Have the player start running and start the game.
        playerAnimator.SetFloat("Speed_f", 1f);
        isGameActive = true;
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        if (!isGameActive)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKey(KeyCode.RightArrow))
            SetDoubleSpeedActive(true);

        if (Input.GetKeyUp(KeyCode.RightArrow))
            SetDoubleSpeedActive(false);
    }

    private void Jump()
    {
        if (!isOnGround && jumpCount >= maxJumps)
            return;

        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerAnimator.SetTrigger("Jump_trig");
        playerAudioSource.PlayOneShot(jumpSound, 1f);
        jumpCount++;
    }

    private void SetDoubleSpeedActive(bool isActive)
    {
        // We don't want to spam redundant events.
        if (isDoubleSpeedActive == isActive)
            return;

        isDoubleSpeedActive = isActive;

        float animationSpeed = isDoubleSpeedActive ? 2f : 1f;
        playerAnimator.SetFloat("Speed_Multiplier", animationSpeed);
        OnDoubleSpeedActive?.Invoke(isActive);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isGameActive)
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            jumpCount = 0;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameActive = false;

            playerAnimator.SetBool("Death_b", true);
            playerAnimator.SetInteger("DeathType_int", 1);

            dirtParticle.Stop();
            explosionParticle.Play();

            playerAudioSource.PlayOneShot(crashSound, 1f);

            OnGameOver?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isGameActive)
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
            dirtParticle.Stop();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
