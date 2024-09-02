using EntityModule;
namespace Monsters.All;
public class Spider : Monster {
    public Spider() : base("Spider") {
        MaxHealth = 35;
        BaseDamage = 4;
        DropExp = 7;
        DropGold = new Tuple<int, int>(3, 4);
    }
}