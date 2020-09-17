using System;
using System.IO;
using BuildingManager.CustomEventArgs;
using BuildingManager.Devices;

namespace BuildingManager
{
    public static class Helper
    {
        private static int _idCounter;

        public static int CalculateNewId() => _idCounter++;

        #region EventHandlers
        public static void OnDeviceModified(BuildingPartModifiedEventArgs args) =>
            PrintDeviceInfo(args.Device);

        public static void OnSectionModified(BuildingPartModifiedEventArgs args) =>
            PrintSectionInfo(args.Section);

        public static void OnDeviceError(Device device, ErrorEventArgs e) =>
            PrintError(e.GetException().Message);

        public static void OnDeviceRemoved(BuildingPartModifiedEventArgs args) =>
            PrintDeviceDeletedFromSectionMessage(args.Device, args.Section);

        public static void OnSectionRemoved(BuildingPartModifiedEventArgs args)
        {
            args.Building.BuildingPlan();
            PrintSectionDeleted(args.Section);
        }

        public static void OnDeviceMoved(BuildingPartModifiedEventArgs args)
        {
            args.Building.BuildingPlan();
            PrintDeviceMoved(args.Device, args.OldDeviceSection, args.CurrentDeviceSection);
        }
        #endregion


        #region GeneralPrints
        public static void PrintError(string msg)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"ERROR: {msg}");
            Console.ResetColor();
        }

        public static void Greetings()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("WELCOME TO BUILDING MANAGER!");
            Console.ResetColor();
        }


        public static void PrintHelp()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("*COMMAND OVERVIEW*");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Overall commands");
            Console.ForegroundColor = ConsoleColor.Black;
            PrintCommand("'exit'", "Terminates the application");
            PrintCommand("'help'/'h'", "Displays this help table");
            PrintCommand("'building'/'plan'", "Displays plan of the whole building, Sections and Devices included");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Section Commands");
            Console.ForegroundColor = ConsoleColor.Black;
            PrintCommand("'add section [name]'", "Creates new section");
            PrintCommand("'info section [name]'", "Displays info about section");
            PrintCommand("'rename section [name] [new name]'", "Sets new name for section");
            PrintCommand("'delete section [name]'", "Removes section");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Device Commands");
            Console.ForegroundColor = ConsoleColor.Black;
            PrintCommand("'add device [section name] [device name]'", "Creates new device in section");
            PrintCommand("'info device [name/id]'", "Displays info about device");
            PrintCommand("'rename device [name/id] [new name]'", "Sets new name for device");
            PrintCommand("'delete device [name/id]'", "Removes selected device");
            PrintCommand("'move [name/id] [new section]'/'mv'", "Moves selected device to another section");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Change Command");
            Console.ForegroundColor = ConsoleColor.Black;
            PrintCommand("'change access [name/id] [new access number]'", "Sets new AccessCard Number to CardReader");
            PrintCommand("'change volume [name/id] [new value]'", "Sets new (float) volume value for the Speaker");
            PrintCommand("'change sound [name/id] [Alarm/Music/None]'", "Sets what will speaker play");
            PrintCommand("'change text [name/id] [new text]'", "Sets new Message for the LedPanel");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Set Command");
            Console.ForegroundColor = ConsoleColor.Black;
            PrintCommand("'set/door [name/id] [Open]'", "Toggles door's OPEN state");
            PrintCommand("'set/door [name/id] [Lock]'", "Toggles door's LOCK state");
            PrintCommand("'set/door [name/id] [openedforcibly/force]'", "Toggles door's OpenedForcibly state");
            PrintCommand("'set/door [name/id] [openfortoolong/toolong/long]'", "Toggles door's OpenForTooLong state");
            Console.WriteLine();
            Console.ResetColor();
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
        #endregion


        #region InfoAndMessages
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
            Console.WriteLine($"Device moved: {device.Name} (ID: {device.Id})\nFrom: {previousSection.Name}\nTo: {newSection.Name}");
            Console.ResetColor();
            PrintSectionInfo(newSection);
        }
        #endregion
    }
}