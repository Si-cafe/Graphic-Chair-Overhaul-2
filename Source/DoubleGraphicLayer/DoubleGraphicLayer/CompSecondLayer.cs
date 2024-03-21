using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DoubleGraphicLayer
{
    public class CompProperties_SecondLayer : CompProperties
    {
        public GraphicData graphicData = null;
        public AltitudeLayer altitudeLayer = AltitudeLayer.PawnState;

        public bool allwaysShowGizmo = false;

        public float Altitude => Altitudes.AltitudeFor(altitudeLayer);

        public CompProperties_SecondLayer() => compClass = typeof(CompSecondLayer);
    }

    public class CompSecondLayer : ThingComp
    {
        private static readonly SecondLayerSettings Settings = LoadedModManager.GetMod<SecondLayerMod>().GetSettings<SecondLayerSettings>();

        private Graphic graphicInt;
        public bool toggleSecondLayer = Settings.defaultToggle;

        private bool ShowSecondLayer => parent.Rotation == Rot4.North || Props.allwaysShowGizmo;

        public CompProperties_SecondLayer Props => (CompProperties_SecondLayer)props;

        // Mod "Faster Game Loading" delays the loading of graphics (optional), so graphicInt might be null
        public virtual Graphic Graphic
        {
            get
            {
                if (graphicInt == null) SecondLayerAssign();
                return graphicInt ?? BaseContent.BadGraphic;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            var propsData = Props.graphicData;
            var parentData = parent.def.graphicData;

            propsData.drawSize = parentData.drawSize;
            propsData.graphicClass = parentData.graphicClass;
            propsData.shaderType = parentData.shaderType;
            propsData.drawOffset = parentData.drawOffset;
            propsData.drawOffsetNorth = parentData.drawOffsetNorth;
            propsData.drawOffsetEast = parentData.drawOffsetEast;
            propsData.drawOffsetSouth = parentData.drawOffsetSouth;
            propsData.drawOffsetWest = parentData.drawOffsetWest;

            LongEventHandlerSecondLayerAssign();
        }

        public override void PostDraw()
        {
            if (toggleSecondLayer && ShowSecondLayer)
            {
                base.PostDraw();
                var loc = GenThing.TrueCenter(parent.Position, parent.Rotation, parent.def.size, Props.Altitude);
                Graphic.Draw(loc, parent.Rotation, parent);
            }
        }

        public override void PostExposeData() => Scribe_Values.Look(ref toggleSecondLayer, "toggleSecondLayer", true);

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra()) yield return gizmo;

            if (ShowSecondLayer)
            {
                if (Settings.showGizmo) yield return new Command_Toggle
                {
                    defaultLabel = "DoubleGraphicLayer_Toggle".Translate(),
                    defaultDesc = "DoubleGraphicLayer_ToggleDesc".Translate(),

                    icon = ContentFinder<Texture2D>.Get("UI/DGL_Toggle", true),
                    isActive = () => toggleSecondLayer,
                    toggleAction = () => toggleSecondLayer = !toggleSecondLayer
                };
                // If Harmony is used, the patches should make this unnecessary 
                if (Graphic.Color != parent.Graphic.Color || (Graphic.path != $"{parent.Graphic.path}_back")) yield return new Command_Action
                {
                    defaultLabel = "DoubleGraphicLayer_Update".Translate(),
                    defaultDesc = "DoubleGraphicLayer_UpdateDesc".Translate(),

                    icon = ContentFinder<Texture2D>.Get("UI/DGL_Update", true),
                    action = () =>
                    {
                        SoundDefOf.Designate_Paint.PlayOneShotOnCamera(null);
                        LongEventHandlerSecondLayerAssign();
                    }
                };
            }
            yield break;
        }

        public void LongEventHandlerSecondLayerAssign() => LongEventHandler.ExecuteWhenFinished(SecondLayerAssign);

        public void SecondLayerAssign()
        {
            try
            {
                var graphicClass = Props.graphicData.graphicClass;

                if (graphicClass == typeof(Graphic_Multi)) SecondLayerAssignInt<Graphic_Multi>();
                else if (graphicClass == typeof(Graphic_Single)) SecondLayerAssignInt<Graphic_Multi>();
            }
            catch (System.Exception)
            {
                // Log.Error() helps in debugging, but it's not necessary
                Log.Error($"::Graphic Chair Overhaul => Not applicable to {parent.def.defName}.");
            }
        }

        private void SecondLayerAssignInt<G>() where G : Graphic, new()
        {
            var graphic = parent.Graphic;
            var data = parent.StyleDef?.graphicData;

            var path = $"{data?.texPath ?? graphic.path}_back";
            data ??= parent.def.graphicData;
            var shaderType = data.shaderType;

            // Mod "Faster Game Loading" delays the loading of graphics (optional), so shaderType might be null
            if (shaderType == null) return;

            graphicInt = GraphicDatabase.Get<G>(path, shaderType.Shader, data.drawSize, graphic.Color, graphic.ColorTwo, Props.graphicData);
        }
    }
}