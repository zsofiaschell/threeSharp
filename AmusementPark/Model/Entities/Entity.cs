using System;
using System.Drawing;
using System.Windows.Forms;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Entity //frame for other entities
    { 
        public Bitmap Bitmap { get; set; }
        public Bitmap WaterBitmap { get; set; }
        public int BuildingPrice { get; set; }
        public int BuildingPriceWater { get; set; }
        public Field.FieldTypes FieldType { get; set; }

        public int ScreenHeight { get { return (Screen.PrimaryScreen.Bounds.Height / 13) - 5; } }
        public int ScreenWidth { get { return (Screen.PrimaryScreen.Bounds.Width / 24) - 5; } }
    }
}
