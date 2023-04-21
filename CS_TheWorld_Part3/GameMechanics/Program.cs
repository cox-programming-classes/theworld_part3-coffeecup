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
    
    private static Area InitializeTheWorld()
    {
        // Create a new Area using the init methods for each property.
        var start = new Area()
        {
            Name = "Kansas (generally)",
            Description = "A barren plane with an ambient temperature around 22C and moderate humidity."
        };
        var tundra = new Area()
        {
            Name = "The Tundra",
            Description = "Cold, barren Wasteland with vodka and polar bears."
        };
        var florida = new Area()
        {
            Name = "Florida (yee haw!)",
            Description = "The dwelling of the specific subspecies of homo sapiens known as the Florida Man"
        };
        var desert = new Area()
        {
            Name = "The Wild, Wild West",
            Description = "The land of cowboys, cacti, and conflict"
        };
        var salem = new Area()
        {
            Name = "Salem",
            Description = "Like the rest of New England but with more ghosts and spook"
        };
        var california = new Area()
        {
            Name = "California",
            Description = "The waves are big; the egos are bigger"
        };
        
        var greenland = new Area()
        {
            Name = "Greenland",
            Description = "A cold place where you can eat green eggs and ham"
        };
        
        var texas = new Area()
        {
            Name = "Texas",
            Description = "Even the dumbass senator doesn't want to be there, but there are a few nice people like a few"
        };
        
        var alaska = new Area()
        {
            Name = "Alaska",
            Description = "Life below 0"
        };
        
        start.AddNeighboringArea(new Direction("NORTH", DisplayPhrase: $"{tundra.Name}"), tundra);
        start.AddNeighboringArea(new Direction("EAST", DisplayPhrase: $"{salem.Name}"), salem);
        start.AddNeighboringArea(new Direction("SOUTH", DisplayPhrase: $"{texas.Name}"), texas);
        start.AddNeighboringArea(new Direction("WEST", DisplayPhrase: $"{california.Name}"), california);

        tundra.AddNeighboringArea(new Direction("EAST", DisplayPhrase: $"{greenland.Name}"), greenland);
        tundra.AddNeighboringArea(new Direction("SOUTH", DisplayPhrase: $"{start.Name}"), start);
        tundra.AddNeighboringArea(new Direction("WEST", DisplayPhrase: $"{alaska.Name}"), alaska);

        florida.AddNeighboringArea(new Direction("NORTH", DisplayPhrase: $"{salem.Name}"), salem);
        florida.AddNeighboringArea(new Direction("WEST", DisplayPhrase: $"{texas.Name}"), texas);
        
        desert.AddNeighboringArea(new Direction("NORTH", DisplayPhrase: $"{california.Name}"), california);
        desert.AddNeighboringArea(new Direction("EAST", DisplayPhrase: $"{texas.Name}"), texas);
        
        salem.AddNeighboringArea(new Direction("NORTH", DisplayPhrase: $"{greenland.Name}"), greenland);
        salem.AddNeighboringArea(new Direction("SOUTH", DisplayPhrase: $"{florida.Name}"), florida);
        salem.AddNeighboringArea(new Direction("WEST", DisplayPhrase: $"{start.Name}"), start);

        california.AddNeighboringArea(new Direction("NORTH", DisplayPhrase: $"{alaska.Name}"), alaska);
        california.AddNeighboringArea(new Direction("EAST", DisplayPhrase: $"{start.Name}"), start);
        california.AddNeighboringArea(new Direction("SOUTH", DisplayPhrase: $"{desert.Name}"), desert);
        
        greenland.AddNeighboringArea(new Direction("SOUTH", DisplayPhrase: $"{salem.Name}"), salem);
        greenland.AddNeighboringArea(new Direction("WEST", DisplayPhrase: $"{tundra.Name}"), tundra);
        
        texas.AddNeighboringArea(new Direction("NORTH", DisplayPhrase:$"{start.Name}"), start);
        texas.AddNeighboringArea(new Direction("EAST", DisplayPhrase:$"{florida.Name}"), florida);
        texas.AddNeighboringArea(new Direction("WEST", DisplayPhrase:$"{desert.Name}"), desert);
        
        alaska.AddNeighboringArea(new Direction("EAST", DisplayPhrase: $"{tundra.Name}"), tundra);
        alaska.AddNeighboringArea(new Direction("SOUTH", DisplayPhrase: $"{california.Name}"), california);

        // Add an item directly into the area.
        // by creating the item directly inside this statement,
        // you can't add more information to the item.
        // Also note that the DataType of "uniqueName" is a
        // UniqueName.  But we are passing a string here!
        // this is the implicit operator at work!
        start.AddItem("rock", 
            new Item()
            {
                Name = "Rock", 
                Description = "It appears to be sandstone and is worn smooth by the wind"
            });
        desert.AddItem("tumbleweed", 
            new Item()
            {
                Name = "Tumbleweed", 
                Description = "A rolling ball of dead plant stuff"
            });
        // create a creature!
        var moth = new Creature()
        {
            Name = "Giant Moth",
            Description = "Holy shit that things huge!", 
            Stats = new StatChart(12, 8, Dice.D20, new(1, 6, -1))
        };
        // var Wizard = new Creature()
        // {
        //     Name= "Wizard",
        //     Description= "",
        //     Stats=new StatChart();
        // };
        // Here we can assign a lambda expression
        // to be the OnDeath action when the moth is killed
        moth.Stats.OnDeath += (sender, args) =>
        {
            _player.Stats.GainExp(moth.Stats.Exp); //give the moth's xp to the player
            WriteLineSurprise($"{moth.Name} falls" +
                              $" in a flutter of wings and ichor.");
        };
        // Add the Moth to the area.
        start.AddCreature("moth", moth);
        
        var armadillo = new Creature()
        {
            Name = "Evil Armadillo",
            Description = "It's rolling around in a ball of death and destruction", 
            Stats = new StatChart(18, 8, Dice.D20, new(1, 8, -1))
        };
        armadillo.Stats.OnDeath += (sender, args) =>
        {
            _player.Stats.GainExp(armadillo.Stats.Exp); //give the armadillo's xp to the player
            WriteLineSurprise($"{armadillo.Name} rolls away" +
                              $" looking pretty dead.");
        };
        // Add the armadillo to the area.
        desert.AddCreature("armadillo", armadillo);
        
        var ghost = new Creature()
        {
            Name = "Ghost",
            Description = "The bloody ghost of a Victorian child in a nightdress", 
            Stats = new StatChart(10, 8, Dice.D20, new(2, 5, 0))
        };
        ghost.Stats.OnDeath += (sender, args) =>
        {
            _player.Stats.GainExp(ghost.Stats.Exp); //give the ghost's xp to the player
            WriteLineSurprise($"{ghost.Name} dissapates in a howling storm of death the sequel");
        };
        // Add the ghost to the area.
        salem.AddCreature("ghost", ghost);
        
        return start;
    }

}