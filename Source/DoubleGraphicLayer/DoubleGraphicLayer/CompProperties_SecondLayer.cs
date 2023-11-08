using Verse;
using System.Collections.Generic;

namespace DoubleGraphicLayer
{
    public class CompProperties_SecondLayer : CompProperties
    {
        public GraphicData graphicData = null;
        public AltitudeLayer altitudeLayer = AltitudeLayer.PawnState;
        public float Altitude
        {
            get
            {
                return Altitudes.AltitudeFor(this.altitudeLayer);
            }
        }
        public bool allwaysShowGizmo = false;
        public CompProperties_SecondLayer()
        {
            this.compClass = typeof(CompSecondLayer);
        }
    }
}
