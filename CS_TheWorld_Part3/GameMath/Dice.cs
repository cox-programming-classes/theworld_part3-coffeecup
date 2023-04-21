using System.Diagnostics;

namespace CS_TheWorld_Part3.GameMath;

public readonly struct Dice
{
    public static Dice None = new(count: 0, sides: 0);
    
    public static Dice D4 = new(sides: 4);
    public static Dice D6 = new(sides: 6);
    public static Dice D8 = new(sides: 8);
    public static Dice D10 = new(sides: 10);
    public static Dice D12 = new(sides: 12);
    public static Dice D20 = new(sides: 20);
    public static Dice D100 = new(sides: 100);
    
    
    /// <summary>
    /// the Random Number Generator!
    /// </summary>
    private static readonly Random rng = new();

    /// <summary>
    /// how many dice do you roll.
    /// </summary>
    public uint Count { get; init; }
    
    /// <summary>
    /// A Dice can have any number of sides, but it must be set at initialization
    /// and, if no SideCount is specified, the default value of 6 is used.
    /// </summary>
    public uint SideCount { get; init; }

    /// <summary>
    /// The Modifier may be changed after initialization, but only from within the Dice class,
    /// and it has a default value of 0.
    /// </summary>
    public int Modifier { get; init; } = 0;

    /// <summary>
    /// Create a new Dice with the given values.
    /// All parameters have default values.
    /// </summary>
    /// <param name="count">default 1</param>
    /// <param name="sides">default 6</param>
    /// <param name="mod">default 0</param>
    public Dice(uint count = 1, uint sides = 6, int mod = 0)
    {
        Count = count;
        SideCount = sides;
        Modifier = mod;
    }

    public Dice(string diceString)
    {
        // TODO:  Implement the string constructor for Dice that converts "2d4+5" into a Dice. [Moderate]
        // TODO:  Improve the constructor using Regular Expressions to validate the diceString [Difficult]
        throw new NotImplementedException("You Gotta Write this!");
    }
    
    /// <summary>
    /// Roll this dice!
    /// </summary>
    /// <returns>returns a random number based on the number of dice with added modifier</returns>
    public int Roll()
    {

        int roll = 0;
        // roll as many times as you have.
        for(int i = 0; i < Count; i++)
            roll += rng.Next((int) SideCount) + 1; // add 1 because dice don't have a 0 side.
        
        roll += Modifier; // add the modifier (might be negative!)
        
        Debug.WriteLine($"Rolling {this}:  {roll}");
        return roll;
    }

    /// <summary>
    /// Return a D&D style Dice name scheme
    /// something like "4d10+5"
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var str = $"{Count}d{SideCount}";
        if (Modifier == 0)
            return str;
        
        if (Modifier < 0)
            return $"{str}{Modifier}";

        return $"{str}+{Modifier}";
    }
}