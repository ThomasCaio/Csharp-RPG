using Entities;
namespace Monsters.All;
public class Orc : Monster {
    public Orc() : base("Orc") {
        BaseDamage = 5;
        DropGold = new Tuple<int, int>(2, 5);
        DropExp = 10;
    }
}