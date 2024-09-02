namespace ItemModule.All;

public class LeatherLegs : Legs {
    public LeatherLegs() : base("Leather Legs") {
        Attributes.Add(new ItemAttribute("Armor", 1));
        BuyPrice = 15;
    }
}