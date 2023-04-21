namespace CS_TheWorld_Part3.Items;

public class Item
{
    public string Name { get; init; }
    public string Description { get; init; }

    public override string ToString() => Name;
}