using UnityEngine;

public class Chuck : BaseBird
{
    [Header("Speed Boost Settings")]
    public float boostMultiplier = 2f;   // How much faster Chuck goes

    protected override void BirdAbility()
    {
        // Only apply if Rigidbody2D exists
        if (rb == null) return;

        // Get current velocity
        Vector2 currentVelocity = rb.linearVelocity;

        if (currentVelocity.magnitude <= 0.1f) return; // avoid boosting while stopped

        // Apply speed boost in the direction of current movement
        rb.linearVelocity = currentVelocity * boostMultiplier;

        // Optional: small visual effect or trail can be triggered here

        // Ability used flag is handled in BaseBird
    }
}