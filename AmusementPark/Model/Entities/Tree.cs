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
    public class Tree : Plant
    {
        public Tree() //constructor of tree
        {
            Bitmap = new Bitmap(Resources.tree, ScreenHeight, ScreenWidth); //set the pricture of tree
            HappinessBoost = 3; //set the happiness
            BuildingPrice = 100; //set the price
        }
    }
}
