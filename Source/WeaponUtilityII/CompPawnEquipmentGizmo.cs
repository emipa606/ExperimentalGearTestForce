using System.Collections.Generic;
using Verse;

namespace WeaponUtilityII;

internal class CompPawnEquipmentGizmo : ThingComp
{
    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        var thingWithComps = parent;
        var thingWithComps2 = thingWithComps is Pawn pawn ? pawn.equipment.Primary : null;
        if (thingWithComps2?.AllComps.NullOrEmpty() ?? true)
        {
            yield break;
        }

        foreach (var allComp in thingWithComps2.AllComps)
        {
            if (allComp is not CompSecondaryVerb compSecondaryVerb)
            {
                continue;
            }

            foreach (var item in compSecondaryVerb.CompGetGizmosExtra())
            {
                yield return item;
            }

            yield break;
        }
    }
}