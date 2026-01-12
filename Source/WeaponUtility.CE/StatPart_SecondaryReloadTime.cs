using RimWorld;
using Verse;

namespace WeaponUtility.CE;

internal class StatPart_SecondaryReloadTime : StatPart
{
    public override string ExplanationPart(StatRequest req)
    {
        return "";
    }

    public override void TransformValue(StatRequest req, ref float val)
    {
        if (!req.HasThing)
        {
            return;
        }

        var compSecondaryAmmo = req.Thing.TryGetComp<CompSecondaryAmmo>();
        if (compSecondaryAmmo is { IsSharedAmmo: false, IsSecondaryAmmoSelected: true })
        {
            val = compSecondaryAmmo.Props.secondaryAmmoProps.reloadTime;
        }
    }
}