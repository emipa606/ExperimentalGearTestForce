using UnityEngine;
using Verse;

namespace WeaponUtility.CE;

[StaticConstructorOnStartup]
public class GizmoSecondaryAmmoStatus : Gizmo
{
    private static Texture2D FullTex;

    private static Texture2D EmptyTex;

    private static Texture2D BGTex;

    private static bool inited;
    public CompSecondaryAmmo compAmmo;

    private string Label { get; set; }

    public override float GetWidth(float maxWidth)
    {
        return 120f;
    }

    public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
    {
        if (!inited)
        {
            InitializeTextures();
        }

        var rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
        Widgets.DrawBox(rect);
        GUI.DrawTexture(rect, BGTex);
        var rect2 = rect.ContractedBy(6f);
        var rect3 = rect2;
        rect3.height = rect.height / 2f;
        Text.Font = GameFont.Tiny;
        Label = compAmmo.IsSecondaryAmmoSelected ? compAmmo.MainAmmoName : compAmmo.SecondaryAmmoName;
        Widgets.Label(rect3, Label);
        if (compAmmo.Props.secondaryAmmoProps.magazineSize > 0)
        {
            var rect4 = rect2;
            rect4.yMin = rect.y + (rect.height / 2f);
            var fillPercent = compAmmo.ScondaryMagCount / (float)compAmmo.ScondaryMagSize;
            Widgets.FillableBar(rect4, fillPercent, FullTex);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect4, $"{compAmmo.ScondaryMagCount} / {compAmmo.ScondaryMagSize}");
            Text.Anchor = TextAnchor.UpperLeft;
        }

        if (Mouse.IsOver(rect))
        {
            TooltipHandler.TipRegion(rect, "PLA_NotInUse".Translate());
        }

        return new GizmoResult(GizmoState.Clear);
    }

    public override bool GroupsWith(Gizmo other)
    {
        return other is GizmoSecondaryAmmoStatus;
    }

    private static void InitializeTextures()
    {
        if (FullTex == null)
        {
            FullTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));
        }

        if (EmptyTex == null)
        {
            EmptyTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
        }

        if (BGTex == null)
        {
            BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG");
        }

        inited = true;
    }
}