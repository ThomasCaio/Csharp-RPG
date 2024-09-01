namespace ItemModule.All;

public class TestSword : Sword{
    public TestSword() : base("Test Sword"){
        Attributes.Add(new ItemAttribute("Damage", 50));
        Attributes.Add(new ItemAttribute("Defense", 20));
        Attributes.Add(new ItemAttribute("Armor", 50));
        Attributes.Add(new ItemAttribute("Max Health", 500));
        // Attributes.Add(new ItemAttribute("Max Mana", 500));
        Attributes.Add(new ItemAttribute("Physical Resistance", 5));
        Attributes.Add(new ItemAttribute("Fire Resistance", 5));
        Attributes.Add(new ItemAttribute("Water Resistance", 5));
        Attributes.Add(new ItemAttribute("Air Resistance", 5));
        Attributes.Add(new ItemAttribute("Earth Resistance", 5));
    }
}