namespace ItemModule.All;

public class LeatherArmor : Armor {
    public LeatherArmor() : base("Leather Armor") {
        Attributes.Add(new ItemAttribute("Armor", 2));
        BuyPrice = 25;
    }
}