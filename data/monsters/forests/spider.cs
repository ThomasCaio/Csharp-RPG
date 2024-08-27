using Entities;
namespace Monsters.All;
public class Spider : Monster {
    public Spider() : base("Spider") {
        MaxHealth = 35;
        BaseDamage = 4;
        DropExp = 3;
    }
}