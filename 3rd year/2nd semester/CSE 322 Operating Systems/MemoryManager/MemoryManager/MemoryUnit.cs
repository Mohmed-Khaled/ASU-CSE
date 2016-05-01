namespace MemoryManager
{
    public class MemoryUnit
    {
        public string Name { get; set; }
        public uint StartingAddress { get; set; }
        public uint Size { get; set; }
        public uint EndAddress { get; set; }
        public UnitType Type { get; set; }

        public MemoryUnit(string name, uint startingAddress, uint size, UnitType type)
        {
            Name = name;
            StartingAddress = startingAddress;
            Size = size;
            EndAddress = StartingAddress + Size - 1;
            Type = type;
        }
    }

    public enum UnitType
    {
        Process, Hole, Reserved
    }
}
