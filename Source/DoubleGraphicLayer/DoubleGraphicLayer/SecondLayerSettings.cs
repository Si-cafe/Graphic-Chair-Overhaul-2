using System;
using UnityEngine;
using Verse;

namespace DoubleGraphicLayer
{
    public class SecondLayerSettings : ModSettings
    {
        public bool showGizmo;
        public bool defaultToggle;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.showGizmo, "DoubleGraphicLayer_ShowGizmo", true, false);
            Scribe_Values.Look<bool>(ref this.defaultToggle, "DoubleGraphicLayer_DefaultToggle", true, true);
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
            listing_Standard.CheckboxLabeled("DoubleGraphicLayer_DefaultToggle".Translate(), ref settings.defaultToggle, "DoubleGraphicLayer_DefaultToggle".Translate());
            listing_Standard.End();
        }
        public override string SettingsCategory()
        {
            return "DoubleGraphicLayer_ModName".Translate();
        }


    }
}