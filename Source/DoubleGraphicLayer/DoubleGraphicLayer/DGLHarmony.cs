using RimWorld;
using Verse;

namespace DoubleGraphicLayer
{
    public class DGLHarmony : Mod
    {
        public static string harmID = "rimworld.DGL.harmony";

        public DGLHarmony(ModContentPack content) : base(content)
        {
            // Check if Harmony is loaded
            try
            {
                Patch();
            }
            catch
            {
            }
        }

        // Separate function for the check to work
        private static void Patch()
        {
            HarmonyLib.Harmony harm = new(harmID);
            harm.PatchAll();

            System.Type harmType = typeof(DGLHarmony);

            // Reassign the second layer when the color changes
            harm.Patch(
                typeof(Thing).GetMethod(nameof(Thing.Notify_ColorChanged)),
                postfix: new(harmType, nameof(Postfix_SecondLayerAssign_Color))
            );

            // Reassign the second layer when the style changes
            harm.Patch(
                typeof(ThingStyleHelper).GetMethod(nameof(ThingStyleHelper.SetStyleDef)),
                postfix: new(harmType, nameof(Postfix_SecondLayerAssign_Style))
            );
        }

        // the name of the Thing must match the signature of the patched method
        private static void Postfix_SecondLayerAssign_Color(Thing __instance)
        {
            Postfix_SecondLayerAssign_Style(__instance);
        }

        private static void Postfix_SecondLayerAssign_Style(Thing thing)
        {
            thing.TryGetComp<CompSecondLayer>()?.LongEventHandlerSecondLayerAssign();
        }
    }
}

