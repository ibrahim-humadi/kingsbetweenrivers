using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Models
{
    abstract class Sprite
    {
        private Texture2D texture;

        public Rectangle Area { get; set; }
        public String Role { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Centre { get; set; }

        public Sprite(Vector2 position,Texture2D texture)
        {
            this.Position = position;
            this.texture = texture;
            this.Area = GetArea();
            this.Centre = Area.Center.ToVector2();
        }

        public Rectangle GetArea()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, texture.Bounds.Width, texture.Bounds.Height);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}