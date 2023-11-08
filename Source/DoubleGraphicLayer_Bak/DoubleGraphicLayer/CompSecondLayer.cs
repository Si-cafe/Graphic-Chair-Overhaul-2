using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DoubleGraphicLayer
{
    internal class CompSecondLayer : ThingComp
    {
        private Graphic graphicInt;
        public bool toggleSecondLayer = true;

        public CompProperties_SecondLayer Props
        {
            get
            {
                return (CompProperties_SecondLayer)this.props;
            }
        }
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            this.Props.graphicData.texPath = this.parent.def.graphicData.texPath + "_back";
            this.Props.graphicData.drawSize = this.parent.def.graphicData.drawSize;
            this.Props.graphicData.graphicClass = this.parent.def.graphicData.graphicClass;
            this.Props.graphicData.shaderType = this.parent.def.graphicData.shaderType;
            this.Props.graphicData.drawOffset = this.parent.def.graphicData.drawOffset;
            this.Props.graphicData.drawOffsetNorth = this.parent.def.graphicData.drawOffsetNorth;
            this.Props.graphicData.drawOffsetEast = this.parent.def.graphicData.drawOffsetEast;
            this.Props.graphicData.drawOffsetSouth = this.parent.def.graphicData.drawOffsetSouth;
            this.Props.graphicData.drawOffsetWest = this.parent.def.graphicData.drawOffsetWest;
        }

        public virtual Graphic Graphic
        { 
            get
            {
                if (this.graphicInt == null)
                {
                    this.graphicInt = this.Props.graphicData.GraphicColoredFor(this.parent);
                }
                return this.graphicInt;
            }
        }
        public override void PostDraw()
        {
            if (toggleSecondLayer)
            {
                base.PostDraw();
                this.Graphic.Draw(GenThing.TrueCenter(this.parent.Position, this.parent.Rotation, this.parent.def.size, this.Props.Altitude), this.parent.Rotation, this.parent);
            }
        }
        public override void PostExposeData()
        {
            Scribe_Values.Look<bool>(ref this.toggleSecondLayer, "toggleSecondLayer", true);
        }

        public void SecondLayerColorUpdate()
        {
            SoundDefOf.Designate_Paint.PlayOneShotOnCamera(null);
            this.graphicInt = this.Props.graphicData.GraphicColoredFor(this.parent);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            //IEnumerator<Gizmo> enumerator = null;
            if (this.parent.Rotation == Rot4.North || this.Props.allwaysShowGizmo == true)
            {
                if (LoadedModManager.GetMod<SecondLayerMod>().GetSettings<SecondLayerSettings>().showGizmo)
                {
                    yield return new Command_Toggle
                    {
                        defaultLabel = "DoubleGraphicLayer_Toggle".Translate(),
                        defaultDesc = "DoubleGraphicLayer_ToggleDesc".Translate(),

                        icon = ContentFinder<Texture2D>.Get("UI/DGL_Toggle", true),
                        isActive = (() => this.toggleSecondLayer),
                        toggleAction = delegate ()
                        {
                            this.toggleSecondLayer = !this.toggleSecondLayer;
                        }
                    };
                } 
                if (toggleSecondLayer && this.graphicInt.Color != this.parent.Graphic.Color)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "DoubleGraphicLayer_ColorUpdate".Translate(),
                        defaultDesc = "DoubleGraphicLayer_ColorUpdateDesc".Translate(),

                        icon = ContentFinder<Texture2D>.Get("UI/DGL_Update", true),
                        action = delegate ()
                        {
                            LongEventHandler.ExecuteWhenFinished(delegate { SecondLayerColorUpdate(); });
                        }
                    };
                }
            }
            yield break;
        }
    }
}