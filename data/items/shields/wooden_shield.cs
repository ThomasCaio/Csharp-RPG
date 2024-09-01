namespace ItemModule.All;

public class WoodenShield : Shield {
    public WoodenShield() : base("Wooden Shield") {
        Attributes.Add(new ItemAttribute("Defense", 10));
        Price = 10;
    }
}