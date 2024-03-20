namespace Domain;

public class Item
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Categories? Type { get; set; }

    public enum Categories
    {
        Comidas, Bebidas
    }

    public Item(string name, string description, Categories type)
    {
        Name = name;
        Description = description;
        Type = type;
    }

    public Item()
    {
        
    }
}

public record SimplifiedItem
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public Categories? Type { get; init; }

    public enum Categories
    {
        Comidas, Bebidas
    }
}

