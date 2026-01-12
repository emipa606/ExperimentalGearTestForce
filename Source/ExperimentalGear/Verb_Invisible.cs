using Verse;

namespace ExperimentalGear;

public class Verb_Invisible : Verb
{
    protected override bool TryCastShot()
    {
        var named = DefDatabase<HediffDef>.GetNamed("PsychicInvisibility");
        if (named == null || CasterPawn == null)
        {
            return false;
        }

        var hediff = HediffMaker.MakeHediff(named, CasterPawn);
        hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 600;
        CasterPawn.health.AddHediff(hediff);
        ReloadableCompSource?.UsedOnce();
        return true;
    }
}