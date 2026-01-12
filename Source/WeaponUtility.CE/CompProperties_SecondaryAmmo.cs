using System.Collections.Generic;
using CombatExtended;
using Verse;

namespace WeaponUtility.CE;

public class CompProperties_SecondaryAmmo : CompProperties
{
    public readonly string description = "";

    public readonly string mainWeaponLabel = "";

    public readonly List<int> secondaryWeaponChargeSpeeds = [];

    public readonly string secondaryWeaponLabel = "";

    public readonly bool showSecondaryAmmoStat = true;

    public float loadedAmmoBulkFactor;

    public string mainCommandIcon = "";
    public CompProperties_AmmoUser secondaryAmmoProps;

    public string secondaryCommandIcon = "";

    public VerbPropertiesCE secondaryVerb;

    public CompProperties_SecondaryAmmo()
    {
        compClass = typeof(CompSecondaryAmmo);
    }
}