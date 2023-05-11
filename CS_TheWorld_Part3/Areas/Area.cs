using System.Collections.ObjectModel;
using CS_TheWorld_Part3.Creatures;
using CS_TheWorld_Part3.GameMath;
using CS_TheWorld_Part3.Items;

namespace CS_TheWorld_Part3.Areas;

public class Area
{

    /// <summary>
    /// Something that happens when the player Enters the area...
    /// the Function takes the Player as a parameter
    /// and returns TRUE if this should interrupt the go command and
    /// prevent the player from entering this area.
    ///
    /// TODO:  Reaserch!  What are these data types Func<...> and Action<...>? [Moderate]
    /// TODO:  Identify other areas in the game where a Func<...> and Action<...> might be useful. [Varying Difficulty]
    /// </summary>
    public Func<Player, bool>? OnEntryAction { get; init; } = (player) => false;
    // function pointer: gives a name to a chunk of code that defines it as a variable(?)
    // points to executionable code within memory

    /// <summary>
    /// Something that happens when the player Leaves the area...
    /// the Function takes the Player as a parameter
    /// and returns TRUE if this should interrupt the go command and
    /// prevent the player from leaving this area.
    /// </summary>
    public Func<Player, bool>? OnExitAction { get; init; } = (player) => false;
    
    /// <summary>
    /// Name of an Area.
    /// </summary>
    public string Name { get; init; }
    /// <summary>
    /// Description that prints when you look around an area.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// All the Items that are in an Area.
    /// You must interface with this via the "AddItem" or "GetItem" method
    /// </summary>
    private readonly Dictionary<UniqueName, Item> _items = new();
    /// <summary>
    /// Creatures in an area
    /// </summary>
    private readonly Dictionary<UniqueName, Creature> _creatures = new();
    /// <summary>
    /// Neighbors of this area.
    /// </summary>
    private readonly Dictionary<Direction, Area> _neighbors = new();

    public ReadOnlyDictionary<UniqueName, Item> Items => new (_items);
    public void AddItem(UniqueName uniqueName, Item item)
    {
        if (_items.ContainsKey(uniqueName))
            throw new WorldException<Area>(this, $"{uniqueName} already exists in this area's _Items");
        // if you already have the item
        _items.Add(uniqueName, item);
    }

    public void RemoveItem(UniqueName uniqueName, Item item)
    {
        if (!_items.ContainsKey(uniqueName))
            throw new WorldException<Area>(this, $"You do not have {uniqueName} in your inventory.");
        _items.Remove(uniqueName);
    }// if the unique name of the item matches the input 
    public bool HasItem(UniqueName uniqueName) => _items.ContainsKey(uniqueName);
    
    public Item? GetItem(UniqueName uniqueName)
    {
        if (!this.HasItem(uniqueName))
            return null;

        return _items[uniqueName];
    }

    /// <summary>
    /// TODO:  Write a RemoveItem method that removes an item from the area so that it is not duplicated when picked up [Easy]
    /// </summary>

    public ReadOnlyDictionary<UniqueName, Creature> Creatures => new(_creatures);
    public void AddCreature(UniqueName uniqueName, Creature creature)
    {
        if (_creatures.ContainsKey(uniqueName))
            throw new WorldException<Area>(this, $"{uniqueName} already exists in this area's _Items");
        
        _creatures.Add(uniqueName, creature);
    }

    public bool HasCreature(UniqueName uniqueName) => _creatures.ContainsKey(uniqueName);
    
    public Creature? GetCreature(UniqueName uniqueName)
    {
        if (!this.HasCreature(uniqueName))
            return null;

        return _creatures[uniqueName];
    }

    public void DeleteCreature(UniqueName uniqueName)
    {
        if (this.HasCreature(uniqueName))
            _creatures.Remove(uniqueName);
    }
    
    public ReadOnlyDictionary<Direction, Area> Neighbors => new(_neighbors);

    public void AddNeighboringArea(Direction direction, Area area)
    {
        if (_neighbors.Keys.Any(dir => dir.DirectionName == direction.DirectionName))
            throw new WorldException<Area>(this, $"{direction.DirectionName} already exists in this area's _Items");
        
        _neighbors.Add(direction, area);
    }

    public void RemoveNeighboringArea(Direction direction)
    {
        if (!_neighbors.Keys.Any(dir => dir.DirectionName == direction.DirectionName))
            throw new WorldException<Area>(this, $"You do not have {direction.DirectionName} as a neighbor.");
        _neighbors.Remove(direction);
    }

    public bool HasNeighbor(UniqueName uniqueName) => _neighbors.Keys.Any(dir => dir.DirectionName == uniqueName);
    
    public Area? GetNeighboringArea(UniqueName uniqueName)
    {
        if (_neighbors.Keys.All(dir => dir.DirectionName != uniqueName))
            return null;
        return _neighbors.First(kvp => kvp.Key.DirectionName == uniqueName).Value;
    }

    
    // TODO:  Write a RemoveNeighbor method.  This may be deceptive--think about how and why you might need this! [Moderate]
}
