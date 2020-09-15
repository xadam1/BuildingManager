using System;
using System.Linq;
using BuildingManager.Devices;

namespace BuildingManager
{
    internal class Program
    {
        private static readonly Building Building = new Building();


        #region Initialization
        // Simply populate building with some data
        private static void PopulateBuilding()
        {/*
            // SectionA
            var sectionA = new Section("SectionA");
            sectionA.DeviceAdded += Helper.OnSectionModified;
            sectionA.SectionRenamed += Helper.OnSectionModified;
            sectionA.CardReaders.AddRange(new List<CardReader>()
            {
                new CardReader("MainDoorReader"),
                new CardReader("CaffeeCardReader"),
                new CardReader("FrontGateReader")
            });
            foreach (var aCard in sectionA.CardReaders)
            {
                aCard.CardReaderModified += Helper.OnDeviceModified;
                aCard.DeviceRenamed += Helper.OnDeviceModified;
            }

            sectionA.Speakers.AddRange(new List<Speaker>()
            {
                new Speaker("Subwoofer"),
                new Speaker("FrontSpeaker"),
                new Speaker("SideSpeakerLeft"),
                new Speaker("SideSpeakerRight"),
                new Speaker("RareSpeaker")
            });
            foreach (var aSpeaker in sectionA.Speakers)
            {
                aSpeaker.SpeakerModified += Helper.OnDeviceModified;
                aSpeaker.DeviceRenamed += Helper.OnDeviceModified;
            }

            // SectionB
            var sectionB = new Section("SectionB");
            sectionB.DeviceAdded += Helper.OnSectionModified;
            sectionB.SectionRenamed += Helper.OnSectionModified;
            sectionB.Doors.AddRange(new List<Door>()
            {
                new Door("FrontGate"),
                new Door("CanteenDoor"),
                new Door("BackDoor")
            });
            foreach (var bDoor in sectionB.Doors)
            {
                bDoor.DoorModified += Helper.OnDeviceModified;
                bDoor.DeviceRenamed += Helper.OnDeviceModified;
            }


            sectionB.LedPanels.AddRange(new List<LedPanel>()
            {
                new LedPanel("FrontGateLabel"),
                new LedPanel("HallPanel")
            });
            foreach (var bPanel in sectionB.LedPanels)
            {
                bPanel.LedPanelModified += Helper.OnDeviceModified;
                bPanel.DeviceRenamed += Helper.OnDeviceModified;
            }

            sectionB.CardReaders.AddRange(new List<CardReader>()
            {
                new CardReader("FrontGateCardReader"),
                new CardReader("HeadmasterDoorReader")
            });
            foreach (var bCard in sectionB.CardReaders)
            {
                bCard.CardReaderModified += Helper.OnDeviceModified;
                bCard.DeviceRenamed += Helper.OnDeviceModified;
            }

            Building.Sections.Add(sectionA);
            Building.Sections.Add(sectionB);
            */
        }

        // Does some initial moving, renaming, deleting, etc...
        private static void PerformSomeOperations()
        {
            /*
            // MOVE DEVICE TO ANOTHER LOCATION
            Console.WriteLine("Moving 'Subwoofer' from 'SectionA' -> 'SectionB'");
            _selectedSection = Building.Sections.Single(sec => sec.Name == "SectionA");
            _selectedDevice = _selectedSection.FindDeviceByName("Subwoofer");
            MoveDeviceToAnotherSection(Building.Sections.Single(sec => sec.Name == "SectionB"));

            // RENAME SECTION
            Console.WriteLine("Renaming section 'SectionA' -> 'MainSection'");
            _selectedSection.Name = "MainSection";

            // Change Access Number
            Console.WriteLine("Changing AccessNumber at 'MainDoorReader' -> 'A01234DE7FFF'");
            _selectedDevice = _selectedSection.FindDeviceByName("MainDoorReader");
            ((CardReader)_selectedDevice).AccessCardNumber = "A01234DE7FFF";

            // Delete device
            Console.WriteLine("Deleting 'RareSpeaker' from 'MainSection'");
            _selectedSection = Building.Sections.Single(sec => sec.Name == "MainSection");
            _selectedDevice = _selectedSection.FindDeviceByName("RareSpeaker");
            RemoveSelectedDeviceFromSelectedSection();

            Console.WriteLine("INITIALIZATION DONE!\n\n");

            _selectedSection = null;
            _selectedDevice = null;
            */
        }
        #endregion

        private static void Init()
        {
            PopulateBuilding();
            Helper.Greetings();
            PerformSomeOperations();
            Helper.PrintHelp();
        }

