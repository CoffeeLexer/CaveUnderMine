namespace DungeonScripts
{
    public enum Direction : byte
    {
        Center    = 0b00000000,
        
        Up        = 0b00000001,
        Left      = 0b00000010,
        Down      = 0b00000100,
        Right     = 0b00001000,
        
        UpLeft    = 0b10000011,
        UpRight   = 0b10001001,
        DownLeft  = 0b10000110,
        DownRight = 0b10001100,
    }
}