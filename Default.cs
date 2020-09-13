using System;

namespace BuildingManager
{
    public class Default
    {
        public delegate void OnDeviceModifiedEventHandler(Default device, EventArgs args);

        public event OnDeviceModifiedEventHandler DeviceRenamed;

        public Default(Device type, string name)
        {
            Type = type;
            Name = name;
        }

        public Device Type { get; }

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