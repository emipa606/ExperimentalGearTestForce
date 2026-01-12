using System.Collections.Generic;
using CombatExtended;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace WeaponUtility.CE;

public class CompSecondaryAmmo : CompRangedGizmoGiver
{
    private CompAmmoUser compAmmo;

    private AmmoData mainAmmo;

    private VerbProperties mainVerbProps;

    private CompProperties_AmmoUser originAmmoProps;

    private AmmoData secondaryAmmo;

    private CompCharges secondaryAmmoCharges;

    private CompProperties_AmmoUser secondaryAmmoProps;

    private VerbProperties secondaryVerbProps;

    private bool useSecondaryAmmo;

    public CompProperties_SecondaryAmmo Props => (CompProperties_SecondaryAmmo)props;

    private Verb_LaunchProjectileCE AttackVerb
    {
        get
        {
            var primaryVerb = CompEquippable.PrimaryVerb;
            return (Verb_LaunchProjectileCE)(primaryVerb is Verb_LaunchProjectileCE ? primaryVerb : null);
        }
    }

    private CompAmmoUser CompAmmo
    {
        get
        {
            if (compAmmo == null && parent != null)
            {
                compAmmo = parent.TryGetComp<CompAmmoUser>();
            }

            return compAmmo;
        }
    }

    private CompEquippable CompEquippable
    {
        get
        {
            if (field == null && parent != null)
            {
                field = parent.TryGetComp<CompEquippable>();
            }

            if (field == null)
            {
                Log.Error("Can not get CompEquippable.");
            }

            return field;
        }
    }

    private CompInventory CompInventory
    {
        get
        {
            if (field == null && Holder != null)
            {
                field = Holder.TryGetComp<CompInventory>();
            }

            if (field == null)
            {
                Log.Error("Can not get CompInventory.");
            }

            return field;
        }
    }

    private Pawn Holder
    {
        get
        {
            if (CompEquippable is not { PrimaryVerb.caster: not null })
            {
                return null;
            }

            var pawn = (CompEquippable?.parent?.ParentHolder as Pawn_InventoryTracker)?.pawn;
            if (pawn != null && pawn != CompEquippable?.PrimaryVerb?.CasterPawn)
            {
                return null;
            }

            var pawn2 = CompEquippable?.PrimaryVerb.CasterPawn;
            if (pawn2 != null)
            {
                return pawn2;
            }

            if (CompEquippable?.parent?.ParentHolder is not Pawn_InventoryTracker obj)
            {
                return null;
            }

            pawn2 = obj.pawn;

            return pawn2;
        }
    }

    private CompProperties_AmmoUser OriginAmmoSetData
    {
        get
        {
            if (originAmmoProps == null)
            {
                Log.Error("CompProperties_AmmoUser 'originAmmoProps' not initialized");
            }

            return originAmmoProps;
        }
    }

    private CompProperties_AmmoUser SecondaryAmmoSetData
    {
        get
        {
            if (secondaryAmmoProps == null)
            {
                Log.Error("CompProperties_AmmoUser 'secondaryAmmoProps' not initialized");
            }

            return secondaryAmmoProps;
        }
    }

    public bool IsSecondaryAmmoSelected => useSecondaryAmmo;

    public bool IsSharedAmmo { get; private set; }

    private bool HasAmmoUser => CompAmmo != null;

    public string MainAmmoName => OriginAmmoSetData.ammoSet.label;

    public string SecondaryAmmoName => SecondaryAmmoSetData.ammoSet.label;

    public int ScondaryMagSize =>
        !IsSecondaryAmmoSelected ? SecondaryAmmoSetData.magazineSize : OriginAmmoSetData.magazineSize;

    private AmmoData MainAmmoData
    {
        get
        {
            mainAmmo ??= new AmmoData(0, CompAmmo.Props.ammoSet.ammoTypes[0].ammo);

            return mainAmmo;
        }
    }

    private AmmoData SecondaryAmmoData
    {
        get
        {
            secondaryAmmo ??= new AmmoData(0, Props.secondaryAmmoProps.ammoSet.ammoTypes[0].ammo);

            return secondaryAmmo;
        }
    }

    public AmmoDef CurrentSecondaryAmmo =>
        !IsSecondaryAmmoSelected ? SecondaryAmmoData.selectedAmmo : MainAmmoData.selectedAmmo;

    public int ScondaryMagCount => !IsSecondaryAmmoSelected ? SecondaryAmmoData.curMagCount : MainAmmoData.curMagCount;

    private void SwitchLauncher()
    {
        if (!useSecondaryAmmo)
        {
            SaveAmmo(MainAmmoData);
            LoadAmmo(SecondaryAmmoData);
            LoadAmmoProps(SecondaryAmmoSetData);
            CompEquippable.PrimaryVerb.verbProps = secondaryVerbProps;
            useSecondaryAmmo = true;
        }
        else
        {
            SaveAmmo(SecondaryAmmoData);
            LoadAmmo(MainAmmoData);
            LoadAmmoProps(OriginAmmoSetData);
            CompEquippable.PrimaryVerb.verbProps = mainVerbProps;
            useSecondaryAmmo = false;
        }

        if (compAmmo.CurMagCount == 0 && compAmmo.IsEquippedGun)
        {
            TryReload();
        }

        if (Holder != null)
        {
            CompInventory.UpdateInventory();
        }
    }

    private void LoadAmmo(AmmoData data)
    {
        if (!HasAmmoUser || IsSharedAmmo)
        {
            return;
        }

        CompAmmo.ResetAmmoCount(data.selectedAmmo);
        CompAmmo.CurMagCount = data.curMagCount;
        CompAmmo.SelectedAmmo = data.selectedAmmo;
    }

