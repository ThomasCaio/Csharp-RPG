using Entities;
using Spells;
namespace Monsters.All;
public class DwarfMagician : Monster {
    public DwarfMagician() : base("Dwarf Magician") {
        MaxHealth = 80;
        BaseDamage = 5;
        BaseDefense = 2;
        Passives.Add(new MonsterCast(new Firebolt(), 30));
        DropExp = 25;
        DropGold = new Tuple<int, int>(12, 18);
    }
}