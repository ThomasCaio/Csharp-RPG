using EntityModule;
namespace Monsters.All;
public class DwarfSoldier : Monster {
    public DwarfSoldier() : base("Dwarf Soldier") {
        MaxHealth = 60;
        BaseDamage = 6;
        BaseDefense = 3;
        Passives.Add(new BoltShot());
        DropExp = 12;
        DropGold = new Tuple<int, int>(4, 10);
    }
}