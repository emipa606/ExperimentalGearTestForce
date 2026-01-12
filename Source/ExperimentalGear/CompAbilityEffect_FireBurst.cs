using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ExperimentalGear;

public class CompAbilityEffect_FireBurst : CompAbilityEffect
{
    private new CompProperties_AbilityFireBurst Props => (CompProperties_AbilityFireBurst)props;

    private Pawn Pawn => parent.pawn;

    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        GenExplosion.DoExplosion(Pawn.Position, Pawn.MapHeld, Props.radius, DamageDefOf.Flame, Pawn, -1, -1f, null,
            null, null, null, ThingDefOf.Filth_Fuel, 1f, 1, null, null, 9, false, null, 0f, 1, 1f, false, null, null,
            null, false, 0.6f);
        base.Apply(target, dest);
    }

    public override IEnumerable<PreCastAction> GetPreCastActions()
    {
        yield return new PreCastAction
        {
            action = delegate
            {
                parent.AddEffecterToMaintain(EffecterDefOf.Fire_Burst.Spawn(parent.pawn.Position, parent.pawn.Map),
                    parent.pawn.Position, 17, parent.pawn.Map);
            },
            ticksAwayFromCast = 17
        };
    }

    public override bool AICanTargetNow(LocalTargetInfo target)
    {
        if (Pawn.Faction != Faction.OfPlayer && target is { HasThing: true, Thing: Pawn pawn })
        {
            return pawn.TargetCurrentlyAimingAt == Pawn;
        }

        return false;
    }
}