        private static void Main()
        {
            Init();
            string mainCommand = null;

            // Main App Loop
            while (mainCommand != "exit")
            {
                var commands = Console.ReadLine()?.Split(' ');
                mainCommand = commands?[0];

                switch (mainCommand)
                {
                    case "help":
                    case "h":
                        Helper.PrintHelp();
                        break;

                    case "building":
                    case "plan":
                        Building.BuildingPlan();
                        break;

                    // Add Sections and Devices
                    case "add":
                    case "Add":
                        switch (commands[1])
                        {
                            case "device":
                            case "Device":
                                if (!CheckNumberOfArgs(commands, 4))
                                { continue; }

                                var sectionName = commands[2];
                                var targetSection = GetSectionByName(sectionName);
                                if (targetSection is null)
                                {
                                    Helper.PrintError("Target Section not found.");
                                    continue;
                                }
                                var deviceType = commands[3];

                                string deviceName = null;
                                // If user input contains name for new device use it, default otherwise
                                if (commands.Length == 5)
                                { deviceName = commands[4]; }
                                
                                if (!Enum.TryParse(deviceType, out DeviceTypes type))
                                {
                                    Helper.PrintError("Device type not recognized.");
                                    continue;
                                }
                                targetSection.AddDevice(type, deviceName);
                                break;

                            case "section":
                            case "Section":
                                if (!CheckNumberOfArgs(commands, 3))
                                { continue; }

                                var newSectionName = commands[2];
                                if (!CheckSectionNameAvailability(newSectionName))
                                {
                                    Helper.PrintError("Sorry, this section already exists.");
                                    continue;
                                }
                                Building.AddSection(newSectionName);
                                break;

                            // Unknown second command
                            default:
                                Helper.PrintError("Invalid Add Command Syntax. Check 'help'.");
                                break;
                        }
                        break;

                    // Display info about section or device
                    case "info":
                    case "Info":
                        if (CheckNumberOfArgs(commands, 3))
                        { continue; }

                        switch (commands[1])
                        {
                            case "device":
                            case "Device":
                                var device = GetDevice(commands[2]);

                                if (device is null)
                                {
                                    Helper.PrintError("Device not found.");
                                    continue;
                                }
                                Helper.PrintDeviceInfo(device);
                                break;

                            case "section":
                            case "Section":
                                var section = GetSectionByName(commands[2]);
                                if (section is null)
                                {
                                    Helper.PrintError("Section not found.");
                                    continue;
                                }
                                Helper.PrintSectionInfo(section);
                                break;
                        }
                        break;

                    // Rename device or section
                    case "rename":
                    case "Rename":
                        if (!CheckNumberOfArgs(commands, 4))
                        { continue; }

                        switch (commands[1])
                        {
                            case "device":
                            case "Device":
                                var device = GetDevice(commands[2]);
                                if (device is null)
                                {
                                    Helper.PrintError("Device not found.");
                                    continue;
                                }

                                device.Name = commands[3];
                                break;

                            case "section":
                            case "Section":
                                var section = GetSectionByName(commands[2]);
                                if (section is null)
                                {
                                    Helper.PrintError("Section not found.");
                                    continue;
                                }
                                section.Name = commands[3];
                                break;
                        }
                        break;

                    // Removing devices or sections
                    case "delete":
                    case "Delete":
                        if (!CheckNumberOfArgs(commands, 3))
                        { continue; }

                        switch (commands[1])
                        {
                            case "device":
                            case "Device":
                                if (!RemoveDevice(commands[2]))
                                { Helper.PrintError("Error during removing device."); }
                                break;

                            case "section":
                            case "Section":
                                if (!Building.RemoveSection(commands[2]))
                                { Helper.PrintError("Error during removing section."); }
                                break;
                        }
                        break;

                    // Move 
                    case "mv":
                    case "Mv":
                    case "move":
                    case "Move":
                        if (!CheckNumberOfArgs(commands, 3))
                        { continue; }

                        var newSection = GetSectionByName(commands[2]);
                        if (newSection is null)
                        {
                            Helper.PrintError("New section not found!");
                            continue;
                        }

                        if (!Building.MoveDevice(commands[1], newSection))
                        { Helper.PrintError("An error occured during moving operation."); }
                        break;

                    // Changing Access Number, Sound, Volume or Text of devices
                    case "change":
                    case "ch":
                        if (!CheckNumberOfArgs(commands, 4))
                        { continue; }

                        var targetDevice = GetDevice(commands[2]);
                        if (targetDevice is null)
                        {
                            Helper.PrintError("Target device not found.");
                            continue;
                        }

                        switch (commands[1])
                        {
                            case "access":
                            case "Access":
                                if (IsDeviceTypeMatch(targetDevice, DeviceTypes.CardReader))
                                {
                                    (targetDevice as CardReader).AccessCardNumber = commands[3];
                                }
                                break;

                            case "volume":
                            case "Volume":
                                if (IsDeviceTypeMatch(targetDevice, DeviceTypes.Speaker))
                                {
                                    if (!float.TryParse(commands[3], out var volume))
                                    {
                                        Helper.PrintError("Volume must be a float.");
                                        continue;
                                    }
                                    (targetDevice as Speaker).Volume = volume;
                                }
                                break;

                            case "sound":
                            case "Sound":
                                if (IsDeviceTypeMatch(targetDevice, DeviceTypes.Speaker))
                                {
                                    switch (commands[3])
                                    {
                                        case "none":
                                        case "None":
                                            (targetDevice as Speaker).Sound = Speaker.SoundOptions.None;
                                            break;

                                        case "alarm":
                                        case "Alarm":
                                            (targetDevice as Speaker).Sound = Speaker.SoundOptions.Alarm;
                                            break;

                                        case "music":
                                        case "Music":
                                            (targetDevice as Speaker).Sound = Speaker.SoundOptions.Music;
                                            break;

                                        default:
                                            Helper.PrintError("Sound can be changed to 'None'/'Alarm'/'Music'.");
                                            break;
                                    }
                                }
                                break;

                            case "text":
                            case "Text":
                                if (IsDeviceTypeMatch(targetDevice, DeviceTypes.LedPanel))
                                {
                                    (targetDevice as LedPanel).Message = commands[3];
                                }
                                break;
                        }
                        break;

                    // set MainDoor open
                    case "setdoor":
                    case "setDoor":
                    case "SetDoor":
                    case "door":
                    case "set":
                        if (!CheckNumberOfArgs(commands, 3))
                        { continue; }

                        var door = GetDevice(commands[1]);
                        if (door is null)
                        {
                            Helper.PrintError("Door could not be found.");
                            continue;
                        }

                        switch (commands[2])
                        {
                            case "open":
                            case "Open":
                                (door as Door).Open = !(door as Door).Open;
                                break;

                            case "lock":
                            case "Lock":
                                (door as Door).Locked = !(door as Door).Locked;
                                break;

                            case "openedforcibly":
                            case "OpenedForcibly":
                            case "forcibly":
                            case "force":
                            case "forceopen":
                            case "openforced":
                            case "openforce":
                                (door as Door).OpenedForcibly = !(door as Door).OpenedForcibly;
                                break;

                            case "openfortoolong":
                            case "OpenForTooLong":
                            case "toolong":
                            case "long":
                            case "longopen":
                            case "opentoolong":
                            case "openlong":
                                (door as Door).OpenForTooLong = !(door as Door).OpenForTooLong;
                                break;
                        }
                        break;

                    // If Command was invalid or was not recognized
                    default:
                        Helper.PrintError("Unknown Command!");
                        break;
                }
            }
        }


