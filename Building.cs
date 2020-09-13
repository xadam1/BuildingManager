using System.Collections.Generic;

namespace BuildingManager
{
    public class Building
    {
        public List<Section> Sections { get; set; } = new List<Section>();

        
        public void AddSection(string name)
        {
            var section = new Section(name);
            Sections.Add(section);
            section.DeviceAdded += Helper.OnSectionModified;
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
    }
}