using UnityEngine;

public class GlassBlock : BaseBlock
{
    protected override void ApplyImpactForce(Rigidbody2D birdRb, float impactVelocity)
    {
        if (birdRb != null)
        {
            // Glass gives minimal bounce back
            birdRb.linearDamping *= blockData.impactMultiplier;
        }
    }

}
