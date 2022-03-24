namespace Monsters.Factory;
using Entities;

using Monsters.All;


public static class MonsterFactory {
    public static Monster? New(string name) {
        switch (name) {
            case "Rat":
            return new Rat();

            case "Spider":
            return new Spider();

            case "Wolf":
            return new Wolf();

            case "Orc":
            return new Orc();

            // case "Troll":
            // return new Troll();
        }
        return null;
    }
}