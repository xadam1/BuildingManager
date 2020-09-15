using System;
using System.IO;
using BuildingManager.CustomEventArgs;
using BuildingManager.Devices;

namespace BuildingManager
{
    public static class Helper
    {
        private static int _idCounter;

        #region EventHandlers
        public static void OnDeviceModified(BuildingPartModifiedEventArgs args)
        {
            var device = args.Device;
            switch (device.Type)
            {
                case DeviceTypes.Door:
                    PrintDeviceInfo(device as Door);
                    break;
                case DeviceTypes.Speaker:
                    PrintDeviceInfo(device as Speaker);
                    break;
                case DeviceTypes.LedPanel:
                    PrintDeviceInfo(device as LedPanel);
                    break;
                case DeviceTypes.CardReader:
                    PrintDeviceInfo(device as CardReader);
                    break;
            }
        }

        public static void OnDeviceError(Device device, ErrorEventArgs e)
        {
            PrintError(e.GetException().Message);
        }

        public static void OnSectionModified(BuildingPartModifiedEventArgs args)
        {
            PrintSectionInfo(args.Section);
        }

        public static void OnSectionRemoved(BuildingPartModifiedEventArgs args)
        {
            args.Building.BuildingPlan();
            PrintSectionDeleted(args.Section);
        }

        public static void OnDeviceRemoved(BuildingPartModifiedEventArgs args)
        {
            PrintDeviceDeletedFromSectionMessage(args.Device, args.Section);
        }

        public static void OnDeviceMoved(BuildingPartModifiedEventArgs args)
        {
            args.Building.BuildingPlan();
            PrintDeviceMoved(args.Device, args.oldDeviceSection, args.currentDeviceSection);
        }
        #endregion

        public static int CalculateNewId() => _idCounter++;

        public static void Greetings()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("WELCOME TO BUILDING MANAGER!");
            Console.ResetColor();
        }

        public static void PrintError(string msg)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"ERROR: {msg}");
            Console.ResetColor();
        }

        public static void PrintDeviceInfo(Device device)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n---------- {device.Type} Info ----------");
            Console.ResetColor();

            Console.WriteLine(device.GetCurrentState());

            Console.ForegroundColor = ConsoleColor.Green;
            // 27 => 10x '-' then 'Info' and 10x '-' + 3x ' '
            Console.WriteLine(new string('-', 27 + device.Type.ToString().Length));
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void PrintHelp()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("*Command Overview*");
            Console.ResetColor();

            PrintCommand("'exit'", "Terminates the application");
            PrintCommand("'help'/'h'", "Displays this help table");
            PrintCommand("'building plan'/'plan'", "Displays plan of the whole building, Sections and Devices included");
            Console.WriteLine();

            PrintCommand("'section'", "Displays currently selected SECTION");
            PrintCommand("'select section'", "Select another SECTION");
            PrintCommand("'new section'", "Creates new section");
            PrintCommand("'section info'", "Displays info about selected section");
            PrintCommand("'rename section'", "Sets new name for selected section");
            PrintCommand("'delete section'", "Removes selected section");
            Console.WriteLine();

            PrintCommand("'device'", "Displays currently selected DEVICE");
            PrintCommand("'select device'", "Select another DEVICE");
            PrintCommand("'new device'", "Creates new device in CURRENT SECTION");
            PrintCommand("'device info'", "Displays info about selected device");
            PrintCommand("'rename device'", "Sets new name for selected device");
            PrintCommand("'delete device'/'delete'", "Removes selected device");
            PrintCommand("'move device'/'move'", "Moves selected device to another section");
            Console.WriteLine();

            PrintCommand("'change text'", "If LedPanel is selected change the Message");
            Console.WriteLine();

            PrintCommand("'open door'/'open'", "If Door is selected sets state to Open");
            PrintCommand("'lock door'/'lock'", "If Door is selected sets state to Locked");
            PrintCommand("'opentoolong door'/'toolong'", "If Door is selected sets state to OpenForTooLong");
            PrintCommand("'openforcibly door'/'forcibly'", "If Door is selected sets state to OpenedForcibly");
            Console.WriteLine();

            PrintCommand("'change volume'/'volume'", "If Speaker is selected sets volume");
            PrintCommand("'change sound'/'sound'", "If Speaker is selected select what should play at the speaker");
            Console.WriteLine();

            PrintCommand("'change access'/'access'", "If CardReader is selected set new 'AccessCardNumber'");
            Console.WriteLine();
        }

        private static void PrintCommand(string command, string description)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(command);
            Console.ResetColor();
            Console.Write(" - ");
            Console.WriteLine(description);
        }

        public static void PrintSectionInfo(Section section)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n---------- {section.Name} Info ----------");
            Console.ResetColor();

            Console.WriteLine(section.ListDevices());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(new string('-', 27 + section.Name.Length));
            Console.ResetColor();
            Console.WriteLine();
        }


        public static void PrintDeviceDeletedFromSectionMessage(Device device, Section section)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"Device Removed: {device.Name}\nFrom: {section.Name}");
            Console.ResetColor();
            PrintSectionInfo(section);
        }

        public static void PrintSectionDeleted(Section section)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"Section Removed: {section.Name}");
            Console.ResetColor();
        }

        public static void PrintDeviceMoved(Device device, Section previousSection, Section newSection)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"Device moved: {device.Name}\nFrom: {previousSection.Name}\nTo: {newSection.Name}");
            Console.ResetColor();
            PrintSectionInfo(newSection);
        }
    }

}