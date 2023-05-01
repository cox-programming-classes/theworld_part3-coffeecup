using CS_TheWorld_Part3.Creatures;
using CS_TheWorld_Part3.GameMath;
namespace CS_TheWorld_Part3.GameMechanics;
using static TextFormatter;

public static partial class Program
{
    // TODO:  Expand the list of battle command to include:
    // TODO:  `use` an item in battle [Easy] (after use item has been written)
    // TODO:  Special commands might be required to use Special abilities.  How should that be handled? [Moderate]
    private static List<UniqueName> _battleCommands = new() {"attack", "defend", "flee"};

    /// <summary>
    /// Player does battle with a single creature
    /// TODO:  Expand this to take more than one creature in battle!  [Difficult]
    /// TODO:  This is NOT "clean code"  FIXIT! [Moderate]
    /// </summary>
    /// <param name="creature"></param>
    public static void DoBattle(Creature creature)
    {
        WriteLineWarning($"You engage {creature.Name} in combat!");
        while (_player.Stats.HP > 0 && creature.Stats.HP > 0)
        {
            var command = (Command)GetPlayerInput("(battle) ");
            if (!_battleCommands.Contains(command.CommandWord))
            {
                WriteLineWarning($"{command.CommandWord} is not a valid command word.");
                continue;
            }
            WriteLineNeutral($"Player has {_player.Stats.HP} heath at start.");
            WriteLineNeutral($"{creature.Name} has {creature.Stats.HP} heath at start.");
            int playerDamage = _player.Stats.AttackDice.Roll();
            int creatureDamage = creature.Stats.AttackDice.Roll();
            WriteLineNeutral($"Player has rolled a {playerDamage}.");
            WriteLineNeutral($"{creature.Name} has rolled a {creatureDamage}.");
            if (command.CommandWord == "attack" && playerDamage > creatureDamage)
            {
                WriteLineSurprise("You attack!");
                creature.Stats.ChangeHP(-playerDamage);
                WriteLineNeutral($"Creature has {creature.Stats.HP} heath.");
            }

            if (command.CommandWord == "attack" && creatureDamage > playerDamage)
            {
                WriteLineNegative("You aren't strong enough. The creature fights back.");
                _player.Stats.ChangeHP(-creatureDamage);
                WriteLineNeutral($"Player has {_player.Stats.HP} heath.");
            }

            if (command.CommandWord == "attack" && creatureDamage == playerDamage)
            {
                WriteLineNeutral("You both miss. How??");
            }
            _player.CombatLogic(creature, command);
            if (creature.CombatLogic is null)
                ((ICreature) creature).CombatLogic(_player, "");
            else
                creature.CombatLogic(_player, command);

            if (command.CommandWord == "flee")
            {
                // TODO:  Maybe running away shouldn't be this easy.... [Moderate]
                break;
            }
            
                
        }
    }
}
