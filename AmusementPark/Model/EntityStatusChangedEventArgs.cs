using System;

namespace AmusementPark.Model
{
    public class EntityStatusChangedEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }

        public EntityStatusChangedEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
