using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;

namespace CSharp_Roguelike.Interfaces
{
    public interface ITargetable
    {
       void SelectTarget(Point target);
        
    }
}
