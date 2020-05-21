using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3.Interfaces
{
    interface IHeapItem<T> : IComparable<T>
    {
         int HeapIndex { get; set; }
    }
}
