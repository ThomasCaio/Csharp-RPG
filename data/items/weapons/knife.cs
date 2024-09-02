namespace ItemModule.All;

public class Knife : Sword{
    public Knife() : base("Knife"){
        Attributes.Add(new ItemAttribute("Damage", 4));
        BuyPrice = 10;
    }
}