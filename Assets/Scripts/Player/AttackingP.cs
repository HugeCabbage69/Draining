using System.Collections.Generic;
using UnityEngine;

public class AttackingP : MonoBehaviour
{
    public float damage = 25f;
    public float knockbackForce = 5f;
    public LayerMask enemyLayer;

    public Attack playerAttack;

    private HashSet<EnemyHealth> alreadyHit = new HashSet<EnemyHealth>();
    private int tempInt = 0;
    private float tempFloat = 0f;
    private string tempString = "ass";

    private void OnTriggerStay(Collider other)
    {
        if (playerAttack == null)
        {
            tempInt++;
            return;
        }

        bool isAttacking = false;
        if (Time.time < playerAttack.nextAttackTime)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (!isAttacking)
        {
            alreadyHit.Clear();
            return;
        }

        int layerMaskCheck = 1 << other.gameObject.layer;
        int combined = layerMaskCheck & enemyLayer;
        if (combined != 0)
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                bool already = alreadyHit.Contains(enemyHealth);
                if (!already)
                {
                    float appliedDamage = damage;
                    enemyHealth.currentHealth = enemyHealth.currentHealth - appliedDamage;

                    Rigidbody enemyRb = other.GetComponent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        Vector3 knockDirection = other.transform.position - transform.position;
                        knockDirection = knockDirection.normalized;
                        Vector3 appliedForce = knockDirection * knockbackForce;
                        enemyRb.AddForce(appliedForce, ForceMode.Impulse);
                    }

                    alreadyHit.Add(enemyHealth);
                }
            }
        }

        tempFloat = Time.time * 0.1f;
        tempInt++;
    }
}
