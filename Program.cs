﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingManager
{
    internal class Program
    {
        private static Section _selectedSection;
        private static Device _selectedDevice;

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
        private static void Initialize()
        {
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
        }
        #endregion


        private static void Main()
        {
            PopulateBuilding();
            Helper.Greetings();
            Initialize();
            Helper.PrintHelp();

            var input = "";
            // Main App Loop, listening for commands
            while (input != "exit")
            {
                input = Console.ReadLine();
                switch (input?.ToLower())
                {
                    case "help":
                    case "h":
                        Helper.PrintHelp();
                        break;

                    case "building plan":
                    case "plan":
                        Building.BuildingPlan();
                        break;


                    // SECTION COMMANDS
                    case "section":
                        if (!CheckIfSelectedSectionIsNull()) { Helper.SectionSelectedMessage(_selectedSection); }
                        break;

                    case "new section":
                        Console.WriteLine("What is the name of this section?");
                        var newSectionName = Console.ReadLine();
                        if (!CheckSectionNameAvailability(newSectionName))
                        {
                            Helper.PrintError("Sorry, this section already exists.");
                            continue;
                        }
                        Building.AddSection(newSectionName);
                        break;

                    case "select section":
                    case "section select":
                        _selectedSection = GetSection();
                        if (CheckIfSelectedSectionIsNull()) { continue; }

                        Helper.SectionSelectedMessage(_selectedSection);
                        break;

                    case "section info":
                        if (CheckIfSelectedSectionIsNull()) { continue; }
                        Helper.PrintSectionInfo(_selectedSection);
                        break;

                    case "rename section":
                    case "section rename":
                        if (CheckIfSelectedSectionIsNull()) { continue; }

                        Console.WriteLine("Enter new name for selected section...");
                        _selectedSection.Name = Console.ReadLine();
                        break;

                    case "delete section":
                    case "section delete":
                        if (CheckIfSelectedSectionIsNull()) { continue; }

                        Building.Sections.Remove(_selectedSection);
                        Helper.PrintSectionDeleted(_selectedSection);
                        _selectedSection = null;
                        break;


                    // DEVICE COMMANDS
                    case "device":
                        if (!CheckIfSelectedDeviceIsNull()) { Helper.DeviceSelectedMessage(_selectedDevice); }
                        break;

                    case "select device":
                        if (CheckIfSelectedSectionIsNull()) { continue; }
                        _selectedDevice = GetDevice();
                        if (!CheckIfSelectedDeviceIsNull()) { Helper.DeviceSelectedMessage(_selectedDevice); }
                        break;

                    case "new device":
                        if (CheckIfSelectedSectionIsNull()) { continue; }
                        var (device, name) = GetDeviceTypeAndName();
                        _selectedSection.AddDevice(device, name);
                        break;

                    case "rename device":
                    case "device rename":
                        if (CheckIfSelectedDeviceIsNull()) { continue; }
                        Console.WriteLine("Enter new name for device...");
                        _selectedDevice.Rename(Console.ReadLine());
                        break;

                    case "device info":
                        if (!CheckIfSelectedDeviceIsNull()) { Helper.PrintDeviceInfo(_selectedDevice); }
                        break;

                    case "delete device":
                    case "delete":
                        if (CheckIfSelectedSectionIsNull() || CheckIfSelectedDeviceIsNull()) { continue; }
                        RemoveSelectedDeviceFromSelectedSection();
                        break;

                    case "move device":
                    case "move":
                        if (CheckIfSelectedSectionIsNull() || CheckIfSelectedDeviceIsNull()) { continue; }
                        var targetSection = GetSection();
                        if (targetSection == null)
                        {
                            Helper.PrintError("Target Section Not Found!");
                            continue;
                        }
                        MoveDeviceToAnotherSection(targetSection);
                        break;


                    // LEDPANEL MANIPULATION
                    case "change text":
                        if (CheckIfSelectedDeviceIsMatch(DeviceType.LedPanel))
                        {
                            Console.WriteLine("Enter new text...");
                            (_selectedDevice as LedPanel).Message = Console.ReadLine();
                        }
                        break;


                    // DOOR MANIPULATION
                    case "open door":
                    case "open":
                        if (!CheckIfSelectedDeviceIsNull() && CheckIfSelectedDeviceIsMatch(DeviceType.Door))
                        {
                            (_selectedDevice as Door).OpenDoor();
                        }
                        break;

                    case "lock door":
                    case "lock":
                        if (!CheckIfSelectedDeviceIsNull() && CheckIfSelectedDeviceIsMatch(DeviceType.Door))
                        {
                            (_selectedDevice as Door).LockDoor();
                        }
                        break;

                    case "opentoolong door":
                    case "opentoolong":
                    case "toolong":
                        if (!CheckIfSelectedDeviceIsNull() && CheckIfSelectedDeviceIsMatch(DeviceType.Door))
                        {
                            (_selectedDevice as Door).SetOpenForTooLong();
                        }
                        break;

                    case "openforcibly door":
                    case "door openforcibly":
                    case "openforcibly":
                    case "forcibly":
                        if (!CheckIfSelectedDeviceIsNull() && CheckIfSelectedDeviceIsMatch(DeviceType.Door))
                        {
                            (_selectedDevice as Door).SetOpenedForcibly();
                        }
                        break;


                    // SPEAKER MANIPULATION
                    case "volume":
                    case "change volume":
                        if (CheckIfSelectedDeviceIsNull() || !CheckIfSelectedDeviceIsMatch(DeviceType.Speaker)) { continue; }

                        Console.WriteLine("Enter new volume (Number between 0 and 100)...");
                        if (!float.TryParse(Console.ReadLine(), out var parsedInput))
                        {
                            Helper.PrintError("Enter a number");
                            continue;
                        }
                        (_selectedDevice as Speaker).Volume = parsedInput;
                        break;

                    case "sound":
                    case "change sound":
                        if (CheckIfSelectedDeviceIsNull() || !CheckIfSelectedDeviceIsMatch(DeviceType.Speaker)) { continue; }

                        Console.WriteLine($"What do you want {_selectedDevice.Name} to play? (Alarm/Music/None)");
                        switch (Console.ReadLine())
                        {
                            case "Alarm":
                            case "alarm":
                                (_selectedDevice as Speaker).PlayAlarm();
                                break;

                            case "Music":
                            case "music":
                                (_selectedDevice as Speaker).PlayMusic();
                                break;

                            case "None":
                            case "none":
                                (_selectedDevice as Speaker).StopPlaying();
                                break;

                            default:
                                Helper.PrintError("Invalid sound. Select 'Alarm/Music/None'.");
                                break;
                        }
                        break;


                    // CARD MANIPULATION
                    case "access":
                    case "change access":
                        if (!CheckIfSelectedDeviceIsMatch(DeviceType.CardReader)) { continue; }

                        Console.WriteLine("Please enter new card number...");
                        (_selectedDevice as CardReader).AccessCardNumber = Console.ReadLine();
                        break;

                    // If Command was invalid or was not recognized
                    default:
                        Helper.PrintError("Unknown Command!");
                        break;
                }

                // print new line for cleaner output
                Console.WriteLine();
            }
        }

        #region CheckMethods
        private static bool CheckIfSelectedSectionIsNull()
        {
            if (!(_selectedSection is null)) return false;
            Helper.PrintError("No section selected.");
            return true;
        }

        private static bool CheckIfSelectedDeviceIsNull()
        {
            if (!(_selectedDevice is null)) return false;
            Helper.PrintError("Unknown Device!");
            return true;
        }

        private static bool CheckIfSelectedDeviceIsMatch(DeviceType deviceType)
        {
            if (_selectedDevice.Type == deviceType) return true;
            Helper.PrintError($"You must select {deviceType} to modify it.");
            return false;
        }

        // Returns false if section with given name already exists
        private static bool CheckSectionNameAvailability(string name) =>
            Building.Sections.SingleOrDefault(x => x.Name == name) == null;
        #endregion

        // Moves _selectedDevice into targetSection
        private static void MoveDeviceToAnotherSection(Section targetSection)
        {
            switch (_selectedDevice.Type)
            {
                case DeviceType.Door:
                    targetSection.Doors.Add(_selectedDevice as Door);
                    _selectedSection.Doors.Remove(_selectedDevice as Door);
                    break;

                case DeviceType.Speaker:
                    targetSection.Speakers.Add(_selectedDevice as Speaker);
                    _selectedSection.Speakers.Remove(_selectedDevice as Speaker);
                    break;

                case DeviceType.LedPanel:
                    targetSection.LedPanels.Add(_selectedDevice as LedPanel);
                    _selectedSection.LedPanels.Remove(_selectedDevice as LedPanel);
                    break;

                case DeviceType.CardReader:
                    targetSection.CardReaders.Add(_selectedDevice as CardReader);
                    _selectedSection.CardReaders.Remove(_selectedDevice as CardReader);
                    break;
            }
            Helper.PrintDeviceMoved(_selectedDevice, _selectedSection, targetSection);
        }

        private static void RemoveSelectedDeviceFromSelectedSection()
        {
            switch (_selectedDevice.Type)
            {
                case DeviceType.Door:
                    _selectedSection.Doors.Remove(_selectedDevice as Door);
                    break;

                case DeviceType.Speaker:
                    _selectedSection.Speakers.Remove(_selectedDevice as Speaker);
                    break;

                case DeviceType.LedPanel:
                    _selectedSection.LedPanels.Remove(_selectedDevice as LedPanel);
                    break;

                case DeviceType.CardReader:
                    _selectedSection.CardReaders.Remove(_selectedDevice as CardReader);
                    break;
            }

            Helper.PrintDeviceDeletedFromSectionMessage(_selectedDevice, _selectedSection);
            _selectedDevice = null;
        }


        #region GetMethods
        // Returns Device object or null if not found
        private static Device GetDevice()
        {
            var (type, name) = GetDeviceTypeAndName();
            return _selectedSection.FindDeviceByName(type, name);
        }

        // TODO CHECK WRONG INPUT
        // Asks user to type DeviceType and name -> parses the type into correct enum, returns tuple
        private static (DeviceType, string) GetDeviceTypeAndName()
        {
            Console.WriteLine("What device (CardReader/Door/LedPanel/Speaker) and name? Ex.: 'Door MainDoor'");
            var ln = Console.ReadLine()?.Trim().Split(' ');
            string name = null;
            if (ln?.Length == 2)
            {
                name = ln[1];
            }

            Enum.TryParse(ln?[0], out DeviceType type);
            return (type, name);
        }

        // Asks user to type section -> returns Section object or null if not found
        private static Section GetSection()
        {
            Console.WriteLine("Please enter section name...");
            var section = Console.ReadLine();
            return Building.Sections.FirstOrDefault(x => x.Name == section);
        }
        #endregion
    }
}
