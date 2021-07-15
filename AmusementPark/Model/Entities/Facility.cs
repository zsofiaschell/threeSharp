using System;

namespace AmusementPark.Model.Entities
{
    [Serializable]
    public class Facility : Entity //frame for facilities
    {
        public enum States // building states of facility
        {
            Building,
            Working,
            Stopped,
            InUse
        }

        public int BuildingTimeInSec { get; set; }
        public DateTime PlacingTime { get; set; }
        public int PriceToKeepOpen { get; set; }
        public int PriceToUse { get; set; }
        public int Capacity { get; set; }
        public virtual States State { get; set; }
        public bool HasElectricity { get; set; }
        public bool IsNextToConnectedRoad { get; set; }
    }
}
