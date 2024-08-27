using Entities;
namespace Monsters.All;
public class DwarfGuard : Monster {
    public DwarfGuard() : base("Dwarf Guard") {
        MaxHealth = 110;
        BaseDamage = 10;
        BaseDefense = 7;
        Passives.Add(new Bash(20, 1, 15));
        DropExp = 21;
        DropGold = new Tuple<int, int>(7, 20);
    }
}