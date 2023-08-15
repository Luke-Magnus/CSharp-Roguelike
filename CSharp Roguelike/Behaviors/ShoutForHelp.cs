using RogueSharp;
using CSharp_Roguelike;
using CSharp_Roguelike.Interfaces;
using CSharp_Roguelike.Core;
using CSharp_Roguelike.Systems;

namespace CSharp_Roguelike.Behaviors
{
    public class ShoutForHelp : IBehavior
    {
    public bool Act(Monster monster, CommandSystem commandSystem)
    {
        bool didShoutForHelp = false;
        DungeonMap dungeonMap = Game.DungeonMap;
        FieldOfView monsterFov = new FieldOfView(dungeonMap);
        monsterFov.ComputeFov( monster.X, monster.Y, monster.Awareness, true );

        foreach ( var monsterLocation in dungeonMap._monsters )
        {
            if ( monsterFov.IsInFov( monsterLocation.X, monsterLocation.Y ) )
            {
                Monster alertedMonster = dungeonMap.GetMonsterAt(monsterLocation.X, monsterLocation.Y);
                if ( !alertedMonster.TurnsAlerted.HasValue )
                {
                    alertedMonster.TurnsAlerted = 1;
                    didShoutForHelp = true;
                }
            }
        }

            if ( didShoutForHelp )
            {
                Game.MessageLog.Add( string.Format( "{0} shouts for help!", monster.Name ) );
            }
            return didShoutForHelp;
       }
    }
}
