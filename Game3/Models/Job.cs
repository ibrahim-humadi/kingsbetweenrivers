using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game3.Models
{
    class Job : Sprite
    {
        private Texture2D texture;

        public int WorkSlots { get; set; } = 2;
        public List<AiUnit> Workers { get; set; }

        public Job(Vector2 position, Texture2D texture, string role) : base(position,texture)
        {
            this.Position = position;
            this.texture = texture;
            this.Role = role;
            this.Workers = new List<AiUnit>();
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
