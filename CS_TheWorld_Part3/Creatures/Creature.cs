using System.Collections.ObjectModel;
using CS_TheWorld_Part3.GameMath;
using CS_TheWorld_Part3.Items;

namespace CS_TheWorld_Part3.Creatures;

public class Creature : ICreature
{
    public string Name { get; init; }
    
    public string Description { get; init; }
    
    /// <summary>
    /// Note that in a Creature, the StatChart.Exp is how much experience the Player gets from 
    /// </summary>
    public StatChart Stats { get; init; }

    

    public ReadOnlyDictionary<EquipSlot, IEquipable> Equipment { get; init; } = new(new Dictionary<EquipSlot, IEquipable>());
    public Backpack Items { get; init; } = new(5, 20);

    /// <summary>
    /// Different Creatures can have Different Combat mechanics.
    /// TODO:  Add new Combat Logic for different creatures! [Moderate]
    /// </summary>
    public Action<ICreature, Command> CombatLogic { get; init; }
}