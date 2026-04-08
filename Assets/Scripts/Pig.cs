using UnityEngine;

public class Pig : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    public float impactThreshold = 5f; // minimum hit force to take damage
    public float damageMultiplier = 5f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        float impact = collision.relativeVelocity.magnitude;

        if (impact > impactThreshold)
        {
            float damage = impact * damageMultiplier;
            TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // TODO: play particle / sound
        Destroy(gameObject);
    }
}
