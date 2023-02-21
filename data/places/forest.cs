using Places;
using Monsters.All;

public class Forest : HuntingPlace {
    public Forest(RPG.Game parent) : base("Forest", parent) {
        NormalMonsters.Add(new Rat());

        MediumMonsters.Add(new Spider());
        MediumMonsters.Add(new Rat());
        
        HardMonsters.Add(new Wolf());
        HardMonsters.Add(new Troll());
        HardMonsters.Add(new Elf());
    }
}