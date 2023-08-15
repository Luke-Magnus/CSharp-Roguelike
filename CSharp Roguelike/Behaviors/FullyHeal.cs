using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp_Roguelike.Core;
using CSharp_Roguelike.Interfaces;
using CSharp_Roguelike.Systems;

namespace CSharp_Roguelike.Behaviors
{
    public class FullyHeal : IBehavior

    {

       public bool Act(Monster monster, CommandSystem commandSystem)

       {

          if ( monster.Health<monster.MaxHealth )

          {

             int healthToRecover = monster.MaxHealth - monster.Health;

             monster.Health = monster.MaxHealth;

             Game.MessageLog.Add( string.Format( "{0} catches his breath and recovers {1} health", monster.Name, healthToRecover ) );    

             return true;

          }
          return false;
       }
    }
}
