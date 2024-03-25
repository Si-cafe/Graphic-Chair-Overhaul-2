using RimWorld;
using UnityEngine;
using Verse;

namespace DoubleGraphicLayer
{
    // CompProperties
    public class CompProperties_FireOverlay_OverLayer : CompProperties_FireOverlay
    {
        public CompProperties_FireOverlay_OverLayer()
        {
            compClass = typeof(CompFireOverlay_OverLayer);
        }
    }

    // ThingComp
    [StaticConstructorOnStartup]
    public class CompFireOverlay_OverLayer : CompFireOverlayBase
    {
        protected CompRefuelable refuelableComp;

        public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);

        public new CompProperties_FireOverlay_OverLayer Props => (CompProperties_FireOverlay_OverLayer)props;
        public override void PostDraw()
        {
            if (refuelableComp != null && !refuelableComp.HasFuel)
            {
                return;
            }
            CompSecondLayer comp = parent.GetComp<CompSecondLayer>();
            bool toggleSecondLayer = comp.ToggleSecondLayer;
            float altitude = toggleSecondLayer ? AltitudeLayer.PawnState.AltitudeFor() : parent.def.altitudeLayer.AltitudeFor();
            Vector3 drawPos = GenThing.TrueCenter(parent.Position, parent.Rotation, parent.def.size, altitude);
            drawPos.y += 0.04054054f;
            CompFireOverlay.FireGraphic.Draw(drawPos, Rot4.North, parent, 0f);
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            refuelableComp = parent.GetComp<CompRefuelable>();
        }
        public override void CompTick()
        {
            if (refuelableComp != null && !refuelableComp.HasFuel)
            {
                return;
            }
            if (startedGrowingAtTick < 0)
            {
                startedGrowingAtTick = GenTicks.TicksAbs;
            }
        }
    }
}