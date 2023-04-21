using System.Text.RegularExpressions;

namespace CS_TheWorld_Part3.GameMath;


/// <summary>
/// A command string. take a list of words typed by the user.
/// </summary>
/// <param name="Words">the words of the command</param>
public record Command(string[] Words)
{
    public string CommandWord => Words.Any() ? Words[0] : string.Empty;
    public string Target => Words.Length > 1 ? Words[1] : string.Empty;
    public string SecondaryTarget => Words.Length > 2 ? Words[2] : string.Empty;

    /// <summary>
    /// Implicitly convert an array of strings into a Command.
    /// </summary>
    /// <param name="words"></param>
    /// <returns></returns>
    public static implicit operator Command(string[] words) => new(words);
    
    /// <summary>
    /// Implicitly convert a string into a Command by
    /// splitting on SPACE characters.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static implicit operator Command(string command) => new(command.Split(' '));
}

/// <summary>
/// A Direction is a UniqueName used for Going to a different location.
/// </summary>
/// <param name="DirectionName">the UniqueName for this DirectionName</param>
/// <param name="DisplayPhrase">
/// What is displayed in the game console
/// when this DirectionName is Printed.
/// </param>
public readonly record struct Direction(UniqueName DirectionName, string DisplayPhrase)
{
    public override string ToString() => $"[{DirectionName}] {DisplayPhrase}";
}


/// <summary>
/// Note the use of a STRUCT rather than a CLASS here
/// if you change the word STRUCT to CLASS and attempt to run the
/// program, you'll see why!
/// </summary>
public readonly struct UniqueName
{
    /// <summary>
    /// Regular Expressions are used to validate text data.
    /// this particular one means that it must be 2 or more lowercase letters.
    /// </summary>
    private static Regex _validationExpression = new("^[a-z_-]{2,}$", RegexOptions.Compiled);
    private static bool Validate(string value) => _validationExpression.IsMatch(value);
    
    private string Value { get; }
    
    private UniqueName(string v)
    {
        Value = v.ToLowerInvariant();
        if (!Validate(Value))
            throw new WorldException<UniqueName>(this, $"[{Value}] is not a valid unique identifier.");
    }

    /// <summary>
    /// Implicitly convert a string to a UniqueName
    /// </summary>
    /// <param name="value">the string you are using.</param>
    /// <returns>A UniqueName or throws an exception</returns>
    public static implicit operator UniqueName(string value) => new(value);

    public override string ToString() => Value;

    public static bool operator ==(UniqueName a, UniqueName b) => a.Value == b.Value;

    public static bool operator !=(UniqueName a, UniqueName b) => !(a == b);

    public bool Equals(UniqueName other) => Value == other.Value;
    public bool Equals(string other) => Value == other;
}

/// <summary>
/// This is an exception thrown within The World.
/// It is thrown when an object within the World has an error.
/// </summary>
/// <typeparam name="T">This is the Type of the object that has an error</typeparam>
public class WorldException<T> : Exception
{
    public T ObjectError { get; }
    
    public WorldException(T objectError, string message, Exception? innerException = null) : base(message, innerException)
    {
        ObjectError = objectError;
    }
}