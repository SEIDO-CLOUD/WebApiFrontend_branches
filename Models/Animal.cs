namespace Models;

public class Animal:IAnimal
{
    public virtual Guid AnimalId { get; set; } = Guid.NewGuid();
    public AnimalKind Kind { get; set; }
    public AnimalMood Mood { get; set; }

    public int Age { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual IZoo Zoo { get; set; }
}