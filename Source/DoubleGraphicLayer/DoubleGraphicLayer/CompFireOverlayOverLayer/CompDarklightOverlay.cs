using RimWorld;
using UnityEngine;
using Verse;

namespace DoubleGraphicLayer
{
    // CompProperties
    public class CompProperties_DarklightOverlay_OverLayer : CompProperties_DarklightOverlay
    {
        public CompProperties_DarklightOverlay_OverLayer()
        {
            compClass = typeof(CompDarklightOverlay_OverLayer);
        }
    }

    // ThingComp
    [StaticConstructorOnStartup]
    public class CompDarklightOverlay_OverLayer : CompFireOverlayBase
    {
        protected CompRefuelable refuelableComp;
        public static readonly Graphic DarklightGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Darklight", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);

        public new CompProperties_DarklightOverlay_OverLayer Props => (CompProperties_DarklightOverlay_OverLayer)props;
        public override void PostDraw()
        {
            base.PostDraw();
            if (refuelableComp != null && !refuelableComp.HasFuel)
            {
                return;
            }
            CompSecondLayer comp = parent.GetComp<CompSecondLayer>();
            bool toggleSecondLayer = comp.ToggleSecondLayer;
            float altitude = toggleSecondLayer ? AltitudeLayer.PawnState.AltitudeFor() : parent.def.altitudeLayer.AltitudeFor();
            Vector3 drawPos = GenThing.TrueCenter(parent.Position, parent.Rotation, parent.def.size, altitude);
            drawPos.y += 0.04054054f;
            CompDarklightOverlay.DarklightGraphic.Draw(drawPos, Rot4.North, parent, 0f);
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            refuelableComp = parent.GetComp<CompRefuelable>();
        }
    }
}
