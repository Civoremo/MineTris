using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace MineTris
{
    public class Blocks
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public bool isActive { get; set; }

        public int textureWidth { get; set; }
        public int textureHeight { get; set; }

        public Blocks(Texture2D tex, Vector2 pos, bool active)
        {
            Texture = tex;
            Position = pos;
            isActive = active;

        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
