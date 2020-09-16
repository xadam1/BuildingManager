using System;
using System.IO;
using BuildingManager.CustomEventArgs;

namespace BuildingManager.Devices
{
    public class Device
    {
        private string _name;

        public delegate void OnDeviceModifiedEventHandler(BuildingPartModifiedEventArgs args);
        public event OnDeviceModifiedEventHandler DeviceModified;

        public delegate void OnDeviceErrorEventHandler(Device device, ErrorEventArgs args);
        public event OnDeviceErrorEventHandler DeviceError;

        public Device(DeviceTypes type, string name)
        {
            Type = type;
            if (name is null)
            {
                name = type.ToString();
            }
            Name = name;

            DeviceModified += Helper.OnDeviceModified;
            DeviceError += Helper.OnDeviceError;
        }

        public DeviceTypes Type { get; }

        public int Id { get; } = Helper.CalculateNewId();

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnDeviceModified();
            }
        }

        public virtual string GetCurrentState()
        {
            return $"Type:\t{Type}\nName:\t{Name}\nID:\t{Id}";
        }


        protected void OnDeviceModified()
        {
            var args = new BuildingPartModifiedEventArgs()
            {
                Device = this
            };
            DeviceModified?.Invoke(args);
        }

        protected void OnDeviceError(string message)
        {
            var exception = new Exception(message);
            DeviceError?.Invoke(this, new ErrorEventArgs(exception));
        }
    }
}