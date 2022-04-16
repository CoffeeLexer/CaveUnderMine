using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{


    class Layout
    {
        private int Width;
        private int Height;
    }

    class Column
    {
        public GameObject Object;
        public Dictionary<Direction, SidePair> SidePairs;

        public Column()
        {
            SidePairs = new Dictionary<Direction, SidePair>
            {
                {Direction.Up, null},
                {Direction.Left, null},
                {Direction.Right, null},
                {Direction.Down, null}
            };
        }
    }

    class Side
    {
        public GameObject Object;
    }

    class SidePair
    {
        public Dictionary<Direction, Side> Pair;
        public bool Horizontal;
        
        public SidePair(bool horizontal)
        {
            Horizontal = horizontal;
            Pair = new Dictionary<Direction, Side>
            {
                {Direction.Up, null},
                {Direction.Left, null},
                {Direction.Right, null},
                {Direction.Down, null}
            };
        }
    }

    class Tile
    {
        // TODO: Game object
        // TODO: Sides
        // TODO: Columns
    }
    enum Direction : byte
    {
        Center    = 0b00000000,
        
        Up        = 0b00000001,
        Left      = 0b00000010,
        Down      = 0b00000100,
        Right     = 0b00001000,
        
        UpLeft    = 0b10000011,
        UpRight   = 0b10001001,
        DownLeft  = 0b10000110,
        DownRight = 0b10001100
    }

    class Utilities
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
    }
}