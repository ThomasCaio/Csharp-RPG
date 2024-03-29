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
    public static List<Entities.Monster>? NormalMonsters;
    public static List<Entities.Monster>? MediumMonsters;
    
    public static List<Entities.Monster>? HardMonsters;
    public static List<Entities.Monster>? Boss;

    public HuntingPlace(string name, RPG.Game parent) : base(name, parent) {}

    public Party NewParty(int score) {
        var pt = new Party();
        int pt_size = 1;
        if (score > 100) {
            pt_size = RPG.Game.RNG.Next(2, 5);
        }
        else if (score > 25) {
            pt_size = RPG.Game.RNG.Next(1, 3);
        }

        foreach (int i in Range(0, pt_size)) {
            NewMonster(score, pt);
        }

        return pt;
    }

    public void NewMonster(int score, Party party) {
        Monster? monster;
        if (score > 100) {
            var rng = RPG.Game.RNG.Next(HardMonsters.Count);
            monster = MonsterFactory.New(HardMonsters[rng].Name);
            foreach(var m in HardMonsters)
            {
                File.AppendAllText("./tests/monsters.test", $"{m.Name} {rng} {HardMonsters[rng]}\n");
            }
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