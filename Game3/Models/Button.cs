using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Models
{
    class Button
    {
        private bool isHovering;
        public Color FontColour { get; set; } = Color.Black;
        private Texture2D texture;
        private SpriteFont font;
        private Rectangle area;

        public bool Clicked { get; set; }
        public string Text { get; set; }
        public Vector2 Position { get; set; }

        public Button(Vector2 position,Texture2D texture, SpriteFont font)
        {
            this.Position = position;
            this.texture = texture;
            this.font = font;
            this.isHovering = false;
            this.Clicked = false;
            this.area = GetArea();
        }

        public Button(Vector2 position, Texture2D texture, SpriteFont font, string text)
        {
            this.Position = position;
            this.texture = texture;
            this.font = font;
            this.Text = text;
            this.isHovering = false;
            this.Clicked = false;
            this.area = GetArea();
        }

        public Rectangle GetArea()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, texture.Bounds.Width, texture.Bounds.Height);
        }

        public void Update(GameTime gameTime, string text)
        {
            if (area.Contains(Mouse.GetState().Position))
            {
                isHovering = true;
            }
            else
            {
                isHovering = false;
            }

            if (area.Contains(Mouse.GetState().Position) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Clicked = true;
            }
            else
            {
                Clicked = false;
            }

            Text = text;
            area = GetArea();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var colour = Color.White;
            if (isHovering)
            {
                colour = Color.LightBlue;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(texture, area, colour);
            if (!string.IsNullOrEmpty(Text))
            {
                var x = area.Center.X;
                var y = area.Center.Y;

                spriteBatch.DrawString(font, Text, new Vector2(x, y), FontColour);
            }
            spriteBatch.End();
        }

        public void DrawToPosition(SpriteBatch spriteBatch, Camera camera)
        {
            var colour = Color.White;
            if (isHovering)
            {
                colour = Color.LightBlue;
            }
            spriteBatch.Begin(transformMatrix: camera.Transform);
            spriteBatch.Draw(texture, area, colour);
            if (!string.IsNullOrEmpty(Text))
            {
                var x = area.Center.X;
                var y = area.Center.Y;

                spriteBatch.DrawString(font, Text, new Vector2(x, y), FontColour);
            }
            spriteBatch.End();
        }
    }
}
