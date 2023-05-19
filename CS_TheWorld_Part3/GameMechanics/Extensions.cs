
using CS_TheWorld_Part3.GameMath;
using CS_TheWorld_Part3.Creatures;
using CS_TheWorld_Part3.Items;
using CS_TheWorld_Part3.Areas;

namespace CS_TheWorld_Part3.GameMechanics;
using static TextFormatter;

/// <summary>
/// Extension Methods!  Oh Geez this is high level stuff.
/// These are methods that are /Extending/ the given classes.
/// They are appear as behaviors attached to the class...
/// however, they are actually external methods.
/// These methods cannot access Private data inside a class.
/// In trade-off, it is safe to combine external method calls
/// with these class.
///
/// In these examples, we are using the external library "TextFormatter"
/// to access only the public interface of these classes.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds the method "LookAt()" to the Item class.
    /// use like this:
    /// item.LookAt();
    /// </summary>
    /// <param name="item">The Item you're looking at.</param>
    public static void LookAt(this Item item)
    {
        WriteLineNeutral($"{item.Name} : {item.Description}");
    }
    
    /// <summary>
    /// Look at a Creature in much the same way that you look at an item.
    /// </summary>
    /// <param name="creature"></param>
    public static void LookAt(this Creature creature)
    {
        WriteLineNeutral($"{creature.Name} : {creature.Description}");
    }
    
    /// <summary>
    /// Look around a given area
    /// </summary>
    /// <param name="area"></param>
    public static void LookAround(this Area area)
    {
        {
            WriteLineNeutral($"{area.Description}{Environment.NewLine}You see:");
        
            if (area.Items.Any())
            {
                foreach (UniqueName name in area.Items.Keys)
                {
                    WriteNeutral("\tA [");
                    WriteSurprise($"{name}");
                    WriteLineNeutral("]");
                }
            }

            if (area.Creatures.Any())
            {
                foreach (UniqueName name in area.Creatures.Keys)
                {
                    WriteNeutral("\tA [");
                    WriteSurprise($"{name}");
                    WriteLineNeutral("]");
                }
            }

            if (area.Neighbors.Any())
            {
                foreach (Direction dir in area.Neighbors.Keys)
                {
                    WriteNeutral($"{dir.DisplayPhrase} [");
                    WriteSurprise($"{dir.DirectionName}");
                    WriteLineNeutral("]");
                }
            }
        }
    }
    public static void FindItems(this Player player)
    {
        {
            WriteLineNeutral($"Your inventory includes:");
        
            if (player.Items.Any())
            {
                foreach (UniqueName name in player.Items.Keys)
                {
                    WriteNeutral("\tA [");
                    WriteSurprise($"{name}");
                    WriteLineNeutral("]");
                }
            }
        }
    }
}