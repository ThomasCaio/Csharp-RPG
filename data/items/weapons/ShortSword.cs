namespace ItemModule.All;

public class ShortSword : Sword{
    public ShortSword() : base("Short Sword"){
        Attributes.Add(new ItemAttribute("Damage", 5));
        Price = 10;
    }
}