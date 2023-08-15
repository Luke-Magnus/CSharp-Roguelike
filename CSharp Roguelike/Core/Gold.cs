﻿using RLNET;
using RogueSharp;
using CSharp_Roguelike;
using CSharp_Roguelike.Core;
using CSharp_Roguelike.Interfaces;
using CSharp_Roguelike.Systems;

namespace CSharp_Roguelike
{
    public class Gold : IDrawable, ITreasure
    {
        public int Amount { get; set; }

      public Gold( int amount )
      {
         Amount = amount;
         Symbol = '$';
         Color = RLColor.Yellow;
      }

      public bool PickUp( IActor actor )
      {
         actor.Gold += Amount;
         Game.MessageLog.Add( string.Format( "{0} picked up {1} gold",actor.Name, Amount) );
         return true;
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