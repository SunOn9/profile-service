namespace ProfileService.Model.Entity;

public class SomethingEntity
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public int Age  { get; set; }

    public ProfileService.Something Into()
    {
        return new ProfileService.Something()
        {
            Id = Id,
            Name = Name,
            Age = Age,
        };
    }
}