using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Models
{
    class Grid : Sprite
    {
        private int size = 32;
        private Texture2D texture;

        public Node[] Positions { get; set; }

        public Grid(Vector2 position, Texture2D texture) : base(position,texture)
        {
            this.Position = position;
            this.texture = texture;
            this.Positions = CreateGridNodes();
        }

        public Node[] CreateGridNodes()
        {
            Positions = new Node[1024000];
            int x = 0;
            for (int i = 0; i <= 100; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    Positions[x] = new Node(true, new Vector2(i * 32, j * 32));
                    x++;
                }
            }
            return Positions;
        }

        public Node[] GetNeighbours(Node node)
        {
            List<Node> neighbourNodes = new List<Node>();
            List<Vector2> children = new List<Vector2>();

            if (node.Position.X > 0 && node.Position.Y > 0 && node.Position.X < 3200 && node.Position.Y < 3200)
            {
                children.Add(new Vector2(node.Position.X - size, node.Position.Y - size));
                children.Add(new Vector2(node.Position.X, node.Position.Y - size));
                children.Add(new Vector2(node.Position.X + size, node.Position.Y - size));
                children.Add(new Vector2(node.Position.X - size, node.Position.Y));
                children.Add(new Vector2(node.Position.X + size, node.Position.Y));
                children.Add(new Vector2(node.Position.X - size, node.Position.Y + size));
                children.Add(new Vector2(node.Position.X, node.Position.Y + size));
                children.Add(new Vector2(node.Position.X + size, node.Position.Y + size));
            }

            foreach (var child in children)
            {           
                    if (Positions.FirstOrDefault(x => x.Position == child) != null)
                    {
                        neighbourNodes.Add(Positions.FirstOrDefault(x => x.Position == child));
                    }               
            }
            return neighbourNodes.ToArray();  
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
