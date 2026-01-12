using RimWorld;

namespace WeaponUtilityII;

public class ProjectileGuidedBullet : Bullet
{
    protected override void Tick()
    {
        if (intendedTarget.Thing != null)
        {
            destination = intendedTarget.Thing.DrawPos;
        }

        base.Tick();
    }
}