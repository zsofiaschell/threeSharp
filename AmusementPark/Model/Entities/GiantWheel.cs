using AmusementPark.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class GiantWheel : Game
    {
        public GiantWheel() //constructor of game
        {
            GuestsInGame = 0;
            IsRoundFinished = false;
            PriceToKeepOpen = 10;
            BuildingTimeInSec = 10;
            BuildingPrice = 50;

            //Set images
            InUse = new Bitmap(Resources.giantwheel_inUse, ScreenHeight, ScreenWidth);
            Working = new Bitmap(Resources.giantwheel, ScreenHeight, ScreenWidth);
            Building = new Bitmap(Resources.giatwheel_building, ScreenHeight, ScreenWidth);
            Stopped_Road = new Bitmap(Resources.giantwheel_stopped_road, ScreenHeight, ScreenWidth);
            Stopped_Electricity = new Bitmap(Resources.giantwheel_stopped, ScreenHeight, ScreenWidth);
            Stopped_Both = new Bitmap(Resources.giantwheel_stopped_both, ScreenHeight, ScreenWidth);

            State = States.Building;
        }
    }
}
