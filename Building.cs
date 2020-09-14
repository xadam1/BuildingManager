using System.Collections.Generic;
using System.Linq;

namespace BuildingManager
{
    public class Building
    {
        public List<Section> Sections { get; set; } = new List<Section>();

        
        public void AddSection(string name)
        {
            var section = new Section(name);
            Sections.Add(section);
            section.SectionModified += Helper.OnSectionModified;
            BuildingPlan();
        }

        // Displays info about all sections
        public void BuildingPlan()
        {
            foreach (var section in Sections)
            {
                Helper.PrintSectionInfo(section);
            }
        }

        // Loops through all Sections and if matching ID is found returns that device, null otherwise
        public Devices.Device GetDeviceById(int id)
        {
            return Sections.Select(section => section.FindDeviceById(id))
                .FirstOrDefault(res => res != null);
        }

        // Loops through Sections and returns device with matching name, null otherwise
        public Devices.Device GetDeviceByName(string name)
        {
            return Sections.Select(section => section.FindDeviceByName(name))
                .FirstOrDefault(res => res != null);
        }
        

        public bool RemoveSection(string name)
        {
            var section = Sections.FirstOrDefault(x => x.Name == name);
            if (section is null)
            {
                return false;
            }

            Sections.Remove(section);
            BuildingPlan();
            return true;
        }

        public bool RemoveDevice(int id)
        {
            foreach (var section in Sections)
            {
                var device = section.Devices.FirstOrDefault(x => x.Id == id);
                if (device == null) continue;
                
                section.RemoveDevice(device);
                return true;
            }
            return false;
        }

        public bool RemoveDevice(string name)
        {
            foreach (var section in Sections)
            {
                var device = section.Devices.FirstOrDefault(x => x.Name == name);
                if (device == null) continue;

                section.RemoveDevice(device);
                return true;
            }
            return false;
        }
    }
}