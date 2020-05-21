using Game3.Components;
using Game3.Interfaces;
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
    class Camera
    {
        private bool buttonHeld = false;
        private ContentManager content;
        private float rotation = 0;
        private Game game;
        private GraphicsDevice graphicsDevice;
        private int previousScrollValue;
        private Rectangle selectionBox;
        private Texture2D rectTexture;
        private Vector2 centre;
        private Vector2 mousePreviousPosition;
        private Vector2 mouseNewPosition;
        private Viewport viewport;

        public bool SelectionSwitch { get; set; }
        public float Zoom { get; set; } = 1;
        public Matrix Transform { get; set; }
        public Rectangle SelectionBox { get; set; }
        public Rectangle SelectionArea { get; set; }
        public Vector2 Position { get; set; }
        
        public Camera(Game game, Viewport viewport, ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.game = game;
            this.viewport = viewport;
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            this.SelectionSwitch = true;
        }

        public void AddToSelection(Rectangle selectionBox, UnitController unitController)
        {
            foreach (var aiUnit in unitController.AiUnits)
            {
                if (selectionBox.Contains(aiUnit.Position))
                {
                    aiUnit.Selected = true;
                }
                else
                {
                    aiUnit.Selected = false;
                }
            }
        }

        public Vector2 GetMapCords()
        {
            Vector2 worldPosition = Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(Transform));
            return worldPosition;
        }

        public Vector2 GetNearestGrid32 (Vector2 position, double size)
        {
           return new Vector2(Convert.ToSingle(Math.Floor((position.X + size / 2) / size) * size), Convert.ToSingle(Math.Floor((position.Y + size / 2) / size) * size));          
        }

        public void SelectionRectangle(GameTime gameTime, UnitController unitController)
        {
            if (SelectionSwitch)
            {
                var mstate = Mouse.GetState();
                if (mstate.LeftButton == ButtonState.Pressed && !buttonHeld)
                {
                    mousePreviousPosition = GetMapCords();
                    mouseNewPosition = mousePreviousPosition;
                    buttonHeld = true;
                }

                if (buttonHeld)
                {
                    mouseNewPosition = GetMapCords();
                }

                selectionBox = new Rectangle();
                selectionBox.Width = Math.Abs((int)mouseNewPosition.X - (int)mousePreviousPosition.X);
                selectionBox.Height = Math.Abs((int)mouseNewPosition.Y - (int)mousePreviousPosition.Y);

                if (mouseNewPosition.X <= mousePreviousPosition.X && mouseNewPosition.Y <= mousePreviousPosition.Y)
                {
                    selectionBox.X = (int)mouseNewPosition.X;
                    selectionBox.Y = (int)mouseNewPosition.Y;
                }

                if (mouseNewPosition.X >= mousePreviousPosition.X && mouseNewPosition.Y <= mousePreviousPosition.Y)
                {
                    selectionBox.X = (int)mousePreviousPosition.X;
                    selectionBox.Y = (int)mouseNewPosition.Y;
                }

                if (mouseNewPosition.X >= mousePreviousPosition.X && mouseNewPosition.Y >= mousePreviousPosition.Y)
                {
                    selectionBox.X = (int)mousePreviousPosition.X;
                    selectionBox.Y = (int)mousePreviousPosition.Y;
                }

                if (mouseNewPosition.X <= mousePreviousPosition.X && mouseNewPosition.Y >= mousePreviousPosition.Y)
                {
                    selectionBox.X = (int)mouseNewPosition.X;
                    selectionBox.Y = (int)mousePreviousPosition.Y;
                }

                if (mstate.LeftButton == ButtonState.Released && buttonHeld)
                {
                    AddToSelection(selectionBox, unitController);
                    buttonHeld = false;
                }

                if (!buttonHeld)
                {
                    selectionBox.Height = 0;
                    selectionBox.Width = 0;
                }

                if (selectionBox.Width != 0 && selectionBox.Height != 0 && buttonHeld)
                {
                    Color[] data = new Color[Math.Abs(selectionBox.Width) * Math.Abs(selectionBox.Height)];
                    rectTexture = new Texture2D(graphicsDevice, Math.Abs(selectionBox.Width), Math.Abs(selectionBox.Height));

                    for (int i = 0; i < data.Length; ++i)
                        data[i] = Color.White;

                    rectTexture.SetData(data);
                }
                else
                {
                    Color[] data = new Color[1];
                    rectTexture = new Texture2D(graphicsDevice, 1, 1);

                    for (int i = 0; i < data.Length; ++i)
                        data[i] = Color.Transparent;

                    rectTexture.SetData(data);
                }
            }            
        }


        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            var mstate = Mouse.GetState();
            UnitController unitController = (UnitController)game.Services.GetService(typeof(IUnitController));

            centre = new Vector2(Position.X, Position.Y);

            Transform = Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));

            if (kstate.IsKeyDown(Keys.S))
            {
                Position = Vector2.Add(Position, new Vector2(0f, 6f));
            }

            if (kstate.IsKeyDown(Keys.W))
            {
                Position = Vector2.Add(Position, new Vector2(0f, -6f));
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                Position = Vector2.Add(Position, new Vector2(-6f, 0f));
            }

            if (kstate.IsKeyDown(Keys.D))
            {
                Position = Vector2.Add(Position, new Vector2(6f, 0f));
            }

            if (mstate.ScrollWheelValue < previousScrollValue)
            {
                if (!(Zoom <= 0.3f))
                {
                    Zoom -= 0.1f;
                }
            }
             if (mstate.ScrollWheelValue > previousScrollValue)
            {
                if (!(Zoom >= 3.5f))
                {
                    Zoom += 0.1f;
                }
            }
            previousScrollValue = mstate.ScrollWheelValue;
            SelectionRectangle(gameTime,unitController);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Transform);
            if (SelectionSwitch)
            {
                spriteBatch.Draw(rectTexture, selectionBox.Location.ToVector2(), Color.White);
            }
            spriteBatch.End();
        }
    }
}
