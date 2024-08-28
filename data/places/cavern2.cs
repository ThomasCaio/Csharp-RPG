using PlaceModule;
using EntityModule;
using Monsters.All;

public class Cavern2 : HuntingPlace {
    public Cavern2(RPG.Game parent) : base("Cavern", parent) {
        NormalMonsters = new List<Monster> {
            new Dwarf(),
            new DwarfSoldier()
        };

        MediumMonsters = new List<Monster> {
            new DwarfSoldier(),
            new DwarfGuard()
        };

        HardMonsters = new List<Monster> {
            new DwarfGuard(),
            new DwarfMagician(),

        };
    }
}