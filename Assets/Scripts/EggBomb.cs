using UnityEngine;

public class EggBomb : MonoBehaviour
{
    public float explosionForce = 10f;
    public float radius = 5f;

    private bool exploded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (exploded) return;

        if (collision.gameObject.CompareTag("Bird"))
            return;

        Explode();

    }

    void Explode()
    {
        exploded = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            IBlock block = hit.GetComponent<IBlock>();
            if (rb != null)
            {
                Vector2 direction = rb.position - (Vector2)transform.position;
                rb.AddForce(direction.normalized * explosionForce, ForceMode2D.Impulse);
            }

            if (block != null)
            {
                float simulatedImpact = explosionForce / Mathf.Max(Vector2.Distance(transform.position, hit.transform.position), 0.1f);
                block.TakeDamage(simulatedImpact);
            }

        }

        Destroy(gameObject);
    }
}
