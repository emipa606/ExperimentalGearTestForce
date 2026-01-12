using Verse;

namespace ExperimentalGear;

public class Verb_ShootConsumeDurablility : Verb_Shoot
{
    private DurabilityCostExtension Extension
    {
        get
        {
            field ??= EquipmentSource?.def?.GetModExtension<DurabilityCostExtension>();

            return field;
        }
    }

    protected override bool TryCastShot()
    {
        if (!base.TryCastShot())
        {
            return false;
        }

        TryComsumeDurability();
        return true;
    }

    private void TryComsumeDurability()
    {
        var num = 0.334f;
        if (Extension != null)
        {
            num = Extension.cost;
        }

        var equipmentSource = EquipmentSource;
        if (equipmentSource == null || verbProps == equipmentSource.def.Verbs[0])
        {
            return;
        }

        var num2 = equipmentSource.HitPoints - (int)(equipmentSource.MaxHitPoints * num);
        if (num2 > 0)
        {
            equipmentSource.HitPoints = num2;
        }
        else
        {
            SelfConsume();
        }
    }

    private void SelfConsume()
    {
        if (EquipmentSource is { Destroyed: false })
        {
            EquipmentSource.Destroy();
        }
    }
}