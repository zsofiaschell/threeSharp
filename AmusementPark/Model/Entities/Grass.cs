using AmusementPark.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Grass : Plant
    {
        public Grass()
        {
            Bitmap = new Bitmap(Resources.grass, ScreenHeight, ScreenWidth);
            HappinessBoost = 1; //set happiness for grass
            BuildingPrice = 20; //set the price
        }
    }
}