        // Checks if 'device' is of type 'desiredDeviceType', return true if yes,
        // displays error and returns false otherwise
        private static bool IsDeviceTypeMatch(Device device, DeviceTypes desireDeviceType)
        {
            if (device.Type == desireDeviceType) return true;
            Helper.PrintError($"You must select device of type {desireDeviceType} to modify it.");
            return false;
        }

        // Returns false if section with given name already exists
        private static bool CheckSectionNameAvailability(string name) =>
            Building.Sections.SingleOrDefault(x => x.Name == name) == null;

        // Checks if 'commands' contains at least 'desiredNumber' of arguments
        // true if yes, false otherwise
        private static bool CheckNumberOfArgs(string[] commands, int desiredNumber)
        {
            if (commands.Length >= desiredNumber) return true;
            Helper.PrintError("Insufficient number of arguments!");
            return false;
        }


        private static Section GetSectionByName(string sectionName)
        {
            return Building.Sections.FirstOrDefault(section => section.Name == sectionName);
        }


        private static Device GetDevice(string identificator)
        {
            return int.TryParse(identificator, out var id)
                 ? Building.GetDeviceById(id)
                 : Building.GetDeviceByName(identificator);
        }


        private static bool RemoveDevice(string identificator)
        {
            return int.TryParse(identificator, out var id)
                ? Building.RemoveDevice(id)
                : Building.RemoveDevice(identificator);
        }
    }
}
