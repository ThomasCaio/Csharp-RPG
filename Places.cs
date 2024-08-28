namespace PlaceModule;
using Monsters.Factory;
using EntityModule;
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
    public int RequiredLevel;
    public List<EntityModule.Monster>? NormalMonsters;
    public List<EntityModule.Monster>? MediumMonsters;
    
    public List<EntityModule.Monster>? HardMonsters;
    public List<EntityModule.Monster>? Boss;

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

    private List<Monster>? GetMonstersForScore(int score)
    {
        return score switch
        {
            > 100 => HardMonsters,
            > 25 => MediumMonsters,
            _ => NormalMonsters
        };
    }

    public void NewMonster(int score, Party party) {
        var monsters = GetMonstersForScore(score);
        var monster = MonsterFactory.New(monsters![RPG.Game.RNG.Next(monsters.Count)].Name);
        if (monster != null)
        {
            party.Add(monster);
        }
    }
}