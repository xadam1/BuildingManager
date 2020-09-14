using System;

namespace BuildingManager
{
    public class Door : Device
    {
        public Door(string name) : base(DeviceType.Door, name ?? "Door")
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