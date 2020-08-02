using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnGameOver;
    public static Action<bool> OnDoubleSpeedActive;

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
    private bool isGameOver = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKey(KeyCode.RightArrow))
            SetDoubleSpeedActive(true);

        if (Input.GetKeyUp(KeyCode.RightArrow))
            SetDoubleSpeedActive(false);
    }

    private void Jump()
    {
        if (isGameOver)
            return;

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
        playerAnimator.SetFloat("Speed_f", animationSpeed);
        OnDoubleSpeedActive?.Invoke(isActive);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGameOver)
        {
            isOnGround = true;
            jumpCount = 0;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;

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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
            dirtParticle.Stop();
        }
    }
}
