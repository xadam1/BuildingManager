using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManager.CustomEventArgs;
using BuildingManager.Devices;

namespace BuildingManager
{
    public class Section
    {
        private string _name;

        public event Building.BuildingPartModifiedEventHandler SectionModified;


        public Section(string name)
        {
            _name = name;
            SectionModified += Helper.OnSectionModified;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnSectionModified();
            }
        }

        public List<Device> Devices { get; set; } = new List<Device>();


        public void AddDevice(DeviceTypes deviceType, string name)
        {
            Device device;

            switch (deviceType)
            {
                case DeviceTypes.Door:
                    device = new Door(name);
                    Devices.Add(device);
                    break;
                case DeviceTypes.Speaker:
                    device = new Speaker(name);
                    Devices.Add(device);
                    break;
                case DeviceTypes.CardReader:
                    device = new CardReader(name);
                    Devices.Add(device);
                    break;
                case DeviceTypes.LedPanel:
                    device = new LedPanel(name);
                    Devices.Add(device);
                    break;

                default:
                    return;
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
            var sb = new StringBuilder();

            // Check if Devices contains any CardReaders
            if (Devices.Any(x => x.Type == DeviceTypes.CardReader))
            {
                sb.AppendLine("CardReaders:");
                foreach (var cardReader in Devices.Where(x => x.Type == DeviceTypes.CardReader).ToList())
                {
                    sb.Append($"{cardReader.Name}(ID: {cardReader.Id}) ");
                }
                sb.AppendLine("\n");
            }

            // Contains Doors?
            if (Devices.Any(x => x.Type == DeviceTypes.Door))
            {
                sb.AppendLine("Doors:");
                foreach (var door in Devices.Where(x => x.Type == DeviceTypes.Door).ToList())
                {
                    sb.Append($"{door.Name}(ID: {door.Id}) ");
                }
                sb.AppendLine("\n");
            }

            // Contains LedPanel?
            if (Devices.Any(x => x.Type == DeviceTypes.LedPanel))
            {
                sb.AppendLine("LedPanels:");
                foreach (var ledPanel in Devices.Where(x => x.Type == DeviceTypes.LedPanel).ToList())
                {
                    sb.Append($"{ledPanel.Name}(ID: {ledPanel.Id}) ");
                }
                sb.AppendLine("\n");
            }

            // Contains Speakers?
            if (Devices.Any(x => x.Type == DeviceTypes.Speaker))
            {
                sb.AppendLine("Speakers:");
                foreach (var speaker in Devices.Where(x => x.Type == DeviceTypes.Speaker).ToList())
                {
                    sb.Append($"{speaker.Name}(ID: {speaker.Id}) ");
                }
            }

            // Return trimmed version
            return sb.ToString().TrimEnd();
        }


        protected void OnSectionModified()
        {
            var args = new BuildingPartModifiedEventArgs()
            {
                Section = this,
            };
            SectionModified?.Invoke(args);
        }
    }
}