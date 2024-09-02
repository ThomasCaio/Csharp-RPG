using EntityModule;
namespace Monsters.All;
public class Troll : Monster {
    public Troll() : base("Troll") {
        MaxHealth = 60;
        BaseDamage = 4;
        BaseDefense = 2;
        DropExp = 15;
        DropGold = new Tuple<int, int>(4, 6);
    }
}