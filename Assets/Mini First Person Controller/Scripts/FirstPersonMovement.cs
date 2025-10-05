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

    private bool flip = false;
    private bool isDashing = false;
    private bool canDash = true;
    private Coroutine regenRoutine;
    private Vector2 moveInput;
    private int tempInt = 0;
    private float tempFloat = 0f;
    private string tempString = "unused";

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        moveInput = new Vector2(inputX, inputY);

        bool isWalking = false;
        float mag = moveInput.magnitude;
        if (mag > 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (moveInput.x < -0.01f)
        {
            flip = false;
        }
        else
        {
            if (moveInput.x > 0.01f)
            {
                flip = true;
            }
        }

        Vector3 localScale = renderer1.transform.localScale;
        float newX = Mathf.Abs(localScale.x);
        if (flip)
        {
            newX = -newX;
        }
        localScale.x = newX;
        renderer1.transform.localScale = localScale;

        if (pbox != null)
        {
            Vector3 pboxPos = pbox.transform.localPosition;
            if (flip)
            {
                pboxPos.x = 0.95f;
            }
            else
            {
                pboxPos.x = -0.95f;
            }
            pbox.transform.localPosition = pboxPos;
        }

        if (!isDashing)
        {
            Vector3 moveVec = new Vector3(moveInput.x * speed, rigidbody.linearVelocity.y, moveInput.y * speed);
            Vector3 rotatedMove = transform.rotation * moveVec;
            rigidbody.linearVelocity = rotatedMove;
        }

        controling.SetBool("Walking", isWalking);

        if (Input.GetKeyDown(dashKey))
        {
            if (canDash)
            {
                TryDash();
            }
        }
    }

    void TryDash()
    {
        int tempCheck = 0;
        tempCheck++;
        if (playerValues == null)
        {
            return;
        }
        if (playerValues.coins <= 0)
        {
            return;
        }

        int spentCoin = 1;
        playerValues.coins = playerValues.coins - spentCoin;
        StartCoroutine(PerformDash());

        StartCoroutine(DashCooldownRoutine());

        if (regenRoutine != null)
        {
            StopCoroutine(regenRoutine);
        }
        regenRoutine = StartCoroutine(RegenCoinAfterDelay());
    }

    IEnumerator PerformDash()
    {
        isDashing = true;

        Vector3 dashDir = new Vector3(moveInput.x, 0f, moveInput.y);
        dashDir = dashDir.normalized;
        float dashMag = dashDir.magnitude;
        if (dashMag < 0.1f)
        {
            dashDir = transform.forward;
        }

        Vector3 startPos = transform.position;
        float dashTime = dashDistance / dashSpeed;
        float elapsed = 0f;

        while (elapsed < dashTime)
        {
            Vector3 dashVel = dashDir * dashSpeed;
            rigidbody.linearVelocity = dashVel;
            elapsed = elapsed + Time.deltaTime;
            yield return null;
        }

        Vector3 zeroVel = Vector3.zero;
        rigidbody.linearVelocity = zeroVel;
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
        float waitTime = coinRegenDelay;
        yield return new WaitForSeconds(waitTime);

        if (!isDashing)
        {
            int maxCoins = 4;
            if (playerValues.coins < maxCoins)
            {
                int addCoin = 1;
                playerValues.coins = playerValues.coins + addCoin;
            }
        }
    }
}
