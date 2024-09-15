using PlaceModule;
using EntityModule;
using Monsters.All;

public class Cavern : HuntingPlace {
    public Cavern(RPG.Game parent) : base("Cavern", parent) {
        RequiredLevel = 5;
        NormalMonsters = [
            new PoisonRat()
        ];

        MediumMonsters = [
            new PoisonSpider(),
            new Dwarf(),
        ];

        HardMonsters = [
            new Dwarf(),
            new DwarfSoldier(),
        ];
    }
}