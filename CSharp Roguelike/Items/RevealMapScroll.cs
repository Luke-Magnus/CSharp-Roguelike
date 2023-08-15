using RogueSharp;
using CSharp_Roguelike.Core;

namespace CSharp_Roguelike.Items
{
   public class RevealMapScroll : Item
   {
      public RevealMapScroll()
      {
         Name = "Magic Map";
         RemainingUses = 1;
      }

      protected override bool UseItem()
      {
         DungeonMap map = Game.DungeonMap;

         Game.MessageLog.Add( string.Format("{0} reads a {1} and gains knowledge of the surrounding area", Game.Player.Name, Name));

         foreach ( Cell cell in map.GetAllCells() )
         {
            if ( cell.IsWalkable )
            {
               map.SetCellProperties( cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true );
            }
         }
         
         RemainingUses--;

         return true;
      }
   }
}