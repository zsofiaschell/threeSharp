using AmusementPark.Properties;
using System;
using System.Drawing;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Restaurant : Facility
    {
        public int ServiceTime { get; set; } // the time of the service in the restaurant

        public int GuestsInRestaurant { get; set; } // the number of the guests in the restaurant

        public bool IsServiceFinished { get; set; }

        private int _roundStart;

        public Bitmap Stopped_Electricity;
        public Bitmap Stopped_Road;
        public Bitmap Stopped_Both;
        public Bitmap Building;
        public Bitmap Working;
        public Bitmap InUse;

        public override States State
        {
            get => base.State; set
            {

                base.State = value;

                if (value == States.Building)
                {
                    Bitmap = Building; // set the picture of the restaurant in state building
                }
                else if (value == States.Working)
                {
                    if (HasElectricity && IsNextToConnectedRoad)
                    {
                        Bitmap = Working;
                        return;
                    }

                    if (!IsNextToConnectedRoad)
                    {
                        State = States.Stopped;
                        Bitmap = Stopped_Road;
                        return;
                    }

                    if (!HasElectricity)
                    {
                        State = States.Stopped;
                        Bitmap = Stopped_Electricity;
                        return;
                    }
                }
                else if (value == States.Stopped)
                {
                    if (!HasElectricity)
                    {
                        Bitmap = Stopped_Electricity; // set the picture of the restaurant in state stopped
                    }
                    if (!IsNextToConnectedRoad)
                    {
                        Bitmap = Stopped_Road;// set the picture of the restaurant in state stopped
                    }
                    if (!HasElectricity && !IsNextToConnectedRoad)
                    {
                        Bitmap = Stopped_Both; // set the picture of the restaurant in state stopped
                    }
                }
                else if(value == States.InUse)
                {
                    Bitmap = InUse; // set the picture of the restaurant in state stopped
                }
            }
        }

        public Restaurant() // constructor of the class restaurant
        {
            GuestsInRestaurant = 0;
            IsServiceFinished = false;
            PriceToKeepOpen = 5;
            BuildingPrice = 15;

            Bitmap = new Bitmap(Resources.restaurant_building, ScreenHeight, ScreenWidth); // set the picture of the restaurant in state building
            
            Stopped_Electricity = new Bitmap(Resources.restaurant_stopped, ScreenHeight, ScreenWidth);
            Stopped_Road = new Bitmap(Resources.restaurant_stopped_road, ScreenHeight, ScreenWidth);
            Stopped_Both = new Bitmap(Resources.restaurant_stopped_both, ScreenHeight, ScreenWidth);
            Building = new Bitmap(Resources.restaurant_building, ScreenHeight, ScreenWidth);
            Working = new Bitmap(Resources.restaurant, ScreenHeight, ScreenWidth);
            InUse = new Bitmap(Resources.restaurant_inUse, ScreenHeight, ScreenWidth);

            State = States.Building;
        }

        public void StartUse()
        {
            _roundStart = 0;
        }

        public void AdvanceTime()
        {
            if (State == States.InUse)
            {
                _roundStart++;
                if (_roundStart > ServiceTime)
                {
                    IsServiceFinished = true;
                }
            }
        }
    }
}
