using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 20f;
    public float dashDistance = 5f;
    public float dashCooldown = 0.2f;
    public float coinRegenDelay = 0.8f;
    public KeyCode dashKey = KeyCode.LeftShift;

    public Rigidbody rigidbody;
    public Animator controling;
    public PlayerValues playerValues;
    public GameObject renderer1;
    public GameObject pbox;

    // 🎵 Add these:
    public AudioSource idleMusic;
    public AudioSource moveMusic;
    public float fadeSpeed = 2f;

    private bool flip = false;
    private bool isDashing = false;
    private bool canDash = true;
    private Coroutine regenRoutine;
    private Vector2 moveInput;
    private bool isWalkingPrev = false;

    void Start()
    {
        idleMusic.volume = 0.4f;
        moveMusic.volume = 0f;
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        moveInput = new Vector2(inputX, inputY);

        bool isWalking = moveInput.magnitude > 0.01f;

        if (isWalking != isWalkingPrev)
        {
            if (isWalking)
            {
                StartCoroutine(FadeAudio(moveMusic, 0.4f));
                StartCoroutine(FadeAudio(idleMusic, 0f));
            }
            else
            {
                StartCoroutine(FadeAudio(moveMusic, 0f));
                StartCoroutine(FadeAudio(idleMusic, 0.4f));
            }
            isWalkingPrev = isWalking;
        }

        if (moveInput.x < -0.01f) flip = false;
        else if (moveInput.x > 0.01f) flip = true;

        Vector3 localScale = renderer1.transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * (flip ? -1 : 1);
        renderer1.transform.localScale = localScale;

        if (pbox != null)
        {
            Vector3 pboxPos = pbox.transform.localPosition;
            pboxPos.x = flip ? 0.95f : -0.95f;
            pbox.transform.localPosition = pboxPos;
        }

        if (!isDashing)
        {
            Vector3 moveVec = new Vector3(moveInput.x * speed, rigidbody.linearVelocity.y, moveInput.y * speed);
            Vector3 rotatedMove = transform.rotation * moveVec;
            rigidbody.linearVelocity = rotatedMove;
        }

        controling.SetBool("Walking", isWalking);

        if (Input.GetKeyDown(dashKey) && canDash)
        {
            TryDash();
        }
    }

    IEnumerator FadeAudio(AudioSource source, float targetVolume)
    {
        if (source == null) yield break;
        float startVolume = source.volume;

        while (!Mathf.Approximately(source.volume, targetVolume))
        {
            source.volume = Mathf.MoveTowards(source.volume, targetVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void TryDash()
    {
        if (playerValues == null || playerValues.coins <= 0) return;

        playerValues.coins--;
        StartCoroutine(PerformDash());
        StartCoroutine(DashCooldownRoutine());

        if (regenRoutine != null)
            StopCoroutine(regenRoutine);
        regenRoutine = StartCoroutine(RegenCoinAfterDelay());
    }

    IEnumerator PerformDash()
    {
        isDashing = true;
        Vector3 dashDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        if (dashDir.magnitude < 0.1f) dashDir = transform.forward;

        float dashTime = dashDistance / dashSpeed;
        float elapsed = 0f;

        while (elapsed < dashTime)
        {
            rigidbody.linearVelocity = dashDir * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rigidbody.linearVelocity = Vector3.zero;
        isDashing = false;
    }

    IEnumerator DashCooldownRoutine()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator RegenCoinAfterDelay()
    {
        yield return new WaitForSeconds(coinRegenDelay);

        if (!isDashing && playerValues.coins < 4)
        {
            playerValues.coins++;
        }
    }
}
