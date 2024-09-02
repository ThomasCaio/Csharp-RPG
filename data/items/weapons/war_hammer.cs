using Monsters.Warlock;

namespace ItemModule.All;

public class WarHammer : TwoHandedClub{
    public WarHammer() : base("War Hammer"){
        Attributes.Add(new ItemAttribute("Damage", 50));
        BuyPrice = 5000;
    }
}