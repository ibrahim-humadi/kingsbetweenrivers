using Game3.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Interfaces
{
    interface IUiController
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime, Camera camera);
        void LoadContent();
        void Update(GameTime gameTime);
    }
}
