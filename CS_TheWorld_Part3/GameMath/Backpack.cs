using System.Collections;
using CS_TheWorld_Part3.Items;
namespace CS_TheWorld_Part3.GameMath;

public class Backpack : IDictionary<UniqueName, ICarryable>
{
    public uint Slots { get; protected set; }
    public uint Capacity { get; protected set; }
    public Dictionary<UniqueName, ICarryable> Inventory { get; protected set; }

    public Backpack(uint slots, uint capacity)
    {
        Slots = slots;
        Capacity = capacity;
        Inventory = new Dictionary<UniqueName, ICarryable> { };
    }

    public int CurrentWeight
    {
        get
        {
            int result = 0;
            foreach (var thing in Inventory.Values)
                result += thing.Weight;
            return result;
        }
    }

    public string AddItem(UniqueName name, ICarryable item)
    {
        if (Inventory.Count < Slots && (CurrentWeight + item.Weight) <= Capacity)
        {
            Inventory.Add(name, item);
            return $"{item} has been added to your backpack.";
        }
        
        return $"{item} couldn't be added to your backpack.";
    }

    public IEnumerator<KeyValuePair<UniqueName, ICarryable>> GetEnumerator()
    {
        return Inventory.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Inventory).GetEnumerator();
    }

    public void Add(KeyValuePair<UniqueName, ICarryable> item)
    {
        Inventory.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        Inventory.Clear();
    }

    public bool Contains(KeyValuePair<UniqueName, ICarryable> item)
    {
        return Inventory.Contains(item);
    }

    public void CopyTo(KeyValuePair<UniqueName, ICarryable>[] array, int arrayIndex)
    {
        //Inventory.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<UniqueName, ICarryable> item)
    {
        return Inventory.Remove(item.Key);
    }

    public int Count => Inventory.Count;

    public bool IsReadOnly => false;

    public void Add(UniqueName key, ICarryable value)
    {
        Inventory.Add(key, value);
    }

    public bool ContainsKey(UniqueName key)
    {
        return Inventory.ContainsKey(key);
    }

    public bool Remove(UniqueName key)
    {
        return Inventory.Remove(key);
    }

    public bool TryGetValue(UniqueName key, out ICarryable value)
    {
        return Inventory.TryGetValue(key, out value);
    }

    public ICarryable this[UniqueName key]
    {
        get => Inventory[key];
        set => Inventory[key] = value;
    }

    public ICollection<UniqueName> Keys => Inventory.Keys;

    public ICollection<ICarryable> Values => Inventory.Values;
}
    