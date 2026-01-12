using System;
using CombatExtended;
using UnityEngine;
using Verse;

namespace WeaponUtility.CE;

public class ProjectileGuidedBulletCE : BulletCE
{
    private Vector3 launcherPos;

    private float Distance { get; set; }

    public Vector3 Origin => launcherPos;

    private Vector3 Direction
    {
        get
        {
            field.x = TargetPos.x - launcherPos.x;
            field.y = TargetPos.y - launcherPos.y;
            return field.normalized;
        }
    } = default;

    private Vector3 TargetPos
    {
        get
        {
            field.x = intendedTargetThing.DrawPos.x;
            field.y = intendedTargetThing.DrawPos.z;
            return field;
        }
    } = default;

    private Vector3 GuidedDestination => launcherPos + (Direction * Distance);

    public override void Launch(Thing launcher, Vector2 origin, Thing equipment = null)
    {
        launcherPos.x = origin.x;
        launcherPos.y = origin.y;
        base.Launch(launcher, origin, equipment);
        double num = Destination.x - origin.x;
        double num2 = Destination.y - origin.y;
        Distance = (float)Math.Sqrt((num * num) + (num2 * num2));
    }

    public override void Tick()
    {
        if (intendedTargetThing != null)
        {
            Destination.x = GuidedDestination.x;
            Destination.y = GuidedDestination.y;
        }

        base.Tick();
    }
}