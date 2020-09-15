using System;

namespace BuildingManager.Devices
{
    public class Door : Device
    {
        public Door(string name) : base(DeviceTypes.Door, name ?? "Door")
        {
        }

        public override string GetCurrentState()
        {
            return base.GetCurrentState() + $"\nState: {State}";
        }


        public DoorStates State { get; set; } = DoorStates.Locked;


        public bool Open
        {
            get => (State & DoorStates.Open) == DoorStates.Open;
            set
            {
                if (value)
                {
                    State |= DoorStates.Open;
                }
                else
                {
                    State ^= DoorStates.Open;
                }
                OnDeviceModified();
            }
        }

        public bool Locked
        {
            get => (State & DoorStates.Locked) == DoorStates.Locked;
            set
            {
                if (value)
                {
                    State |= DoorStates.Locked;
                }
                else
                {
                    State ^= DoorStates.Locked;
                }
                OnDeviceModified();
            }
        }

        public bool OpenForTooLong
        {
            get => (State & DoorStates.OpenForTooLong) == DoorStates.OpenForTooLong;
            set
            {
                if (value)
                {
                    State |= DoorStates.OpenForTooLong;
                }
                else
                {
                    State ^= DoorStates.OpenForTooLong;
                }
                OnDeviceModified();
            }
        }

        public bool OpenedForcibly
        {
            get => (State & DoorStates.OpenedForcibly) == DoorStates.OpenedForcibly;
            set
            {
                if (value)
                {
                    State |= DoorStates.OpenedForcibly;
                }
                else
                {
                    State ^= DoorStates.OpenedForcibly;
                }
                OnDeviceModified();
            }
        }


        [Flags]
        public enum DoorStates
        {
            Locked = 1,
            Open = 2,
            OpenForTooLong = 4,
            OpenedForcibly = 8
        }
    }
}