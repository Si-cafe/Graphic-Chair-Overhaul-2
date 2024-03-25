using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace DoubleGraphicLayer
{
    // CompProperties
    public class CompProperties_SanguophageMeetingFire_OverLayer : CompProperties_FireOverlay
    {
        public CompProperties_SanguophageMeetingFire_OverLayer()
        {
            compClass = typeof(CompSanguophageMeetingFire_OverLayer);
        }
    }
    // ThingComp
    [StaticConstructorOnStartup]
    public class CompSanguophageMeetingFire_OverLayer : CompFireOverlayBase
    {
        public static readonly Graphic RedlightGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Redlight", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
        public new CompProperties_SanguophageMeetingFire_OverLayer Props => (CompProperties_SanguophageMeetingFire_OverLayer)props;
        public override void PostDraw()
        {
            base.PostDraw();
            CompGlower compGlower = parent.TryGetComp<CompGlower>();
            if (compGlower != null && !compGlower.Glows)
            {
                return;
            }
            CompSecondLayer comp = parent.GetComp<CompSecondLayer>();
            bool toggleSecondLayer = comp.ToggleSecondLayer;
            float altitude = toggleSecondLayer ? AltitudeLayer.PawnState.AltitudeFor() : parent.def.altitudeLayer.AltitudeFor();
            Vector3 drawPos = GenThing.TrueCenter(parent.Position, parent.Rotation, parent.def.size, altitude);
            drawPos.y += 0.04054054f;
            CompSanguophageMeetingFire.RedlightGraphic.Draw(drawPos + Props.offset, Rot4.North, parent, 0f);
        }
        public override bool CompPreventClaimingBy(Faction faction)
        {
            Lord lord = ((Building)parent).GetLord();
            return ((lord != null) ? lord.CurLordToil : null) is LordToil_SanguophageMeeting;
        }
    }
}