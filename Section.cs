using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManager.Devices;

namespace BuildingManager
{
    public class Section
    {
        private string _name;

        public delegate void SectionModifiedEventHandler(Section section, EventArgs args);
        public event SectionModifiedEventHandler SectionModified;


        public Section(string name)
        {
            Name = name;
        }

        public string Name
        {
            get => _name;
            set
            {
                //TODO WTF??
                _name = value;
                // To not display newly created empty Section
                if (Name != null) { OnSectionModified(); }
            }
        }

        public List<Device> Devices { get; set; } = new List<Device>();


        public void AddDevice(DeviceTypes deviceType, string name)
        {
            var device = new Device(deviceType, name);
            device.DeviceModified += Helper.OnDeviceModified;
            device.DeviceError += Helper.OnDeviceError;

            switch (deviceType)
            {
                case DeviceTypes.Door:
                    Devices.Add((Door)device);
                    break;
                case DeviceTypes.Speaker:
                    Devices.Add((Speaker)device);
                    break;
                case DeviceTypes.CardReader:
                    Devices.Add((CardReader)device);
                    break;
                case DeviceTypes.LedPanel:
                    Devices.Add((LedPanel)device);
                    break;
            }
            OnSectionModified();
        }

        public void RemoveDevice(Device device)
        {
            Devices.Remove(device);
            OnSectionModified();
        }


        // Returns actual device object, null if None matched the name/id
        public Device FindDeviceByName(string name) => Devices
            .SingleOrDefault(x => x.Name == name);

        public Device FindDeviceById(int id) => Devices
            .SingleOrDefault(x => x.Id == id);

        // Returns string of all devices in this section. Divided into categories
        // Category is omitted if no device is in given category
        public string ListDevices()
        {
            StringBuilder sb = new StringBuilder();

            var cardReaders = new List<Device>();
            var doors = new List<Device>();
            var speakers = new List<Device>();
            var ledPanels = new List<Device>();

            foreach (var device in Devices)
            {
                switch (device.Type)
                {
                    case DeviceTypes.CardReader:
                        cardReaders.Add(device);
                        break;
                    case DeviceTypes.Door:
                        doors.Add(device);
                        break;
                    case DeviceTypes.Speaker:
                        speakers.Add(device);
                        break;
                    case DeviceTypes.LedPanel:
                        ledPanels.Add(device);
                        break;
                }
            }

            if (cardReaders.Count != 0)
            {
                sb.AppendLine("CardReaders:");
                foreach (var cardReader in cardReaders)
                {
                    sb.Append($"{cardReader.Name} ");
                }
                sb.AppendLine("\n");
            }

            if (doors.Count != 0)
            {
                sb.AppendLine("Doors:");
                foreach (var door in doors)
                {
                    sb.Append($"{door.Name} ");
                }
                sb.AppendLine("\n");
            }

            if (ledPanels.Count != 0)
            {
                sb.AppendLine("LedPanels:");
                foreach (var ledPanel in ledPanels)
                {
                    sb.Append($"{ledPanel.Name} ");
                }
                sb.AppendLine("\n");
            }

            if (speakers.Count != 0)
            {
                sb.AppendLine("Speakers:");
                foreach (var speaker in speakers)
                {
                    sb.Append($"{speaker.Name} ");
                }
                sb.AppendLine("\n");
            }

            return sb.ToString().TrimEnd();
        }


        protected void OnSectionModified()
        {
            SectionModified?.Invoke(this, EventArgs.Empty);
        }
    }
}