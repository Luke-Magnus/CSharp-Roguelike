using RogueSharp;
using CSharp_Roguelike.Core;

namespace CSharp_Roguelike.Items
{
   public class TeleportScroll : Item
   {
      public TeleportScroll()
      {
         Name = "Teleport Scroll";
         RemainingUses = 1;
      }

      protected override bool UseItem()
      {
         DungeonMap map = Game.DungeonMap;
         Player player = Game.Player;

         Game.MessageLog.Add( string.Format("{0} uses a {0} and reappears in another place", player.Name, Name) );

         Point point = map.GetRandomLocation();
         
         map.SetActorPosition( player, point.X, point.Y );
         
         RemainingUses--;

         return true;
      }
   }
}
