using System;

namespace BuildingManager.CustomEventArgs
{
    public class BuildingPartModifiedEventArgs : EventArgs
    {
        public Building Building { get; set; }  
        public Section Section { get; set; }
        public Devices.Device Device { get; set; }

        public Section currentDeviceSection { get; set; }
        public Section oldDeviceSection { get; set; }
    }
}