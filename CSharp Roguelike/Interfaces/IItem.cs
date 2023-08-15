namespace CSharp_Roguelike.Interfaces
{
    public interface IItem
    {
        string Name { get; }
        int RemainingUses { get; }

        bool Use();
    }
}