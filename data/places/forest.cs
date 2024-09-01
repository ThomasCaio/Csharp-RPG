using PlaceModule;
using EntityModule;
using Monsters.All;

public class Forest : HuntingPlace {
    public Forest(RPG.Game parent) : base("Forest", parent) {
        NormalMonsters = new List<Monster> { 
            new Rat() 
        };

        MediumMonsters = new List<Monster> {
            new Spider(),
            new Rat()
        };
        
        HardMonsters = new List<Monster> {
            new Wolf(),
            new Troll(),
            new Elf()
        };
    }
}