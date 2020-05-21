using Game3.Components;
using Game3.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Models
{
    class WayPoint
    {
        private UnitController unitController;
        private Game game;
        private AiUnit aiUnit;
        private Grid grid;

        public Vector2[] Paths { get; set; }
        public Vector2 CurrentPos { get; set; }
        public bool PathActive { get; set; } = true;
        public int X = 0;

        public WayPoint(Grid grid, Game game, AiUnit aiUnit)
        {
            this.grid = grid;
            this.game = game;
            this.aiUnit = aiUnit;
            unitController = (UnitController)game.Services.GetService(typeof(IUnitController));
            Paths = SetWayPoint(aiUnit.Position, aiUnit.Destination);
            CurrentPos = Paths.First();
        }

        public Vector2[] SetWayPoint(Vector2 startingPos, Vector2 destinationPos)
        {

            Heap<Node> OpenNodes = new Heap<Node>();
            HashSet<Node> ClosedNodes = new HashSet<Node>();
            List<Vector2> path = new List<Vector2>();

            Node startingNode = new Node(true, startingPos);

            OpenNodes.Add(startingNode);

            while (OpenNodes.Count != 0)
            {
                Node currentNode = OpenNodes.RemoveFirst();

                if (currentNode.Position == destinationPos)
                {
                    OpenNodes.ResetNodes();
                    break;
                }
                else
                {
                    ClosedNodes.Add(currentNode);
                    List<Node> childNodes = grid.GetNeighbours(currentNode).ToList();
                   
                    GetCollisionCoordinates(childNodes);

                    foreach (var node in childNodes)
                    {
                        if (ClosedNodes.FirstOrDefault(x => x.Position == node.Position) != null || node.Walkable == false)
                        {
                            continue;
                        }

                        int cost = currentNode.gCost + GetDistance(currentNode,node);

                        if (!OpenNodes.Contains(node) || cost < node.gCost)
                        {
                            node.Parent = currentNode;
                            node.gCost = cost;
                            node.hCost = (int)Vector2.Distance(node.Position, destinationPos);

                            if (!OpenNodes.Contains(node))
                            {
                                OpenNodes.Add(node);
                            }
                            else
                            {
                                OpenNodes.UpdateItem(node);
                            }
                        }
                    }                
                }
            }

            Node contextEndNode = ClosedNodes.First();
            Node contextCurrentNode = ClosedNodes.Last();

            path.Add(destinationPos);

            while (contextCurrentNode != contextEndNode)
            {
                path.Add(contextCurrentNode.Position);
                contextCurrentNode = contextCurrentNode.Parent;
            }
                       
            path.Reverse();
            return path.ToArray();
            }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = (int)Math.Abs(nodeA.Position.X - nodeB.Position.X);
            int dstY = (int)Math.Abs(nodeA.Position.Y - nodeB.Position.Y);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX = dstY);
            }
            return 14 * dstX + 10 * (dstY = dstX);
        }

        public void GetCollisionCoordinates(List<Node> nodes)
            {
                foreach (var node in nodes)
                {
                    if (unitController.CollisionPositions.Contains(node.Position))
                    {
                    node.Walkable = false;
                    }

                    if (unitController.FlooringPositions.Contains(node.Position))
                    {
                    node.Walkable = true;
                    }
                }
            }
        }
    }