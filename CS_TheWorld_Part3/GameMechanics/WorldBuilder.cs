using CS_TheWorld_Part3.Areas;
using CS_TheWorld_Part3.Creatures;
using CS_TheWorld_Part3.Items;
using CS_TheWorld_Part3.GameMath;

namespace CS_TheWorld_Part3.GameMechanics;
using static TextFormatter;

public static partial class Program
{
    // TODO:  Non-Coding!  Create a storyboard layout planning your world
    // TODO:  Create a story to your world.  This can be written out at first, but should be incorporated into the game
    // TODO:  Add Lore to the game in the form of special items, game events that have a narrative, and creatures that can engage in dialog
    
    /// <summary>
    /// Build all the areas and link them together.
    /// TODO: Expand the world to include all sorts of new things [Varying difficulty]
    /// TODO: This is skirting the fringes of "Clean Code" Make it better! [Moderate]
    /// </summary>
    /// <returns></returns>
    private static Area InitializeTheWorld()
    {   
        // Create a new Area using the init methods for each property.
        var start = new Area()
        {
            Name = "This Place",
            Description = "A barren plane with an ambient temperature around 22C and moderate humidity."
        };

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
        // create a creature!
        var moth = new Creature()
        {
            Name = "Giant Moth",
            Description = "Holy shit that things huge!",
            Items= new(new Dictionary<UniqueName, ICarryable>()
            {
                {
                    "sword",
                    StandardEquipment.Sword
                }
            }),
            Stats = new StatChart(12, 8, Dice.D20, new(1, 6, -1))
        };
        // Here we can assign a lambda expression
        // to be the PlayerDeath action when the moth is killed
        moth.Stats.Death += (sender, args) =>
        {
            OnCreatureDeath("moth", moth, 
                $"{moth.Name} falls in a flutter of wings and ichor.");
        };
        // Add the Moth to the area.
        start.AddCreature("moth", moth);

        var tundra = new Area()
        {
            Name = "The Tundra",
            Description = "Cold, Barren Wasteland."
        };

        var salamander = new Creature()
        {
            Name="Salamander",
            Description = "A lizard looking critter that has a flickering flame down its spine.",
            Items = new(new Dictionary<UniqueName, ICarryable>()
            {   
                {
                    "firestone", 
                    StandardItems.FireStone
                }
            }),
            Stats = new(15, 12, Dice.D20, Dice.D6)
        };

        // TODO:  Research!  This command is long... wtf is going on here, and why is it written this way? [Moderate]
        salamander.Stats.Death += (sender, args) =>
            OnCreatureDeath("salamander", salamander,
                "The fire along the salamander's back flickers out.");
        
        start.AddCreature("salamander", salamander);

        start.AddNeighboringArea(new ("north", "Far to the North"), tundra);
        tundra.AddNeighboringArea(new Direction("south", "Far to the South"), start);

        var planeOfFire = new Area()
        {
            Name = "Plane of Fire",
            Description = "That's a whoooooole lot of lava.....",
            OnEntryAction = (player) =>
            // this is a function that takes the player as a parameter
            // goes from ProcessGoCommand to the area class
            // default behavior is false, so it runs the function in the area clas
            {
                // Check to see if the player HAS a KeyStone item.
                // Keystone implementation here
                if (!player.Items.Any(kvp => kvp.Value is KeyStone))
                {
                    // If Not, the player is denied entry and takes 1d4 damage!
                    WriteLineWarning("The heat from the portal drives you back.");
                    player.Stats.ChangeHP(-Dice.D4.Roll());
                    return true;
                }
                
                WriteLinePositive("The flames of the portal part for you");
                return false;
            }
        };

        // TODO:  This Mechanic of creating a creature then applying the death event is clunky [Extremely Difficult]
        //        Can you make it better?  
        var firebird = StandardCreatures.FireBird;
        firebird.Stats.Death += (sender, args) =>
            OnCreatureDeath("firebird", firebird, 
                "The flames wither and the husk falls to the ground.");
        
        planeOfFire.AddCreature("firebird", firebird);
        
        start.AddNeighboringArea(new("portal", "a Firey portal"), planeOfFire);
        
        // return the starting area.
        return start;
    }
    
    /// <summary>
    /// Encapsulate all the basic things that need to happen when a creature is killed.
    /// Use this in the creature.Stats.PlayerDeath event handler.
    /// </summary>
    /// <param name="creatureUid"></param>
    /// <param name="deadCritter"></param>
    /// <param name="deathMessage"></param>
    private static void OnCreatureDeath(UniqueName creatureUid, ICreature deadCritter, string deathMessage)
    {
        _player.Stats.GainExp(deadCritter.Stats.Exp);
        WriteLineSurprise(deathMessage);
        if (deadCritter.Items.Any())
        {
            WriteLineSurprise($"{deadCritter.Name} drops:");
            foreach (var name in deadCritter.Items.Keys)
            {
                WriteNeutral("\tA [");
                WriteSurprise($"{name}");
                WriteLineNeutral("]");
                // TODO:  There is potentially an error here!  Watchout! [Moderate]
                _currentArea.AddItem(name, (deadCritter.Items[name] as Item)!);
            }
        }
        _currentArea.DeleteCreature(creatureUid);
    }
}