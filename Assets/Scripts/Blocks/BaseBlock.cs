using UnityEngine;

public abstract class BaseBlock : MonoBehaviour ,IBlock
{
    [SerializeField]
    private BlockDataHolder blockDataHolder;    // ScriptableObject with thresholds

    protected Rigidbody2D rb;

    protected BlockData blockData;

    [SerializeField]
    private BlockType blockType;

    protected float currentHealth;

    protected virtual void Awake()
    {

        blockData = blockDataHolder.GetBlockData(blockType);
        currentHealth = blockData.maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRb = collision.rigidbody;

        float impactVelocity = 0f;

        // 1️⃣ Velocity contribution from colliding object
        if (otherRb != null)
        {
            // Velocity along contact normal
            impactVelocity = Vector2.Dot(otherRb.linearVelocity, collision.contacts[0].normal);
            impactVelocity = Mathf.Abs(impactVelocity);
        }
        else
        {
            // Object without Rigidbody
            impactVelocity = collision.relativeVelocity.magnitude;
        }

        // 2️⃣ Wall's own velocity contribution (only downward or along collision normal)
        Vector2 wallVel = rb.linearVelocity;
        Vector2 collisionNormal = collision.contacts[0].normal;

        // Project wall velocity onto collision normal
        float wallImpact = Mathf.Abs(Vector2.Dot(wallVel, -collisionNormal));

        // 3️⃣ Total damage calculation
        float damage = (impactVelocity + wallImpact) * 10f; // tweak multiplier

        TakeDamage(damage);

        // 4️⃣ Apply force to birds if needed
        if (otherRb != null && otherRb.CompareTag("Bird"))
        {
            ApplyImpactForce(otherRb, impactVelocity);
        }
    }

    public virtual void OnHit(Rigidbody2D birdRb, float impactVelocity)
    {
        // Deal damage based on impactVelocity
        float damage = impactVelocity * 10f; // tweak multiplier for balance
        TakeDamage(damage);

        ApplyImpactForce(birdRb, impactVelocity);
    }

    public virtual bool CanBreak(float impactVelocity)
    {
        return impactVelocity >= blockData.breakThreshold;
    }

    protected virtual void BreakWall()
    {
        // Default: destroy wall
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            BreakWall();
        }
    }


    protected abstract void ApplyImpactForce(Rigidbody2D birdRb, float impactVelocity);
}
