using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManager
{
    public class Section
    {
        private string _name;

        public delegate void DeviceAddedEventHandler(Section section, EventArgs args);
        public event DeviceAddedEventHandler DeviceAdded;

        public delegate void SectionRenamedEventHandler(Section section, EventArgs args);
        public event SectionRenamedEventHandler SectionRenamed;

        public Section(string name)
        {
            Name = name;
        }

        public string Name
        {
            get => _name;
            set
            {
                var isRenaming = Name != null;
                _name = value;

                // To not display newly created empty Section
                if (isRenaming) { OnSectionRenamed(); }
            }
        }

        /* TODO
        // Each section has it's own Collection of devices
        // I found this the easiest way, since we only deal with 4 different device types
        public List<CardReader> CardReaders { get; set; } = new List<CardReader>();
        public List<Door> Doors { get; set; } = new List<Door>();
        public List<LedPanel> LedPanels { get; set; } = new List<LedPanel>();
        public List<Speaker> Speakers { get; set; } = new List<Speaker>();
        */

        public List<Default> Devices { get; set; } = new List<Default>();

        /* TODO
        public void AddDevice(Device device, string name)
        {
            switch (device)
            {
                case Device.Door:
                    var door = new Door(name);
                    door.DoorModified += Helper.OnDeviceModified;
                    door.DeviceRenamed += Helper.OnDeviceModified;
                    Doors.Add(door);
                    break;

                case Device.CardReader:
                    var reader = new CardReader(name);
                    reader.CardReaderModified += Helper.OnDeviceModified;
                    reader.DeviceRenamed += Helper.OnDeviceModified;
                    CardReaders.Add(reader);
                    break;

                case Device.Speaker:
                    var speaker = new Speaker(name);
                    speaker.SpeakerModified += Helper.OnDeviceModified;
                    speaker.DeviceRenamed += Helper.OnDeviceModified;
                    Speakers.Add(speaker);
                    break;

                case Device.LedPanel:
                    var panel = new LedPanel(name);
                    panel.LedPanelModified += Helper.OnDeviceModified;
                    panel.DeviceRenamed += Helper.OnDeviceModified;
                    LedPanels.Add(panel);
                    break;
            }
            OnDeviceAdded();
        }
        */

        public void AddDevice(Device deviceType, string name)
        {
            switch (deviceType)
            {
                case Device.Door:
                    Devices.Add(new Door(name));
                    break;
                case Device.Speaker:
                    Devices.Add(new Speaker(name));
                    break;
                case Device.CardReader:
                    Devices.Add(new CardReader(name));
                    break;
                case Device.LedPanel:
                    Devices.Add(new LedPanel(name));
                    break;
            }
            OnDeviceAdded();
        }

        // Returns actual device object, null if None matched the name
        public Default FindDeviceByName(string name) => Devices.SingleOrDefault(x => x.Name == name);

        public Default FindDeviceById(int id) => Devices.SingleOrDefault(x => x.Id == id);

        // Returns string of all devices in this section. Divided into categories
        // Category is omitted if no device is in given category
        public string ListDevices()
        {
            StringBuilder sb = new StringBuilder();

            if (CardReaders.Count != 0)
            {
                sb.AppendLine("CardReaders:");
                foreach (var cardReader in CardReaders)
                {
                    sb.Append($"{cardReader.Name} ");
                }
                sb.AppendLine("\n");
            }

            if (Doors.Count != 0)
            {
                sb.AppendLine("Doors:");
                foreach (var door in Doors)
                {
                    sb.Append($"{door.Name} ");
                }
                sb.AppendLine("\n");
            }

            if (LedPanels.Count != 0)
            {
                sb.AppendLine("LedPanels:");
                foreach (var ledPanel in LedPanels)
                {
                    sb.Append($"{ledPanel.Name} ");
                }
                sb.AppendLine("\n");
            }

            if (Speakers.Count != 0)
            {
                sb.AppendLine("Speakers:");
                foreach (var speaker in Speakers)
                {
                    sb.Append($"{speaker.Name} ");
                }
                sb.AppendLine("\n");
            }

            return sb.ToString().TrimEnd();
        }


        protected virtual void OnDeviceAdded()
        {
            DeviceAdded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSectionRenamed()
        {
            SectionRenamed?.Invoke(this, EventArgs.Empty);
        }
    }
}