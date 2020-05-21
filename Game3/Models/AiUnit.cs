using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game3.Models
{
    class AiUnit : Sprite
    {
        private Texture2D texture;

        public bool Selected { get; set; }
        public bool IsMoving { get; set; }
        public bool PlayerControlled { get; set; }
        public bool Interrupt { get; set; }
        public Button UnitSelectedBtn { get; set; }
        public Dwelling Dwelling { get; set; }
        public float Speed { get; set; } = 10;
        public Job Occupation { get; set; }
        public List<Flag> Flags { get; set; }
        public WayPoint CurrentWaypoint { get; set; }
        public Vector2 Destination { get; set; }

        public AiUnit(Vector2 position,Texture2D texture, string role) : base(position, texture)
        {
            this.Position = position;
            this.texture = texture;
            this.Role = role;
            this.Flags = new List<Flag>();
            this.Interrupt = false;
        }

        public override void Update(GameTime gameTime)
        {
            Area = GetArea();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }     
    }
}
