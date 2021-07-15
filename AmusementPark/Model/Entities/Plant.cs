using AmusementPark.Properties;
using System;
using System.Drawing;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Plant : Entity
    {
        public int HappinessBoost { get; set; } // happiness state property
        public int Range { get; set; } // scope where the plant is valid
    }
}
