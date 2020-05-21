using Game3.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game3.Models
{
    class Node : IHeapItem<Node>
    {
        private int heapIndex;

        public int fCost { get { return gCost + hCost; } set { } }
        public int gCost { get; set; } = 0;
        public int hCost { get; set; } = 0;
        public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }
        public bool Walkable { get; set; }
        public Node Parent { get; set; }
        public Vector2 Position { get; set; }
        
        public Node(bool walkable, Vector2 position)
        {
            Walkable = walkable;
            Position = position;
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }
}
