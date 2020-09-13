using System;

namespace BuildingManager
{
    public class Device
    {
        public delegate void OnDeviceModifiedEventHandler(Device device, EventArgs args);

        public event OnDeviceModifiedEventHandler DeviceRenamed;

        public Device(DeviceType type, string name)
        {
            Type = type;
            Name = name;
        }

        public DeviceType Type { get; }

        public int Id { get; } = Helper.CalculateNewId();

        public string Name { get; set; }

        public virtual string GetCurrentState()
        {
            return $"Type:\t{Type}\nName:\t{Name}\nID:\t{Id}";
        }


        public virtual void Rename(string newName)
        {
            Name = newName;
            OnDeviceRenamed();
        }

        protected virtual void OnDeviceRenamed()
        {
            DeviceRenamed?.Invoke(this, EventArgs.Empty);
        }
    }
}