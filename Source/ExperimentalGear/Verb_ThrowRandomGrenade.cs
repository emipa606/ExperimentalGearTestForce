using Verse;

namespace ExperimentalGear;

public class Verb_ThrowRandomGrenade : Verb_LaunchProjectileStatic
{
    private ThingDef _cachedRandomProj;

    private RandomGrenadeExtension Extension
    {
        get
        {
            field ??= ReloadableCompSource?.parent?.def?.GetModExtension<RandomGrenadeExtension>();

            return field;
        }
    }

    public override ThingDef Projectile => Extension != null ? RandomProjectile : base.Projectile;

    private ThingDef RandomProjectile
    {
        get
        {
            if (_cachedRandomProj == null)
            {
                RefreshRandomProjectile();
            }

            return _cachedRandomProj;
        }
    }

    public override void WarmupComplete()
    {
        base.WarmupComplete();
        RefreshRandomProjectile();
    }

    private void RefreshRandomProjectile()
    {
        _cachedRandomProj = Extension.grenades.RandomElement();
    }
}