using UnityEngine;

public class EggBird : BaseBird
{
    [Header("Egg Settings")]
    public GameObject eggPrefab;        // Egg prefab (TNT script attached)
    public float upwardSpeedMultiplier = 2f; // How much faster bird moves on ability
    public float launchAngle = 60f;     // Degrees above horizontal for bird's new path

    protected override void BirdAbility()
    {
        if (rb == null) return;

        // 1️⃣ Spawn the egg at bird's current position
        if (eggPrefab != null)
        {
            Instantiate(eggPrefab, transform.position + new Vector3(0,-0.3f,0), Quaternion.identity);
        }

        // 2️⃣ Calculate new velocity for bird itself
        float currentSpeed = rb.linearVelocity.magnitude * upwardSpeedMultiplier;

        Vector2 newVelocity = DegreeToVector2(launchAngle).normalized * currentSpeed;

        rb.linearVelocity = newVelocity;

        // Ability used flag is handled in BaseBird
    }

    // Helper: convert degrees to 2D vector
    private Vector2 DegreeToVector2(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}