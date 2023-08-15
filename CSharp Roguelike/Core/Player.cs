using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using CSharp_Roguelike.Abilities;
using CSharp_Roguelike.Equipment;
using CSharp_Roguelike.Interfaces;
using CSharp_Roguelike.Items;

namespace CSharp_Roguelike.Core
{
    public class Player : Actor
    {
        public IAbility QAbility { get; set; }
        public IAbility WAbility { get; set; }
        public IAbility EAbility { get; set; }
        public IAbility RAbility { get; set; }
        public IItem Item1 { get; set; }
        public IItem Item2 { get; set; }
        public IItem Item3 { get; set; }
        public IItem Item4 { get; set; }

        public Player()
        {
            QAbility = new DoNothing();
            WAbility = new DoNothing();
            EAbility = new DoNothing();
            RAbility = new DoNothing();
            Item1 = new NoItem();
            Item2 = new NoItem();
            Item3 = new NoItem();
            Item4 = new NoItem();
        }

        public bool AddAbility(IAbility ability)
        {
            if (QAbility is DoNothing)
            {
                QAbility = ability;
            }
            else if (WAbility is DoNothing)
            {
                WAbility = ability;
            }
            else if (EAbility is DoNothing)
            {
                EAbility = ability;
            }
            else if (RAbility is DoNothing)
            {
                RAbility = ability;
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool AddItem(IItem item)
        {
            if (Item1 is NoItem)
            {
                Item1 = item;
            }
            else if (Item2 is NoItem)
            {
                Item2 = item;
            }
            else if (Item3 is NoItem)
            {
                Item3 = item;
            }
            else if (Item4 is NoItem)
            {
                Item4 = item;
            }
            else
            {
                return false;
            }

            return true;
        }
        
        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print( 1, 1, string.Format("Name:    {0}", Name), RLColor.White);
            statConsole.Print( 1, 3, string.Format( "Health:  {0}/{1}", Health, MaxHealth ), RLColor.White );
            statConsole.Print( 1, 5, string.Format( "Attack:  {0} ({1}%)", Attack, AttackChance ), RLColor.White );
            statConsole.Print( 1, 7, string.Format( "Defense: {0} ({1}%)", Defense, DefenseChance ), RLColor.White );
            statConsole.Print( 1, 9, string.Format( "Gold:    {0}", Gold ), RLColor.Yellow );
            
        }

        public void DrawInventory(RLConsole inventoryConsole)
        {
            inventoryConsole.Print(1, 1, "Equipment", RLColor.White);
            inventoryConsole.Print( 1, 3, string.Format( "Head: {0}", Head.Name), RLColor.LightGray );
            inventoryConsole.Print( 1, 5, string.Format( "Body: {0}", Body.Name), RLColor.LightGray );
            inventoryConsole.Print( 1, 7, string.Format( "Hand: {0}", Hand.Name), RLColor.LightGray );
            inventoryConsole.Print( 1, 9, string.Format( "Feet: {0}", Feet.Name), RLColor.LightGray );
            
            inventoryConsole.Print(28, 1, "Abilities", RLColor.White);
            inventoryConsole.Print(28, 3, string.Format( "Q - {0}", QAbility.Name), RLColor.LightGray);
            inventoryConsole.Print(28, 5, string.Format( "W - {0}", WAbility.Name), RLColor.LightGray);
            inventoryConsole.Print(28, 7, string.Format( "E - {0}", EAbility.Name), RLColor.LightGray);
            inventoryConsole.Print(28, 9, string.Format( "R - {0}", RAbility.Name), RLColor.LightGray);

            inventoryConsole.Print(55, 1, "Items", RLColor.White);
            inventoryConsole.Print(55, 3, string.Format( "1 - {0}", Item1.Name), RLColor.LightGray);
            inventoryConsole.Print(55, 5, string.Format( "2 - {0}", Item2.Name), RLColor.LightGray);
            inventoryConsole.Print(55, 7, string.Format( "3 - {0}", Item3.Name), RLColor.LightGray);
            inventoryConsole.Print(55, 9, string.Format( "4 - {0}", Item4.Name), RLColor.LightGray);

            DrawAbility(QAbility, inventoryConsole, 0);
            DrawAbility(WAbility, inventoryConsole, 1);
            DrawAbility(EAbility, inventoryConsole, 2);
            DrawAbility(RAbility, inventoryConsole, 3);
        }

        private void DrawAbility(IAbility ability, RLConsole inventoryConsole, int position)
        {
            char letter = 'Q';
            if ( position == 0 )
        {
            letter = 'Q';
        }

        else if ( position == 1 )
        {
            letter = 'W';
        }

        else if ( position == 2 )       
        {
            letter = 'E';
        }

        else if ( position == 3 )
        {
            letter = 'R';
        }

        RLColor highlightTextColor = RLColor.LightGray;

        if ( !( ability is DoNothing ) )
        {
            if ( ability.TurnsUntilRefreshed == 0 && !( ability is DoNothing ) )
            {
                highlightTextColor = Palette.PrimaryLightest;
            }

            else
            {
                highlightTextColor = Palette.SecondaryLightest;
            }
          } 
          int xPosition = 28;
          int xHighlightPosition = 28 + 4;
          int yPosition = 3 + (position * 2);
          inventoryConsole.Print(xPosition, yPosition, string.Format("{0} - {1}", letter, ability.Name), highlightTextColor);
 
          if ( ability.TurnsToRefresh > 0 )
          {
             int width = Convert.ToInt32(((double)ability.TurnsUntilRefreshed / (double)ability.TurnsToRefresh) * 16.0);
             int remainingWidth = 20 - width;
             inventoryConsole.SetBackColor( xHighlightPosition, yPosition, width, 1, Palette.AlternateDarkest );
             inventoryConsole.SetBackColor( xHighlightPosition + width, yPosition, remainingWidth, 1, RLColor.Black );
          }
       }

       public void Tick()
       {
          QAbility.Tick();
          WAbility.Tick();
          EAbility.Tick();
          RAbility.Tick();
        }
    }
}
