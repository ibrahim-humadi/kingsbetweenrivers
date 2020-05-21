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
    class UnitController : Component, IUnitController 
    {
        private ContentManager content;
        private Camera camera;
        private Game game;
        private Grid grid;
        private List<WayPoint> activeWaypoints;
        private MouseState mousePreviousState;
        private MouseState mouseNewState;

        public HashSet<Vector2> CollisionPositions { get; set; }
        public HashSet<Vector2> FlooringPositions { get; set; }
        public HashSet<Vector2> AiUnitPositions { get; set; }
        public List<AiUnit> AiUnits { get; set; }
        public List<Job> Jobs { get; set; }
        public List<Dwelling> Dwellings { get; set; }
        public List<Structure> Structures { get; set; }
        public Player Player { get; set; }

        public UnitController(Game game, ContentManager content, Camera camera) : base(content)
        {
            this.game = game;
            this.content = content;
            this.camera = camera;
            this.AiUnits = new List<AiUnit>();
            this.Jobs = new List<Job>();
            this.Dwellings = new List<Dwelling>();
            this.activeWaypoints = new List<WayPoint>();
            this.Structures = new List<Structure>();
            this.CollisionPositions = new HashSet<Vector2>();
            this.FlooringPositions = new HashSet<Vector2>();
            this.AiUnitPositions = new HashSet<Vector2>();
        }

        public void AddStructure(Vector2 position, string type)
        {
            if (type == "wall")
            {
                Structures.Add(new Structure(position, content.Load<Texture2D>("wall"), "wall"));
                CollisionPositions.Add(position);
            }

            if (type == "wall2")
            {
                Structures.Add(new Structure(position, content.Load<Texture2D>("wall2"), "wall2"));
                CollisionPositions.Add(position);
            }

            if (type == "wall3")
            {
                Structures.Add(new Structure(position, content.Load<Texture2D>("wall3"), "wall3"));
                CollisionPositions.Add(position);
            }

            if (type == "sandstonebasewall")
            {
                Structures.Add(new Structure(position, content.Load<Texture2D>("sandstonebasewall"), "sandstonebasewall"));
                CollisionPositions.Add(position);
            }

            if (type == "door")
            {
                Structures.Add(new Structure(position, content.Load<Texture2D>("sandstonedoorway"), "door"));
            }

            if (type == "flooring")
            {
                Structures.Add(new Structure(position, content.Load<Texture2D>("sandstonefloor"), "flooring"));
                FlooringPositions.Add(position);
            }
        }

        public void AddPerson(Vector2 position, string type)
        {
            if (type == "worker")
            {
                AiUnits.Add(new AiUnit(position, content.Load<Texture2D>("worker"),"worker"));
                AiUnitPositions.Add(position);
            }
        }

        public void RemoveStructure(Vector2 position)
        {
            Structure structure = Structures.FirstOrDefault(x => x.Position == position);
            Structures.Remove(structure);
            if (CollisionPositions.Contains(position))
            {
                CollisionPositions.Remove(position);
            }

            if (FlooringPositions.Contains(position))
            {
                FlooringPositions.Remove(position);
            }
        }

        public void GiveOrders(GameTime gameTime)
        {
             mousePreviousState = Mouse.GetState();
             mouseNewState = mousePreviousState;

            var mstate = Mouse.GetState();

            foreach (var aiUnit in AiUnits)
            {
                bool collision;

                if (CollisionPositions.Contains(camera.GetNearestGrid32(camera.GetMapCords(), 32)) || aiUnit.Position == camera.GetNearestGrid32(camera.GetMapCords(), 32))
                {
                    collision = true;
                }
                else
                {
                    collision = false;
                }

                if (aiUnit.Selected && mstate.RightButton == ButtonState.Pressed)
                {                   
                    if (aiUnit.CurrentWaypoint == null || aiUnit.CurrentWaypoint.PathActive == false)
                    {
                        
                        if (collision == false)
                        {
                            aiUnit.IsMoving = true;
                            aiUnit.Destination = camera.GetNearestGrid32(camera.GetMapCords(), 32);
                            aiUnit.CurrentWaypoint = new WayPoint(grid, game, aiUnit);
                        }
                    }
                }

                if (aiUnit.IsMoving)
                {
                    if (!collision && aiUnit.Selected && mstate.RightButton == ButtonState.Pressed && !aiUnit.Interrupt)
                    { 
                        aiUnit.Interrupt = true;
                    }

                    if (aiUnit.Interrupt && mstate.RightButton == ButtonState.Released)
                    {                      
                        aiUnit.Position = MoveToThisPos(aiUnit.Position, aiUnit.CurrentWaypoint.CurrentPos, gameTime, aiUnit.Speed);

                        if (Vector2.Distance(aiUnit.Position, aiUnit.CurrentWaypoint.CurrentPos) < 1)
                        {
                            aiUnit.Position = aiUnit.CurrentWaypoint.CurrentPos;
                            aiUnit.IsMoving = true;
                            aiUnit.Destination = camera.GetNearestGrid32(camera.GetMapCords(), 32);
                            aiUnit.CurrentWaypoint = new WayPoint(grid, game, aiUnit);
                            aiUnit.Interrupt = false;
                        }
                    }
                    else
                    {
                        aiUnit.Position = MoveToThisPos(aiUnit.Position, aiUnit.CurrentWaypoint.CurrentPos, gameTime, aiUnit.Speed);

                        if (Vector2.Distance(aiUnit.Position, aiUnit.CurrentWaypoint.CurrentPos) < 1)
                        {
                            aiUnit.Position = aiUnit.CurrentWaypoint.CurrentPos;
                            if (aiUnit.CurrentWaypoint.X < aiUnit.CurrentWaypoint.Paths.Count() - 1)
                            {
                                aiUnit.CurrentWaypoint.X += 1;
                            }
                            aiUnit.CurrentWaypoint.CurrentPos = aiUnit.CurrentWaypoint.Paths[aiUnit.CurrentWaypoint.X];
                        }

                        if (Vector2.Distance(aiUnit.Position, aiUnit.CurrentWaypoint.Paths.Last()) < 1)
                        {
                            aiUnit.Position = aiUnit.CurrentWaypoint.Paths.Last();
                            aiUnit.IsMoving = false;
                            aiUnit.CurrentWaypoint.PathActive = false;
                        }
                    }
                }
            }
        }

        public Vector2 MoveToThisPos(Vector2 starting_pos, Vector2 target_pos, GameTime gameTime, float unitspeed)
        {

            Vector2 direction = Vector2.Normalize(target_pos - starting_pos);
            float speed = 100;

            Vector2 result = starting_pos += direction * (speed + unitspeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            return result;
        }


        public override void LoadContent()
        {
            grid = new Grid(new Vector2(0, 0), content.Load<Texture2D>("grid"));
        }

        public override void Update(GameTime gameTime)
        {
            GiveOrders(gameTime);

            foreach (var waypoint in activeWaypoints)
            {
                if (waypoint.PathActive)
                {
                }
                else
                {
                    activeWaypoints.Remove(waypoint);
                }
            }

            foreach (var aiUnit in AiUnits)
            {
                aiUnit.Update(gameTime);
            }

            foreach (var job in Jobs)
            {
                job.Update(gameTime);
            }

            foreach (var dwelling in Dwellings)
            {
                dwelling.Update(gameTime);
            }

            foreach (var structure in Structures)
            {
                structure.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Camera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.Transform);
            foreach (var structure in Structures)
            {
                structure.Draw(spriteBatch);
            }

            foreach (var aiUnit in AiUnits)
            {
                aiUnit.Draw(spriteBatch);
            }

            foreach (var job in Jobs)
            {
                job.Draw(spriteBatch);
            }

            foreach (var dwelling in Dwellings)
            {
                dwelling.Draw(spriteBatch);
            }          

            if (grid != null)
            {
                grid.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
