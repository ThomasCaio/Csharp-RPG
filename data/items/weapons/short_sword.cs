namespace ItemModule.All;

public class ShortSword : Sword{
    public ShortSword() : base("Short Sword"){
        Attributes.Add(new ItemAttribute("Damage", 6));
        BuyPrice = 25;
    }
}