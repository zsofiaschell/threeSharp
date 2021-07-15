using AmusementPark.Properties;
using System;
using System.Drawing;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Electricity : Entity
    {
        public int Range { get; set; }

        public Electricity()
        { // constructor to Electricity class
            Bitmap = new Bitmap(Resources.fusebox, ScreenHeight, ScreenWidth); // set the picture of the fuse box
        }
    }
}
