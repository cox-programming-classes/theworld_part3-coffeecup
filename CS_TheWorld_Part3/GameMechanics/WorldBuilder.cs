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

        var possum = StandardCreatures.Marsupial;

        possum.Stats.Death += (sender, args) =>
        {
            OnCreatureDeath("possum", possum, $"Himalayan Soup Base is ready");
        };
        
        start.AddCreature("possum", possum);
        
        // Here we can assign a lambda expression
        // to be the PlayerDeath action when the moth is killed
        moth.Stats.Death += (sender, args) =>
        {
            OnCreatureDeath("moth", moth, 
                $"{moth.Name} falls in a flutter of wings and ichor.");
        };
        // Add the Moth to the area.
        start.AddCreature("moth", moth);
        
        
        var possum  = StandardCreatures.Marsupial;
        possum.Stats.Death += (sender, args) =>
        {
            OnCreatureDeath("possum", possum, 
            $"is it playing dead? or just dead? eat it and find out :D");
        };
        start.AddCreature("possum", possum);



        var armadillo = new Creature()
        {
            Name = "Evil Armadillo",
            Description = "It's rolling around in a ball of death and destruction", 
            Stats = new StatChart(18, 8, Dice.D20, new(1, 8, -1))
        };
        armadillo.Stats.Death += (sender, args) =>
        {
            _player.Stats.GainExp(armadillo.Stats.Exp); //give the armadillo's xp to the player
            WriteLineSurprise($"{armadillo.Name} rolls away" +
                              $" looking pretty dead.");
        };
        
        var tundra = new Area()
        {
            Name = "The Tundra",
            Description = "Cold, Barren Wasteland."
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


        salem.AddItem("brandy", 
            new Item()
            {
                Name = "Brandy", 
                Description = "it tastes like shit!"
            });

        // Add the armadillo to the area.
        desert.AddCreature("armadillo", armadillo);
        
        var ghost = new Creature()
        {
            Name = "Ghost",
            Description = "The bloody ghost of a Victorian child in a nightdress", 
            Stats = new StatChart(10, 8, Dice.D20, new(2, 5, 0))
        };
        ghost.Stats.Death += (sender, args) =>
        {
            _player.Stats.GainExp(ghost.Stats.Exp); //give the ghost's xp to the player
            WriteLineSurprise($"{ghost.Name} dissapates in a howling storm of death the sequel");
        };
        // Add the ghost to the area.
        salem.AddCreature("ghost", ghost);
        
        
        var poleindeer = new Creature()
        {
            Name = "Poleindeer",
            Description = "a hellish fusion of a half of a polar bear and a half of a reindeer. we don't frick around in the tundra.", 
            Stats = new StatChart(30, 12, Dice.D20, new(2, 5, 0))
        };
        poleindeer.Stats.Death += (sender, args) =>
        {
            _player.Stats.GainExp(poleindeer.Stats.Exp); //give the ghost's xp to the player
            WriteLineSurprise($"{poleindeer.Name} is split in half! the reindeer and polar bear halves are now freed and can live separate lives :)");
        };
        tundra.AddCreature("poleindeer", poleindeer);

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

        
        
        var planeOfFire = new Area()
        {
            Name = "Plane of Fire",
            Description = "That's a whoooooole lot of lava.....",
            OnEntryAction = (player) =>
            {
                // Check to see if the player HAS a KeyStone item.
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