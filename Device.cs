using System;

namespace BuildingManager
{
    public class Device
    {
        public delegate void OnDeviceModifiedEventHandler(Device device, EventArgs args);
        public event OnDeviceModifiedEventHandler DeviceModified;

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
        
        public void Rename(string newName)
        {
            Name = newName;
            OnDeviceModified();
        }
        
        protected void OnDeviceModified()
        {
            DeviceModified?.Invoke(this, EventArgs.Empty);
        }
    }
}