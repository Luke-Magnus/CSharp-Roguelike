using RLNET;
using RogueSharp;
using CSharp_Roguelike.Interfaces;

namespace CSharp_Roguelike.Core
{
   public class Item : IItem, ITreasure, IDrawable
   {
      public Item()
      {
         Symbol = '!';
         Color = RLColor.Yellow;
      }

      public string Name { get; protected set; }
      public int RemainingUses { get; protected set; }

      public bool Use()
      {
         return UseItem();
      }

      protected virtual bool UseItem()
      {
         return false;
      }

      public bool PickUp( IActor actor )
      {
         Player player = actor as Player;

         if ( player != null )
         {
            if ( player.AddItem( this ) )
            {
               Game.MessageLog.Add( string.Format( "{0} picked up {1}", actor.Name, Name) );
               return true;
            }
         }

         return false;
      }

      public RLColor Color { get; set; }

      public char Symbol { get; set; }

      public int X { get; set; }

      public int Y { get; set; }

      public void Draw( RLConsole console, IMap map )
      {
         if ( !map.IsExplored( X, Y ) )
         {
            return;
         }

         if ( map.IsInFov( X, Y ) )
         {
            console.Set( X, Y, Color, Colors.FloorBackgroundFov, Symbol );
         }
         else
         {
            console.Set( X, Y, RLColor.Blend( Color, RLColor.Gray, 0.5f ), Colors.FloorBackground, Symbol );
         }
      }
   }
}