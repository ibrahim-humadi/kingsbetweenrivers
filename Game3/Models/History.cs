using Game3.Components;
using Game3.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Models
{
    class History
    {
        private Game game;
        private List<string> redoHistory;
        private UnitController unitController;

        public List<string> ItemHistory { get; set; }
        

        public History(Game game)
        {
            this.ItemHistory = new List<string>();
            this.redoHistory = new List<string>();

            this.game = game;
            this.unitController = (UnitController)game.Services.GetService(typeof(IUnitController));           
        }

        private void AddRedoItem (string type, Vector2 position)
        {
            redoHistory.Add(type + " " + position.X + " " + position.Y);
        }

        public void AddItem(string type, Vector2 position)
        {
            ItemHistory.Add(type + " " + position.X + " " + position.Y);
            redoHistory.Clear();
        }

        public void RemoveItem(string type, Vector2 position)
        {
            ItemHistory.Remove(type + " " + position.X + " " + position.Y);
        }

        public void Redo()
        {
            if (redoHistory.Count > 0)
            {
                string[] currentRedoElements = redoHistory.Last().Split(' ');
                string currentRedoType = currentRedoElements[0];
                Vector2 currentRedoPos = new Vector2(Convert.ToInt32(currentRedoElements[1]), Convert.ToInt32(currentRedoElements[2]));

                unitController.AddStructure(currentRedoPos, currentRedoType);
                ItemHistory.Add(redoHistory.Last());
                redoHistory.Remove(redoHistory.Last());
            }          
        }

        public void Undo()
        {
            if (ItemHistory.Count() > 0)
            {
                string[] currentItemElements = ItemHistory.Last().Split(' ');
                string currentItemType = currentItemElements[0];
                Vector2 currentItemPos = new Vector2(Convert.ToInt32(currentItemElements[1]), Convert.ToInt32(currentItemElements[2]));

                unitController.RemoveStructure(currentItemPos);
                RemoveItem(currentItemType, currentItemPos);
                AddRedoItem(currentItemType, currentItemPos);
            }
        }

        public void Load(string filename)
        {
            unitController.Structures.Clear();
            unitController.CollisionPositions.Clear();
            unitController.FlooringPositions.Clear();
            string output = File.ReadAllText("file.txt");
            char[] chars = new[] { '\r', '\n' };
            string[] entries = output.Split(chars, StringSplitOptions.RemoveEmptyEntries);

            foreach (var entry in entries)
            {
                string[] entryInfo = entry.Split(' ');
                unitController.AddStructure(new Vector2(Convert.ToInt32(entryInfo[1]), Convert.ToInt32(entryInfo[2])), entryInfo[0]);
            }
        }

        public void Save(string filename)
        {
            File.Delete("file.txt");

            List<string> items = new List<string>();

            foreach (var structure in unitController.Structures.ToList())
            {
                items.Add(structure.Role + " " + structure.Position.X + " " + structure.Position.Y);
                File.AppendAllText("file.txt", items.Last() + Environment.NewLine);
            }
        }
    }
}
