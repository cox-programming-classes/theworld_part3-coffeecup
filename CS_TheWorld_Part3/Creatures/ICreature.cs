using System.Collections.ObjectModel;
using CS_TheWorld_Part3.GameMath;
using CS_TheWorld_Part3.Items;

namespace CS_TheWorld_Part3.Creatures;

public interface ICreature
{
    public string Name { get; init; }
    public StatChart Stats { get; init; }

    /// <summary>
    /// Combine the ICreature's base Stats.AC with EquipmentAC
    /// this can be overriden in a class that implements this!
    /// </summary>
    public uint EffectiveAC
    {
        get
        {
            var result = Stats.AC;
            foreach (var equpiment in Equipment.Values)
            {
                result += equpiment.EquipBonuses.AC;
            }

            return result;
        }
    }
    
    // TODO:  Define an EffectiveAttackDice Property that selects the creatures Base attack dice if no weapon is equiped,
    //        OR Selects the equiped weapon(s) attack dice [Difficult]
    
    
    public ReadOnlyDictionary<EquipSlot, IEquipable> Equipment { get; }
    
    public ReadOnlyDictionary<UniqueName, ICarryable> Items { get; }

    /// <summary>
    /// Default behavior is just to do an unarmed attack against the target.
    /// TODO:  Override this behavior in the Player class to use the player's equpiment to attack [Difficult]
    /// TODO:  Add Special Ability Logic to let the player (or a creature) use Special Abilities.
    /// </summary>
    /// <param name="creature"></param>
    /// <param name="command"></param>
    public void CombatLogic(ICreature creature, Command command)
    {
        var hit = Stats.HitDice.Roll() > creature.EffectiveAC;
        if (hit)
        {
            var value = Stats.AttackDice.Roll();
            creature.Stats.ChangeHP(-value);
        }
    }
}