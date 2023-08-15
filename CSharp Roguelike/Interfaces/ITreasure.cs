using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Roguelike.Interfaces
{
    public interface ITreasure
    {
        bool PickUp(IActor actor);
    }
}