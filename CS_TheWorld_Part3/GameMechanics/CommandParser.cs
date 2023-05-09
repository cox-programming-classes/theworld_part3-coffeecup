
using System.ComponentModel.Design;
using System.Runtime;
using CS_TheWorld_Part3.GameMath;
using CS_TheWorld_Part3.Items;

namespace CS_TheWorld_Part3.GameMechanics;
using static TextFormatter;

/// <summary>
/// Program is now a PARTIAL class, so it can be spread across
/// multiple files.  Different logical components of the Program
/// are distributed so that they're easier to understand, but
/// comprise the larger whole.
/// </summary>
public static partial class Program
{
    /// <summary>
    /// Upgraded Command Words!  Now this is a mapping from the command word to the Action that happens!
    /// </summary>
    private static Dictionary<UniqueName, Action<Command>> _commandWords = new()
    {
        {"look", ProcessLookCommand },
        {"get", ProcessGetCommand },
        {"drop", ProcessDropCommand },
        {"fight", ProcessFightCommand },
        {"cheat", command => _player.Stats.GainExp(50) }, 
        {"go", ProcessGoCommand },
        {"backpack", ProcessBackpackCommand }
    };

    // TODO:  Add a `stats` command that displays the Players current Stats. [Easy]
    // TODO:  Add a `help` command that displays the list of allowed commands and describes how to use them [Easy]
    // TODO:  Expand the `help` command to take a second parameter like `help look` that describes 
    //        all the possible ways to use the `look` command. [Easy, Multipart]
    // TODO:  Add a `backpack` command that lists the contents of your players Inventory. [Easy]
    // TODO:  Implement the `get` command to pick up an item and place it in the player inventory [Easy]
    // TODO:  Implement a `drop` command that removes an item from the players inventory and places it in the current Area [Easy]
    // TODO:  Add a `use` command that allows you to use any IUseableItem that exists in the current Area [Moderate]
    // TODO:  extend the `use` command to allow you to use any IUseableItem in the players Inventory [Moderate]
    // TODO:  Make sure that the `use`, `get`, and `drop` commands do not conflict with other items with the same uniqueName [Moderate]
    // TODO:  Make it possible for the player to have more than one of the same item (same uniqueName) in their inventory [Difficult]
    // TODO:  Extend the `use` command to allow the player to target themself by accepting the word `self` as the secondary target of a command [Difficult]
    
    
    /// <summary>
    /// Process the Command string typed by the player.
    /// </summary>
    /// <param name="command"></param>
    private static void ProcessCommandString(Command command)
    {
        if (string.IsNullOrWhiteSpace(command.CommandWord))
            return;
        
        if (!_commandWords.ContainsKey(command.CommandWord))
        {
            WriteLineWarning("I don't know what that means.");
        }

        // TODO:  Research!  Oh good god what the hell is this? [Moderate]
        _commandWords[command.CommandWord](command); // if a valid command word, finds the command word from the dictionary, which the parameter is a command word
    }
    
    private static void ProcessGoCommand(Command command)
    {
        if (command.Target == "")
        {
            WriteLineWarning("Go Where?");
            return;
        }

        if (!_currentArea.HasNeighbor(command.Target))
        {
            WriteLineWarning($"I don't know where {command.Target} is.");
            return;
        }

        var place = _currentArea.GetNeighboringArea(command.Target)!;
        
        // Check the new Actions that Areas have.
        // Are there actions that happen when you leave an area or when you enter a new area?
        if (_currentArea.OnExitAction?.Invoke(_player) ?? false)
            return;  // you were denied exit from the current area.

        if (place.OnEntryAction?.Invoke(_player) ?? false)
            return;  // you were denied entry to this area.
        
        _currentArea = place;
    }

    private static void ProcessGetCommand(Command command)
    {
        if (_currentArea.HasItem(command.Target) && command.Target is ICarryable)
        {
            _currentArea.GetItem(command.Target);
            return;
        }
        
        if (!_currentArea.HasItem(command.Target) && command.Target is ICarryable)
        {
            WriteLineWarning("This item isn't here.");
            return;
        }
        
        else
        {
            WriteLineWarning("You can't pick that up.");
            return;
        }
    }
    private static void ProcessDropCommand(Command command)
    {
        
        throw new NotImplementedException("Gotta write that code (in CommandParser.cs)");
    }
    private static void ProcessFightCommand(Command command)
    {
        if (command.Target == "")
        {
            WriteLineWarning("Stop hitting yourself.");
            return;
        }

        if (_currentArea.HasItem(command.Target))
        {
            WriteLineWarning($"You can't fight [{command.Target}].");
            return;
        }
        
        if (!_currentArea.HasCreature(command.Target))
        {
            WriteLineWarning($"You don't see [{command.Target}].");
            return;
        }
        
        WriteLineWarning("Gotta write that code yet....");
        var target = _currentArea.GetCreature(command.Target)!;
        DoBattle(target);
    }

    private static void ProcessLookCommand(Command cmd)
    {
        // if the command is literally just "look"
        // look around the current area.
        // the LookAround() method is an Extension!
        if(cmd.Target == "")
            _currentArea.LookAround();
        else
        {
            if (_currentArea.HasItem(cmd.Target))
                _currentArea.GetItem(cmd.Target)!.LookAt(); 
            // the ! in this line means I'm certain that this item isn't null.
            if (_currentArea.HasCreature(cmd.Target))
                _currentArea.GetCreature(cmd.Target)!.LookAt();
        }
    }
    
    private static void ProcessBackpackCommand(Command cmd)
    {
        if(_player.Items.Count <= 0)
        {
            WriteLineNeutral("You have no items in your inventory.");
        }
        
        else
        {
            WriteLineNeutral("You have these items in your inventory:");
            foreach(var things in _player.Items)
                WriteNeutral($"[{things.Key}] ");
        }
    }
}