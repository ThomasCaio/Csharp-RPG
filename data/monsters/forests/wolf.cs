using EntityModule;
namespace Monsters.All;
public class Wolf : Monster {
    public Wolf() : base("Wolf") {
        MaxHealth = 50;
        BaseDamage = 4;
        Passives.Add(new CriticalStrike(25));
        DropExp = 5;
        DropGold = new Tuple<int, int>(2, 3);
    }
}