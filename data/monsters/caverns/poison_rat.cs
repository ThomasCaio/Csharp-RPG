using Entities;
namespace Monsters.All;
public class PoisonRat : Monster {
    public PoisonRat() : base("Poison Rat") {
        MaxHealth = 30;
        BaseDamage = 3;
        Passives.Add(new Poison(baseDamage: 10));
        DropExp = 3;
        DropGold = new Tuple<int, int>(1, 2);
    }
}