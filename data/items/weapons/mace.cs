namespace ItemModule.All;

public class Mace : Sword{
    public Mace() : base("Mace"){
        Attributes.Add(new ItemAttribute("Damage", 10));
        BuyPrice = 150;
    }
}