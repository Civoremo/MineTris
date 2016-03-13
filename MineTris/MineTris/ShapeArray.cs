using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace MineTris
{
    public class ArrayBlock
    {


        public Vector2[,] Block;
        public Vector2 position;


        public ArrayBlock(Vector2 startingPosition)
        {
            position = startingPosition;
            Block = new Vector2[4, 4];

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Block[row, col] = new Vector2(position.X, position.Y);
                    position.X += 32;
                }
                position.X -= (4 * 32);
                position.Y += 32;
            }
        }
    }
}
