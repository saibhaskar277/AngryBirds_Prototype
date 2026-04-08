using UnityEngine;

public interface IBlock 
{
    void OnHit(Rigidbody2D birdRb, float impactVelocity);
    bool CanBreak(float impactVelocity);
    void TakeDamage(float impact);

}
