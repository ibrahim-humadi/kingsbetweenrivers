using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game3.Interfaces;
using Game3.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game3.Components
{
    class UiController : Component, IUiController
    {
        private bool buildingModeActive;
        private bool peopleModeActive;
        private bool buttonHeld = false;
        private bool demoModeActive = false;
        private Button selectedPlayerBtn;
        private Button selectedUnitsBtn;
        private Button buildingModeBtn;
        private Button peopleModeBtn;
        private Button pointer;
        private Button undoBtn;
        private Button redoBtn;
        private Button demoBtn;
        private Button saveBtn;
        private Button loadBtn;
        private Button currentBuildingBtn;
        private Button currentPeopleBtn;
        private ContentManager content;
        private Camera camera;
        private Game game;
        private History history;
        private List<Button> buildingsBtns;
        private List<Button> peopleBtns;
        private UnitController unitController;
        private Vector2 mousePosition;

        public UiController(Game game, ContentManager content, Camera camera) : base(content)
        {
            this.game = game;
            this.content = content;
            this.camera = camera;
            this.buildingsBtns = new List<Button>();
            this.peopleBtns = new List<Button>();
            this.history = new History(game);
        }

        public void GetBuildings(Vector2 position)
        {
            buildingsBtns.Add(new Button(position, content.Load<Texture2D>("selected60"), content.Load<SpriteFont>("Ariel"), ""));
            buildingsBtns.Add(new Button(new Vector2(position.X, position.Y - 48), content.Load<Texture2D>("wall"), content.Load<SpriteFont>("Ariel"), "wall"));
            buildingsBtns.Add(new Button(new Vector2(position.X, position.Y - 81), content.Load<Texture2D>("wall2"), content.Load<SpriteFont>("Ariel"), "wall2"));
            buildingsBtns.Add(new Button(new Vector2(position.X, position.Y - 114), content.Load<Texture2D>("wall3"), content.Load<SpriteFont>("Ariel"), "wall3"));
            buildingsBtns.Add(new Button(new Vector2(position.X, position.Y - 147), content.Load<Texture2D>("sandstonebasewall"), content.Load<SpriteFont>("Ariel"), "sandstonebasewall"));
            buildingsBtns.Add(new Button(new Vector2(position.X, position.Y - 180), content.Load<Texture2D>("sandstonedoorway"), content.Load<SpriteFont>("Ariel"), "door"));
            buildingsBtns.Add(new Button(new Vector2(position.X, position.Y - 213), content.Load<Texture2D>("sandstonefloor"), content.Load<SpriteFont>("Ariel"), "flooring"));
        }

        public void GetPeople(Vector2 position)
        {
            peopleBtns.Add(new Button(position, content.Load<Texture2D>("selected60"), content.Load<SpriteFont>("Ariel"), ""));
            peopleBtns.Add(new Button(new Vector2(position.X, position.Y - 48), content.Load<Texture2D>("worker"), content.Load<SpriteFont>("Ariel"), "worker"));
        }

        public override void LoadContent()
        {
            unitController = (UnitController)game.Services.GetService(typeof(IUnitController));
            selectedPlayerBtn = new Button(new Vector2(1100, 100), content.Load<Texture2D>("transparent32"), content.Load<SpriteFont>("Ariel"));
            selectedUnitsBtn = new Button(new Vector2(1100, 200), content.Load<Texture2D>("transparent32"), content.Load<SpriteFont>("Ariel"));
            buildingModeBtn = new Button(new Vector2(1100, 800), content.Load<Texture2D>("build"), content.Load<SpriteFont>("Ariel"));
            peopleModeBtn = new Button(new Vector2(1030, 800), content.Load<Texture2D>("people"), content.Load<SpriteFont>("Ariel"));
            pointer = new Button(new Vector2(0, 0), content.Load<Texture2D>("pointer"), content.Load<SpriteFont>("Ariel"));

            undoBtn = new Button(new Vector2(700, 800), content.Load<Texture2D>("undobtn"), content.Load<SpriteFont>("Ariel"));
            redoBtn = new Button(new Vector2(760, 800), content.Load<Texture2D>("redobtn"), content.Load<SpriteFont>("Ariel"));

            saveBtn = new Button(new Vector2(860, 800), content.Load<Texture2D>("save"), content.Load<SpriteFont>("Ariel"));
            loadBtn = new Button(new Vector2(920, 800), content.Load<Texture2D>("load"), content.Load<SpriteFont>("Ariel"));
            demoBtn = new Button(new Vector2(1180, 800), content.Load<Texture2D>("demolition"), content.Load<SpriteFont>("Ariel"));

            GetBuildings(buildingModeBtn.Position);
            GetPeople(peopleModeBtn.Position);
        }

        bool undoIsHeld = false;
        bool redoIsHeld = false;
        bool saveIsHeld = false;
        bool loadIsHeld = false;
        bool demoisHeld = false;

        public override void Update(GameTime gameTime)
        {
            MouseState mstate = Mouse.GetState();

            if (selectedPlayerBtn != null)
            {
                string debug = " mode: ";
                selectedPlayerBtn.Update(gameTime, debug);
            }

            if (selectedUnitsBtn != null)
            {
                string debugTwo = "Nearest GridPoint: " + camera.GetNearestGrid32(camera.GetMapCords(), 32);
                selectedUnitsBtn.Update(gameTime, debugTwo);
            }

            if (pointer != null)
            {
                pointer.Position = camera.GetNearestGrid32(camera.GetMapCords(), 32);
                pointer.Update(gameTime, "A");
            }

            if (undoBtn != null)
            {
                undoBtn.Update(gameTime, "");

                if (!undoIsHeld && undoBtn.Clicked && mstate.LeftButton == ButtonState.Pressed)
                {
                    undoIsHeld = true;
                }

                if (undoIsHeld)
                {
                    if (mstate.LeftButton == ButtonState.Released)
                    {
                        history.Undo();
                        undoIsHeld = false;
                    }
                }
            }

            if (redoBtn != null)
            {
                redoBtn.Update(gameTime, "");

                if (!redoIsHeld && redoBtn.Clicked && mstate.LeftButton == ButtonState.Pressed)
                {
                    redoIsHeld = true;
                }

                if (redoIsHeld)
                {
                    if (mstate.LeftButton == ButtonState.Released)
                    {
                        history.Redo();
                        redoIsHeld = false;
                    }
                }
            }

            if (saveBtn != null)
            {
                saveBtn.Update(gameTime, "");

                if (!saveIsHeld && saveBtn.Clicked && mstate.LeftButton == ButtonState.Pressed)
                {
                    saveIsHeld = true;
                }

                if (saveIsHeld)
                {
                    if (mstate.LeftButton == ButtonState.Released)
                    {
                        history.Save("a");
                        saveIsHeld = false;
                    }
                }
            }

            if (loadBtn != null)
            {
                loadBtn.Update(gameTime, "");

                if (!loadIsHeld && loadBtn.Clicked && mstate.LeftButton == ButtonState.Pressed)
                {
                    loadIsHeld = true;
                }

                if (loadIsHeld)
                {
                    if (mstate.LeftButton == ButtonState.Released)
                    {
                        history.Load("a");
                        loadIsHeld = false;
                    }
                }
            }

            if (peopleModeBtn != null)
            {
                if (peopleModeBtn.Clicked && Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    buildingModeActive = false;
                    peopleModeActive = !peopleModeActive;
                }
                peopleModeBtn.Update(gameTime, "");
            }

            if (buildingModeBtn != null)
            {
                if (buildingModeBtn.Clicked && Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    peopleModeActive = false;
                    buildingModeActive = !buildingModeActive;
                }
                buildingModeBtn.Update(gameTime, "");
            }

            if (peopleModeActive)
            {
                foreach (var aiUnit in unitController.AiUnits)
                {
                    aiUnit.Selected = false;
                }

                camera.SelectionSwitch = false;

                mousePosition = camera.GetNearestGrid32(camera.GetMapCords(), 32);

                bool uiCollision = false;

                foreach (var peopleBtn in peopleBtns)
                {
                    if (peopleBtn.GetArea().Contains(mstate.Position))
                    {
                        uiCollision = true;
                        break;
                    }

                    if (undoBtn.GetArea().Contains(mstate.Position))
                    {
                        uiCollision = true;
                        break;
                    }

                    if (redoBtn.GetArea().Contains(mstate.Position))
                    {
                        uiCollision = true;
                        break;
                    }

                    if (saveBtn.GetArea().Contains(mstate.Position))
                    {
                        uiCollision = true;
                        break;
                    }

                    if (loadBtn.GetArea().Contains(mstate.Position))
                    {
                        uiCollision = true;
                        break;
                    }
                }
                    
                foreach (var peopleBtn in peopleBtns)
                {
                    if (peopleBtn.Clicked && !buttonHeld)
                    {
                        currentPeopleBtn = peopleBtn;
                        buttonHeld = true;
                    }
                    else
                    {
                        buttonHeld = false;
                    }

                    if (buttonHeld)
                    {
                        mousePosition = camera.GetNearestGrid32(camera.GetMapCords(), 32);
                    }

                    if (currentPeopleBtn != null && mstate.RightButton == ButtonState.Pressed)
                    {
                        currentPeopleBtn = null;
                    }

                    peopleBtn.Update(gameTime, "");

                    if (!uiCollision && currentPeopleBtn != null && mstate.LeftButton == ButtonState.Pressed)
                    {
                        if (unitController.AiUnits.Count == 0)
                        {
                            unitController.AddPerson(mousePosition, "worker");
                        }
                        else
                        {
                            if (!unitController.AiUnitPositions.Contains(mousePosition))
                            {
                                unitController.AddPerson(mousePosition, "worker");
                            }
                        }
                    }
                }          
            }
            else
            {
                camera.SelectionSwitch = true;
                currentPeopleBtn = null;
            }
        
            if (buildingModeActive)
            {

                foreach (var aiUnit in unitController.AiUnits)
                {
                    aiUnit.Selected = false;
                }

                camera.SelectionSwitch = false;

                mousePosition = camera.GetNearestGrid32(camera.GetMapCords(), 32);

                if (demoBtn != null)
                {
                    demoBtn.Update(gameTime, "");

                    if (!demoisHeld && demoBtn.Clicked && mstate.LeftButton == ButtonState.Pressed)
                    {
                        demoModeActive = !demoModeActive;
                        demoisHeld = true;
                    }

                    if (demoisHeld)
                    {
                        if (mstate.LeftButton == ButtonState.Released)
                        {
                            demoisHeld = false;
                        }
                    }
                }

                bool uiCollision = false;

                if (buildingModeBtn.GetArea().Contains(mstate.Position))
                {
                    uiCollision = true;
                }
                else
                {
                    foreach (var buildingBtn in buildingsBtns)
                    {
                        if (buildingBtn.GetArea().Contains(mstate.Position))
                        {
                            uiCollision = true;
                            break;
                        }

                        if (undoBtn.GetArea().Contains(mstate.Position))
                        {
                            uiCollision = true;
                            break;
                        }

                        if (redoBtn.GetArea().Contains(mstate.Position))
                        {
                            uiCollision = true;
                            break;
                        }

                        if (saveBtn.GetArea().Contains(mstate.Position))
                        {
                            uiCollision = true;
                            break;
                        }

                        if (loadBtn.GetArea().Contains(mstate.Position))
                        {
                            uiCollision = true;
                            break;
                        }
                    }
                }
              
                foreach (var buildingBtn in buildingsBtns)
                {
                    if (buildingBtn.Clicked && !buttonHeld)
                    {
                        currentBuildingBtn = buildingBtn;
                        buttonHeld = true;
                    }
                    else
                    {
                        buttonHeld = false;
                    }

                    if (buttonHeld)
                    {
                        mousePosition = camera.GetNearestGrid32(camera.GetMapCords(), 32);
                    }

                    if (currentBuildingBtn != null && mstate.RightButton == ButtonState.Pressed)
                    {
                        currentBuildingBtn = null;
                    }

                    if (demoModeActive)
                    {
                        currentBuildingBtn = null;

                        if (unitController.CollisionPositions.Contains(mousePosition) && mstate.LeftButton == ButtonState.Pressed)
                        {
                            unitController.RemoveStructure(mousePosition);
                        }
                    }

                    buildingBtn.Update(gameTime, buildingBtn.Text);

                    if (!uiCollision && currentBuildingBtn != null && mstate.LeftButton == ButtonState.Pressed)
                    {
                        if (unitController.Structures.Count == 0 && !demoModeActive)
                        {
                            unitController.AddStructure(mousePosition, currentBuildingBtn.Text);
                            history.AddItem(currentBuildingBtn.Text, mousePosition);
                        }
                        else
                        {
                            bool collision = false;
                            if (unitController.CollisionPositions.Contains(mousePosition) || unitController.FlooringPositions.Contains(mousePosition))
                            {
                                collision = true;
                            }

                            if (collision == false && !demoModeActive)
                            {
                                unitController.AddStructure(mousePosition, currentBuildingBtn.Text);
                                history.AddItem(currentBuildingBtn.Text, mousePosition);
                            }
                        }            
                    }
                }
            }
            else
            {
                camera.SelectionSwitch = true;
                currentBuildingBtn = null;
            }

            foreach (var aiUnit in unitController.AiUnits)
            {
                if (aiUnit.Selected)
                {
                    aiUnit.UnitSelectedBtn = new Button(aiUnit.Position, content.Load<Texture2D>("selected32"), content.Load<SpriteFont>("Ariel"));
                    aiUnit.UnitSelectedBtn.Text = aiUnit.Position.ToString();
                    aiUnit.UnitSelectedBtn.Update(gameTime, aiUnit.UnitSelectedBtn.Text);
                }
                else
                {
                    aiUnit.UnitSelectedBtn = null;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Camera camera)
        {
            if (selectedPlayerBtn != null)
            {
                selectedPlayerBtn.Draw(spriteBatch);
            }

            if (selectedUnitsBtn != null)
            {
                selectedUnitsBtn.Draw(spriteBatch);
            }

            if (peopleModeBtn != null)
            {
                peopleModeBtn.Draw(spriteBatch);
            }

            if (buildingModeBtn != null)
            {
                buildingModeBtn.Draw(spriteBatch);
            }

            if (pointer != null)
            {
                pointer.DrawToPosition(spriteBatch,camera);
            }

            if (undoBtn != null)
            {
                undoBtn.Draw(spriteBatch);
            }

            if (redoBtn != null)
            {
                redoBtn.Draw(spriteBatch);
            }

            if (saveBtn != null)
            {
                saveBtn.Draw(spriteBatch);
            }

            if (loadBtn != null)
            {
                loadBtn.Draw(spriteBatch);               
            }

            if (peopleModeActive)
            {
                foreach (var peopleBtn in peopleBtns)
                {
                    peopleBtn.Draw(spriteBatch);
                }
            }

            if (buildingModeActive)
            {
                if (demoBtn != null)
                {
                    demoBtn.Draw(spriteBatch);
                }

                foreach (var buildingBtn in buildingsBtns)
                {
                    buildingBtn.Draw(spriteBatch);
                }
            }

            foreach (var aiUnit in unitController.AiUnits)
            {
                if (aiUnit.Selected)
                {
                    aiUnit.UnitSelectedBtn.DrawToPosition(spriteBatch,camera);
                }
                else
                {
                    aiUnit.UnitSelectedBtn = null;
                }
            }
        }
    }
}
