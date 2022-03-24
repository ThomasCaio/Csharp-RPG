using Entities;
namespace Monsters.All;
public class Spider : Monster {
    public Spider() : base("Spider") {
        MaxHealth = 35;
        BaseDamage = 3;
        Passives.Add(new Poison(20, 10));
        DropExp = 3;
    }
}