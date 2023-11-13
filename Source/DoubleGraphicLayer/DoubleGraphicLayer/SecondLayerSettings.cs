using Verse;

namespace DoubleGraphicLayer
{
    public class SecondLayerSettings : ModSettings
    {
        public bool showGizmo;
        public bool showUpdateGizmo;
        public bool defaultToggle;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref showGizmo, "DoubleGraphicLayer_ShowGizmo", true, false);
            Scribe_Values.Look(ref showUpdateGizmo, "DoubleGraphicLayer_ShowUpdateGizmo", true, false);
            Scribe_Values.Look(ref defaultToggle, "DoubleGraphicLayer_DefaultToggle", true, true);
        }
    }

    public class SecondLayerMod : Mod
    {
        private readonly SecondLayerSettings settings;

        public SecondLayerMod(ModContentPack content) : base(content) => settings = GetSettings<SecondLayerSettings>();

        public override void DoSettingsWindowContents(UnityEngine.Rect rect)
        {
            Listing_Standard listing_Standard = new();
            listing_Standard.Begin(rect);
            listing_Standard.CheckboxLabeled("DoubleGraphicLayer_ShowGizmo".Translate(), ref settings.showGizmo, "DoubleGraphicLayer_ShowGizmoTip".Translate());
            listing_Standard.CheckboxLabeled("DoubleGraphicLayer_ShowUpdateGizmo".Translate(), ref settings.showUpdateGizmo, "DoubleGraphicLayer_ShowUpdateGizmoTip".Translate());
            listing_Standard.CheckboxLabeled("DoubleGraphicLayer_DefaultToggle".Translate(), ref settings.defaultToggle, "DoubleGraphicLayer_DefaultToggle".Translate());
            listing_Standard.End();
        }

        public override string SettingsCategory() => "DoubleGraphicLayer_ModName".Translate();


    }
}