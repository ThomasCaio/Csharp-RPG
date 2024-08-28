using EntityModule;
namespace Monsters.All;
public class PoisonSpider : Monster {
    public PoisonSpider() : base("Poison Spider") {
        MaxHealth = 35;
        BaseDamage = 4;
        Passives.Add(new Poison(baseDamage: 10,turns: 10));
        DropExp = 3;
    }
}