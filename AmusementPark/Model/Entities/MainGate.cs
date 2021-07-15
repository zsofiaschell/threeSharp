using AmusementPark.Properties;
using System;
using System.Drawing;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class MainGate : Road
    {
        private bool _isClosed;
        public int EnterPrice { get; set; } // the entrance price
        public bool IsClosed
        {
            get { return _isClosed; }
            set
            {
                if (value)
                {
                    Bitmap = new Bitmap(Resources.entrance_closed, ScreenHeight, ScreenWidth); // set the picture of the gate when it is closed
                }
                else
                {
                   Bitmap = new Bitmap(Resources.entrance_opened, ScreenHeight, ScreenWidth); // set the picture of the gate when it is opened
                }
                _isClosed = value;
            }
        }

        public MainGate() // constructor of the main gate
        {
            IsConnectedToGate = true; // it is conneted to the gate
            IsClosed = true; // the gate is closed
            EnterPrice = 200; // set the entrance price
        }
    }
}
