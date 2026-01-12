using System.Collections.Generic;
using System.Linq;
using System.Text;
using CombatExtended;
using CombatExtended.Compatibility;
using RimWorld;
using Verse;

namespace WeaponUtility.CE;

internal class StatWorker_SecondaryAmmoCaliber : StatWorker
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

    private static Thing Gun(StatRequest req)
    {
        return (req.Thing is Building_Turret obj ? obj.GetGun() : null) ?? req.Thing;
    }

    public override bool ShouldShowFor(StatRequest req)
    {
        if (!base.ShouldShowFor(req))
        {
            return false;
        }

        var compProperties_SecondaryAmmo = GunDef(req)?.GetCompProperties<CompProperties_SecondaryAmmo>();
        if (compProperties_SecondaryAmmo is { showSecondaryAmmoStat: false })
        {
            return false;
        }

        var ammoSet = compProperties_SecondaryAmmo?.secondaryAmmoProps.ammoSet;
        return ShouldDisplayAmmoSet(ammoSet);
    }

    public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
    {
        var val = GunDef(statRequest)?.GetCompProperties<CompProperties_SecondaryAmmo>()?.secondaryAmmoProps.ammoSet;
        if (!ShouldDisplayAmmoSet(val))
        {
            yield break;
        }

        if (val?.ammoTypes == null)
        {
            yield break;
        }

        foreach (var ammoType in val.ammoTypes)
        {
            yield return new Dialog_InfoCard.Hyperlink(ammoType.ammo);
        }
    }

    public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
    {
        var stringBuilder = new StringBuilder();
        var val = GunDef(req)?.GetCompProperties<CompProperties_SecondaryAmmo>()?.secondaryAmmoProps.ammoSet;
        if (ShouldDisplayAmmoSet(val))
        {
            if (val == null)
            {
                return stringBuilder.ToString().TrimEndNewlines();
            }

            stringBuilder.AppendLine(val.LabelCap + "\n");
            foreach (var ammoType in val.ammoTypes)
            {
                var text = string.IsNullOrEmpty(ammoType.ammo.ammoClass.LabelCapShort)
                    ? (string)ammoType.ammo.ammoClass.LabelCap
                    : ammoType.ammo.ammoClass.LabelCapShort;
                stringBuilder.AppendLine($"{text}:\n{ammoType.projectile.GetProjectileReadout(Gun(req))}");
            }
        }
        else
        {
            foreach (var item in from x in GunDef(req)?.Verbs?.Where(x => x.defaultProjectile != null)
                     select x.defaultProjectile)
            {
                stringBuilder.AppendLine(item.LabelCap + ":\n" + item.GetProjectileReadout(Gun(req)));
            }
        }

        return stringBuilder.ToString().TrimEndNewlines();
    }

    public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense,
        StatRequest optionalReq, bool finalized = true)
    {
        var val = GunDef(optionalReq)?.GetCompProperties<CompProperties_SecondaryAmmo>()?.secondaryAmmoProps.ammoSet;
        if (ShouldDisplayAmmoSet(val))
        {
            var taggedString = val?.LabelCap;
            return taggedString;
        }

        var source = from x in GunDef(optionalReq)?.Verbs?.Where(x => x.defaultProjectile != null)
            select x.defaultProjectile;
        return source.First().LabelCap + (source.Count() > 1 ? "(+" + (source.Count() - 1) + ")" : "");
    }

    private static bool ShouldDisplayAmmoSet(AmmoSetDef ammoSet)
    {
        return ammoSet != null && AmmoUtility.IsAmmoSystemActive(ammoSet);
    }
}