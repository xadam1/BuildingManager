using System;
using BuildingManager.Devices;

namespace BuildingManager.CustomEventArgs
{
    public class BuildingPartModifiedEventArgs : EventArgs
    {
        public Building Building { get; set; }  
        public Section Section { get; set; }
        public Device Device { get; set; }

        public Section CurrentDeviceSection { get; set; }
        public Section OldDeviceSection { get; set; }
    }
}