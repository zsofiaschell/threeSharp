using AmusementPark.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class WaterGame : Game
    {
        public WaterGame() //constructor of game
        {
            //WaterBitmap = new Bitmap(Resources.tree, ScreenHeight, ScreenWidth);
            GuestsInGame = 0;
            IsRoundFinished = false;
            PriceToKeepOpen = 20;
            PriceToUse = 20;

            BuildingPrice = 70;
            BuildingTimeInSec = 4;
            PriceToKeepOpen = 10;
            
            //Set images
            WaterInUse = new Bitmap(Resources.water_game_inUse, ScreenHeight, ScreenWidth);
            WaterWorking = new Bitmap(Resources.water_game, ScreenHeight, ScreenWidth);
            WaterBuilding = new Bitmap(Resources.water_game_building, ScreenHeight, ScreenWidth);
            WaterStopped_Road = new Bitmap(Resources.giantwheel_stopped_road, ScreenHeight, ScreenWidth);
            WaterStopped_Electricity = new Bitmap(Resources.water_game_stopped_electricity, ScreenHeight, ScreenWidth);
            WaterStopped_Both = new Bitmap(Resources.water_game_stopped_both, ScreenHeight, ScreenWidth);

            State = States.Building;
        }
    }
}
