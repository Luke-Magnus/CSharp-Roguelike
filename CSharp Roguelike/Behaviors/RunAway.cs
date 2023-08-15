﻿using RogueSharp;
using CSharp_Roguelike;
using CSharp_Roguelike.Interfaces;
using CSharp_Roguelike.Core;
using CSharp_Roguelike.Systems;

namespace CSharp_Roguelike.Behaviors
{
    public class RunAway : IBehavior
    {
        public bool Act(Monster monster, CommandSystem commandSystem)
        {
            Player player = Game.Player;
            DungeonMap dungeonMap = Game.DungeonMap;

            GoalMap goalMap = new GoalMap(dungeonMap);
            goalMap.AddGoal( player.X, player.Y, 0 );

            dungeonMap.SetIsWalkable(monster.X, monster.Y, true);
            dungeonMap.SetIsWalkable(player.X, player.Y, true);

            Path path = null;
            try
            {
                path = goalMap.FindPathAvoidingGoals(monster.X, monster.Y);
            }

            catch (PathNotFoundException)
            {
                Game.MessageLog.Add(string.Format("{0} cowers in fear", monster.Name));
            }

            dungeonMap.SetIsWalkable(monster.X, monster.Y, false);
            dungeonMap.SetIsWalkable(player.X, player.Y, false);

            if (path != null)
            {
                try
                {
                    commandSystem.MoveMonster(monster, path.StepForward());
                }

                catch (NoMoreStepsException)
                {
                    Game.MessageLog.Add(string.Format("{0} cowers in fear", monster.Name));
                }
            }
        return true;
        }
      }
   }
