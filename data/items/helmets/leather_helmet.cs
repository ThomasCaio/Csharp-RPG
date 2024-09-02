namespace ItemModule.All;

public class LeatherHelmet : Helmet {
    public LeatherHelmet() : base("Leather Helmet") {
        Attributes.Add(new ItemAttribute("Armor", 1));
        BuyPrice = 10;
    }
}