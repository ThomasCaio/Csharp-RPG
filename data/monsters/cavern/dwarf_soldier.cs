using Entities;
namespace Monsters.All;
public class DwarfSoldier : Monster {
    public DwarfSoldier() : base("Dwarf Soldier") {
        MaxHealth = 55;
        BaseDamage = 6;
        BaseDefense = 3;
        Passives.Add(new BoltShot());
        DropExp = 5;
        DropGold = new Tuple<int, int>(3, 6);
    }
}