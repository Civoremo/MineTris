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
    public class Shapes
    {
        public Vector2[,] Blocks { get; set; }

        public int blockCount;
        public int rotationCount;
        public Vector2[,] Block;
        public Vector2 position;
        public List<Texture2D> TextureList;
        Random random = new Random();

        public float alphaColor = 1;

        public Vector2 posBlock1, posBlock2, posBlock3, posBlock4;

        public Texture2D block1, block2, block3, block4;

        public Rectangle boundingBoxBlock1, boundingBoxBlock2, boundingBoxBlock3, boundingBoxBlock4;


        public Shapes(Vector2[,] block, List<Texture2D> tex)
        {

            TextureList = tex;

            block1 = TextureList[0];
            block2 = TextureList[0];
            block3 = TextureList[0];
            block4 = TextureList[0];

            

            Blocks = block;


            blockCount = 0;
            rotationCount = 0;
        }

        public void LoadContent(ContentManager Content)
        {
            
            block4 = Content.Load<Texture2D>("mainGame/blue");
            block3 = Content.Load<Texture2D>("mainGame/blue");
            block2 = Content.Load<Texture2D>("mainGame/blue");
            block1 = Content.Load<Texture2D>("mainGame/blue");
        }

        public void Update()
        {

            CurrentShape();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(block1, posBlock1, Color.White * alphaColor);
            spriteBatch.Draw(block2, posBlock2, Color.White * alphaColor);
            spriteBatch.Draw(block3, posBlock3, Color.White * alphaColor);
            spriteBatch.Draw(block4, posBlock4, Color.White * alphaColor);
        }


        public void CurrentShape()
        {

            switch (blockCount)
            {
                case 0: // L shaped block
                    switch (rotationCount)
                    {
                        case 0:
                            posBlock4 = Blocks[1, 1];
                            posBlock3 = Blocks[2, 1];
                            posBlock1 = Blocks[3, 1];
                            posBlock2 = Blocks[3, 2];
                            break;
                        case 1:
                            posBlock3 = Blocks[1, 3];
                            posBlock1 = Blocks[2, 1];
                            posBlock4 = Blocks[2, 2];
                            posBlock2 = Blocks[2, 3];
                            break;
                        case 2:
                            posBlock1 = Blocks[0, 1];
                            posBlock4 = Blocks[0, 2];
                            posBlock3 = Blocks[1, 2];
                            posBlock2 = Blocks[2, 2];
                            break;
                        case 3:
                            posBlock1 = Blocks[1, 0];
                            posBlock2 = Blocks[1, 1];
                            posBlock3 = Blocks[1, 2];
                            posBlock4 = Blocks[2, 0];
                            break;
                        case 4:
                            rotationCount = 0;
                            break;
                    }
                    break;
                case 1: // T shaped block
                    switch (rotationCount)
                    {
                        case 0:
                            posBlock4 = Blocks[1, 1];
                            posBlock1 = Blocks[2, 0];
                            posBlock2 = Blocks[2, 1];
                            posBlock3 = Blocks[2, 2];
                            break;
                        case 1:
                            posBlock4 = Blocks[1, 1];
                            posBlock2 = Blocks[2, 1];
                            posBlock3 = Blocks[2, 2];
                            posBlock1 = Blocks[3, 1];
                            break;
                        case 2:
                            posBlock1 = Blocks[2, 0];
                            posBlock2 = Blocks[2, 1];
                            posBlock3 = Blocks[2, 2];
                            posBlock4 = Blocks[3, 1];
                            break;
                        case 3:
                            posBlock4 = Blocks[1, 1];
                            posBlock2 = Blocks[2, 0];
                            posBlock3 = Blocks[2, 1];
                            posBlock1 = Blocks[3, 1];
                            break;
                        case 4:
                            rotationCount = 0;
                            break;
                    }
                    break;
                case 2: // Z shaped block
                    switch (rotationCount)
                    {
                        case 0:
                            posBlock1 = Blocks[1, 0];
                            posBlock3 = Blocks[1, 1];
                            posBlock2 = Blocks[2, 1];
                            posBlock4 = Blocks[2, 2];
                            break;
                        case 1:
                            posBlock4 = Blocks[1, 1];
                            posBlock2 = Blocks[2, 0];
                            posBlock3 = Blocks[2, 1];
                            posBlock1 = Blocks[3, 0];
                            break;
                        case 2:
                            rotationCount = 0;
                            break;
                    }
                    break;
                case 3: // S shaped block
                    switch (rotationCount)
                    {
                        case 0:
                            posBlock1 = Blocks[1, 1];
                            posBlock2 = Blocks[1, 2];
                            posBlock3 = Blocks[2, 0];
                            posBlock4 = Blocks[2, 1];
                            break;
                        case 1:
                            posBlock4 = Blocks[1, 0];
                            posBlock2 = Blocks[2, 0];
                            posBlock3 = Blocks[2, 1];
                            posBlock1 = Blocks[3, 1];
                            break;
                        case 2:
                            rotationCount = 0;
                            break;
                    }
                    break;
                case 4: // J shaped block
                    switch (rotationCount)
                    {
                        case 0:
                            posBlock4 = Blocks[1, 2];
                            posBlock3 = Blocks[2, 2];
                            posBlock2 = Blocks[3, 1];
                            posBlock1 = Blocks[3, 2];
                            break;
                        case 1:
                            posBlock2 = Blocks[1, 1];
                            posBlock3 = Blocks[1, 2];
                            posBlock4 = Blocks[1, 3];
                            posBlock1 = Blocks[2, 3];
                            break;
                        case 2:
                            posBlock1 = Blocks[0, 1];
                            posBlock2 = Blocks[0, 2];
                            posBlock4 = Blocks[1, 1];
                            posBlock3 = Blocks[2, 1];
                            break;
                        case 3:
                            posBlock4 = Blocks[1, 0];
                            posBlock1 = Blocks[2, 0];
                            posBlock2 = Blocks[2, 1];
                            posBlock3 = Blocks[2, 2];
                            break;
                        case 4:
                            rotationCount = 0;
                            break;
                    }
                    break;
                case 5: // I shaped block
                    switch (rotationCount)
                    {
                        case 0:
                            posBlock4 = Blocks[0, 1];
                            posBlock3 = Blocks[1, 1];
                            posBlock2 = Blocks[2, 1];
                            posBlock1 = Blocks[3, 1];
                            break;
                        case 1:
                            posBlock1 = Blocks[1, 0];
                            posBlock2 = Blocks[1, 1];
                            posBlock3 = Blocks[1, 2];
                            posBlock4 = Blocks[1, 3];
                            break;
                        case 2:
                            rotationCount = 0;
                            break;
                    }
                    break;
                case 6: // O shaped block
                    switch (rotationCount)
                    {
                        case 0:
                            posBlock2 = Blocks[1, 1];
                            posBlock4 = Blocks[1, 2];
                            posBlock1 = Blocks[2, 1];
                            posBlock3 = Blocks[2, 2];
                            break;
                        case 1:
                            rotationCount = 0;
                            break;
                    }
                    break;
                case 7:
                    blockCount = 0;
                    break;
            }
        }

    }
}
