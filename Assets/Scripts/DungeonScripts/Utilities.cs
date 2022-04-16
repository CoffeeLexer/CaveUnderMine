namespace DungeonScripts
{
    public class Utilities
    {
        public static Direction Invert(Direction d)
        {
            if (((byte)d & 0b10000000) != 0)
            {
                return (Direction)((byte)d ^ 0b00001111);
            }
            else
            {
                if (((byte) d & 0b00000011) != 0)
                {
                    return (Direction) ((byte) d << 2 );
                }
                else
                {
                    return (Direction) ((byte) d >> 2);
                }
            }
        }

        public static int DirectionToIndex(Direction d)
        {
            switch (d)
            {
                case Direction.Down:
                case Direction.DownRight:
                    return 0;
                case Direction.Right:
                case Direction.DownLeft:
                    return 1;
                case Direction.Left:
                case Direction.UpRight:
                    return 2;
                case Direction.Up:
                case Direction.UpLeft:
                    return 3;
            }
            return -1;
        }
    }
}