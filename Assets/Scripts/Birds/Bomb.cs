using UnityEngine;

public class Bomb : BaseBird
{
    public float explosionForce = 500f;
    public float radius = 5f;

    protected override void BirdAbility()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = rb.position - (Vector2)transform.position;
                rb.AddForce(direction.normalized * explosionForce, ForceMode2D.Impulse);
            }
        }

        //Destroy(gameObject);
    }

}
