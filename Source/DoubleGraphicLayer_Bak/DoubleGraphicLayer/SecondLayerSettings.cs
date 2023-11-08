using System;
using UnityEngine;
using Verse;

namespace DoubleGraphicLayer
{
    public class SecondLayerSettings : ModSettings
    {
        public bool showGizmo;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.showGizmo, "DoubleGraphicLayer_ShowGizmo", true, false);
        }
    }

    public class SecondLayerMod : Mod 
    {
        SecondLayerSettings settings;
        public SecondLayerMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<SecondLayerSettings>();
        }
        public override void DoSettingsWindowContents(Rect rect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect);
            listing_Standard.CheckboxLabeled("DoubleGraphicLayer_ShowGizmo".Translate(), ref settings.showGizmo, "DoubleGraphicLayer_ShowGizmoTip".Translate());
            listing_Standard.End();
        }
        public override string SettingsCategory()
        {
            return "DoubleGraphicLayer_ModName".Translate();
        }


    }
}