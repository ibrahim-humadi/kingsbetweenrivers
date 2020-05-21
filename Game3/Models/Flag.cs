using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Models
{
    class Flag
    {
        public bool IsActive { get; set; }
        public string FlagType { get; set; }

        public Flag(string flagType)
        {
            this.FlagType = flagType;
            this.IsActive = true;
        }
        public Flag(string flagType, bool isActive)
        {
            this.FlagType = flagType;
            this.IsActive = isActive;
        }
    }
}
