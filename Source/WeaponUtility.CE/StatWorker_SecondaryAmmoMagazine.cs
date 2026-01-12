using System.Text;
using RimWorld;
using Verse;

namespace WeaponUtility.CE;

public class StatWorker_SecondaryAmmoMagazine : StatWorker
{
    private static ThingDef GunDef(StatRequest req)
    {
        var thingDef = req.Def as ThingDef;
        if (thingDef?.building?.IsTurret == true)
        {
            thingDef = thingDef.building.turretGunDef;
        }

        return thingDef;
    }

    public override bool ShouldShowFor(StatRequest req)
    {
        var compProperties_SecondaryAmmo = GunDef(req)?.GetCompProperties<CompProperties_SecondaryAmmo>();
        if (compProperties_SecondaryAmmo is { showSecondaryAmmoStat: false })
        {
            return false;
        }

        if (base.ShouldShowFor(req))
        {
            return (compProperties_SecondaryAmmo?.secondaryAmmoProps.magazineSize ?? 0) > 0;
        }

        return false;
    }

    public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
    {
        return (GunDef(req)?.GetCompProperties<CompProperties_SecondaryAmmo>()?.secondaryAmmoProps.magazineSize)
            .GetValueOrDefault();
    }

    public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
    {
        var stringBuilder = new StringBuilder();
        var compProperties_SecondaryAmmo = GunDef(req)?.GetCompProperties<CompProperties_SecondaryAmmo>();
        if (compProperties_SecondaryAmmo == null)
        {
            return stringBuilder.ToString().TrimEndNewlines();
        }

        stringBuilder.AppendLine("CE_MagazineSize".Translate() + ": " +
                                 GenText.ToStringByStyle(
                                     compProperties_SecondaryAmmo.secondaryAmmoProps.magazineSize,
                                     ToStringStyle.Integer));
        stringBuilder.AppendLine("CE_ReloadTime".Translate() + ": " +
                                 compProperties_SecondaryAmmo.secondaryAmmoProps.reloadTime.ToStringByStyle(
                                     ToStringStyle.FloatTwo) + " " + "LetterSecond".Translate());

        return stringBuilder.ToString().TrimEndNewlines();
    }

    public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense,
        StatRequest optionalReq, bool finalized = true)
    {
        var compProperties_SecondaryAmmo = GunDef(optionalReq)?.GetCompProperties<CompProperties_SecondaryAmmo>();
        if (compProperties_SecondaryAmmo != null)
        {
            return
                $"{compProperties_SecondaryAmmo.secondaryAmmoProps.magazineSize} / {compProperties_SecondaryAmmo.secondaryAmmoProps.reloadTime.ToStringByStyle(ToStringStyle.FloatTwo)} " +
                "LetterSecond".Translate();
        }

        return base.GetStatDrawEntryLabel(stat, value, numberSense, optionalReq, finalized);
    }
}