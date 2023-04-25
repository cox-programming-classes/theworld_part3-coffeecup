using CS_TheWorld_Part3.GameMath;

namespace CS_TheWorld_Part3.Items;

/// <summary>
/// Equipment Slots
/// </summary>
public enum EquipSlot //enum can only be the things listed below
{
    Head,
    Arms,
    Body,
    Legs,
    Feet,
    Hands,
    Accessory,
    MainHand,
    OffHand,
    TwoHand
}

public interface IEquipable
{
    public EquipSlot Slot { get; init; }
    public StatChart EquipBonuses { get; init; }
}