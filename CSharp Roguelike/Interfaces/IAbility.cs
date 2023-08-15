using System.Security.Cryptography.X509Certificates;

namespace CSharp_Roguelike.Interfaces
{
    public interface IAbility
    {
       string Name { get; }
       int TurnsToRefresh { get; }
       int TurnsUntilRefreshed { get; }
 
       bool Perform();
       void Tick();
    }

}