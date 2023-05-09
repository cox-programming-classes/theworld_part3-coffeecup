using CS_TheWorld_Part3.Creatures;
using CS_TheWorld_Part3.Items;
using CS_TheWorld_Part3.Areas;
namespace CS_TheWorld_Part3.GameMechanics;

using static TextFormatter;

/// <summary>
/// This is a specialized Item that doesn't have any extra characteristics yet....
/// TODO:  Research!  How is this used in the current example context?  What is it about this item that is useful [Moderate]
/// </summary>
public class KeyStone : Item, ICarryable, IUsable
{
    public string Element { get; init; }
    public int Weight { get; init; }

    /// <summary>
    /// Becareful what you use this on!
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

    /// <summary>
    /// Becareful what you use this on!
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public string UseOn(object target)
    {
        if (target is Player player)
        {
            player.Stats.ChangeHP(5);
            return $"{player.Name} is protected by {Element}";
        }

        return $"{this} has no effect on {target}";
    }
}

public class MagicWand : Item, ICarryable, IUsable
{
    public (string, Creature) Monster { get; init; }
    public int Weight { get; init; }
    
    public string Cast(object target)
    {
        if (target is Area area)
        {
            area.AddCreature(Monster.Item1, Monster.Item2);
            return $"There is now a {Monster.Item2.Name} in {target}";
        }

        return $"{this} has no effect on {target}";
    }
}

// TODO:  Create a specialized item that can be USED to Heal the player [Moderate]

public static class StandardItems
{
    /// <summary>
    /// A reusable instance of a KeyStone 
    /// </summary>
    public static KeyStone FireStone => new()
    {
        Name = "Fire Stone",
        Description = "A stone that glows bright orange and is warm to the touch.",
        Weight = 1
    };
    
    public static SafeItem LifeJacket => new()
    {
        Name = "Life Jacket",
        Description = "A magical jacket that can pull you out of any trap.",
        Weight = 3
    };

    public static MagicWand SlothWand => new()
    {
        Name = "Sloth Wand",
        Description = "A wand that can put a scary sloth in any area",
        Monster = ("sloth", StandardCreatures.Sloth),
        Weight = 2
    };

    // TODO:  Create more cookie-cutter items that you can initialize at will
}