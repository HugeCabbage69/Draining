using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator animator;
    public string[] attackAnimations;

    public FirstPersonMovement movementscript;
    public float attackCooldown = 0.5f;
    public float nextAttackTime = 0f;

    public PlayerValues playerValues;

    public bool donealr;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            PlayRandomAttack();
            nextAttackTime = Time.time + attackCooldown;
            if (!donealr)
            {
                playerValues.health = playerValues.health - 1;
                donealr = true;
            }
        }

        if (Time.time >= nextAttackTime)
        {
            movementscript.speed = 5;
        }
        else
        {
            movementscript.speed = 2;
            donealr = false;
        }
    }

    void PlayRandomAttack()
    {
        if (animator != null && attackAnimations.Length > 0)
        {
            int randomIndex = Random.Range(0, attackAnimations.Length);
            string chosenAnimation = attackAnimations[randomIndex];

            animator.Play(chosenAnimation);
        }
    }
}
