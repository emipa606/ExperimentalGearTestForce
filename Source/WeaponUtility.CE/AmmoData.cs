using CombatExtended;
using Verse;

namespace WeaponUtility.CE;

public class AmmoData : IExposable
{
    public int curMagCount;

    public AmmoDef selectedAmmo;

    public AmmoData()
    {
    }

    public AmmoData(int curMagCount, AmmoDef selectedAmmo)
    {
        this.curMagCount = curMagCount;
        this.selectedAmmo = selectedAmmo;
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref curMagCount, "magCount");
        Scribe_Defs.Look(ref selectedAmmo, "selectedAmmo");
    }

    public override string ToString()
    {
        return "EGTF.AmmoData".Translate(curMagCount, selectedAmmo.defName);
    }
}