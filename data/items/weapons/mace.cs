namespace ItemModule.All;

public class Mace : Club{
    public Mace() : base("Mace"){
        Attributes.Add(new ItemAttribute("Damage", 10));
        BuyPrice = 150;
    }
}