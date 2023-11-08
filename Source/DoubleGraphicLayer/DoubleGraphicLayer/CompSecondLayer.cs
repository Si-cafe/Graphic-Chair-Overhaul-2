using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DoubleGraphicLayer
{
    public class CompSecondLayer : ThingComp
    {
        private Graphic graphicInt;
        public bool toggleSecondLayer = LoadedModManager.GetMod<SecondLayerMod>().GetSettings<SecondLayerSettings>().defaultToggle;
        public Graphic_Multi secondLayer;
        public Graphic_Single secondLayerSingle;

        private string defaultLayerPath;
        private string styleLayerPath;

        public CompProperties_SecondLayer Props
        {
            get
            {
                return (CompProperties_SecondLayer)this.props;
            }
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            GraphicData propsData = this.Props.graphicData;
            GraphicData parentData = this.parent.def.graphicData;
            propsData.drawSize = parentData.drawSize;
            propsData.graphicClass = parentData.graphicClass;
            propsData.shaderType = parentData.shaderType;
            propsData.drawOffset = parentData.drawOffset;
            propsData.drawOffsetNorth = parentData.drawOffsetNorth;
            propsData.drawOffsetEast = parentData.drawOffsetEast;
            propsData.drawOffsetSouth = parentData.drawOffsetSouth;
            propsData.drawOffsetWest = parentData.drawOffsetWest;
            LongEventHandler.ExecuteWhenFinished(delegate { SecondLayerAssign(); });
        }

        public void SecondLayerAssign()
        {
            try
            {
                ShaderTypeDef layerShader = this.parent.def.graphicData.shaderType;
                Vector2 layerSize = this.parent.def.graphicData.drawSize;
                Color layerColor = this.parent.Graphic.Color;
                Color layerColotwo = this.parent.Graphic.ColorTwo;
                GraphicData layerData = this.Props.graphicData;
                ThingStyleDef styleDef = new ThingStyleDef();
                Type graphicClass = Props.graphicData.graphicClass;
                if (graphicClass == typeof(Graphic_Multi))
                {
                    if (styleDef != null && this.parent.StyleDef != null)
                    {
                        ShaderTypeDef styleShader = this.parent.StyleDef.graphicData.shaderType;
                        Vector2 styleSize = this.parent.StyleDef.graphicData.drawSize;
                        styleLayerPath = this.parent.StyleDef.graphicData.texPath + "_back";
                        secondLayer = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(styleLayerPath, styleShader.Shader, styleSize, layerColor, layerColotwo, layerData);
                    }
                    else
                    {
                        defaultLayerPath = parent.Graphic.path + "_back";
                        secondLayer = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(defaultLayerPath, layerShader.Shader, layerSize, layerColor, layerColotwo, layerData);
                    }
                    this.graphicInt = secondLayer;
                }
                else if (graphicClass == typeof(Graphic_Single))
                {
                    if (styleDef != null && this.parent.StyleDef != null)
                    {
                        ShaderTypeDef styleShader = this.parent.StyleDef.graphicData.shaderType;
                        Vector2 styleSize = this.parent.StyleDef.graphicData.drawSize;
                        styleLayerPath = this.parent.StyleDef.graphicData.texPath + "_back";
                        secondLayerSingle = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>(styleLayerPath, styleShader.Shader, styleSize, layerColor, layerColotwo, layerData);
                    }
                    else
                    {
                        defaultLayerPath = parent.Graphic.path + "_back";
                        secondLayerSingle = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>(defaultLayerPath, layerShader.Shader, layerSize, layerColor, layerColotwo, layerData);
                    }
                    this.graphicInt = secondLayerSingle;
                }
            } 
            catch (Exception)
            {
                Log.Message("::Graphic Chair Overhaul => Not applicable to " + this.parent.def.defName + ".");
            }
        }
        public virtual Graphic Graphic
        {
            get
            {
                if (this.graphicInt == null)
                {
                    return BaseContent.BadGraphic;
                }
                return this.graphicInt;
            }
        }
        public override void PostDraw()
        {
            bool flag1 = toggleSecondLayer;
            bool flag2 = this.parent.Rotation == Rot4.North || this.Props.allwaysShowGizmo;
            if (flag1 && flag2)
            {
                base.PostDraw();
                Vector3 loc = GenThing.TrueCenter(this.parent.Position, this.parent.Rotation, this.parent.def.size, this.Props.Altitude);
                this.Graphic.Draw(loc, this.parent.Rotation, this.parent);
            }
        }
        public override void PostExposeData()
        {
            Scribe_Values.Look<bool>(ref this.toggleSecondLayer, "toggleSecondLayer", true);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            bool flag3 = this.parent.Rotation == Rot4.North || this.Props.allwaysShowGizmo == true;         
            if (flag3)
            {
                bool flag4 = LoadedModManager.GetMod<SecondLayerMod>().GetSettings<SecondLayerSettings>().showGizmo;
                bool flag5 = this.Graphic.Color != this.parent.Graphic.Color || (this.Graphic.path != this.parent.Graphic.path + "_back");
                if (flag4)
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
                if (flag5)
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "DoubleGraphicLayer_Update".Translate(),
                        defaultDesc = "DoubleGraphicLayer_UpdateDesc".Translate(),

                        icon = ContentFinder<Texture2D>.Get("UI/DGL_Update", true),
                        action = delegate ()
                        {
                            SoundDefOf.Designate_Paint.PlayOneShotOnCamera(null);
                            LongEventHandler.ExecuteWhenFinished(delegate { SecondLayerAssign(); });
                        }
                    };
                }
            }
            yield break;
        }
    }
}