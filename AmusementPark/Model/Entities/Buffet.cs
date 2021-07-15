using AmusementPark.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Buffet : Restaurant
    {


        public Buffet() // constructor of the class buffet
        {
            GuestsInRestaurant = 0;
            IsServiceFinished = false;
            PriceToKeepOpen = 1;
            BuildingTimeInSec = 3;
            BuildingPrice = 10;

            Stopped_Electricity = new Bitmap(Resources.buffet_stopped_electricity, ScreenHeight, ScreenWidth);
            Stopped_Road = new Bitmap(Resources.buffet_stopped_road, ScreenHeight, ScreenWidth);
            Stopped_Both = new Bitmap(Resources.buffet_stopped_both, ScreenHeight, ScreenWidth);
            Building = new Bitmap(Resources.buffet_building, ScreenHeight, ScreenWidth);
            Working = new Bitmap(Resources.buffet, ScreenHeight, ScreenWidth);
            InUse = new Bitmap(Resources.buffet_inUse, ScreenHeight, ScreenWidth);

            State = States.Building;
        }
    }
}
