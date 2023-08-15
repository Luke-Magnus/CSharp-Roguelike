using RogueSharp;
using RogueSharp.DiceNotation;
using CSharp_Roguelike.Core;
using CSharp_Roguelike.Behaviors;
using CSharp_Roguelike.Interfaces;
using CSharp_Roguelike.Systems;

namespace CSharp_Roguelike.Monsters
{
    class Ooze : Monster
   {
      public static Ooze Create(int level)

      {
         int health = Dice.Roll("4D5");
         return new Ooze {
            Attack = Dice.Roll( "1D2" ) + level / 3,
            AttackChance = Dice.Roll( "10D5" ),
            Awareness = 10,
            Color = Colors.MonsterColor,
            Defense = Dice.Roll( "1D2" ) + level / 3,
            DefenseChance = Dice.Roll( "10D4" ),
            Gold = Dice.Roll( "1D20" ),
            Health = health,
            MaxHealth = health,
            Name = "Ooze",
            Speed = 14,
            Symbol = 'o',
         };
      }

      public override void PerformAction(CommandSystem commandSystem)
      {
         var splitOozeBehavior = new Split();
         if ( !splitOozeBehavior.Act( this, commandSystem ) )
         {

                base.PerformAction(commandSystem);

        }
      }
   }
}
