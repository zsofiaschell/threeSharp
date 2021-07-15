using AmusementPark.Properties;
using System;
using System.Drawing;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Field : Entity
    {
        public enum FieldTypes
        {
            Field,
            Water
        }

        public int BuildingWaterPrice { get; set; }

        public Field(FieldTypes fieldType)
        {
            FieldType = fieldType;
            Bitmap = new Bitmap(Resources.dirt, ScreenHeight, ScreenWidth); // set the picture of the field
            WaterBitmap = new Bitmap(Resources.water, ScreenHeight, ScreenWidth); // set the picture of the field on water
            BuildingWaterPrice = 20;
        }
    }
}
