using System;
using System.IO;

namespace BuildingManager.Devices
{
    public class Device
    {
        public delegate void OnDeviceModifiedEventHandler(Device device, EventArgs args);
        public event OnDeviceModifiedEventHandler DeviceModified;

        public delegate void OnDeviceErrorEventHandler(Device device, ErrorEventArgs args);
        public event OnDeviceErrorEventHandler DeviceError; 

        public Device(DeviceTypes type, string name)
        {
            Type = type;
            Name = name;    
        }

        public DeviceTypes Type { get; }

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

        protected void OnDeviceError(string message)
        {
            var exception = new Exception(message);
            DeviceError?.Invoke(this, new ErrorEventArgs(exception));
        }
    }
}