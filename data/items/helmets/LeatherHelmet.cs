namespace ItemModule.All;

public class LeatherHelmet : Helmet {
    public LeatherHelmet() : base("Leather Helmet") {
        Attributes.Add(new ItemAttribute("Defense", 1));

        Price = 25;
    }
}