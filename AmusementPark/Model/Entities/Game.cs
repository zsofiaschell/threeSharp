using AmusementPark.Properties;
using System;
using System.Drawing;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Game : Facility
    {
        #region Properties
        public int RoundTime { get; set; }

        public bool IsRoundFinished { get; set; } // status of game working

        private int _roundStart;

        public int PriceToPayWhenUsed { get; set; } // the price of one round
        public int MinimumCapacity { get; set; } // minimum count of people on the game

        public int GuestsInGame { get; set; }

        public Bitmap Stopped_Electricity;
        public Bitmap Stopped_Road;
        public Bitmap Stopped_Both;
        public Bitmap Building;
        public Bitmap Working;
        public Bitmap InUse;

        public Bitmap WaterStopped_Electricity;
        public Bitmap WaterStopped_Road;
        public Bitmap WaterStopped_Both;
        public Bitmap WaterBuilding;
        public Bitmap WaterWorking;
        public Bitmap WaterInUse;


        public override States State
        {
            get => base.State; set
            {
                base.State = value;

                if (value == States.Building)
                {
                    Bitmap = Building; // set the picture of the game
                    WaterBitmap = WaterBuilding;
                }
                else if (value == States.Working)
                {
                    if (!HasElectricity && !IsNextToConnectedRoad)
                    {
                        State = States.Stopped;
                        Bitmap = Stopped_Both;
                        WaterBitmap = WaterStopped_Both;
                        return;
                    }

                    if (!HasElectricity)
                    {
                        State = States.Stopped;
                        Bitmap = Stopped_Electricity;
                        WaterBitmap = WaterStopped_Electricity;
                        return;
                    }

                    if (!IsNextToConnectedRoad)
                    {
                        State = States.Stopped;
                        Bitmap = Stopped_Road;
                        WaterBitmap = WaterStopped_Road;
                        return;
                    }

                    Bitmap = Working;
                    WaterBitmap = WaterWorking;
                }
                else if (value == States.Stopped)
                {
                    if (!HasElectricity)
                    {
                        Bitmap = Stopped_Electricity;
                        WaterBitmap = WaterStopped_Electricity;
                    }
                    if (!IsNextToConnectedRoad)
                    {
                        Bitmap = Stopped_Road;
                        WaterBitmap = WaterStopped_Road;
                    }
                    if (!HasElectricity && !IsNextToConnectedRoad)
                    {
                        Bitmap = Stopped_Both;
                        WaterBitmap = WaterStopped_Both;
                    }
                }
                else if(value == States.InUse)
                {
                    Bitmap = InUse;
                    WaterBitmap = WaterInUse;
                }
            }
        }

        #endregion

        #region Constructor
        public Game() //constructor of game
        {
            GuestsInGame = 0;
            IsRoundFinished = false;
        }

        #endregion

        public void StartUse()
        {
            _roundStart = 0;
        }

        public void AdvanceTime()
        {
            if (State == States.InUse)
            {
                _roundStart++;
                if (_roundStart > RoundTime)
                {
                    IsRoundFinished = true;
                }
            }
        }
    }
}
