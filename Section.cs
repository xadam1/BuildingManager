using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                _name = value;
                // To not display newly created empty Section
                if (Name != null) { OnSectionModified(); }
            }
        }

        public List<Device> Devices { get; set; } = new List<Device>();


        // TODO add events that something was added
        public void AddDevice(DeviceType deviceType, string name)
        {
            switch (deviceType)
            {
                case DeviceType.Door:
                    Devices.Add(new Door(name));
                    break;
                case DeviceType.Speaker:
                    Devices.Add(new Speaker(name));
                    break;
                case DeviceType.CardReader:
                    Devices.Add(new CardReader(name));
                    break;
                case DeviceType.LedPanel:
                    Devices.Add(new LedPanel(name));
                    break;
            }
            OnSectionModified();
        }

        // Returns actual device object, null if None matched the name
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
                    case DeviceType.CardReader:
                        cardReaders.Add(device);
                        break;
                    case DeviceType.Door:
                        doors.Add(device);
                        break;
                    case DeviceType.Speaker:
                        speakers.Add(device);
                        break;
                    case DeviceType.LedPanel:
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