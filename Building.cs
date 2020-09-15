using System;
using System.Collections.Generic;
using System.Linq;
using BuildingManager.CustomEventArgs;
using BuildingManager.Devices;

namespace BuildingManager
{
    public class Building
    {
        public delegate void BuildingPartModifiedEventHandler(BuildingPartModifiedEventArgs args);

        public event BuildingPartModifiedEventHandler SectionRemoved;
        public event BuildingPartModifiedEventHandler DeviceRemoved;
        public event BuildingPartModifiedEventHandler DeviceMoved;

        public Building()
        {
            DeviceRemoved += Helper.OnDeviceRemoved;
            DeviceMoved += Helper.OnDeviceMoved;
            SectionRemoved += Helper.OnDeviceRemoved;
        }


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
            OnSectionRemoved(section);
            return true;
        }

        public bool RemoveDevice(int id)
        {
            foreach (var section in Sections)
            {
                var device = section.Devices.FirstOrDefault(x => x.Id == id);
                if (device == null) continue;

                section.RemoveDevice(device);
                OnDeviceRemoved(device, section);
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
                OnDeviceRemoved(device, section);
                return true;
            }
            return false;
        }

        // Moves device from it's current section into 'newSection',
        // returns true if succeeded, false otherwise
        public bool MoveDevice(string deviceIdentificator, Section newSection)
        {
            Section currentSection = null;
            Devices.Device device = null;

            // If identificator is ID
            if (int.TryParse(deviceIdentificator, out var id))
            {
                foreach (var section in Sections)
                {
                    device = section.Devices.SingleOrDefault(x => x.Id == id);
                    if (device == null) continue;

                    currentSection = section;
                    break;
                }
            }
            else
            {
                foreach (var section in Sections)
                {
                    device = section.Devices.SingleOrDefault(x => x.Name == deviceIdentificator);
                    if (device == null) continue;

                    currentSection = section;
                    break;
                }
            }

            // Was device found? if not return false
            if (device == null) return false;

            newSection.Devices.Add(device);
            currentSection.Devices.Remove(device);
            OnDeviceMoved(device, currentSection, newSection);
            return true;
        }

        protected void OnSectionRemoved(Section section)
        {
            var args = new BuildingPartModifiedEventArgs()
            {
                Building = this,
                Section = section
            };
            SectionRemoved?.Invoke(args);
        }

        protected void OnDeviceRemoved(Device device, Section section)
        {
            var args = new BuildingPartModifiedEventArgs()
            {
                Device = device,
                Section = section
            };
            DeviceRemoved?.Invoke(args);
        }

        protected virtual void OnDeviceMoved(Device device, Section oldSection, Section newSection)
        {
            var args = new BuildingPartModifiedEventArgs()
            {
                Building = this,
                Device = device,
                currentDeviceSection = newSection,
                oldDeviceSection = oldSection
            };
            DeviceMoved?.Invoke(args);
        }
    }
}