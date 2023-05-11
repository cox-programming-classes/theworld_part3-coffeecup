using CS_TheWorld_Part3.Areas;
using CS_TheWorld_Part3.Creatures;
using CS_TheWorld_Part3.Items;
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

public class TransportItem : Item, ICarryable, IUsable
{
    public Area TravellingTo { get; }
    public int Weight { get; init; }
    public string TravelTo(object target)
    {
        if (target == TravellingTo)
        {
            _player.currentArea = TravellingTo;
            return $"You are now in {TravellingTo.Name}";
        }

        return $"You can't use {this} on creatures";
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
    
    public static TransportItem Teleporting => new()
    {
        Name = "Teleport Machine",
        Description = "A stone that can allow you to travel to a place.",
        Weight = 1
    };
    // TODO:  Create more cookie-cutter items that you can initialize at will
}