using CombatExtended;
using Verse;

namespace ExperimentalGear.CE;

public class Verb_ThrowRandomGrenade : Verb_ThrowGrenade
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

    private ThingDef RandomProjectile
    {
        get
        {
            if (_cachedRandomProj == null)
            {
                refreshRandomProjectile();
            }

            return _cachedRandomProj;
        }
    }

    public override ThingDef Projectile => Extension != null ? RandomProjectile : Projectile;

    public override void WarmupComplete()
    {
        base.WarmupComplete();
        refreshRandomProjectile();
    }

    private void refreshRandomProjectile()
    {
        _cachedRandomProj = Extension.grenades.RandomElement();
    }
}