using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Models
{
    class Structure : AiUnit
    {
        private Texture2D texture;

        public Structure(Vector2 position, Texture2D texture, string role) : base(position, texture, role)
        {
            this.Position = position;
            this.texture = texture;
            this.Role = role;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
