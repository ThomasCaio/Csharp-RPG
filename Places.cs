namespace Places;
using Monsters.Factory;
using Entities;
using static System.Linq.Enumerable;

public enum PlaceType {
    Open,
    Protected,
}

public abstract class Place {
    public string Name;
    public RPG.Game Parent;

    public Place(string name, RPG.Game parent) {
        Name = name;
        Parent = parent;
    }
}

public abstract class City : Place {
    public City(string name, RPG.Game parent) : base(name, parent) {}
}

public abstract class HuntingPlace : Place {
    public static int RequiredLevel;
    public static List<Entities.Monster> NormalMonsters = new List<Entities.Monster>();
    public static List<Entities.Monster> MediumMonsters = new List<Entities.Monster>();
    
    public static List<Entities.Monster> HardMonsters = new List<Entities.Monster>();
    public static List<Entities.Monster> Boss = new List<Entities.Monster>();

    public HuntingPlace(string name, RPG.Game parent) : base(name, parent) {}

    public Party NewParty(int score) {
        var pt = new Party();
        int quantity = 1;
        if (score > 100) {
            quantity = RPG.Game.RNG.Next(2, 5);
        }
        else if (score > 25) {
            quantity = RPG.Game.RNG.Next(1, 3);
        }

        foreach (int i in Range(0, quantity)) {
            NewMonster(score, pt);
        }

        return pt;
    }

    public void NewMonster(int score, Party party) {
        Monster? monster;
        if (score > 100) {
            monster = MonsterFactory.New(HardMonsters[RPG.Game.RNG.Next(HardMonsters.Count)].Name);
        }
        else if (score > 25) {
            monster = MonsterFactory.New(MediumMonsters[RPG.Game.RNG.Next(MediumMonsters.Count)].Name);
        }
        else {
            monster = MonsterFactory.New(NormalMonsters[RPG.Game.RNG.Next(NormalMonsters.Count)].Name);
        }
        if (monster != null) {
            party.Add(monster);
        }
    }
}