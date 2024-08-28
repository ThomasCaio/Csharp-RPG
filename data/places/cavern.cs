using PlaceModule;
using EntityModule;
using Monsters.All;

public class Cavern : HuntingPlace {
    public Cavern(RPG.Game parent) : base("Cavern", parent) {
        NormalMonsters = new List<Monster> {
            new PoisonRat()
        };

        MediumMonsters = new List<Monster> {
            new PoisonSpider(),
            new Dwarf(),
        };

        HardMonsters = new List<Monster> {
            new Dwarf(),
            new DwarfSoldier(),
        };
    }
}