namespace ItemModule.All;

public class LongSword : Sword{
    public LongSword() : base("Long Sword"){
        Attributes.Add(new ItemAttribute("Damage", 8));
        BuyPrice = 80;
    }
}