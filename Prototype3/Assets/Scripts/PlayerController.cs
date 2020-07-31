using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnGameOver;

    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityModifier = 1.5f;
    [SerializeField] private ParticleSystem explosionParticle = null;
    [SerializeField] private ParticleSystem dirtParticle = null;
    [SerializeField] private AudioClip jumpSound = null;
    [SerializeField] private AudioClip crashSound = null;

    private Rigidbody playerRigidbody = null;
    private Animator playerAnimator = null;
    private AudioSource playerAudioSource = null;
    private bool isOnGround = true;
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
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !isGameOver)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetTrigger("Jump_trig");
            playerAudioSource.PlayOneShot(jumpSound, 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGameOver)
        {
            isOnGround = true;
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
