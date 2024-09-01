namespace ItemModule.All;

public class LeatherBoots : Boots {
    public LeatherBoots() : base("Leather Boots") {
        Attributes.Add(new ItemAttribute("Armor", 1));
        Price = 10;
    }
}