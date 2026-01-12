using CombatExtended;
using RimWorld;
using Verse;

namespace WeaponUtility.CE;

internal class StatPart_LoadedSecondaryAmmo : StatPart
{
    public override void TransformValue(StatRequest req, ref float val)
    {
        if (TryGetValue(req, out var num))
        {
            val += num;
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        return !TryGetValue(req, out var num)
            ? null
            : "CE_StatsReport_LoadedAmmo".Translate() + ": " + parentStat.ValueToString(num);
    }

    private bool TryGetValue(StatRequest req, out float num)
    {
        num = 0f;
        if (!req.HasThing)
        {
            return num != 0f;
        }

        var compSecondaryAmmo = req.Thing.TryGetComp<CompSecondaryAmmo>();
        if (compSecondaryAmmo is not { IsSharedAmmo: false, CurrentSecondaryAmmo: not null })
        {
            return num != 0f;
        }

        num = compSecondaryAmmo.CurrentSecondaryAmmo.GetStatValueAbstract(parentStat) *
              compSecondaryAmmo.ScondaryMagCount;
        if (parentStat == CE_StatDefOf.Bulk)
        {
            num *= compSecondaryAmmo.Props.loadedAmmoBulkFactor;
        }

        return num != 0f;
    }
}