using EntityModule;
namespace Monsters.All;
public class Rat : Monster {
    public Rat() : base("Rat") {
        MaxHealth = 20;
        BaseDamage = 2;
        DropExp = 2;
        DropGold = new Tuple<int, int>(1, 2);
    }
}