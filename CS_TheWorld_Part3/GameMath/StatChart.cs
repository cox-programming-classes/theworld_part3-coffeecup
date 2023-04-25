
namespace CS_TheWorld_Part3.GameMath;

public class StatChart
{
    /// <summary>
    /// This Event occurs when the HP on this stat chart drops below zero
    /// </summary>
    public event EventHandler Death;

    public event EventHandler<int> HPChanged;
    
    /// <summary>
    /// This Event occurs when your Exp reaches the next level (calculated in the GainExp function)
    /// </summary>
    public event EventHandler LevelUp;
    
    /// <summary>
    /// The player's level.
    /// "protected set" means that this property cannot be modified outside of this class.
    /// </summary>
    public int Level { get; protected set; }
    
    /// <summary>
    /// How Much Experience has this player accumulated
    /// </summary>
    public uint Exp { get; protected set; }
    
    /// <summary>
    /// The Player's current HP
    /// </summary>
    public int HP { get; protected set; }
    
    /// <summary>
    /// The Player's Maximum HP
    /// </summary>
    public uint MaxHP { get; protected set; }

    /// <summary>
    /// Calculated Property using shorthand.
    /// This property is readonly because it has no "set" method.
    /// </summary>
    public double PercentHP =>  HP / (double)MaxHP;

    public uint AC { get; protected set; } 
    
    /// <summary>
    /// A dice instance will be given to this player.
    /// </summary>
    public Dice AttackDice { get; protected set; }

    public Dice HitDice { get; protected set; } = new(1, 20, 0);

    public StatChart(uint maxHp, uint ac, Dice hitDice, Dice attackDice)
    {
        HP = (int)maxHp;
        MaxHP = maxHp;
        AC = ac;
        HitDice = hitDice;
        AttackDice = attackDice;
        Exp = 1;
    }
    
    /// <summary>
    /// Add or Subtract HP from this.
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHP(int amount)
    {
        HPChanged?.Invoke(this, amount);
        HP += amount;
        if(HP > MaxHP)
            HP = (int)MaxHP;

        if (HP < 0)
        {
            var overkill = HP;
            HP = 0;
            Death?.Invoke(this, new OnDeathEventArgs() { Overkill = overkill});
        }
    }

    /// <summary>
    /// What happens when you Gain Experience, this also covers the logic for leveling up.
    /// </summary>
    /// <param name="amount"></param>
    public void GainExp(uint amount)
    {
        var prev = Exp;
        Exp += amount;
        // Right now, leveling is calculated on a Logarithmic curve.  Each level needs e^n more Exp to achieve.
        // TODO:  Improve the algorithm for leveling up to be less ... awful [Moderate]
        var logPrev = Math.Floor(Math.Log(1+(prev / 10.0)));
        var logNew = Math.Floor(Math.Log(1+(Exp / 10.0)));
        var levelChange = (int) (logNew - logPrev);
        if (levelChange >= 1)
        {
            Level += levelChange;
            for (int i = 0; i < levelChange; i++)
            {
                MaxHP += (uint)(Level + (Dice.D20.Roll() / 5));
            }

            HP = (int)MaxHP;
            // This is how we raise an Event that is defined.
            // This calls all places where the LevelUp event has been attached to this instance.
            LevelUp?.Invoke(this, EventArgs.Empty);
        }
    }

    public override string ToString()
    {
        string stats = {$"HP: {HP}, AC: {AC}, HitDice: {HitDice}, AttackDice: {AttackDice}, Exp: {Exp}$"};
    }
}

/// <summary>
/// EventArgs are passed out of an Event to the place that is handling this event.
/// </summary>
public class OnDeathEventArgs : EventArgs { public int Overkill { get; init; } }
