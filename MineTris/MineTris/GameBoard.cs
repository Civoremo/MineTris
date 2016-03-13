using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MineTris
{
    public class GameBoard
    {
        public List<Blocks> blocks;
        public List<Texture2D> textures;
        public Vector2 StartingPos = new Vector2(50, 80);

        int x = 1;
        int row = 12;

        public GameBoard(List<Texture2D> textures)
        {
            this.textures = textures;
            this.blocks = new List<Blocks>();
        }


        public Blocks BlockGenerator(Vector2 position, bool active)
        {
            Texture2D tex = null;
            Vector2 pos = position;
            bool Active = active;


            return new Blocks(tex, pos, Active); 
        }


        public void Update()
        {
           if (x > 0)
            {
                FillGameBoard();
                x--;
            }
            
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //for (int n = 0; n < blocks.Count; n++)
            //{
            //    if (blocks[n].isActive == true)
            //    {
            //        blocks[n].Draw(spriteBatch);
            //    }

            //}

            if (blocks.Count != 0)
            {
                for (int k = 0; k < 12; k++)
                {
                    for (int s = 17; s >= 0; s--)
                    {
                        if (blocks[k + (row * s)].isActive)
                        {
                            blocks[k + (row * s)].Draw(spriteBatch);
                        }
                    }
                }
            }

        }


        public void FillGameBoard()
        {
            
            Vector2 position = StartingPos;
            int OffSetX = 32;
            int OffSetY = 32;
            int count = 0;
            bool active;
            

            for (int i = 0; i < 18; i++ )
            {
                count++;

                for (int j = 0; j < 12; j++ )
                {
                    if (i > 10) 
                    {
                        active = false;
                        blocks.Add(BlockGenerator(position, active));
                    }
                    else
                    {
                        active = false;
                        blocks.Add(BlockGenerator(position, active));
                    }
                    position.X += OffSetX;
                }
                position = StartingPos;
                position.Y += (OffSetY * count);
            }
        }
      
    }
}
