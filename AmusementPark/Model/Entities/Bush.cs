using AmusementPark.Properties;
using System;
using System.Drawing;


namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Bush : Plant
    {
        public Bush()
        {
            Bitmap = new Bitmap(Resources.bush, ScreenHeight, ScreenWidth); //set the picture of bush
            HappinessBoost = 2; //set the boost value
            BuildingPrice = 50; //set the building price
        }
    }
}
