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
    class Player : Sprite
    {
        private Texture2D texture;

        public bool Selected { get; set; }
        public bool IsMoving { get; set; }
        public float Speed { get; set; } = 200;
        public List<Flag> Flags { get; set; }
        public Vector2 Destination { get; set; }

        public Player(Vector2 position,Texture2D texture) : base(position, texture)
        {
            this.Position = position;
            this.texture = texture;
            this.Area = GetArea();
            this.Centre = Area.Center.ToVector2();
            this.Flags = new List<Flag>();
            this.Speed = 200;
            this.Selected = false;
            this.IsMoving = false;
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
