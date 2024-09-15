using PlaceModule;
using EntityModule;
using Monsters.All;

public class Cavern2 : HuntingPlace {
    public Cavern2(RPG.Game parent) : base("Cavern 2", parent) {
        RequiredLevel = 10;
        NormalMonsters = [
            new Dwarf(),
            new DwarfSoldier()
        ];

        MediumMonsters = [
            new DwarfSoldier(),
            new DwarfGuard()
        ];

        HardMonsters = [
            new DwarfGuard(),
            new DwarfMagician(),

        ];
    }
}