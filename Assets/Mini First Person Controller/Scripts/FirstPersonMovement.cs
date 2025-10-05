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

    public AudioSource idleMusic;
    public AudioSource moveMusic;
    public AudioSource splashing;

    public float fadeSpeed = 2f;

    bool flip = false;
    bool isDashing = false;
    bool canDash = true;
    Coroutine regenRoutine;
    Vector2 moveInput;
    bool isWalkingPrev = false;

    float splashTimer = 0f;
    float splashPitchChangeSpeed = 0.05f;
    float randPitch = 1f;

    void Start()
    {
        idleMusic.volume = 0.4f;
        moveMusic.volume = 0f;
        splashing.volume = 0f;
        splashing.pitch = 1.8f;
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        moveInput = new Vector2(x, y);

        bool walking = false;
        if (moveInput.magnitude > 0.01f) walking = true;

        if (walking != isWalkingPrev)
        {
            if (walking)
            {
                StartCoroutine(FadeAudio(moveMusic, 0.4f));
                StartCoroutine(FadeAudio(splashing, 0.12f));
                StartCoroutine(FadeAudio(idleMusic, 0f));
                if (!splashing.isPlaying) splashing.Play();
            }
            else
            {
                StartCoroutine(FadeAudio(moveMusic, 0f));
                StartCoroutine(FadeAudio(splashing, 0f));
                StartCoroutine(FadeAudio(idleMusic, 0.4f));
            }
            isWalkingPrev = walking;
        }

        if (walking)
        {
            splashTimer += Time.deltaTime;
            if (splashTimer >= splashPitchChangeSpeed)
            {
                splashTimer = 0f;
                randPitch = Random.Range(0.5f, 2f);
                splashing.pitch = randPitch;
            }
        }

        if (moveInput.x < -0.01f)
        {
            flip = false;
        }
        else if (moveInput.x > 0.01f)
        {
            flip = true;
        }

        Vector3 sc = renderer1.transform.localScale;
        float newX = Mathf.Abs(sc.x);
        if (flip) newX = -newX;
        sc.x = newX;
        renderer1.transform.localScale = sc;

        if (pbox != null)
        {
            Vector3 pos = pbox.transform.localPosition;
            if (flip) pos.x = 0.95f; else pos.x = -0.95f;
            pbox.transform.localPosition = pos;
        }

        if (!isDashing)
        {
            Vector3 moveVec = new Vector3(moveInput.x * speed, rigidbody.linearVelocity.y, moveInput.y * speed);
            Vector3 rotatedMove = transform.rotation * moveVec;
            rigidbody.linearVelocity = rotatedMove;
        }

        controling.SetBool("Walking", walking);

        if (Input.GetKeyDown(dashKey))
        {
            if (canDash)
            {
                TryDash();
            }
        }
    }

    IEnumerator FadeAudio(AudioSource src, float tVol)
    {
        if (src == null) yield break;
        float sVol = src.volume;
        while (!Mathf.Approximately(src.volume, tVol))
        {
            src.volume = Mathf.MoveTowards(src.volume, tVol, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void TryDash()
    {
        int c = 0;
        c++;
        if (playerValues == null) return;
        if (playerValues.coins <= 0) return;

        playerValues.coins = playerValues.coins - 1;
        StartCoroutine(PerformDash());
        StartCoroutine(DashCooldownRoutine());
        if (regenRoutine != null) StopCoroutine(regenRoutine);
        regenRoutine = StartCoroutine(RegenCoinAfterDelay());
    }

    IEnumerator PerformDash()
    {
        isDashing = true;
        Vector3 d = new Vector3(moveInput.x, 0, moveInput.y);
        if (d.magnitude < 0.1f) d = transform.forward;
        d.Normalize();

        float t = dashDistance / dashSpeed;
        float el = 0f;

        while (el < t)
        {
            rigidbody.linearVelocity = d * dashSpeed;
            el += Time.deltaTime;
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
            int add = 1;
            playerValues.coins = playerValues.coins + add;
        }
    }
}
