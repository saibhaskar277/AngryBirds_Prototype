using UnityEngine;

public class SplitBird : BaseBird
{
    public GameObject splitPrefab;   // Prefab of the bird clone
    public float spreadAngle = 15f;  // Degrees for left/right birds
    public float forceMultiplier = 1f; // Optional speed adjustment for clones

    protected override void BirdAbility()
    {
        if (splitPrefab == null) return;

        Vector2 originalVelocity = rb.linearVelocity;

        // Loop 3 times: left, center, right
        for (int i = 0; i < 3; i++)
        {
            GameObject clone = Instantiate(splitPrefab, transform.position, Quaternion.identity);
            Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>();

            if (cloneRb != null)
            {
                float angleOffset = 0f;
                if (i == 0) angleOffset = -spreadAngle; // left
                if (i == 1) angleOffset = 0f;           // center
                if (i == 2) angleOffset = spreadAngle;  // right

                Vector2 newVelocity = RotateVector(originalVelocity, angleOffset) * forceMultiplier;

                cloneRb.linearVelocity = newVelocity;
            }
        }

        Destroy(gameObject);
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}