using EntityModule;
namespace Monsters.All;
public class Elf : Monster {
    public Elf() : base("Elf") {
        MaxHealth = 30;
        BaseDamage = 6;
        BaseDefense = -1;
        DropExp = 10;
        DropGold = new Tuple<int, int>(3, 3);
    }
}