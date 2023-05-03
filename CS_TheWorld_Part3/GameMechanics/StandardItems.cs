using CS_TheWorld_Part3.Creatures;
using CS_TheWorld_Part3.Items;
namespace CS_TheWorld_Part3.GameMechanics;

using static TextFormatter;

/// <summary>
/// This is a specialized Item that doesn't have any extra characteristics yet....
/// TODO:  Research!  How is this used in the current example context?  What is it about this item that is useful [Moderate]
/// </summary>
public class KeyStone : Item, ICarryable, IUsable // these are the things it inherits from
// an item you need to have to access the portal
{
    // implements ICarryable
    // ex: if item is ICarryable, return true
    public string Element { get; init; }
    public int Weight { get; init; }

    /// <summary>
    /// Be careful what you use this on!
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public string UseOn(object target)
    {
        if (target is ICreature creature)
        {
            creature.Stats.ChangeHP(-3);
            return $"{creature.Name} is bathed in the light of {Element}";
        }

        return $"{this} has no effect on {target}";
    }
}

public class SafeItem : Item, ICarryable, IUsable
{
    public string Element { get; init; }
    public int Weight { get; init; }

    public string UseOn(object target)
    {
        if (target is Player _player)
        {
            _player.Stats.ChangeHP(20);
            return $"{_player.Name} took a potion and is healed. Your HP is {_player.Stats.HP}.";
        }

        return $"{this} has no effect.";
    }
}
// TODO:  Create a specialized item that can be USED to Heal the player [Moderate]

public static class StandardItems
{
    /// <summary>
    /// A reusable instance of a KeyStone 
    /// </summary>
    // definition of a keystone
    public static KeyStone FireStone => new()
    {
        Name = "Fire Stone",
        Description = "A stone that glows bright orange and is warm to the touch.",
        Weight = 1
    };

    public static SafeItem LifeJacket => new()
    {
        Name = "Life Jacket",
        Description = "A magical life jacket that can pull you out of any trap.",
        Weight = 3
    };
    public static SafeItem Potion => new()
    {
        Name = "Health Potion",
        Description = "This potion will save your life. Literally.",
        Weight = 2
    };

    // TODO:  Create more cookie-cutter items that you can initialize at will
}
