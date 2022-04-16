using UnityEngine;
using System.Collections.Generic;

namespace DungeonScripts
{
    public class DoubleSide
    {
        public Vector3 position;
        public Side[] sides;
        public readonly bool Horizontal;
        public DoubleSide(Side upLeft, Side downRight, bool horizontal, Vector3 pos)
        {
            // HORIZONTAL:
            //       UP
            //     ------
            //      DOWN
            // VERTICAL:
            //      |
            // LEFT | RIGHT
            //      |
            position = pos;
            Horizontal = horizontal;
            sides = new Side[2];
            sides[0] = upLeft;
            sides[1] = downRight;
        }
        public void SetSide(Side s, Direction d)
        {
            int index = Utilities.DirectionToIndex(d) / 2;
            sides[index] = s;
        }
    }
}