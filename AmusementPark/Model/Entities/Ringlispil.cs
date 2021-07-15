using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AmusementPark.Properties;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Ringlispil : Game
    {
        #region Constructor
        public Ringlispil() //constructor of game
        {
            GuestsInGame = 0;
            IsRoundFinished = false;
            PriceToKeepOpen = 10;
            BuildingTimeInSec = 10;
            BuildingPrice = 40;

            //Set images
            InUse = new Bitmap(Resources.ringlispil_inUse, ScreenHeight, ScreenWidth);
            Working = new Bitmap(Resources.ringlispil, ScreenHeight, ScreenWidth);
            Building = new Bitmap(Resources.ringlispil_building, ScreenHeight, ScreenWidth);
            Stopped_Road = new Bitmap(Resources.ringlispil_stopped_road, ScreenHeight, ScreenWidth);
            Stopped_Electricity = new Bitmap(Resources.ringlispil_stopped_electricity, ScreenHeight, ScreenWidth);
            Stopped_Both = new Bitmap(Resources.ringlispil_stopped_both, ScreenHeight, ScreenWidth);

            State = States.Building;
        }

        #endregion

        

        
    }
}
