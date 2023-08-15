using System.Text;
using RLNET;
using RogueSharp;
using RogueSharp.DiceNotation;
using CSharp_Roguelike.Core;
using CSharp_Roguelike.Equipment;
using CSharp_Roguelike.Interfaces;
using CSharp_Roguelike.Items;

namespace CSharp_Roguelike.Systems
{
    public class CommandSystem
    {
        Player player = Game.Player;

        public bool IsPlayerTurn { get; set; }

        // Return value is true if the player was able to move
        // false when the player couldn't move, such as trying to move into a wall
        public bool MovePlayer(Direction direction)
        {
            int x = player.X;
            int y = player.Y;

            switch (direction)
            {
                case Direction.Up:
                    {
                        x = player.X;
                        y = player.Y - 1;
                        break;
                    }
                case Direction.Down:
                    {
                        x = player.X;
                        y = player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = player.X - 1;
                        y = player.Y;
                        break;
                    }
                case Direction.Right:
                    {
                        x = player.X + 1;
                        y = player.Y;
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            if (Game.DungeonMap.SetActorPosition(Game.Player, x, y))
            {
                return true;
            }

            Monster monster = Game.DungeonMap.GetMonsterAt(x, y);

            if (monster != null)
            {
                Attack(player, monster);
                return true;
            }

            return false;
        }
        //These keys refer to player activated abilities.
        public bool HandleKey(RLKey key)
        {
            if (key == RLKey.Q)
            {
                return player.QAbility.Perform();
            }
            if (key == RLKey.W)
            {
                return player.WAbility.Perform();
            }
            if (key == RLKey.E)
            {
                return player.EAbility.Perform();
            }
            if (key == RLKey.R)
            {
                return player.RAbility.Perform();
            }

            //These keys refer to player activated items.
            bool didUseItem = false;
            if (key == RLKey.Number1)
            {
                didUseItem = player.Item1.Use();
            }
            else if (key == RLKey.Number2)
            {
                didUseItem = player.Item2.Use();
            }
            else if (key == RLKey.Number3)
            {
                didUseItem = player.Item3.Use();
            }
            else if (key == RLKey.Number4)
            {
                didUseItem = player.Item4.Use();
            }

            if (didUseItem)
            {
                RemoveItemsWithNoRemainingUses();
            }
            return didUseItem;
        }

        private static void RemoveItemsWithNoRemainingUses()
        {
            if (Game.Player.Item1.RemainingUses <= 0)
            {
                Game.Player.Item1 = new NoItem();
            }
            if (Game.Player.Item2.RemainingUses <= 0)
            {
                Game.Player.Item2 = new NoItem();
            }
            if (Game.Player.Item3.RemainingUses <= 0)
            {
                Game.Player.Item3 = new NoItem();
            }
            if (Game.Player.Item4.RemainingUses <= 0)
            {
                Game.Player.Item4 = new NoItem();
            }
        }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
            Game.Player.Tick();
        }

        public void ActivateMonsters()
        {
            IScheduleable scheduleable = Game.SchedulingSystem.Get();
            if (scheduleable is Player)
            {
                IsPlayerTurn = true;
                Game.SchedulingSystem.Add(Game.Player);
            }
            else
            {
                Monster monster = scheduleable as Monster;

                if (monster != null)
                {
                    monster.PerformAction(this);
                    Game.SchedulingSystem.Add(monster);
                }

                ActivateMonsters();
            }
        }

        public void MoveMonster(Monster monster, Cell cell)
        {
            if (!Game.DungeonMap.SetActorPosition(monster, cell.X, cell.Y))
            {
                if (Game.Player.X == cell.X && Game.Player.Y == cell.Y)
                {
                    Attack(monster, Game.Player);
                }
            }
        }

        public void Attack(Actor attacker, Actor defender)
        {
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            int hits = ResolveAttack(attacker, defender, attackMessage);

            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            Game.MessageLog.Add(attackMessage.ToString());
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                Game.MessageLog.Add(defenseMessage.ToString());
            }

            int damage = hits - blocks;

            ResolveDamage(defender, damage);
        }

        // The attacker rolls based on his stats to see if he gets any hits
        private static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;

            attackMessage.AppendFormat("{0} attacks {1} and rolls: ", attacker.Name, defender.Name);

            // Roll a number of 100-sided dice equal to the Attack value of the attacking actor
            DiceExpression attackDice = new DiceExpression().Dice(attacker.Attack, 100);
            DiceResult attackResult = attackDice.Roll();

            // Look at the face value of each single die that was rolled
            foreach (TermResult termResult in attackResult.Results)
            {
                attackMessage.Append(termResult.Value + ", ");
                // Compare the value to 100 minus the attack chance and add a hit if it's greater
                if (termResult.Value >= 100 - attacker.AttackChance)
                {
                    hits++;
                }
            }

            return hits;
        }

        // The defender rolls based on his stats to see if he blocks any of the hits from the attacker
        private static int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;

            if (hits > 0)
            {
                attackMessage.AppendFormat("scoring {0} hits.", hits);
                defenseMessage.AppendFormat("  {0} defends and rolls: ", defender.Name);

                // Roll a number of 100-sided dice equal to the Defense value of the defendering actor
                DiceExpression defenseDice = new DiceExpression().Dice(defender.Defense, 100);
                DiceResult defenseRoll = defenseDice.Roll();

                // Look at the face value of each single die that was rolled
                foreach (TermResult termResult in defenseRoll.Results)
                {
                    defenseMessage.Append(termResult.Value + ", ");
                    // Compare the value to 100 minus the defense chance and add a block if it's greater
                    if (termResult.Value >= 100 - defender.DefenseChance)
                    {
                        blocks++;
                    }
                }
                defenseMessage.AppendFormat("resulting in {0} blocks.", blocks);
            }
            else
            {
                attackMessage.Append("and misses completely.");
            }

            return blocks;
        }

        // Apply any damage that wasn't blocked to the defender
        private static void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                defender.Health = defender.Health - damage;

                Game.MessageLog.Add(string.Format("  {0} was hit for {1} damage", defender.Name, damage));

                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                Game.MessageLog.Add(string.Format("{0} blocked all damage", defender.Name));
            }
        }

        // Remove the defender from the map and add some messages upon death.
        private static void ResolveDeath(Actor defender)
        {
            if (defender is Player)
            {
                Game.MessageLog.Add(string.Format("  {0} was killed, GAME OVER MAN!", defender.Name));
            }
            else if (defender is Monster)
            {
                
                if ( defender.Head != null && defender.Head != HeadEquipment.None() )
                {
                    Game.DungeonMap.AddTreasure( defender.X, defender.Y, defender.Head );
                }
                if ( defender.Body != null && defender.Body != BodyEquipment.None() )
                {
                    Game.DungeonMap.AddTreasure(defender.X, defender.Y, defender.Body);
                }
                if ( defender.Hand != null && defender.Hand != HandEquipment.None() )
                {
                    Game.DungeonMap.AddTreasure(defender.X, defender.Y, defender.Hand);
                }
                if ( defender.Feet != null && defender.Feet != FeetEquipment.None() )
                {
                    Game.DungeonMap.AddTreasure(defender.X, defender.Y, defender.Feet);
                }
                
                Game.DungeonMap.AddGold(defender.X, defender.Y, defender.Gold);
                Game.DungeonMap.RemoveMonster((Monster)defender);

                Game.MessageLog.Add(string.Format("  {0} died and dropped {1} gold", defender.Name, defender.Gold));
            }
        }
    }
}
