using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game3.Models
{
    class Dwelling : Sprite
    {
        private Texture2D texture;

        public int OccupantSlots { get; set; } = 2; 
        public List<AiUnit> Occupants { get; set; }
        public string Type { get; set; }

        public Dwelling(Vector2 position, Texture2D texture, string type) : base(position,texture)
        {
            this.Position = position;
            this.texture = texture;
            this.Type = type;
            this.Occupants = new List<AiUnit>();
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