    private void SaveAmmo(AmmoData data)
    {
        if (!HasAmmoUser)
        {
            return;
        }

        data.curMagCount = CompAmmo.CurMagCount;
        data.selectedAmmo = CompAmmo.SelectedAmmo;
    }

    private void LoadAmmoProps(CompProperties_AmmoUser ammoProps)
    {
        if (HasAmmoUser)
        {
            CompAmmo.props = ammoProps;
        }
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            PreSaveData();
        }

        Scribe_Values.Look(ref useSecondaryAmmo, "CE_useSecondaryAmmo");
        Scribe_Deep.Look(ref mainAmmo, "CE_mainAmmoData", null);
        Scribe_Deep.Look(ref secondaryAmmo, "CE_secondaryAmmoProps", null);
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            PostAmmoDataLoaded();
        }
    }

    private void PreSaveData()
    {
        if (CompAmmo.Props != secondaryAmmoProps)
        {
            MainAmmoData.curMagCount = CompAmmo.CurMagCount;
            MainAmmoData.selectedAmmo = CompAmmo.SelectedAmmo;
        }
        else
        {
            SecondaryAmmoData.curMagCount = CompAmmo.CurMagCount;
            SecondaryAmmoData.selectedAmmo = CompAmmo.SelectedAmmo;
        }
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
        if (Props.secondaryAmmoProps == null)
        {
            Log.Error("Prop secondaryAmmoProps not initialized");
        }

        if (Props.secondaryVerb == null)
        {
            Log.Error("Prop secondaryVerb not initialized");
        }

        InitData();
        Log.Message(IsSharedAmmo.ToString());
    }

    private void PostAmmoDataLoaded()
    {
        InitData();
        if (!useSecondaryAmmo)
        {
            return;
        }

        LoadAmmo(SecondaryAmmoData);
        LoadAmmoProps(secondaryAmmoProps);
        CompEquippable.PrimaryVerb.verbProps = secondaryVerbProps;
    }

    private void InitData()
    {
        //IL_003c: Unknown result type (might be due to invalid IL or missing references)
        //IL_0042: Expected O, but got Unknown
        //IL_0054: Unknown result type (might be due to invalid IL or missing references)
        //IL_005e: Expected O, but got Unknown
        mainVerbProps = CompEquippable.PrimaryVerb.verbProps;
        secondaryVerbProps = Props.secondaryVerb;
        if (secondaryAmmoCharges == null && Props.secondaryWeaponChargeSpeeds != null)
        {
            var val = new CompProperties_Charges
            {
                chargeSpeeds = Props.secondaryWeaponChargeSpeeds
            };
            secondaryAmmoCharges = new CompCharges
            {
                props = val
            };
        }

        if (HasAmmoUser)
        {
            originAmmoProps ??= CompAmmo.Props;

            secondaryAmmoProps ??= Props.secondaryAmmoProps;
        }

        if (Props.mainCommandIcon == "")
        {
            Props.mainCommandIcon = "UI/Buttons/Reload";
        }

        if (Props.secondaryCommandIcon == "")
        {
            Props.secondaryCommandIcon = "UI/Buttons/Reload";
        }

        CompProperties_AmmoUser val2 = null;
        var hashSet = new HashSet<AmmoDef>();
        IsSharedAmmo = true;
        foreach (var comp in parent.def.comps)
        {
            val2 = (CompProperties_AmmoUser)(comp is CompProperties_AmmoUser ? comp : null);
            if (val2 != null)
            {
                break;
            }
        }

        if (val2 == null || Props.secondaryAmmoProps.ammoSet.ammoTypes.Count != val2.ammoSet.ammoTypes.Count)
        {
            IsSharedAmmo = false;
            return;
        }

        foreach (var ammoType in Props.secondaryAmmoProps.ammoSet.ammoTypes)
        {
            hashSet.Add(ammoType.ammo);
        }

        foreach (var ammoType2 in val2.ammoSet.ammoTypes)
        {
            if (!hashSet.Contains(ammoType2.ammo))
            {
                IsSharedAmmo = false;
            }
        }
    }

    private void TryReload()
    {
        var job = compAmmo.TryMakeReloadJob();
        if (job == null)
        {
            return;
        }

        job.playerForced = true;
        var jobs = compAmmo.Holder.jobs;
        var jobCondition = JobCondition.Ongoing | JobCondition.Incompletable;
        jobs.StartJob(job, jobCondition, null, compAmmo.Holder.CurJob?.def != job.def);
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (CompAmmo is { UseAmmo: false } || Holder != null && !Holder.Faction.Equals(Faction.OfPlayer))
        {
            yield break;
        }

        yield return new Command_Action
        {
            action = SwitchLauncher,
            defaultDesc = Props.description,
            icon = ContentFinder<Texture2D>.Get(
                IsSecondaryAmmoSelected ? Props.secondaryCommandIcon : Props.mainCommandIcon, false),
            defaultLabel = IsSecondaryAmmoSelected ? Props.secondaryWeaponLabel : Props.mainWeaponLabel
        };
        if (!IsSharedAmmo)
        {
            yield return new GizmoSecondaryAmmoStatus
            {
                compAmmo = this
            };
        }
    }

    private void EnableChargeSpeed()
    {
        if (secondaryAmmoCharges == null || AttackVerb == null)
        {
            return;
        }

        parent.AllComps.Add(secondaryAmmoCharges);
        AttackVerb.compCharges = secondaryAmmoCharges;
    }

    private void DisableChargeSpeed()
    {
        if (secondaryAmmoCharges == null || AttackVerb == null)
        {
            return;
        }

        parent.AllComps.Remove(secondaryAmmoCharges);
        AttackVerb.compCharges = null;
    }
}