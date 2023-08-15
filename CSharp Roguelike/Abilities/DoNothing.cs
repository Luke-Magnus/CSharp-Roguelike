using CSharp_Roguelike.Core;

namespace CSharp_Roguelike.Abilities
{
    public class DoNothing : Ability
    {
        public DoNothing()
        {
            Name = "None";
            TurnsToRefresh = 0;
            TurnsUntilRefreshed = 0;
        }

        protected override bool PerformAbility()
        {
            Game.MessageLog.Add( "No ability in that slot" );
            return false;
        }
    }
 }