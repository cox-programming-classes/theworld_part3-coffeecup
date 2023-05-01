using CS_TheWorld_Part3.Areas;
using CS_TheWorld_Part3.Creatures;
using CS_TheWorld_Part3.GameMath;

namespace CS_TheWorld_Part3.GameMechanics;

using static TextFormatter;

// TODO:  Save your Game!  (Create a save file so that your game state can be saved and re-loaded later!) [Very Difficult]
// TODO:  Save Game Step 1:  Serializing the game state -- every object in the computers memory related to this game need to be converted into data. [Difficult]
// TODO:  Save Game Step 2:  A file format must be chosen (created) in order to store all the game data. [Very Difficult]
// TODO:  Save Game Step 3:  `save` must be implemented as a command in the CommandParser and should initiate the serialization of the game [Moderate]
// TODO:  Save Game Step 4:  Once data is saved to a file, it does no good unless you can load it from that file later! [Very Difficult]
public static partial class Program
{
    #region Global Variables
    /// <summary>
    /// The Player playing the game.
    /// Initialized at the beginning of the Main method.
    /// </summary>
    private static Player _player = null!;
    
    /// <summary>
    /// The area the player is currently in.
    /// Initialized as the result returned by the
    /// InitializeTheWorld() method.
    /// </summary>
    private static Area _currentArea = null!;
    #endregion // global variables
    
    /// <summary>
    /// This is the Explicit start of the program.
    /// </summary>
    /// <param name="args">Not used</param>
    public static void Main(string[] args)
    {
        _currentArea = InitializeTheWorld();
        _player = new(GetPlayerInput("What is your name?"));
        // By "Adding" a method handler to each of these Events
        // we can define what happens for the player when each
        // of these things happens.
        _player.Stats.LevelUp += PlayerLevelUp;
        _player.Stats.Death += PlayerDeath;
        _player.Stats.HPChanged += PlayerHPChanged;
        
        WriteLinePositive($"Hello, {_player.Name}");
        string command = GetPlayerInput();
        while (command != "quit")
        {
            // TODO:  Implement a background thread that can interupt the game loop to add depth to the game.  [Varying Difficulty]
            
            ProcessCommandString(command);
            command = GetPlayerInput();
        }
        
        WriteLinePositive("BYE!");
    }
    
    private static void PlayerHPChanged(object? sender, int e)
    {
        if (e == 0)
            return;
        if (e < 0)
            WriteLineNegative($"You take {-e} damage!");
        else
            WriteLinePositive($"You gain {e} hit points");
    }

    private static void PlayerDeath(object? sender, EventArgs e)
    {
        if (e is OnDeathEventArgs deathArgs)
        {
            WriteNegative($"Oof that hurt:  Your HP hit {deathArgs.Overkill}");
        }
    }

    private static void PlayerLevelUp(object? sender, EventArgs e)
    {
        WriteLinePositive($"Congratz You're now Level {_player.Stats.Level}");
    }

}